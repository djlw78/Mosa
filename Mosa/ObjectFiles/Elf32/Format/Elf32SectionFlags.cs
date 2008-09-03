﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    enum Elf32SectionFlags : uint
    {
        /// <summary></summary>
        SHF_NONE = 0x0,
        ///<summary></summary>
        SHF_WRITE = 0x1,
        ///<summary></summary>
        SHF_ALLOC = 0x2,
        ///<summary></summary>
        SHF_EXECINSTR = 0x4,
        ///<summary></summary>
        SHF_MASKPROC = 0xf0000000,
    }
}