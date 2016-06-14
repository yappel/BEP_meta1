// <copyright file="WaterLevelTracker.h" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#ifndef __WATERLEVELTRACKER_H__
#define __WATERLEVELTRACKER_H__

#include <iostream>
#include <opencv2/opencv.hpp>
#include <math.h>
#include "Square.h"
#include "MarkerProperties.h"
#include "StripeProperties.h"

namespace waterleveltracking {
	/// <summary>
	/// The class can calculate the water level.
	/// </summary>
	class WaterLevelTracker
	{
	public:
		/// <summary>
		/// Predict the height of the water level using markers.
		/// Return NULL if the the water level cannot be derived from the information
		/// Return -1 if the input was wrong e.g. markerCornerPixelLocations is of incorrect size
		/// </summary>
		/// <param name="frame">The captured frame of the video feed</param>
		/// <param name="markerProperties">Properties of the measured marker</param>
		/// <param name="stripeProperties">Properties of the striped underneath the measured marker</param>
		/// <param name="rotation">Angle in degrees of the rotation of the marker</param>
		/// <returns>The height of the water in meters</returns>
		static double WaterLevelTracker::CalculateWaterLevel(Mat &frame, MarkerProperties &markerProperties, StripeProperties &stripeProperties, double rotation);

	private:
		/// <summary>
		/// Perform a smoothing on the image using a Gaussian blur.
		/// </summary>
		/// <param name="frame">The captured frame of the video feed</param>
		static void Blur(Mat &frame);

		/// <summary>
		/// Crop the area underneath the marker that contains the stripes
		/// </summary>
		/// <param name="frame">The captured frame of the video feed</param>
		/// <param name="bottomLeftCornerX">The x of the bottom left corner of the marker</param>
		/// <param name="bottomRightCornerX">The x of the bottom right corner of the marker</param>
		/// <param name="stripePixelStart">The starting pixel of the highest stripe</param>
		/// <param name="bottom"> The bottom pixel to crop on</param>
		static void Crop(Mat &frame, int bottomLeftCornerX, int bottomRightCornerX, int stripePixelStart, int bottom);

		/// <summary>
		/// Perform a smoothing on the image using a Gaussian blur.
		/// </summary>
		/// <param name="frame">The captured frame of the video feed</param>
		/// <param name="stripePixelSize">The amount of pixels that were expected for a stripe</param>
		static void Segment(Mat &frame, int stripePixelSize);

		/// <summary>
		/// Rotates the image and calculates the new pixel coordinates of the corners
		/// </summary>
		/// <param name="frame">The captured frame of the video feed</param>
		/// <param name="rotation">Angle in degrees of the rotation of the marker</param>
		/// <param name="markerProperties">Properties of the measured marker</param>
		/// <returns>Return the y of the last pixel that should be iterated or the bottom</returns>
		static int Rotate(Mat &frame, double rotation, MarkerProperties &markerProperties);

		/// <summary>
		/// Calculate the new pixel coordinates of the marker corners
		/// </summary>
		/// <param name="markerProperties">The input marker properties</param>
		/// <param name="rotationMatrix">The rotation matrix used for the entire image</param>
		static void MapRotation(MarkerProperties &markerProperties, Mat &rotationMatrix);

		/// <summary>
		/// Count the number of measured striped and return the water level height.
		/// </summary>
		/// <param name="frame">The captured frame of the video feed</param>
		/// <param name="stripeProperties">The properties of the measured striped input</param>
		/// <returns>The amount of iterations in which a 0 or 255 were drawe</returns>
		static double WaterLevelTracker::StripeCount(Mat &frame, StripeProperties &stripeProperties);
	};
}

#endif