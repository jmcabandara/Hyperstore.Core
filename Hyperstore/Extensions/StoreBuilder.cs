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

using Hyperstore.Modeling.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Modeling
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  A store builder.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public sealed class StoreBuilder
    {
        private StoreOptions _options = StoreOptions.None;
        private Assembly[] _assemblies;
        private IServicesContainer _services = new ServicesContainer();
        private Guid? _id;

        private StoreBuilder()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Initialises this instance.
        /// </summary>
        /// <returns>
        ///  A StoreBuilder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static StoreBuilder New()
        {
            return new StoreBuilder();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Enables the extensions.
        /// </summary>
        /// <returns>
        ///  A StoreBuilder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public StoreBuilder EnableExtensions()
        {
            _options |= StoreOptions.EnableExtensions;
            return this;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Compose with.
        /// </summary>
        /// <param name="assemblies">
        ///  A variable-length parameters list containing assemblies.
        /// </param>
        /// <returns>
        ///  A StoreBuilder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public StoreBuilder ComposeWith(params Assembly[] assemblies)
        {
            if (assemblies.Length > 0)
            {
                _assemblies = assemblies;
            }
            return this;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  With identifier.
        /// </summary>
        /// <param name="id">
        ///  The identifier.
        /// </param>
        /// <returns>
        ///  A StoreBuilder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public StoreBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Usings a service
        /// </summary>
        /// <typeparam name="T">
        ///  Generic type parameter.
        /// </typeparam>
        /// <param name="service">
        ///  The service.
        /// </param>
        /// <param name="lifecycle">
        ///  (Optional) the lifecycle.
        /// </param>
        /// <returns>
        ///  A StoreBuilder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public StoreBuilder Using<T>(Func<IServicesContainer, T> service, ServiceLifecycle lifecycle= ServiceLifecycle.Scoped) where T : class
        {
            _services.Register<T>(service, lifecycle);
            return this;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Creates the store.
        /// </summary>
        /// <returns>
        ///  The new store.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IHyperstore Create()
        {
            return new Store(_services, _options, _id);
        }
    }
}
