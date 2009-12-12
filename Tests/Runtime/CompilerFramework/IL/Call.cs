﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class Call : CodeDomTestRunner
	{
		private static string CreateTestCode(string name, string type)
		{
			return @"
                static class Test {
                    static bool " + name + "(" + type + " value) { return value == " + name + @"_Target(value); } 
                    static " + type + " " + name + "_Target(" + type + @" value) { return value; }
                }";
		}

		private static string CreateConstantTestCode(string name, string type, string constant)
		{
			return @"
                static class Test {
                    static bool " + name + "(" + type + " value) { return value == " + name + "_Target(" + constant + @"); } 
                    static " + type + " " + name + "_Target(" + type + @" value) { return value; }
                }";
		}

		/// <summary>
		/// 
		/// </summary>
		delegate void V();
		/// <summary>
		/// 
		/// </summary>
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallEmpty()
		{
			CodeSource = @"
                static class Test { 
                    static void CallEmpty() { CallEmpty_Target(); } 
                    static void CallEmpty_Target() { }
                }";
			Run<V>("", "Test", "CallEmpty");
		}

		#region B
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__B(bool arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(true)]
		[Row(false)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallB(bool value)
		{
			CodeSource = CreateTestCode("CallB", "bool");
			Assert.IsTrue((bool)Run<B__B>("", "Test", "CallB", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(true)]
		[Row(false)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantB(bool value)
		{
			CodeSource = CreateConstantTestCode("CallConstantB", "bool", value.ToString().ToLower());
			Assert.IsTrue((bool)Run<B__B>("", "Test", "CallConstantB", value));
		}
		#endregion

		#region C
		delegate bool B__C([MarshalAs(UnmanagedType.U2)]char arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row('a')]
		[Row('Z')]
		[Row(char.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallC(char value)
		{
			CodeSource = CreateTestCode("CallC", "char");
			Assert.IsTrue((bool)Run<B__C>("", "Test", "CallC", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row('a')]
		[Row('Z')]
		[Row('-')]
		[Row('.')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantC(char value)
		{
			CodeSource = CreateConstantTestCode("CallConstantC", "char", "'" + value.ToString() + "'");
			Assert.IsTrue((bool)Run<B__C>("", "Test", "CallConstantC", value));
		}
		#endregion

		#region I1
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__I1(sbyte arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(sbyte.MinValue)]
		[Row(sbyte.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI1(sbyte value)
		{
			CodeSource = CreateTestCode("CallI1", "sbyte");
			Assert.IsTrue((bool)Run<B__I1>("", "Test", "CallI1", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(-48)]
		[Row(sbyte.MinValue)]
		[Row(sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI1(sbyte value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI1", "sbyte", value.ToString());
			Assert.IsTrue((bool)Run<B__I1>("", "Test", "CallConstantI1", value));
		}
		#endregion

		#region U1
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__U1(byte arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(127)]
		[Row(128)]
		[Row(byte.MinValue)]
		[Row(byte.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU1(byte value)
		{
			CodeSource = CreateTestCode("CallU1", "byte");
			Assert.IsTrue((bool)Run<B__U1>("", "Test", "CallU1", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(127)]
		[Row(128)]
		[Row(byte.MinValue)]
		[Row(byte.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU1(byte value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU1", "byte", value.ToString());
			Assert.IsTrue((bool)Run<B__U1>("", "Test", "CallConstantU1", value));
		}
		#endregion

		#region I2
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__I2(short arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI2(short value)
		{
			CodeSource = CreateTestCode("CallI2", "short");
			Assert.IsTrue((bool)Run<B__I2>("", "Test", "CallI2", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(-48)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI2(short value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI2", "short", value.ToString());
			Assert.IsTrue((bool)Run<B__I2>("", "Test", "CallConstantI2", value));
		}
		#endregion

		#region U2
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__U2(ushort arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ushort.MinValue)]
		[Row(ushort.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU2(ushort value)
		{
			CodeSource = CreateTestCode("CallU2", "ushort");
			Assert.IsTrue((bool)Run<B__U2>("", "Test", "CallU2", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ushort.MinValue)]
		[Row(ushort.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU2(ushort value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU2", "ushort", value.ToString());
			Assert.IsTrue((bool)Run<B__U2>("", "Test", "CallConstantU2", value));
		}
		#endregion

		#region I4
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__I4(int arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(int.MinValue)]
		[Row(int.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI4(int value)
		{
			CodeSource = CreateTestCode("CallI4", "int");
			Assert.IsTrue((bool)Run<B__I4>("", "Test", "CallI4", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(-48)]
		[Row(int.MinValue)]
		[Row(int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI4(int value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI4", "int", value.ToString());
			Assert.IsTrue((bool)Run<B__I4>("", "Test", "CallConstantI4", value));
		}
		#endregion

		#region U4
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__U4(uint arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(uint.MinValue)]
		[Row(uint.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU4(uint value)
		{
			CodeSource = CreateTestCode("CallU4", "uint");
			Assert.IsTrue((bool)Run<B__U4>("", "Test", "CallU4", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(uint.MinValue)]
		[Row(uint.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU4(uint value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU4", "uint", value.ToString());
			Assert.IsTrue((bool)Run<B__U4>("", "Test", "CallConstantU4", value));
		}
		#endregion

		#region I8
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__I8(long arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(long.MinValue)]
		[Row(long.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI8(long value)
		{
			CodeSource = CreateTestCode("CallI8", "long");
			Assert.IsTrue((bool)Run<B__I8>("", "Test", "CallI8", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(-48)]
		[Row(long.MinValue)]
		[Row(long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI8(long value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI8", "long", value.ToString());
			Assert.IsTrue((bool)Run<B__I8>("", "Test", "CallConstantI8", value));
		}
		#endregion

		#region U8
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		delegate bool B__U8(ulong arg);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ulong.MinValue)]
		[Row(ulong.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU8(ulong value)
		{
			CodeSource = CreateTestCode("CallU8", "ulong");
			Assert.IsTrue((bool)Run<B__U8>("", "Test", "CallU8", value));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ulong.MinValue)]
		[Row(ulong.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU8(ulong value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU8", "ulong", value.ToString());
			Assert.IsTrue((bool)Run<B__U8>("", "Test", "CallConstantU8", value));
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		private delegate bool B_I4_I4_I4_I4(int a, int b, int c, int d);

		/// <summary>
		/// Checks the method call parameter order.
		/// </summary>
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void CallOrderI4()
		{
			CodeSource = @"
                static class Test {
                    static bool CallOrderI4(int a, int b, int c, int d) {
                        return (a == 1 && b == 2 && c == 3 && d == 4);
                    }
                }
            ";

			Assert.IsTrue((bool)Run<B_I4_I4_I4_I4>(@"", @"Test", @"CallOrderI4", 1, 2, 3, 4));
		}

		/// <summary>
		/// 
		/// </summary>
		private delegate bool B_U8_U8_U8_U8(ulong a, ulong b, ulong c, ulong d);

		/// <summary>
		/// Checks the method call parameter order.
		/// </summary>
		[Test, Author(@"tgiphil", @"phil@thinkedge.com")]
		public void CallOrderU8()
		{
			CodeSource = @"
                static class Test {
                    static bool CallOrderU8(ulong a, ulong b, ulong c, ulong d) {
                        return (a == 1 && b == 2 && c == 3 && d == 4);
                    }
                }
            ";

			Assert.IsTrue((bool)Run<B_U8_U8_U8_U8>(@"", @"Test", @"CallOrderU8", (ulong)1, (ulong)2, (ulong)3, (ulong)4));
		}

		/// <summary>
		/// 
		/// </summary>
		private delegate bool B_U4_U8_U8_U8(uint a, ulong b, ulong c, ulong d);

		/// <summary>
		/// Checks the method call parameter order.
		/// </summary>
		[Test, Author(@"tgiphil", @"phil@thinkedge.com")]
		public void CallOrderU4_U8_U8_U8()
		{
			CodeSource = @"
                static class Test {
                    static bool CallOrderU4_U8_U8_U8(uint a, ulong b, ulong c, ulong d) {
                        return (a == 1 && b == 2 && c == 3 && d == 4);
                    }
                }
            ";

			Assert.IsTrue((bool)Run<B_U4_U8_U8_U8>(@"", @"Test", @"CallOrderU4_U8_U8_U8", (uint)1, (ulong)2, (ulong)3, (ulong)4));
		}
		/// <summary>
		/// 
		/// </summary>
		private delegate bool B_I4(int arg);

		/// <summary>
		/// Tests intrinsic compiler calls.
		/// </summary>
		[Column(1, 2, Int32.MaxValue, Int32.MinValue)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void CallIntrinsic(int arg)
		{
			CodeSource = @"
                static class Test {
                    static bool CallIntrinsic(int arg) {
                        return (arg == CallMov(arg));
                    }

                    [Mosa.Runtime.CompilerFramework.IntrinsicAttribute(typeof(Mosa.Platforms.x86.Architecture), typeof(Mosa.Platforms.x86.CPUx86.MovInstruction))]
                    static int CallMov(int arg) {
                        return 0;
                    }
                }
            ";

			this.References = new string[] {
                @"Mosa.Runtime.dll",
                @"Mosa.Platforms.x86.dll"
            };
			Assert.IsTrue((bool)Run<B_I4>(@"", @"Test", @"CallIntrinsic", arg));
		}
	}
}
