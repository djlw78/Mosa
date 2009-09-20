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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class MkrefanyInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MkrefanyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public MkrefanyInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

			// Retrieve a type reference From the immediate argument
			// FIXME: Limit the token types
			TokenTypes token;
			decoder.Decode(out token);
			throw new NotImplementedException();
			/*
				_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
				_results[0] = CreateResultOperand(MetadataTypeReference.FromName(decoder.Metadata, @"System", @"TypedReference"));
			 */
			// FIXME: Validate the operands
			// FIXME: Do verification
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Mkrefany(context);
		}

		#endregion Methods

	}
}
