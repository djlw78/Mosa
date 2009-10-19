﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of the unsigned division operation.
    /// </summary>
    public sealed class UDivInstruction : ThreeOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="UDivInstruction"/> class.
        /// </summary>
        public UDivInstruction()
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR.udiv {0}, {1}, {2} ; {0} = {1} / {2}", context.Operand1, context.Operand2, context.Operand3);
        }

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.UDivInstruction(context);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
