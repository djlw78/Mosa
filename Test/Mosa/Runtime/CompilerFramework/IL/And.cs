﻿using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class And : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        // Normal Testcases + (0, 0)
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(sbyte.MinValue, 0)]
        [Row(sbyte.MinValue, 1)]
        [Row(sbyte.MinValue, 17)]
        [Row(sbyte.MinValue, 123)]
        [Row(sbyte.MinValue, -0)]
        [Row(sbyte.MinValue, -1)]
        [Row(sbyte.MinValue, -17)]
        [Row(sbyte.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(sbyte.MaxValue, 0)]
        [Row(sbyte.MaxValue, 1)]
        [Row(sbyte.MaxValue, 17)]
        [Row(sbyte.MaxValue, 123)]
        [Row(sbyte.MaxValue, -0)]
        [Row(sbyte.MaxValue, -1)]
        [Row(sbyte.MaxValue, -17)]
        [Row(sbyte.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, sbyte.MinValue)]
        [Row(1, sbyte.MinValue)]
        [Row(17, sbyte.MinValue)]
        [Row(123, sbyte.MinValue)]
        [Row(-0, sbyte.MinValue)]
        [Row(-1, sbyte.MinValue)]
        [Row(-17, sbyte.MinValue)]
        [Row(-123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, sbyte.MaxValue)]
        [Row(1, sbyte.MaxValue)]
        [Row(17, sbyte.MaxValue)]
        [Row(123, sbyte.MaxValue)]
        [Row(-0, sbyte.MaxValue)]
        [Row(-1, sbyte.MaxValue)]
        [Row(-17, sbyte.MaxValue)]
        [Row(-123, sbyte.MaxValue)]
        // Extremvaluecases
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Row(sbyte.MaxValue, sbyte.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool AndI1(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "AndI1", (a & b), a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, short b);
        // Normal Testcases + (0, 0)
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(short.MinValue, 0)]
        [Row(short.MinValue, 1)]
        [Row(short.MinValue, 17)]
        [Row(short.MinValue, 123)]
        [Row(short.MinValue, -0)]
        [Row(short.MinValue, -1)]
        [Row(short.MinValue, -17)]
        [Row(short.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(short.MaxValue, 0)]
        [Row(short.MaxValue, 1)]
        [Row(short.MaxValue, 17)]
        [Row(short.MaxValue, 123)]
        [Row(short.MaxValue, -0)]
        [Row(short.MaxValue, -1)]
        [Row(short.MaxValue, -17)]
        [Row(short.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, short.MinValue)]
        [Row(1, short.MinValue)]
        [Row(17, short.MinValue)]
        [Row(123, short.MinValue)]
        [Row(-0, short.MinValue)]
        [Row(-1, short.MinValue)]
        [Row(-17, short.MinValue)]
        [Row(-123, short.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, short.MaxValue)]
        [Row(1, short.MaxValue)]
        [Row(17, short.MaxValue)]
        [Row(123, short.MaxValue)]
        [Row(-0, short.MaxValue)]
        [Row(-1, short.MaxValue)]
        [Row(-17, short.MaxValue)]
        [Row(-123, short.MaxValue)]
        // Extremvaluecases
        [Row(short.MinValue, short.MaxValue)]
        [Row(short.MaxValue, short.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI2(short a, short b)
        {
            CodeSource = "static class Test { static bool AndI2(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "AndI2", (a & b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, int b);
        // Normal Testcases + (0, 0)
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(int.MinValue, 0)]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0)]
        [Row(int.MinValue, -1)]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0)]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0)]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Row(int.MaxValue, int.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI4(int a, int b)
        {
            CodeSource = "static class Test { static bool AndI4(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AndI4", (a & b), a, b));
        }

        delegate bool I8_I8_I8(long expect, long a, long b);
        // Normal Testcases + (0, 0)
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0)]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0)]
        [Row(long.MinValue, -1)]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0)]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0)]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        // Extremvaluecases
        [Row(long.MinValue, long.MaxValue)]
        [Row(long.MaxValue, long.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI8(long a, long b)
        {
            CodeSource = "static class Test { static bool AndI8(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "AndI8", (a & b), a, b));
        }
    }
}
