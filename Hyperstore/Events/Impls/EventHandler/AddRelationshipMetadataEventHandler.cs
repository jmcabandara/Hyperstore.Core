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
using System.Collections.Generic;
using Hyperstore.Modeling.Commands;

#endregion

namespace Hyperstore.Modeling.Events
{
    internal class AddRelationshipMetadataEventHandler : IEventHandler<AddSchemaRelationshipEvent>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Enumerates handle in this collection.
        /// </summary>
        /// <exception cref="InvalidElementException">
        ///  Thrown when an Invalid Element error condition occurs.
        /// </exception>
        /// <param name="domainModel">
        ///  The domain model.
        /// </param>
        /// <param name="event">
        ///  The event.
        /// </param>
        /// <returns>
        ///  An enumerator that allows foreach to be used to process handle in this collection.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IEnumerable<IDomainCommand> Handle(IDomainModel domainModel, AddSchemaRelationshipEvent @event)
        {
            Contract.Requires(domainModel, "domainModel");
            Contract.Requires(@event, "@event");

            var metadata = domainModel.Store.GetSchemaRelationship(@event.SchemaRelationshipId);
            if (domainModel.GetRelationship(@event.Id, metadata) == null)
            {
                var start = domainModel.Store.GetSchemaElement(@event.Start);
                if (start == null)
                    throw new InvalidElementException( @event.Start);
                var end = domainModel.Store.GetSchemaElement(@event.End);
                if (end == null)
                    throw new InvalidElementException( @event.End);

                yield return new AddSchemaRelationshipCommand(domainModel as ISchema, @event.Id, metadata, start, end);
            }
        }
    }
}