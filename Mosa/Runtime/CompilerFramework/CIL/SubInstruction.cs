﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SubInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SubInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public SubInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods 
		
		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Sub(context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context ctx)
		{
			return String.Format("{0} ; {1} = {2} - {3}", base.ToString(), ctx.Result, ctx.Operand1, ctx.Operand2);
		}

		#endregion // Methods 

	}
}
