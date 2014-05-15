﻿// Copyright 2014 Zenasoft.  All rights reserved.
//
// This file is part of Hyperstore.
//
//    Hyperstore is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Hyperstore is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Hyperstore.  If not, see <http://www.gnu.org/licenses/>.

#region Imports

using System;

#endregion

namespace Hyperstore.Modeling.Metadata.Primitives
{
    internal class BytePrimitive : PrimitiveMetaValue
    {
        protected BytePrimitive()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="domainModel">
        ///  The domain model.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public BytePrimitive(ISchema domainModel)
            : base(domainModel, typeof(Byte))
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  override this instance to the given stream.
        /// </summary>
        /// <param name="ctx">
        ///  The context.
        /// </param>
        /// <returns>
        ///  An object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override object Deserialize(SerializationContext ctx)
        {
            DebugContract.Requires(ctx);

            if (ctx.Value == null)
                return false;

            if (ctx.Value is Byte)
                return ctx.Value;

            return Convert.FromBase64String((string)ctx.Value)[0];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  override this instance to the given stream.
        /// </summary>
        /// <param name="data">
        ///  The data.
        /// </param>
        /// <returns>
        ///  A string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override string Serialize(object data)
        {
            if (data == null)
                return null;

            var bytes = new[] { (Byte)data };
            return Convert.ToBase64String(bytes);
        }
    }
}