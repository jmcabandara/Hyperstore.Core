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
using Hyperstore.Modeling.Domain;
using Hyperstore.Modeling.HyperGraph;

#endregion

namespace Hyperstore.Modeling
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  A schema definition.
    /// </summary>
    /// <seealso cref="T:Hyperstore.Modeling.ISchemaDefinition"/>
    ///-------------------------------------------------------------------------------------------------
    public abstract class SchemaDefinition : DomainConfiguration, ISchemaDefinition
    {
        private readonly string _name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the behavior.
        /// </summary>
        /// <value>
        ///  The behavior.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DomainBehavior Behavior { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="name">
        ///  The name.
        /// </param>
        /// <param name="behavior">
        ///  (Optional) the behavior.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected SchemaDefinition(string name, DomainBehavior behavior = DomainBehavior.Standard)
        {
            Contract.Requires(name, "name");
            Conventions.CheckValidDomainName(name);

            _name = name;
            Behavior = behavior;
            if ((Behavior & DomainBehavior.Observable) == DomainBehavior.Observable)
            {
                Behavior &= ~DomainBehavior.DisableL1Cache;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the name of the schema.
        /// </summary>
        /// <value>
        ///  The name of the schema.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string ISchemaDefinition.SchemaName { get { return _name; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Called when [schema loaded].
        /// </summary>
        /// <param name="schema">
        ///  The schema.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void ISchemaDefinition.OnSchemaLoaded(ISchema schema)
        {
            DebugContract.Requires(schema);
            OnSchemaLoaded(schema);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Executes the schema loaded action.
        /// </summary>
        /// <param name="schema">
        ///  The schema.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void OnSchemaLoaded(ISchema schema)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Defines the schema.
        /// </summary>
        /// <param name="schema">
        ///  The schema.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void ISchemaDefinition.DefineSchema(ISchema schema)
        {
            DebugContract.Requires(schema);
            DefineSchema(schema);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Defines the schema.
        /// </summary>
        /// <param name="schema">
        ///  The schema.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected abstract void DefineSchema(ISchema schema);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Creates a schema.
        /// </summary>
        /// <param name="services">
        ///  The services.
        /// </param>
        /// <returns>
        ///  The new schema.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        ISchema ISchemaDefinition.CreateSchema(IServicesContainer services)
        {
            DebugContract.Requires(services);
            return CreateSchema(services);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Schema factory. Create a new schema instance
        /// </summary>
        /// <remarks>
        /// You can override this method to create a custom schema instance.
        /// </remarks>
        /// <param name="services">
        ///  The domain services container.
        /// </param>
        /// <returns>
        ///  The new schema.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected virtual ISchema CreateSchema(IServicesContainer services)
        {
            return new Hyperstore.Modeling.Metadata.DomainSchema(_name, services, Behavior);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads dependent schemas. </summary>
        /// <param name="store">    The store. </param>
        ///-------------------------------------------------------------------------------------------------
        void ISchemaDefinition.LoadDependentSchemas(IHyperstore store)
        {
            DebugContract.Requires(store);
            LoadDependentSchemas(store);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Loads dependent schemas.
        /// </summary>
        /// <param name="store">
        ///  The store.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void LoadDependentSchemas(IHyperstore store)
        {
        }
    }
}