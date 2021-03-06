/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	/// Selector proxy type for the register allocator.
	/// 
	/// TODO: put this selector stage somewhere in the actual pipeline.
	/// </summary>
	public class RegisterAllocatorSelector : BaseStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// The linear register allocator.
		/// </summary>
		private IMethodCompilerStage linearRegisterAllocator;

		/// <summary>
		/// The stack register allocator.
		/// </summary>
		private IMethodCompilerStage stackRegisterAllocator;

		/// <summary>
		/// Holds the real stage implementation to use.
		/// </summary>
		private IMethodCompilerStage implementation;

		/// <summary>
		/// Initializes a new instance of the ArchitectureSelector class.
		/// </summary>
		public RegisterAllocatorSelector()
		{
			this.linearRegisterAllocator = new LinearRegisterAllocator();
			this.stackRegisterAllocator = new SimpleRegisterAllocator();
			this.implementation = this.linearRegisterAllocator;
		}

		private IMethodCompilerStage SelectImplementation(string name)
		{
			switch (name.ToLower()) {
				case "linear":
					return linearRegisterAllocator;
				case "stack":
					return stackRegisterAllocator;
				default:
					throw new OptionException(String.Format("Unknown or unsupported register allocator type {0}.", name), "reg-alloc");
			}
		}

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"reg-alloc=",
				"Select the register allocator to use (default is linear) [{linear|stack}].",
				delegate(string name)
				{
					this.implementation = SelectImplementation(name);
				}
			);

			// when the allocators are configurable with options:
			// linearRegisterAllocator.AddOptions(optionSet);
			// stackRegisterAllocator.AddOptions(optionSet);
		}

		/// <summary>
		/// Gets a value indicating whether an implementation is selected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if an implementation is selected; otherwise, <c>false</c>.
		/// </value>
		public bool IsConfigured
		{
			get { return (this.implementation != null); }
		}

		/// <summary>
		/// Checks if an implementation is set.
		/// </summary>
		private void CheckImplementation()
		{
			if (this.implementation == null)
				throw new InvalidOperationException(@"RegisterFormatSelector not initialized.");
		}

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get
			{
				CheckImplementation();
				return ((IPipelineStage)implementation).Name;
			}
		}

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder
		{
			get
			{
				CheckImplementation();
				return ((IPipelineStage)implementation).PipelineStageOrder;
			}
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			CheckImplementation();
			implementation.Setup(MethodCompiler);
			implementation.Run();
		}

	}
}
