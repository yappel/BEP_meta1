// <copyright file="MarkerProperties.h" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#ifndef __MARKERPROPERTIES_H__
#define __MARKERPROPERTIES_H__

#include <opencv2/opencv.hpp>
#include <math.h>
#include "Square.h"

using namespace cv;

namespace waterleveltracking {
	/// <summary>
	/// The class can calculate the water level.
	/// </summary>
	class MarkerProperties {
	public:
		/// <summary>
		/// Initializes a new instance of the <see cref="markerCornerPixelLocations"/> class.
		/// </summary>
		/// <param name="markerCornerPixelLocations">Array of size 8 with the 4 x and y pixel positions of the edges in the camera feed [bottom left x, bottom left y, bottom right x, bottom right y, top right x, top right y, top left x, top left y]</param>
		/// <param name="distanceToStripes">The distance in meters from the center of the marker to the first stripe</param>
		/// <param name="markerHeight">The height of the center of the marker in meters</param>
		MarkerProperties(int corners[], double markerSize, double distanceToStripes, double markerHeight);

		/// <summary>
		/// Recalculates the center of the marker
		/// </summary>
		void ResetCenter();

		/// <summary>
		/// Gets the corners
		/// </summary>
		Square GetCorners();

		/// <summary>
		/// Sets the corners
		/// </summary>
		void SetCorners(Square corners);

		/// <summary>
		/// Gets the marker size
		/// </summary>
		double GetMarkerSize();

		/// <summary>
		/// Sets the marker size
		/// </summary>
		void SetMarkerSize(double markerSize);

		/// <summary>
		/// Gets the meter to pixel factor
		/// </summary>
		double GetMeterToPixelFactor();

		/// <summary>
		/// Sets the meter to pixel factor
		/// </summary>
		void SetMeterToPixelFactor(double meterToPixelFactor);

		/// <summary>
		/// Gets the marker height
		/// </summary>
		double GetMarkerHeight();

		/// <summary>
		/// Sets the marker height
		/// </summary>
		void SetMarkerHeight(double markerHeight);

		/// <summary>
		/// Gets the distance to stripes
		/// </summary>
		double GetDistanceToStripes();

		/// <summary>
		/// Sets the distance to stripes
		/// </summary>
		void SetDistanceToStripes(double distanceToStripes);

		/// <summary>
		/// Gets the center
		/// </summary>
		cv::Point GetCenter();

		/// <summary>
		/// Sets the center
		/// </summary>
		void SetCenter(cv::Point center);

	private:
		/// <summary>
		/// The four pixel coordinates of the marker
		/// </summary>
		Square corners;

		/// <summary>
		/// The size of the marker in meters
		/// </summary>
		double markerSize;

		/// <summary>
		/// The factor to convert a meter to a pixel
		/// </summary>
		double meterToPixelFactor;

		/// <summary>
		/// The height of the center of the marker in meters
		/// </summary>
		double markerHeight;

		/// <summary>
		/// The distance in meters from the center of the marker to the first stripe
		/// </summary>
		double distanceToStripes;

		/// <summary>
		/// The center coordinate of the marker in pixels
		/// </summary>
		Point center;

		/// <summary>
		/// Calculate the center of the marker
		/// </summary>
		/// <param name="markerCornerPixelLocations">Array of size 8 with the 4 x and y pixel positions of the edges in the camera feed [bottom left x, bottom left y, bottom right x, bottom right y, top right x, top right y, top left x, top left y]</param>
		void GetMarkerCenter(int markerCornerPixelLocations[]);

		/// <summary>
		/// Calculate the corners of the marker
		/// </summary>
		/// <param name="markerCornerPixelLocations">Array of size 8 with the 4 x and y pixel positions of the edges in the camera feed [bottom left x, bottom left y, bottom right x, bottom right y, top right x, top right y, top left x, top left y]</param>
		void GetMarkerCorners(int corners[]);
	};
}

#endif