/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata
{
    /// <summary>
    /// 
    /// </summary>
    public enum GenericParamAttributes
    {
        /// <summary>
        /// 
        /// </summary>
        VarianceMask = 0x3,
        /// <summary>
        /// 
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 
        /// </summary>
        Covariant = 0x1,
        /// <summary>
        /// 
        /// </summary>
        Contravariant = 0x2,
        /// <summary>
        /// 
        /// </summary>
        SpecialConstraintMask = 0x1c,
        /// <summary>
        /// 
        /// </summary>
        ReferenceTypeConstraint = 0x4,
        /// <summary>
        /// 
        /// </summary>
        NotNullableValueTypeConstraint = 0x8,
        /// <summary>
        /// 
        /// </summary>
        DefaultConstructorConstraint = 0x10
    }
}
