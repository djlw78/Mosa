﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.ClassLib;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	/// Standard Keyboard Device Driver
	/// </summary>
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0060, PortRange = 1, AltBasePort = 0x0064, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class StandardKeyboard : ISAHardwareDevice, IDevice, IHardwareDevice, IKeyboardDevice
	{
		protected IReadWriteIOPort commandPort;
		protected IReadWriteIOPort dataPort;

		protected const ushort fifoSize = 256;
		protected byte[] fifoBuffer;
		protected uint fifoStart;
		protected uint fifoEnd;

		protected SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKeyboard"/> class.
		/// </summary>
		public StandardKeyboard() { }

		/// <summary>
		/// Setups the standard keyboard driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup()
		{
			base.name = "StandardKeyboard";

			commandPort = base.busResources.GetIOPort(0, 0);
			dataPort = base.busResources.GetIOPort(1, 0);

			this.fifoBuffer = new byte[fifoSize];
			this.fifoStart = 0;
			this.fifoEnd = 0;

			return true;
		}

		/// <summary>
		/// Starts the standard keyboard device.
		/// </summary>
		/// <returns></returns>
		public override bool Start() { return true; }

		/// <summary>
		/// Probes for the standard keyboard device.
		/// </summary>
		/// <returns></returns>
		public override bool Probe() { return true; }

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices() { return null; }

		/// <summary>
		/// Called when interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			ReadScanCode();
			return true;
		}

		/// <summary>
		/// Adds scan code to FIFO.
		/// </summary>
		/// <param name="value">The value.</param>
		protected void AddToFIFO(byte value)
		{
			uint next = fifoEnd + 1;

			if (next == fifoSize)
				next = 0;

			if (next == fifoStart)
				return; // out of room

			fifoBuffer[next] = value;
			fifoEnd = next;
		}

		/// <summary>
		/// Gets scan code from FIFO.
		/// </summary>
		/// <returns></returns>
		protected byte GetFromFIFO()
		{
			if (fifoEnd == fifoStart)
				return 0;	// should not happen

			byte value = fifoBuffer[fifoStart];

			fifoStart++;

			if (fifoStart == fifoSize)
				fifoStart = 0;

			return value;
		}

		/// <summary>
		/// Determines whether FIFO data is available
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [FIFO data is available]; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsFIFODataAvailable()
		{
			return (fifoEnd != fifoStart);
		}

		/// <summary>
		/// Determines whether the FIFO is full
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the FIFO is full; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsFIFOFull()
		{
			if ((((fifoEnd + 1) == fifoSize) ? 0 : fifoEnd + 1) == fifoStart)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Reads the scan code from the device
		/// </summary>
		protected void ReadScanCode()
		{
			spinLock.Enter();

			AddToFIFO(dataPort.Read8());

			spinLock.Exit();
		}

		/// <summary>
		/// Gets the scan code from the fifo
		/// </summary>
		/// <returns></returns>
		public byte GetScanCode()
		{
			if (!IsFIFODataAvailable())
				return 0;

			return GetFromFIFO();
		}
	}
}