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
using System.Text;
using System.Diagnostics;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;


namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 pause instruction.
    /// </summary>
    public sealed class RdtscInstruction : OneOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RdtscInstruction"/> class.
        /// </summary>
        public RdtscInstruction()
        {
        }

        #endregion // Construction

        #region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Rdtsc(context);
		}

        #endregion // Methods
    }
}
