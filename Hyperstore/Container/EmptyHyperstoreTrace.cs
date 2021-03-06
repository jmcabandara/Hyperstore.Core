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
 
#region Imports

using System;
using System.Linq;

#endregion

namespace Hyperstore.Modeling.Container
{
    internal sealed class EmptyHyperstoreTrace : IHyperstoreTrace
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Writes the trace.
        /// </summary>
        /// <param name="category">
        ///  The category.
        /// </param>
        /// <param name="format">
        ///  Describes the format to use.
        /// </param>
        /// <param name="args">
        ///  A variable-length parameters list containing arguments.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void WriteTrace(string category, string format, params object[] args)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Query if 'category' is enabled.
        /// </summary>
        /// <param name="category">
        ///  The category.
        /// </param>
        /// <returns>
        ///  true if enabled, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool IsEnabled(string category)
        {
            return false;
        }
    }
}