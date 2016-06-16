// <copyright file="StripeProperties.h" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#ifndef __STRIPEPROPERTIES_H__
#define __STRIPEPROPERTIES_H__

#include "MarkerProperties.h"

namespace waterleveltracking {
	/// <summary>
	/// The class can calculate the water level.
	/// </summary>
	class StripeProperties
	{
	public:
		/// <summary>
		/// Initializes a new instance of the <see cref="markerCornerPixelLocations"/> class.
		/// </summary>
		/// <param name="markerProperties">The properties of the marker which is currently visible</param>
		/// <param name="stripeHeight">The height of individual stripes in meters</param>
		/// <param name="stripeCount">The amount of stripes located under the marker</pafram>
		StripeProperties(MarkerProperties markerProperties, double stripeHeight, int stripeCount);

		/// <summary>
		/// Sets the stripe height
		/// </summary>
		void SetStripeHeight(double stripeHeight);

		/// <summary>
		/// Gets the stripe height
		/// </summary>
		double GetStripeHeight();

		/// <summary>
		/// Sets the stripe stripe start
		/// </summary>
		void SetStripeStart(double stripeStart);

		/// <summary>
		/// Gets the stripe stripe start
		/// </summary>
		double GetStripeStart();

		/// <summary>
		/// Sets the stripe stripe count
		/// </summary>
		void SetStripeCount(int stripeCount);

		/// <summary>
		/// Gets the stripe stripe count
		/// </summary>
		int GetStripeCount();

		/// <summary>
		/// Sets the stripe stripe pixel height
		/// </summary>
		void SetStripePixelHeight(int stripePixelHeight);

		/// <summary>
		/// Gets the stripe stripe pixel height
		/// </summary>
		int GetStripePixelHeight();

		/// <summary>
		/// Sets the stripe stripe pixel start
		/// </summary>
		void SetStripePixelStart(int stripePixelStart);

		/// <summary>
		/// Gets the stripe stripe pixel start
		/// </summary>
		int GetStripePixelStart();

	private:
		/// <summary>
		/// The height of individual stripes in meters
		/// </summary>
		double stripeHeight;

		/// <summary>
		/// The height of the top stripe in meters
		/// </summary>
		double stripeStart;

		/// <summary>
		/// The amount of stripes located under the marker
		/// </summary>
		int stripeCount;

		/// <summary>
		/// The height of the stripes in pixels
		/// </summary>
		int stripePixelHeight;

		/// <summary>
		/// The y pixel of the top stripe
		/// </summary>
		int stripePixelStart;
	};
}

#endif