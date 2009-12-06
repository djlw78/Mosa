/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Specifies flow-control properties of an instruction.
    /// </summary>
    public enum FlowControl
    {
        /// <summary>
        /// The instruction always continues execution on the next instruction.
        /// </summary>
        Next = 0x0,

        /// <summary>
        /// The instruction invokes another method.
        /// </summary>
        Call = 0x1,

        /// <summary>
        /// The instruction is an unconditional branch.
        /// </summary>
        Branch = 0x2,

        /// <summary>
        /// The instruction is a conditional branch, which never falls through.
        /// </summary>
        ConditionalBranch = 0x4,

        /// <summary>
        /// The instruction is a conditional branch, which may fall-through.
        /// </summary>
        Switch = 0x8,

        /// <summary>
        /// The instruction breaks the control-flow.
        /// </summary>
        Break = 0x10,

        /// <summary>
        /// The instruction returns From the method.
        /// </summary>
        Return = 0x20,

        /// <summary>
        /// The instruction throws an exception.
        /// </summary>
        Throw = 0x40
    }
}
