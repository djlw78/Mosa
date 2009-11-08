/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Container class used to define the pipeline of a compiler.
	/// </summary>
	public sealed class CompilerPipeline : IEnumerable
	{
		#region Data members

		/// <summary>
		/// Holds the current stage of execution of the pipeline.
		/// </summary>
		private int _currentStage = -1;

		/// <summary>
		/// The stages in the compiler pipeline.
		/// </summary>
		private List<IPipelineStage> _pipeline;

		bool _ordered;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerPipeline"/> class.
		/// </summary>
		public CompilerPipeline()
		{
			_pipeline = new List<IPipelineStage>();
			_ordered = true;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the number of stages in the compiler pipeline.
		/// </summary>
		public int Count
		{
			get { return _pipeline.Count; }
		}

		/// <summary>
		/// Retrieves the index of the current stage of execution.
		/// </summary>
		public int CurrentStage
		{
			get { return _currentStage; }
		}

		/// <summary>
		/// Retrieves the indexed compilation stage.
		/// </summary>
		/// <param name="index">The index of the compilation stage to return.</param>
		/// <returns>The compilation stage at the requested index.</returns>
		public IPipelineStage this[int index]
		{
			get { return _pipeline[index]; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Adds the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Add(IPipelineStage stage)
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			_pipeline.Add(stage);
			_ordered = false;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="stages">The stages.</param>
		public void AddRange(IEnumerable stages)
		{
			if (stages == null)
				throw new ArgumentNullException(@"stages");

			foreach (IPipelineStage stage in stages)
				Add(stage);
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_pipeline.Clear();
			_ordered = true;
		}

		/// <summary>
		/// Removes the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Remove(IPipelineStage stage)
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			_pipeline.Remove(stage);
			_ordered = false;
		}

		/// <summary>
		/// Executes the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		public void Execute<T>(Action<T> action)
		{
			if (!_ordered) OrderPipeline();

			_currentStage = 0;
			foreach (T stage in _pipeline) {
				action(stage);
				_currentStage++;
			}
		}

		#endregion // Methods

		#region IEnumerable members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			if (!_ordered) OrderPipeline();

			return _pipeline.GetEnumerator();
		}

		#endregion // IEnumerable members

		/// <summary>
		/// Finds this instance.
		/// </summary>
		/// <typeparam name="StageType">The type of the tage type.</typeparam>
		/// <returns></returns>
		public StageType Find<StageType>() where StageType : class
		{
			StageType result = default(StageType);
			foreach (object o in _pipeline) {
				result = o as StageType;
				if (result != null)
					break;
			}

			return result;
		}


		/// <summary>
		/// Finds the last.
		/// </summary>
		/// <param name="stageOrders">The stage orders.</param>
		/// <param name="first">The first.</param>
		/// <param name="last">The last.</param>
		private void Between(PipelineStageOrder[] stageOrders, ref int first, ref int last)
		{
			first = Int32.MinValue;
			last = Int32.MaxValue;

			if (stageOrders == null || stageOrders.Length == 0)
				return;

			for (int i = _pipeline.Count - 1; i >= 0; i--)
				foreach (PipelineStageOrder order in stageOrders)
					if (order.StageType == _pipeline[i].GetType())
						if (order.Position == PipelineStageOrder.Location.After)
							first = Math.Max(first, i);
						else
							if (order.Position == PipelineStageOrder.Location.Before)
								last = Math.Min(last, i);

			return;
		}

		/// <summary>
		/// Orders the pipeline.
		/// </summary>
		/// <returns></returns>
		private bool OrderPipeline()
		{
			int loops = 0;
			bool changed = true;

			while (changed) {
				changed = false;
				loops++;

				for (int i = 0; i < _pipeline.Count; i++) {
					IPipelineStage stage = _pipeline[i];
					PipelineStageOrder[] order = stage.PipelineStageOrder;

					int first = -1;
					int last = -1;

					Between(order, ref first, ref last);

					if (i > first && i < last)
						continue;

					_pipeline.RemoveAt(i);

					if (i > last)
						_pipeline.Insert(last + 1, stage);
					else if (i < first)
						_pipeline.Insert(first, stage);

					changed = true;
				}

				Debug.Assert(loops < 1000, @"impossible ordering of stages");
			}

			_ordered = true;

			return true;
		}

	}
}
