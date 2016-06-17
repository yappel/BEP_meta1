// <copyright file="Square.h" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

#ifndef __SQUARE_H__
#define __SQUARE_H__

#include <opencv2/opencv.hpp>

	/// <summary>
	/// A square that contains four corners
	/// </summary>
	struct Square {
		/// <summary>
		/// The bottom left corner coordinate
		/// </summary>
		cv::Point bottomLeft;

		/// <summary>
		/// The bottom right corner coordinate
		/// </summary>
		cv::Point bottomRight;

		/// <summary>
		/// The top right corner coordinate
		/// </summary>
		cv::Point topRight;

		/// <summary>
		/// The top left corner coordinate
		/// </summary>
		cv::Point topLeft;
	};

#endif 