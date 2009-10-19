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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
    /// <summary>
    /// 
    /// </summary>
	public struct TypeSpecRow
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _signatureBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSpecRow"/> struct.
        /// </summary>
        /// <param name="signatureBlobIdx">The signature BLOB idx.</param>
        public TypeSpecRow(TokenTypes signatureBlobIdx)
        {
            _signatureBlobIdx = signatureBlobIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the signature BLOB idx.
        /// </summary>
        /// <value>The signature BLOB idx.</value>
        public TokenTypes SignatureBlobIdx
        {
            get { return _signatureBlobIdx; }
        }

        #endregion // Properties
	}
}
