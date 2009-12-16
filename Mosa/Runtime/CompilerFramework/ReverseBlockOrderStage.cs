/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This class orders blocks in reverse order. This stage is used for testing.
	/// </summary>
	public class ReverseBlockOrderStage : BaseStage, IMethodCompilerStage, IPipelineStage, IBlockReorderStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"ReverseBlockOrderStage"; }
		}

		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(StackLayoutStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IR.CILTransformationStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IPlatformTransformationStage)),				
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IBlockOptimizationStage)),				
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(CodeGenerationStage))
			};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		#endregion // IPipelineStage Methods

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		public void Run()
		{
			for (int i = 1; i <= BasicBlocks.Count / 2; i++) {
				BasicBlock temp = BasicBlocks[i];
				BasicBlocks[i] = BasicBlocks[BasicBlocks.Count - i];
				BasicBlocks[BasicBlocks.Count - i] = temp;
			}
		}
	}
}
