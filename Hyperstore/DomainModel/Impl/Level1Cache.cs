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
using System.Collections.Generic;
using System.Linq;
using Hyperstore.Modeling.Commands;
using Hyperstore.Modeling.Utils;
using Hyperstore.Modeling.HyperGraph;
using Hyperstore.Modeling.Platform;

#endregion

namespace Hyperstore.Modeling.Domain
{
    internal sealed class Level1Cache : IDisposable
    {
        private readonly IConcurrentDictionary<Identity, IModelElement> _cache;
        /// <summary>
        ///     Gestionnaire des demandes de vaccum
        /// </summary>
      //  private readonly JobScheduler _jobScheduler;
        private IHyperGraph _innerGraph;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="innerGraph">
        ///  The inner graph.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public Level1Cache(IHyperGraph innerGraph)
        {
            DebugContract.Requires(innerGraph);

            _cache = PlatformServices.Current.CreateConcurrentDictionary<Identity, IModelElement>();
            _innerGraph = innerGraph;

       //     _jobScheduler = new JobScheduler(Vacuum, TimeSpan.FromSeconds(60));

            innerGraph.DomainModel.Store.SessionCreated += OnSessionCreated;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        ///  resources.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public void Dispose()
        {
          //  _jobScheduler.Dispose();
            _innerGraph = null;
        }

        private void OnSessionCreated(object sender, SessionCreatedEventArgs e)
        {
            e.Session.Completing += OnSessionCompleting;
        }

        private void OnSessionCompleting(object sender, SessionCompletingEventArgs e)
        {
            if (e.Session.IsAborted)
                return;

            foreach (var elem in e.Session.TrackingData.GetTrackingElementsByState(TrackingState.Removed))
            {
                IModelElement weak;
                _cache.TryRemove(elem.Id, out weak);
            }
        }

        /// <summary>
        ///     Demande d'une execution du vaccum
        /// </summary>
        private void NotifyVacuum()
        {
#if !DEBUG
       //     _jobScheduler.RequestJob();
#endif
        }

        private void Vacuum()
        {
            //var queue = new Queue<Identity>(_cache.Where(c => c.Value.Target == null)
            //        .Select(c => c.Key));
            //while (queue.Count > 0)
            //{
            //    WeakReference weak;
            //    _cache.TryRemove(queue.Dequeue(), out weak);
            //}
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets an element.
        /// </summary>
        /// <param name="id">
        ///  The identifier.
        /// </param>
        /// <param name="metaclass">
        ///  The metaclass.
        /// </param>
        /// <param name="localOnly">
        ///  true to local only.
        /// </param>
        /// <returns>
        ///  The element.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IModelElement GetElement(Identity id, ISchemaElement metaclass, bool localOnly)
        {
            DebugContract.Requires(id);
            IModelElement weak;
            IModelElement elem;

            NotifyVacuum();
            if (_cache.TryGetValue(id, out weak))
            {
                elem = weak as IModelElement;
                if (elem == null)
                {
                    _cache.TryRemove(id, out weak);
                    if (elem != null) // Implique elem.Status == ModelElementStatus.Disposed
                        return null;
                }
                else
                {
                    if (Session.Current.TrackingData.GetTrackingElementState(elem.Id) == TrackingState.Removed)
                        return null;

                    return elem;
                }
            }

            elem = _innerGraph.GetElement(id, metaclass, localOnly);
            if (elem != null)
            {
                if (!_cache.TryAdd(id, elem))
                {
                    if (_cache.TryGetValue(id, out weak))
                        elem = weak as IModelElement;
                }
            }

            if (elem != null && Session.Current.TrackingData.GetTrackingElementState(elem.Id) == TrackingState.Removed)
                return null;
            return elem;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Adds an element.
        /// </summary>
        /// <param name="instance">
        ///  The instance.
        /// </param>
        /// <returns>
        ///  An IModelElement.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IModelElement AddElement(IModelElement instance)
        {
            DebugContract.Requires(instance);
            NotifyVacuum();

            var val = _cache.GetOrAdd(instance.Id, instance);
            var mel = val as IModelElement;
            if (mel != null)
                return mel;

            val = instance;
            // To ensure data was not removed after the last GetOrAdd
            val = _cache.GetOrAdd(instance.Id, val);
            return val as IModelElement;
        }
    }
}