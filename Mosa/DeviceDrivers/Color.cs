/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
	/// <summary>
	/// Color
	/// </summary>
	public struct Color
	{
		/// <summary>
		/// Red
		/// </summary>
		public byte Red;
		/// <summary>
		/// Green
		/// </summary>
		public byte Green;
		/// <summary>
		/// Blue
		/// </summary>
		public byte Blue;
		/// <summary>
		/// Alpha
		/// </summary>
		public byte Alpha;

		/// <summary>
		/// Initializes a new instance of the <see cref="Color"/> struct.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		public Color(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Color"/> struct.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		/// <param name="alpha">The alpha.</param>
		public Color(byte red, byte green, byte blue, byte alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

	}
}
