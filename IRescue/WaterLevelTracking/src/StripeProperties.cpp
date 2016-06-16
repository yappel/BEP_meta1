// <copyright file="StripeProperties.cpp" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#include "../include/StripeProperties.h"

namespace waterleveltracking {
	/// <summary>
	/// Initializes a new instance of the <see cref="markerCornerPixelLocations"/> class.
	/// </summary>
	/// <param name="markerProperties">The properties of the marker which is currently visible</param>
	/// <param name="stripeHeight">The height of individual stripes in meters</param>
	/// <param name="stripeCount">The amount of stripes located under the marker</param>
	StripeProperties::StripeProperties(MarkerProperties markerProperties, double stripeHeight, int stripeCount) {
		this->stripeHeight = stripeHeight;
		this->stripeStart = markerProperties.GetMarkerHeight() - markerProperties.GetDistanceToStripes();
		this->stripeCount = stripeCount;
		this->stripePixelHeight = int(stripeHeight * markerProperties.GetMeterToPixelFactor());
		this->SetStripePixelStart(int(markerProperties.GetCenter().y + (markerProperties.GetDistanceToStripes() * markerProperties.GetMeterToPixelFactor())));
	}

	/// <summary>
	/// Sets the stripe height
	/// </summary>
	void StripeProperties::SetStripeHeight(double stripeHeight) {
		this->stripeHeight = stripeHeight;
	}

	/// <summary>
	/// Gets the stripe height
	/// </summary>
	double StripeProperties::GetStripeHeight() {
		return this->stripeHeight;
	}

	/// <summary>
	/// Sets the stripe stripe start
	/// </summary>
	void StripeProperties::SetStripeStart(double stripeStart) {
		this->stripeStart = stripeStart;
	}

	/// <summary>
	/// Gets the stripe stripe start
	/// </summary>
	double StripeProperties::GetStripeStart() {
		return this->stripeStart;
	}

	/// <summary>
	/// Sets the stripe stripe count
	/// </summary>
	void StripeProperties::SetStripeCount(int stripeCount) {
		this->stripeCount = stripeCount;
	}

	/// <summary>
	/// Gets the stripe stripe count
	/// </summary>
	int StripeProperties::GetStripeCount() {
		return this->stripeCount;
	}

	/// <summary>
	/// Sets the stripe stripe pixel height
	/// </summary>
	void StripeProperties::SetStripePixelHeight(int stripePixelHeight) {
		this->stripePixelHeight = stripePixelHeight;
	}

	/// <summary>
	/// Gets the stripe stripe pixel height
	/// </summary>
	int StripeProperties::GetStripePixelHeight() {
		return this->stripePixelHeight;
	}

	/// <summary>
	/// Sets the stripe stripe pixel start
	/// </summary>
	void StripeProperties::SetStripePixelStart(int stripePixelStart) {
		this->stripePixelStart = stripePixelStart;
	}

	/// <summary>
	/// Gets the stripe stripe pixel start
	/// </summary>
	int StripeProperties::GetStripePixelStart() {
		return this->stripePixelStart;
	}
}