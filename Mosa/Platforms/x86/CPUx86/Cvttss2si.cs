/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation for the x86 cvtsd2ss instruction.
    /// </summary>
    public class Cvttss2siInstruction : TwoOperandInstruction
    {
        #region Data Members

        private static readonly OpCode opcode = new OpCode (new byte[] {
            0xf3,
            0xf,
            0x2c
        });

        #endregion

        #region Methods

        /// <summary>
        /// Computes the opcode.
        /// </summary>
        /// <param name="destination">The destination operand.</param>
        /// <param name="source">The source operand.</param>
        /// <param name="third">The third operand.</param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode (Operand destination, Operand source, Operand third)
        {
            return opcode;
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit (IX86Visitor visitor, Context context)
        {
            visitor.Cvttss2si (context);
        }

        #endregion
    }
}
