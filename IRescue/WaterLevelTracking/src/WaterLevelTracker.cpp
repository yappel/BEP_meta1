// <copyright file="WaterLevelTracker.cpp" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#include "../include/WaterLevelTracker.h"

namespace waterleveltracking {
	/// <summary>
	/// Predict the height of the water level using markers.
	/// Return NULL or 0 if the the water level cannot be derived from the information
	/// Return -1 if the input was wrong e.g. markerCornerPixelLocations is of incorrect size
	/// </summary>
	/// <param name="frame">The captured frame of the video feed</param>
	/// <param name="markerProperties">Properties of the measured marker</param>
	/// <param name="stripeProperties">Properties of the striped underneath the measured marker</param>
	/// <param name="rotation">Angle in degrees of the rotation of the marker</param>
	/// <returns>The height of the water in meters</returns>
	double WaterLevelTracker::CalculateWaterLevel(Mat &frame, MarkerProperties &markerProperties, StripeProperties &stripeProperties, double rotation) {
		cvtColor(frame, frame, CV_RGB2GRAY);
		int imageBottom = Rotate(frame, 360 - rotation, markerProperties);
		stripeProperties.SetStripePixelStart(markerProperties.GetCenter().y + (int)(markerProperties.GetDistanceToStripes() * markerProperties.GetMeterToPixelFactor()));
		Crop(frame, markerProperties.GetCorners().bottomLeft.x, markerProperties.GetCorners().bottomRight.x, stripeProperties.GetStripePixelStart(), imageBottom);
		Blur(frame);
		Segment(frame);
		return StripeCount(frame, stripeProperties);
	}

	/// <summary>
	/// Perform a smoothing on the image using a Gaussian blur.
	/// </summary>
	/// <param name="frame">The captured frame of the video feed</param>
	void WaterLevelTracker::Blur(Mat &frame) {
		GaussianBlur(frame, frame, Size(5, 5), 0);
	}

	/// <summary>
	/// Crop the area underneath the marker that contains the stripes
	/// </summary>
	/// <param name="frame">The captured frame of the video feed</param>
	/// <param name="bottomLeftCornerX">The x of the bottom left corner of the marker</param>
	/// <param name="bottomRightCornerX">The x of the bottom right corner of the marker</param>
	/// <param name="stripePixelStart">The starting pixel of the highest stripe</param>
		/// <param name="bottom"> The bottom pixel to crop on</param>
	void WaterLevelTracker::Crop(Mat &frame, int bottomLeftCornerX, int bottomRightCornerX, int stripePixelStart, int bottom) {
		int left = bottomLeftCornerX + (bottomRightCornerX - bottomLeftCornerX) / 3;
		int right = bottomLeftCornerX + ((bottomRightCornerX - bottomLeftCornerX) / 3) * 2;
		frame = frame(Rect(left, stripePixelStart, right - left, bottom - stripePixelStart));
	}

	/// <summary>
	/// Perform a smoothing on the image using a Gaussian blur.
	/// </summary>
	/// <param name="frame">The captured frame of the video feed</param>
	void WaterLevelTracker::Segment(Mat &frame) {
		cv::threshold(frame, frame, 150, 255, 0);
		for (int row = 0; row < frame.rows; row++) {
			int count = 0;
			for (int col = 0; col < frame.cols; col++) {
				count += frame.at<uchar>(row, col);
			}

			frame.at<uchar>(row, 0) = (count / 255) >= ((frame.cols) * 0.45) ? 255 : 0;
		}
	}

	/// <summary>
	/// Map the corners of the marker to the new corners in the rotated image
	/// </summary>
	/// <param name="frame">The captured frame of the video feed</param>
	/// <param name="rotation">Angle in degrees of the rotation of the marker</param>
	/// <param name="markerProperties">Properties of the measured marker</param>
	/// <returns>Return the y of the last pixel that should be iterated or the bottom</returns>
	int WaterLevelTracker::Rotate(Mat &frame, double rotation, MarkerProperties &markerProperties) {
		Mat mRotation = getRotationMatrix2D(Point((frame.cols / 2) - 1, (frame.rows / 2) - 1), rotation, 1);
		cv::warpAffine(frame, frame, mRotation, frame.size());
		MapRotation(markerProperties, mRotation);
		markerProperties.ResetCenter();
		for (int row = frame.rows - 1; row >= 0; row--) {
			if (frame.at<uchar>(row, markerProperties.GetCenter().x) != 0.0f) {
				return row;
			}
		}

		return frame.rows;
	}

	/// <summary>
	/// Calculate the new pixel coordinates of the marker corners
	/// </summary>
	/// <param name="corners">The marker corner input</param>
	/// <param name="rotationMatrix">The rotation matrix used for the entire image</param>
	void WaterLevelTracker::MapRotation(MarkerProperties &markerProperties, Mat &rotationMatrix) {
		Mat cornerPoints = rotationMatrix * (Mat_<double>(3, 4) <<
			markerProperties.GetCorners().bottomLeft.x, markerProperties.GetCorners().bottomRight.x, markerProperties.GetCorners().topRight.x, markerProperties.GetCorners().topLeft.x,
			markerProperties.GetCorners().bottomLeft.y, markerProperties.GetCorners().bottomRight.y, markerProperties.GetCorners().topRight.y, markerProperties.GetCorners().topLeft.y,
			1, 1, 1, 1);
		Square newCorners;
		newCorners.bottomLeft = Point((int)cornerPoints.at<double>(0, 0), (int)cornerPoints.at<double>(1, 0));
		newCorners.bottomRight = Point((int)cornerPoints.at<double>(0, 1), (int)cornerPoints.at<double>(1, 1));
		newCorners.topRight = Point((int)cornerPoints.at<double>(0, 2), (int)cornerPoints.at<double>(1, 2));
		newCorners.topLeft = Point((int)cornerPoints.at<double>(0, 3), (int)cornerPoints.at<double>(1, 3));
		markerProperties.SetCorners(newCorners);
	}

	/// <summary>
	/// Count the number of measured striped and return the water level height.
	/// </summary>
	/// <param name="frame">The captured frame of the video feed</param>
	/// <param name="stripeProperties">The properties of the measured striped input</param>
	/// <returns>The amount of iterations in which a 0 or 255 were drawe</returns>
	double WaterLevelTracker::StripeCount(Mat &frame, StripeProperties &stripeProperties) {
		int previous = -1;
		int previousStripeHeight = stripeProperties.GetStripePixelHeight();
		int previousStripeEnd = 0;
		int currentStripeHeight = -1;
		int count = 0;
		for (int i = 0; i < frame.rows - 1; i++) {
			currentStripeHeight = 0;
			while (i < frame.rows - 1 && frame.at<uchar>(i + 1, 0) == frame.at<uchar>(i, 0)) {
				currentStripeHeight++;
				i++;
			}

			// If the current iterated stripe is almost of equal size of the previous, it is accepted
			if (abs(previousStripeHeight - currentStripeHeight) < previousStripeHeight * 0.3) {
				previousStripeHeight = currentStripeHeight;
				previousStripeEnd = i;
				count++;
			} else if (previousStripeEnd + previousStripeHeight < frame.rows) {
				count++;
				break;
			} else {
				return NULL;
			}
		}

		return count >= stripeProperties.GetStripeCount() ? NULL : stripeProperties.GetStripeStart() - (stripeProperties.GetStripeHeight() * count);
	}
}