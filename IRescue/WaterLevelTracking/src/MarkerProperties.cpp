// <copyright file="MarkerProperties.cpp" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#include "../include/MarkerProperties.h"

namespace waterleveltracking {
	/// <summary>
	/// Initializes a new instance of the <see cref="markerCornerPixelLocations"/> class.
	/// </summary>
	/// <param name="markerCornerPixelLocations">Array of size 8 with the 4 x and y pixel positions of the edges in the camera feed [bottom left x, bottom left y, bottom right x, bottom right y, top right x, top right y, top left x, top left y]</param>
	/// <param name="distanceToStripes">The distance in meters from the center of the marker to the first stripe</param>
	/// <param name="markerHeight">The height of the center of the marker in meters</param>
	MarkerProperties::MarkerProperties(int corners[], double markerSize, double distanceToStripes, double markerHeight)
	{
		this->GetMarkerCenter(corners);
		this->GetMarkerCorners(corners);
		this->distanceToStripes = distanceToStripes;
		this->markerHeight = markerHeight;
		this->markerSize = markerSize;
		this->meterToPixelFactor = sqrt(pow(corners[1] - corners[7], 2) + pow(corners[0] - corners[6], 2)) / markerSize;
	}

	/// <summary>
	/// Recalculates the center of the marker
	/// </summary>
	void MarkerProperties::ResetCenter()
	{
		this->center = Point((this->corners.bottomLeft.x + this->corners.bottomRight.x + this->corners.topRight.x + this->corners.topLeft.x) / 4,
			(this->corners.bottomLeft.y + this->corners.bottomRight.y + this->corners.topRight.y + this->corners.topLeft.y) / 4);
	}

	/// <summary>
	/// Gets the corners
	/// </summary>
	Square MarkerProperties::GetCorners() {
		return this->corners;
	}

	/// <summary>
	/// Sets the corners
	/// </summary>
	void MarkerProperties::SetCorners(Square corners) {
		this->corners = corners;
	}

	/// <summary>
	/// Gets the marker size
	/// </summary>
	double MarkerProperties::GetMarkerSize() {
		return this->markerSize;
	}

	/// <summary>
	/// Sets the marker size
	/// </summary>
	void MarkerProperties::SetMarkerSize(double markerSize) {
		this->markerSize = markerSize;
	}

	/// <summary>
	/// Gets the meter to pixel factor
	/// </summary>
	double MarkerProperties::GetMeterToPixelFactor() {
		return this->meterToPixelFactor;
	}

	/// <summary>
	/// Sets the meter to pixel factor
	/// </summary>
	void MarkerProperties::SetMeterToPixelFactor(double meterToPixelFactor) {
		this->meterToPixelFactor = meterToPixelFactor;
	}

	/// <summary>
	/// Gets the marker height
	/// </summary>
	double MarkerProperties::GetMarkerHeight() {
		return this->markerHeight;
	}

	/// <summary>
	/// Sets the marker height
	/// </summary>
	void MarkerProperties::SetMarkerHeight(double markerHeight) {
		this->markerHeight = markerHeight;
	}

	/// <summary>
	/// Gets the distance to stripes
	/// </summary>
	double MarkerProperties::GetDistanceToStripes() {
		return this->distanceToStripes;
	}

	/// <summary>
	/// Sets the distance to stripes
	/// </summary>
	void MarkerProperties::SetDistanceToStripes(double distanceToStripes) {
		this->distanceToStripes = distanceToStripes;
	}

	/// <summary>
	/// Gets the center
	/// </summary>
	cv::Point MarkerProperties::GetCenter() {
		return this->center;
	}

	/// <summary>
	/// Sets the center
	/// </summary>
	void MarkerProperties::SetCenter(cv::Point center) {
		this->center = center;
	}

	/// <summary>
	/// Calculate the center of the marker
	/// </summary>
	/// <param name="markerCornerPixelLocations">Array of size 8 with the 4 x and y pixel positions of the edges in the camera feed [bottom left x, bottom left y, bottom right x, bottom right y, top right x, top right y, top left x, top left y]</param>
	void MarkerProperties::GetMarkerCenter(int markerCornerPixelLocations[])
	{
		this->center = Point((markerCornerPixelLocations[0] + markerCornerPixelLocations[2] + markerCornerPixelLocations[4] + markerCornerPixelLocations[6]) / 4,
			(markerCornerPixelLocations[1] + markerCornerPixelLocations[3] + markerCornerPixelLocations[5] + markerCornerPixelLocations[7]) / 4);
	}

	/// <summary>
	/// Calculate the corners of the marker
	/// </summary>
	/// <param name="markerCornerPixelLocations">Array of size 8 with the 4 x and y pixel positions of the edges in the camera feed [bottom left x, bottom left y, bottom right x, bottom right y, top right x, top right y, top left x, top left y]</param>
	void MarkerProperties::GetMarkerCorners(int corners[])
	{
		Square square;
		square.bottomLeft = Point(corners[0], corners[1]);
		square.bottomRight = Point(corners[2], corners[3]);
		square.topRight = Point(corners[4], corners[5]);
		square.topLeft = Point(corners[6], corners[7]);
		this->corners = square;
	}
}