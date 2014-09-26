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

using System.Collections.Generic;
using Hyperstore.Modeling.Container;

#endregion

namespace Hyperstore.Modeling.Traversal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  A graph path traverser.
    /// </summary>
    /// <seealso cref="T:Hyperstore.Modeling.Traversal.IGraphPathTraverser"/>
    ///-------------------------------------------------------------------------------------------------
    public abstract class GraphPathTraverser : IGraphPathTraverser
    {
        private ITraversalQuery _query;
        private IHyperstoreTrace _trace;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Initializes this instance.
        /// </summary>
        /// <param name="query">
        ///  The query.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void IGraphPathTraverser.Initialize(ITraversalQuery query)
        {
            Contract.Requires(query, "query");
            _query = query;
            _trace = _query.DomainModel.Resolve<IHyperstoreTrace>(false) ?? new EmptyHyperstoreTrace();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Enumerates the items in this collection that meet given criteria.
        /// </summary>
        /// <param name="node">
        ///  The node.
        /// </param>
        /// <returns>
        ///  An enumerator that allows foreach to be used to process the matched items.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual IEnumerable<GraphPath> Traverse(IModelElement node)
        {
            DebugContract.Requires(node);

            // Création du container de stockage des noeuds (ou plutot des chemins jusqu'à ce noeud) restant à traverser
            // Le type est tributaire de l'algorithme de traversé (Queue pour BreadthFirst et Stack pour DepthFirst)
            // Ce container est mis à jour à chaque noeud.
            var paths = CreatePathContainer();

            // Lecture du noeud de départ            
            if (node == null)
                yield break;

            // Constitution du Path courant
            var path = CreatePath(new GraphPosition
                                  {
                                          Node = node,
                                          IsStartPosition = true
                                  });
            // Initialisation du container avec le 1er noeud
            paths.Insert(new[] {path});

            // Tant qu'il y a des noeuds à parcourir
            while (!paths.IsEmpty)
            {
                // On prend le prochain noeud à parcourir (dépend de l'algo de traversé)
                path = paths.Retrieve();

                // On indique que ce noeud a été visité
                _query.UnicityPolicy.MarkVisited(path);

                // Filtrage du chemin courant pour savoir si on continue
                var result = _query.Evaluator.Visit(path);

                // Le chemin courant est à prendre en compte
                if ((GraphTraversalEvaluatorResult.Include & result) == GraphTraversalEvaluatorResult.Include)
                {
                    _trace.WriteTrace(TraceCategory.Traverser, "Include : {0}", path);
                    yield return path;
                }

                // Arrêt forcé
                if ((result & GraphTraversalEvaluatorResult.Exit) == GraphTraversalEvaluatorResult.Exit)
                    break;

                if ((result & GraphTraversalEvaluatorResult.Continue) == GraphTraversalEvaluatorResult.Continue)
                {
                    var childPaths = new List<GraphPath>(31);

                    // Parcours de toutes les relations du noeud courant pour tester si
                    // on les prend en compte
                    foreach (var rel in _query.IncidencesIterator.From(path.EndElement))
                    {
                        _trace.WriteTrace(TraceCategory.Traverser, "Visit : {1} for {0}", path, rel.Id);

                        // OK on la prend, on génére un nouveau chemin
                        var childNode = rel.End;
                        var pos = new GraphPosition
                                  {
                                          Node = childNode,
                                          FromEdge = rel
                                  };
                        var p = CreatePath(pos, path);
                        // Si ce chemin n'a pas dèjà été traité, on l'ajoute dans la liste des chemins à traiter
                        if (!_query.UnicityPolicy.IsVisited(p))
                        {
                            _trace.WriteTrace(TraceCategory.Traverser, "Push : {0}", p);
                            childPaths.Add(p);
                        }
                    }
                    paths.Insert(childPaths);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Creates path container.
        /// </summary>
        /// <returns>
        ///  The new path container.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected abstract IGraphPathList CreatePathContainer();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Creates a path.
        /// </summary>
        /// <param name="pos">
        ///  The position.
        /// </param>
        /// <param name="path">
        ///  (Optional) full pathname of the file.
        /// </param>
        /// <returns>
        ///  The new path.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected virtual GraphPath CreatePath(GraphPosition pos, GraphPath path = null)
        {
            DebugContract.Requires(pos);
            return new GraphPath(pos, path);
        }
    }
}