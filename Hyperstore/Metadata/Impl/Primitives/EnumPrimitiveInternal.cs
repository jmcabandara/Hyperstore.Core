﻿//	Copyright © 2013 - 2014, Alain Metge. All rights reserved.
//
//		This file is part of Hyperstore (http://www.hyperstore.org)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Modeling.Metadata.Primitives
{
    internal class EnumPrimitiveInternal : PrimitiveMetaValue
    {
        protected EnumPrimitiveInternal()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="schema">
        ///  The schema.
        /// </param>
        /// <param name="tenum">
        ///  The tenum.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public EnumPrimitiveInternal(ISchema schema, Type tenum)
            : base(schema, tenum)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Serializes the specified data.
        /// </summary>
        /// <param name="mel">
        ///  The data.
        /// </param>
        /// <param name="serializer">
        ///  The serializer.
        /// </param>
        /// <returns>
        ///  A string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override string Serialize(object mel, IJsonSerializer serializer)
        {
            return mel != null ? ((Enum)mel).ToString("F") : null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Deserializes an element from the specified context.
        /// </summary>
        /// <param name="ctx">
        ///  Serialization context.
        /// </param>
        /// <returns>
        ///  An object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override object Deserialize(SerializationContext ctx)
        {
            DebugContract.Requires(ctx);
            if (ctx.Value == null)
                return DefaultValue;

            if (ctx.Value.GetType() == ImplementedType)
                return ctx.Value;

            return Enum.Parse(ImplementedType, ctx.Value.ToString());
        }
    }
}
