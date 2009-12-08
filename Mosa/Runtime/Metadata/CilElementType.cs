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
    public enum CilElementType
    {
        /// <summary>
        /// Marks the end of the list
        /// </summary>
        End = 0x0,

        /// <summary>
        /// 
        /// </summary>
        Void = 0x1,
        /// <summary>
        /// 
        /// </summary>
        Boolean = 0x2,
        /// <summary>
        /// 
        /// </summary>
        Char = 0x3,
        /// <summary>
        /// 
        /// </summary>
        I1 = 0x4,
        /// <summary>
        /// 
        /// </summary>
        U1 = 0x5,
        /// <summary>
        /// 
        /// </summary>
        I2 = 0x6,
        /// <summary>
        /// 
        /// </summary>
        U2 = 0x7,
        /// <summary>
        /// 
        /// </summary>
        I4 = 0x8,
        /// <summary>
        /// 
        /// </summary>
        U4 = 0x9,
        /// <summary>
        /// 
        /// </summary>
        I8 = 0xa,
        /// <summary>
        /// 
        /// </summary>
        U8 = 0xb,
        /// <summary>
        /// 
        /// </summary>
        R4 = 0xc,
        /// <summary>
        /// 
        /// </summary>
        R8 = 0xd,
        /// <summary>
        /// 
        /// </summary>
        String = 0xe,

        /// <summary>
        /// 
        /// </summary>
        Ptr = 0xf,
        /// <summary>
        /// 
        /// </summary>
        ByRef = 0x10,
        /// <summary>
        /// 
        /// </summary>
        ValueType = 0x11,
        /// <summary>
        /// 
        /// </summary>
        Class = 0x12,
        /// <summary>
        /// 
        /// </summary>
        Var = 0x13,
        /// <summary>
        /// 
        /// </summary>
        Array = 0x14,
        // FIXME: Decode the representation
        /// <summary>
        /// 
        /// </summary>
        GenericInst = 0x15,
        /// <summary>
        /// 
        /// </summary>
        TypedByRef = 0x16,

        /// <summary>
        /// 
        /// </summary>
        I = 0x18,
        /// <summary>
        /// 
        /// </summary>
        U = 0x19,

        /// <summary>
        /// 
        /// </summary>
        FunctionPtr = 0x1b,
        /// <summary>
        /// 
        /// </summary>
        Object = 0x1c,
        /// <summary>
        /// 
        /// </summary>
        SZArray = 0x1d,
        /// <summary>
        /// 
        /// </summary>
        MVar = 0x1e,
        /// <summary>
        /// 
        /// </summary>
        Required = 0x1f,
        /// <summary>
        /// 
        /// </summary>
        Optional = 0x20,
        /// <summary>
        /// 
        /// </summary>
        Internal = 0x21,

        /// <summary>
        /// 
        /// </summary>
        Modifier = 0x40,
        /// <summary>
        /// 
        /// </summary>
        Sentinel = 0x41,
        /// <summary>
        /// 
        /// </summary>
        Pinned = 0x45,

        /// <summary>
        /// 
        /// </summary>
        Type = 0x50,
        /// <summary>
        /// 
        /// </summary>
        BoxedObject = 0x51,
        /// <summary>
        /// 
        /// </summary>
        Reserved = 0x52,
        /// <summary>
        /// 
        /// </summary>
        Field = 0x53,
        /// <summary>
        /// 
        /// </summary>
        Property = 0x54,
        /// <summary>
        /// 
        /// </summary>
        Enum = 0x55
    }
}
