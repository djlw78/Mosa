﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the mul instruction.
    /// </summary>
    public sealed class MulInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MulInstruction"/> class.
        /// </summary>
        public MulInstruction()
        {
        }

        #endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 4; } }

		#endregion // Properties

        #region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Mul(context);
		}

        #endregion // Methods
    }
}
