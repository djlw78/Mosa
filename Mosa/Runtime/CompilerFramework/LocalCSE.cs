/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// A stage to compute local common subexpression elimination
    /// according to Steven S. Muchnick, Advanced Compiler Design 
    /// and Implementation (Morgan Kaufmann, 1997) pp. 378-396
    /// </summary>
    public class LocalCSE : BaseStage, IMethodCompilerStage, IPipelineStage
    {
        /// <summary>
        /// 
        /// </summary>
        struct AEBinExp
        {
            /// <summary>
            /// 
            /// </summary>
            public readonly int Position;
            /// <summary>
            /// 
            /// </summary>
            public readonly Operand Operand1;
            /// <summary>
            /// 
            /// </summary>
            public readonly Operation Operator;
            /// <summary>
            /// 
            /// </summary>
            public readonly Operand Operand2;
            /// <summary>
            /// 
            /// </summary>
            public readonly Operand Var;

            /// <summary>
            /// Initializes a new instance of the <see cref="AEBinExp"/> struct.
            /// </summary>
            /// <param name="pos">The pos.</param>
            /// <param name="op1">The op1.</param>
            /// <param name="opr">The opr.</param>
            /// <param name="op2">The op2.</param>
            /// <param name="var">The var.</param>
            public AEBinExp (int pos, Operand op1, Operation opr, Operand op2, Operand var)
            {
                Position = pos;
                Operand1 = op1;
                Operator = opr;
                Operand2 = op2;
                Var = var;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        enum Operation
        {
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            Add,
            /// <summary>
            /// 
            /// </summary>
            Mul,
            /// <summary>
            /// 
            /// </summary>
            And,
            /// <summary>
            /// 
            /// </summary>
            Or,
            /// <summary>
            /// 
            /// </summary>
            Xor
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        string IPipelineStage.Name {
            get { return "Local Common Subexpression Elimination Stage"; }
        }

        private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
            
            // TODO
        };

        /// <summary>
        /// Gets the pipeline stage order.
        /// </summary>
        /// <value>The pipeline stage order.</value>
        PipelineStageOrder[] IPipelineStage.PipelineStageOrder {
            get { return _pipelineOrder; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public void Run ()
        {
            foreach (BasicBlock block in BasicBlocks)
                EliminateCommonSubexpressions (new Context (InstructionSet, block));
        }

        /// <summary>
        /// Eliminates the common subexpressions.
        /// </summary>
        /// <param name="ctx">The context.</param>
        private static void EliminateCommonSubexpressions (Context ctx)
        {
            List<AEBinExp> AEB = new List<AEBinExp> ();
            List<AEBinExp> tmp;

            AEBinExp aeb;

            for (; !ctx.EndOfInstruction; ctx.GotoNext ())
            {
                IInstruction instruction = ctx.Instruction;
                // block.Instructions[i];
                RegisterOperand temp = null;
                bool found = false;

                if ((instruction is CIL.ArithmeticInstruction) && (instruction is CIL.BinaryInstruction))
                {
                    tmp = new List<AEBinExp> (AEB);

                    while (tmp.Count > 0)
                    {
                        aeb = tmp[0];
                        tmp.RemoveAt (0);

                        // Match current instruction's expression against those
                        // in AEB, including commutativity
                        if (IsCommutative (instruction))
                        {
                            //int position = aeb.Position;
                            found = true;

                            // If no variable in tuple, create a new temporary and
                            // insert an instruction evaluating the expression
                            // and assigning it to the temporary
                            if (aeb.Var == null)
                            {
                                // new_tmp()
                                AEB.Remove (aeb);
                                AEB.Add (new AEBinExp (aeb.Position, aeb.Operand1, aeb.Operator, aeb.Operand2, temp));

                                // Insert new assignment to instruction stream in block
                                Context inserted = ctx.InsertBefore ();

                                switch (aeb.Operator) {
                                case Operation.Add:
                                    inserted.SetInstruction (CIL.Instruction.Get (CIL.OpCode.Add), temp, aeb.Operand1, aeb.Operand2);
                                    break;
                                case Operation.Mul:
                                    inserted.SetInstruction (CIL.Instruction.Get (CIL.OpCode.Mul), temp, aeb.Operand1, aeb.Operand2);
                                    break;
                                case Operation.Or:
                                    inserted.SetInstruction (CIL.Instruction.Get (CIL.OpCode.Or), temp, aeb.Operand1, aeb.Operand2);
                                    break;
                                case Operation.Xor:
                                    inserted.SetInstruction (CIL.Instruction.Get (CIL.OpCode.Xor), temp, aeb.Operand1, aeb.Operand2);
                                    break;
                                default:
                                    break;
                                }

                                //block.Instructions.Insert(position, inst);

                                //++position;
                                //++i;

                                // Replace current instruction by one that copies
                                // the temporary instruction
                                // FIXME PG:
                                // block.Instructions[position] = new IR.MoveInstruction(block.Instructions[position].Results[0], temp);
                                // ctx.SetInstruction(IR.MoveInstruction); // FIXME PG
                                // ctx.Result = block.Instructions[position].Results[0]; // FIXME PG
                                ctx.Operand1 = temp;
                            } else
                            {
                                temp = (RegisterOperand)aeb.Var;
                            }

                            // FIXME PG
                            // block.Instructions[i] = new IR.MoveInstruction(instruction.Results[0], temp);
                        }
                    }

                    if (!found)
                    {
                        Operation opr = Operation.None;

                        if (instruction is CIL.AddInstruction)
                            opr = Operation.Add; else if (instruction is CIL.MulInstruction)
                            opr = Operation.Mul; else if (instruction is IR.LogicalAndInstruction)
                            opr = Operation.And;
                        // Insert new tuple
                        AEB.Add (new AEBinExp (ctx.Index, ctx.Operand1, opr, ctx.Operand2, null));
                    }

                    // Remove all tuples that use the variable assigned to by
                    // the current instruction
                    tmp = new List<AEBinExp> (AEB);

                    while (tmp.Count > 0)
                    {
                        aeb = tmp[0];
                        tmp.RemoveAt (0);

                        if (ctx.Operand1 == aeb.Operand1 || ctx.Operand2 == aeb.Operand2)
                            AEB.Remove (aeb);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified instruction is commutative.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns>
        ///     <c>true</c> if the specified instruction is commutative; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsCommutative (IInstruction instruction)
        {
            return (instruction is CIL.AddInstruction) || (instruction is CIL.MulInstruction) || (instruction is IR.LogicalAndInstruction) || (instruction is IR.LogicalOrInstruction) || (instruction is IR.LogicalXorInstruction);
        }
    }
}
