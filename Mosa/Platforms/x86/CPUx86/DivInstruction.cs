﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the div instruction.
    /// </summary>
    public sealed class DivInstruction : TwoOperandInstruction
    {
        #region Data Member
        private static readonly OpCode R = new OpCode(new byte[] { 0xF7 }, 7);
        private static readonly OpCode M = new OpCode(new byte[] { 0xF7 }, 7);
        #endregion 

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 22; } }

		#endregion // Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="thirdOperand"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand thirdOperand)
        {
            if (dest is RegisterOperand) return R;
            if (dest is MemoryOperand) return M;
            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86.idiv {0}, {1} ; {0} /= {1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Div(context);
		}

        #endregion // Methods
    }
}
