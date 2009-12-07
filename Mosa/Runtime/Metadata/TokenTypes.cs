/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.Metadata
{

    /// <summary>
    /// CIL provider token types according to ISO/IEC 23271:2006 (E).
    /// </summary>
    /// <remarks>
    /// These token types represent the tables in CIL provider. They are
    /// properly formatted to allow or'ing with a provider id to build
    /// a fully qualified provider token identifier.
    /// </remarks>
    [Flags()]
    public enum TokenTypes
    {
        /// <summary>
        /// 
        /// </summary>
        Module = 0x0,
        /// <summary>
        /// 
        /// </summary>
        TypeRef = 0x1000000,
        /// <summary>
        /// 
        /// </summary>
        TypeDef = 0x2000000,
        // 0x03000000 - undefined in ISO/IEC 23271:2006 (E)
        /// <summary>
        /// 
        /// </summary>
        Field = 0x4000000,
        // 0x05000000 - undefined in ISO/IEC 23271:2006 (E)
        /// <summary>
        /// 
        /// </summary>
        MethodDef = 0x6000000,
        // 0x07000000 - undefined in ISO/IEC 23271:2006 (E)
        /// <summary>
        /// 
        /// </summary>
        Param = 0x8000000,
        /// <summary>
        /// 
        /// </summary>
        InterfaceImpl = 0x9000000,
        /// <summary>
        /// 
        /// </summary>
        MemberRef = 0xa000000,
        /// <summary>
        /// 
        /// </summary>
        Constant = 0xb000000,
        /// <summary>
        /// 
        /// </summary>
        CustomAttribute = 0xc000000,
        /// <summary>
        /// 
        /// </summary>
        FieldMarshal = 0xd000000,
        /// <summary>
        /// 
        /// </summary>
        DeclSecurity = 0xe000000,
        /// <summary>
        /// 
        /// </summary>
        ClassLayout = 0xf000000,
        /// <summary>
        /// 
        /// </summary>
        FieldLayout = 0x10000000,
        /// <summary>
        /// 
        /// </summary>
        StandAloneSig = 0x11000000,
        /// <summary>
        /// 
        /// </summary>
        EventMap = 0x12000000,
        // 0x13000000 - undefined in ISO/IEC 23271:2006 (E)
        /// <summary>
        /// 
        /// </summary>
        Event = 0x14000000,
        /// <summary>
        /// 
        /// </summary>
        PropertyMap = 0x15000000,
        // 0x16000000 - undefined in ISO/IEC 23271:2006 (E)
        /// <summary>
        /// 
        /// </summary>
        Property = 0x17000000,
        /// <summary>
        /// 
        /// </summary>
        MethodSemantics = 0x18000000,
        /// <summary>
        /// 
        /// </summary>
        MethodImpl = 0x19000000,
        /// <summary>
        /// 
        /// </summary>
        ModuleRef = 0x1a000000,
        /// <summary>
        /// 
        /// </summary>
        TypeSpec = 0x1b000000,
        /// <summary>
        /// 
        /// </summary>
        ImplMap = 0x1c000000,
        /// <summary>
        /// 
        /// </summary>
        FieldRVA = 0x1d000000,
        // 0x1E000000 - undefined in ISO/IEC 23271:2006 (E)
        // 0x1F000000 - undefined in ISO/IEC 23271:2006 (E)
        /// <summary>
        /// 
        /// </summary>
        Assembly = 0x20000000,
        /// <summary>
        /// 
        /// </summary>
        AssemblyProcessor = 0x21000000,
        /// <summary>
        /// 
        /// </summary>
        AssemblyOS = 0x22000000,
        /// <summary>
        /// 
        /// </summary>
        AssemblyRef = 0x23000000,
        /// <summary>
        /// 
        /// </summary>
        AssemblyRefProcessor = 0x24000000,
        /// <summary>
        /// 
        /// </summary>
        AssemblyRefOS = 0x25000000,
        /// <summary>
        /// 
        /// </summary>
        File = 0x26000000,
        /// <summary>
        /// 
        /// </summary>
        ExportedType = 0x27000000,
        /// <summary>
        /// 
        /// </summary>
        ManifestResource = 0x28000000,
        /// <summary>
        /// 
        /// </summary>
        NestedClass = 0x29000000,
        /// <summary>
        /// 
        /// </summary>
        GenericParam = 0x2a000000,
        /// <summary>
        /// 
        /// </summary>
        MethodSpec = 0x2b000000,
        /// <summary>
        /// 
        /// </summary>
        GenericParamConstraint = 0x2c000000,

        /// <summary>
        /// 
        /// </summary>
        MaxTable = 0x2d000000,

        /// <summary>
        /// Special constant to represent a user string heap token.
        /// </summary>
        UserString = 0x70000000,

        /// <summary>
        /// Special constant to represent a string heap token.
        /// </summary>
        String = 0x71000000,

        /// <summary>
        /// Constant to represent a blob heap token.
        /// </summary>
        Blob = 0x72000000,

        /// <summary>
        /// Constant to represent a guid heap token.
        /// </summary>
        Guid = 0x73000000,

        /// <summary>
        /// Table identification mask.
        /// </summary>
        TableMask = 0x7f000000,
        /// <summary>
        /// 
        /// </summary>
        RowIndexMask = 0xffffff
    }
}
