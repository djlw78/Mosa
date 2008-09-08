﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{

	/// <summary>
	/// Interface for keyboard
	/// </summary>
	public interface IKeyboard
	{
		/// <summary>
		/// Determines whether [scroll lock is on].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [scroll lock is on]; otherwise, <c>false</c>.
		/// </returns>
		bool IsScrollLockOn();

		/// <summary>
		/// Determines whether [cap lock is on].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [cap lock is on]; otherwise, <c>false</c>.
		/// </returns>
		bool IsCapLockOn();

		/// <summary>
		/// Determines whether [num lock is on].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [num lock is on]; otherwise, <c>false</c>.
		/// </returns>
		bool IsNumLockOn();

		/// <summary>
		/// Determines whether [the control key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the control key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		bool IsControlKeyPressed();

		/// <summary>
		/// Determines whether [the alt key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the alt key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		bool IsAltKeyPressed();

		/// <summary>
		/// Determines whether [the shift key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the shift key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		bool IsShiftKeyPressed();

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		char GetKeyPressed();
	}
}
