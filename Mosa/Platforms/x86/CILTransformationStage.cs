/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Transforms CIL instructions into their appropriate 
    /// </summary>
    /// <remarks>
    /// This transformation stage transforms CIL instructions into their equivalent X86 sequences.
    /// </remarks>
    public sealed class CILTransformationStage : BaseTransformationStage, CIL.ICILVisitor, IPipelineStage
    {
        #region IPipelineStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        string IPipelineStage.Name {
            get { return "X86.CILTransformationStage"; }
        }

        private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
            new PipelineStageOrder (PipelineStageOrder.Location.After, typeof(AddressModeConversionStage)),
            new PipelineStageOrder (PipelineStageOrder.Location.Before, typeof(IRTransformationStage))
            //new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IBlockOptimizationStage)),              
            //new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IBlockReorderStage)),           
        };

        /// <summary>
        /// Gets the pipeline stage order.
        /// </summary>
        /// <value>The pipeline stage order.</value>
        PipelineStageOrder[] IPipelineStage.PipelineStageOrder {
            get { return _pipelineOrder; }
        }

        #endregion

        #region ICILVisitor

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Break"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Break (Context ctx)
        {
            ctx.SetInstruction (CPUx86.Instruction.BreakInstruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldarga"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldarga (Context ctx)
        {
            Operand result = ctx.Result;
            ctx.SetInstruction (CPUx86.Instruction.MovInstruction, result, new RegisterOperand (new SigType (CilElementType.Ptr), GeneralPurposeRegister.EBP));
            ctx.AppendInstruction (CPUx86.Instruction.AddInstruction, result, new ConstantOperand (new SigType (CilElementType.Ptr), ctx.Label));
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldloca"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldloca (Context ctx)
        {
            Operand result = ctx.Result;
            ctx.SetInstruction (IR.Instruction.MoveInstruction, result, new RegisterOperand (ctx.Result.Type, GeneralPurposeRegister.EBP));
            ctx.AppendInstruction (CPUx86.Instruction.AddInstruction, result, new ConstantOperand (ctx.Result.Type, ctx.Label));
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Call"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Call (Context ctx)
        {
            HandleInvokeInstruction (ctx);

            return;

            // FIXME PG

            // Move the this pointer to the right place, if this is an object instance
            //RuntimeMethod method = ctx.InvokeTarget;
            //if (method.Signature.HasThis) {
            //    // FIXME PG - 
            //    //_codeEmitter.Mov(new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.Object), GeneralPurposeRegister.ECX), instruction.ThisReference);
            //    //ctx.SetInstruction(CPUx86.Instruction.MoveInstruction, new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.Object), GeneralPurposeRegister.ECX), instruction.ThisReference);

            //    throw new NotImplementedException();
            //}

        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ret"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ret (Context ctx)
        {
            bool eax = false;

            if (ctx.OperandCount == 0 || ctx.Operand1 == null) 
                return;

            Operand retval = ctx.Operand1;
            if (retval.IsRegister)
            {
                // Do not move, if return value is already in EAX
                RegisterOperand rop = (RegisterOperand)retval;
                if (ReferenceEquals (rop.Register, GeneralPurposeRegister.EAX))
                    eax = true;
            }

            if (!eax)
                ctx.SetInstruction (CPUx86.Instruction.MovInstruction, new RegisterOperand (new SigType (CilElementType.I), GeneralPurposeRegister.EAX), retval);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Branch"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Branch (Context ctx)
        {
            ctx.ReplaceInstructionOnly (CPUx86.Instruction.JmpInstruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.UnaryBranch"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.UnaryBranch (Context ctx)
        {
            IBranch branch = ctx.Branch;
            CIL.OpCode opcode = (ctx.Instruction as CIL.ICILInstruction).OpCode;
            Operand op = ctx.Operand1;

            ctx.SetInstruction (CPUx86.Instruction.CmpInstruction, ctx.Operand1, new ConstantOperand (new SigType (CilElementType.I4), 0));

            if (opcode == CIL.OpCode.Brtrue || opcode == CIL.OpCode.Brtrue_s)
                ctx.AppendInstruction (CPUx86.Instruction.BranchInstruction, IR.ConditionCode.Equal);            else if (opcode == CIL.OpCode.Brfalse || opcode == CIL.OpCode.Brfalse_s)
                ctx.AppendInstruction (CPUx86.Instruction.BranchInstruction, IR.ConditionCode.NotEqual);
            else
                throw new NotImplementedException ();

            ctx.SetBranch (branch.Targets[0]);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.BinaryBranch"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.BinaryBranch (Context ctx)
        {
            bool swap = ctx.Operand1 is ConstantOperand;
            IBranch branch = ctx.Branch;
            CIL.OpCode opcode = (ctx.Instruction as CIL.BinaryBranchInstruction).OpCode;
            IR.ConditionCode conditionCode;

            if (swap)
            {
                int tmp = branch.Targets[0];
                branch.Targets[0] = branch.Targets[1];
                branch.Targets[1] = tmp;

                ctx.SetInstruction (CPUx86.Instruction.CmpInstruction, ctx.Operand2, ctx.Operand1);

                switch (opcode) {
                // Signed
                case CIL.OpCode.Beq_s:
                    conditionCode = IR.ConditionCode.NotEqual;
                    break;
                case CIL.OpCode.Bge_s:
                    conditionCode = IR.ConditionCode.LessThan;
                    break;
                case CIL.OpCode.Bgt_s:
                    conditionCode = IR.ConditionCode.LessOrEqual;
                    break;
                case CIL.OpCode.Ble_s:
                    conditionCode = IR.ConditionCode.GreaterThan;
                    break;
                case CIL.OpCode.Blt_s:
                    conditionCode = IR.ConditionCode.GreaterOrEqual;
                    break;

                // Unsigned
                case CIL.OpCode.Bne_un_s:
                    conditionCode = IR.ConditionCode.Equal;
                    break;
                case CIL.OpCode.Bge_un_s:
                    conditionCode = IR.ConditionCode.UnsignedLessThan;
                    break;
                case CIL.OpCode.Bgt_un_s:
                    conditionCode = IR.ConditionCode.UnsignedLessOrEqual;
                    break;
                case CIL.OpCode.Ble_un_s:
                    conditionCode = IR.ConditionCode.UnsignedGreaterThan;
                    break;
                case CIL.OpCode.Blt_un_s:
                    conditionCode = IR.ConditionCode.UnsignedGreaterOrEqual;
                    break;

                // Long form signed
                case CIL.OpCode.Beq:
                    goto case CIL.OpCode.Beq_s;
                case CIL.OpCode.Bge:
                    goto case CIL.OpCode.Bge_s;
                case CIL.OpCode.Bgt:
                    goto case CIL.OpCode.Bgt_s;
                case CIL.OpCode.Ble:
                    goto case CIL.OpCode.Ble_s;
                case CIL.OpCode.Blt:
                    goto case CIL.OpCode.Blt_s;

                // Long form unsigned
                case CIL.OpCode.Bne_un:
                    goto case CIL.OpCode.Bne_un_s;
                case CIL.OpCode.Bge_un:
                    goto case CIL.OpCode.Bge_un_s;
                case CIL.OpCode.Bgt_un:
                    goto case CIL.OpCode.Bgt_un_s;
                case CIL.OpCode.Ble_un:
                    goto case CIL.OpCode.Ble_un_s;
                case CIL.OpCode.Blt_un:
                    goto case CIL.OpCode.Blt_un_s;
                default:

                    throw new NotImplementedException ();
                }
                ctx.AppendInstruction (CPUx86.Instruction.BranchInstruction, conditionCode);
                ctx.SetBranch (branch.Targets[0]);
            }

            else
            {
                ctx.SetInstruction (CPUx86.Instruction.CmpInstruction, ctx.Operand1, ctx.Operand2);

                switch (opcode) {
                // Signed
                case CIL.OpCode.Beq_s:
                    conditionCode = IR.ConditionCode.Equal;
                    break;
                case CIL.OpCode.Bge_s:
                    conditionCode = IR.ConditionCode.GreaterOrEqual;
                    break;
                case CIL.OpCode.Bgt_s:
                    conditionCode = IR.ConditionCode.GreaterThan;
                    break;
                case CIL.OpCode.Ble_s:
                    conditionCode = IR.ConditionCode.LessOrEqual;
                    break;
                case CIL.OpCode.Blt_s:
                    conditionCode = IR.ConditionCode.LessThan;
                    break;

                // Unsigned
                case CIL.OpCode.Bne_un_s:
                    conditionCode = IR.ConditionCode.NotEqual;
                    break;
                case CIL.OpCode.Bge_un_s:
                    conditionCode = IR.ConditionCode.UnsignedGreaterOrEqual;
                    break;
                case CIL.OpCode.Bgt_un_s:
                    conditionCode = IR.ConditionCode.UnsignedGreaterThan;
                    break;
                case CIL.OpCode.Ble_un_s:
                    conditionCode = IR.ConditionCode.UnsignedLessOrEqual;
                    break;
                case CIL.OpCode.Blt_un_s:
                    conditionCode = IR.ConditionCode.UnsignedLessThan;
                    break;

                // Long form signed
                case CIL.OpCode.Beq:
                    goto case CIL.OpCode.Beq_s;
                case CIL.OpCode.Bge:
                    goto case CIL.OpCode.Bge_s;
                case CIL.OpCode.Bgt:
                    goto case CIL.OpCode.Bgt_s;
                case CIL.OpCode.Ble:
                    goto case CIL.OpCode.Ble_s;
                case CIL.OpCode.Blt:
                    goto case CIL.OpCode.Blt_s;

                // Long form unsigned
                case CIL.OpCode.Bne_un:
                    goto case CIL.OpCode.Bne_un_s;
                case CIL.OpCode.Bge_un:
                    goto case CIL.OpCode.Bge_un_s;
                case CIL.OpCode.Bgt_un:
                    goto case CIL.OpCode.Bgt_un_s;
                case CIL.OpCode.Ble_un:
                    goto case CIL.OpCode.Ble_un_s;
                case CIL.OpCode.Blt_un:
                    goto case CIL.OpCode.Blt_un_s;
                default:

                    throw new NotImplementedException ();
                }
                ctx.AppendInstruction (CPUx86.Instruction.BranchInstruction, conditionCode);
                ctx.SetBranch (branch.Targets[0]);
            }

            ctx.AppendInstruction (CPUx86.Instruction.JmpInstruction);
            ctx.SetBranch (branch.Targets[1]);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Switch"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Switch (Context ctx)
        {
            IBranch branch = ctx.Branch;
            Operand operand = ctx.Operand1;

            ctx.SetInstruction (CPUx86.Instruction.NopInstruction);
            for (int i = 0; i < branch.Targets.Length - 1; ++i)
            {
                ctx.AppendInstruction (CPUx86.Instruction.CmpInstruction, operand, new ConstantOperand (new SigType (CilElementType.I), i));
                ctx.AppendInstruction (CPUx86.Instruction.BranchInstruction, IR.ConditionCode.Equal);
                ctx.SetBranch (branch.Targets[i]);
            }
            ctx.AppendInstruction (CPUx86.Instruction.JmpInstruction);
            ctx.SetBranch (branch.Targets[branch.Targets.Length - 1]);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Calli"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Calli (Context ctx)
        {
            HandleInvokeInstruction (ctx);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Callvirt"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Callvirt (Context ctx)
        {
            HandleInvokeInstruction (ctx);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.BinaryComparison"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.BinaryComparison (Context ctx)
        {
            throw new NotSupportedException ();
            //HandleComparisonInstruction(ctx, instruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Add"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Add (Context ctx)
        {
            if (ctx.Operand1.StackType == StackTypeCode.F)
            {
                HandleCommutativeOperation (ctx, CPUx86.Instruction.SseAddInstruction);
                ExtendToR8 (ctx);
            }

            else
                HandleCommutativeOperation (ctx, CPUx86.Instruction.AddInstruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Sub"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Sub (Context ctx)
        {
            if (ctx.Operand1.StackType == StackTypeCode.F)
            {
                HandleNonCommutativeOperation (ctx, CPUx86.Instruction.SseSubInstruction);
                ExtendToR8 (ctx);
            }

            else
                HandleNonCommutativeOperation (ctx, CPUx86.Instruction.SubInstruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Mul"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Mul (Context ctx)
        {
            if (ctx.Operand1.StackType == StackTypeCode.F)
            {
                HandleCommutativeOperation (ctx, CPUx86.Instruction.SseMulInstruction);
                ExtendToR8 (ctx);
            }

            else
                HandleCommutativeOperation (ctx, CPUx86.Instruction.MulInstruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Div"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Div (Context ctx)
        {
            if (IsUnsigned (ctx.Operand1) || IsUnsigned (ctx.Result))
                HandleNonCommutativeOperation (ctx, CPUx86.Instruction.UDivInstruction);            else if (ctx.Operand1.StackType == StackTypeCode.F)
            {
                HandleNonCommutativeOperation (ctx, CPUx86.Instruction.SseDivInstruction);
                ExtendToR8 (ctx);
            }
            else
                HandleNonCommutativeOperation (ctx, CPUx86.Instruction.DivInstruction);
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Rem"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Rem (Context ctx)
        {
            Operand result = ctx.Result;
            Operand operand = ctx.Operand1;
            RegisterOperand eax = new RegisterOperand (new SigType (CilElementType.I4), GeneralPurposeRegister.EAX);
            RegisterOperand ecx = new RegisterOperand (new SigType (CilElementType.I4), GeneralPurposeRegister.ECX);
            RegisterOperand eaxSource = new RegisterOperand (result.Type, GeneralPurposeRegister.EAX);
            RegisterOperand ecxSource = new RegisterOperand (operand.Type, GeneralPurposeRegister.ECX);

            ctx.SetInstruction (CPUx86.Instruction.MovInstruction, eaxSource, result);
            if (IsUnsigned (result))
                ctx.AppendInstruction (IR.Instruction.ZeroExtendedMoveInstruction, eax, eaxSource);
            else
                ctx.AppendInstruction (IR.Instruction.SignExtendedMoveInstruction, eax, eaxSource);

            ctx.AppendInstruction (CPUx86.Instruction.MovInstruction, ecxSource, operand);
            if (IsUnsigned (operand))
                ctx.AppendInstruction (IR.Instruction.ZeroExtendedMoveInstruction, ecx, ecxSource);
            else
                ctx.AppendInstruction (IR.Instruction.SignExtendedMoveInstruction, ecx, ecxSource);

            if (IsUnsigned (result) && IsUnsigned (operand))
                ctx.AppendInstruction (CPUx86.Instruction.UDivInstruction, eax, ecx);
            else
                ctx.AppendInstruction (CPUx86.Instruction.DivInstruction, eax, ecx);

            ctx.AppendInstruction (CPUx86.Instruction.MovInstruction, result, new RegisterOperand (new SigType (CilElementType.I4), GeneralPurposeRegister.EDX));
        }

        #endregion

        #region ICILVisitor - Unused

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldarg"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldarg (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldloc"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldloc (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldc"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldc (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldobj"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldobj (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldstr"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldstr (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldfld"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldfld (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldflda"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldflda (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldsfld"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldsfld (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldsflda"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldsflda (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldftn"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldftn (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldvirtftn"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldvirtftn (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldtoken"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldtoken (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Stloc"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Stloc (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Starg"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Starg (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Stobj"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Stobj (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Stfld"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Stfld (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Stsfld"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Stsfld (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Dup"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Dup (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Pop"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Pop (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Jmp"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Jmp (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.BinaryLogic"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.BinaryLogic (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Shift"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Shift (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Neg"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Neg (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Not"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Not (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Conversion"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Conversion (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Cpobj"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Cpobj (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Newobj"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Newobj (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Castclass"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Castclass (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Isinst"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Isinst (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Unbox"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Unbox (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Throw"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Throw (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Box"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Box (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Newarr"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Newarr (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldlen"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldlen (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldelema"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldelema (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Ldelem"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Ldelem (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Stelem"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Stelem (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.UnboxAny"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.UnboxAny (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Refanyval"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Refanyval (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.UnaryArithmetic"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.UnaryArithmetic (Context ctx)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Mkrefany (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.ArithmeticOverflow"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.ArithmeticOverflow (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Endfinally"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Endfinally (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Leave"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Leave (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Arglist"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Arglist (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Localalloc"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Localalloc (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Endfilter"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Endfilter (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.InitObj"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.InitObj (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Cpblk"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Cpblk (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Initblk"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Initblk (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Prefix"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Prefix (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Rethrow"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Rethrow (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Sizeof"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Sizeof (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Refanytype"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Refanytype (Context ctx)
        {
        }

        /// <summary>
        /// Visitation function for <see cref="CIL.ICILVisitor.Nop"/>.
        /// </summary>
        /// <param name="ctx">The context.</param>
        void CIL.ICILVisitor.Nop (Context ctx)
        {
        }

        #endregion

        #region Internals
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        private static void ExtendToR8 (Context ctx)
        {
            RegisterOperand xmm5 = new RegisterOperand (new SigType (CilElementType.R8), SSE2Register.XMM5);
            RegisterOperand xmm6 = new RegisterOperand (new SigType (CilElementType.R8), SSE2Register.XMM6);
            Context before = ctx.InsertBefore ();

            if (ctx.Result.Type.Type == CilElementType.R4)
            {
                before.SetInstruction (CPUx86.Instruction.Cvtss2sdInstruction, xmm5, ctx.Result);
                ctx.Result = xmm5;
            }

            if (ctx.Operand1.Type.Type == CilElementType.R4)
            {
                before.SetInstruction (CPUx86.Instruction.Cvtss2sdInstruction, xmm6, ctx.Operand1);
                ctx.Operand1 = xmm6;
            }
        }

        /// <summary>
        /// Special handling for commutative operations.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">The instruction.</param>
        /// <remarks>
        /// Commutative operations are reordered by moving the constant to the second operand,
        /// which allows the instruction selection in the code generator to use a instruction
        /// format with an immediate operand.
        /// </remarks>
        private void HandleCommutativeOperation (Context ctx, IInstruction instruction)
        {
            EmitOperandConstants (ctx);
            ctx.ReplaceInstructionOnly (instruction);
        }

        /// <summary>
        /// Handles the non commutative operation.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void HandleNonCommutativeOperation (Context ctx, IInstruction instruction)
        {
            EmitResultConstants (ctx);
            EmitOperandConstants (ctx);
            ctx.ReplaceInstructionOnly (instruction);
        }

        /// <summary>
        /// Special handling for shift operations, which require the shift amount in the ECX or as a constant register.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">The instruction to transform.</param>
        private void HandleShiftOperation (Context ctx, IInstruction instruction)
        {
            EmitOperandConstants (ctx);
        }

        /// <summary>
        /// Processes a method call instruction.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        private void HandleInvokeInstruction (Context ctx)
        {
            ICallingConvention cc = Architecture.GetCallingConvention (ctx.InvokeTarget.Signature.CallingConvention);
            cc.Expand (ctx);
        }

        #endregion
    }
}
