﻿//	Copyright � 2013 - 2014, Alain Metge. All rights reserved.
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

using Hyperstore.Modeling;
using Hyperstore.Modeling.HyperGraph;
using Hyperstore.Modeling.Traversal;
using Hyperstore.Tests.Model;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Tests.Hypergraph
{
    
    public class TraversalTests : HyperstoreTestBase
    {
        [Fact]
        
        public async Task TraversalTest()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var schema = await store.Schemas.New<TestDomainDefinition>().CreateAsync();
            var domain = await store.DomainModels.New().CreateAsync("Test");

            CreateGraph(domain);

            ISchemaEntity metadata;
            using (var session = domain.Store.BeginSession(new SessionConfiguration { Readonly = true }))
            {
                metadata = schema.Definition.XExtendsBaseClass;
            }

            var cx = domain.Traversal.OnEveryPath(
                // Filtre personnalisé 
                // On prend en compte les chemins dont le noeud terminal est le 7 et on ignore les autres
                    p => p.EndElement.Key == "7"
                        ? GraphTraversalEvaluatorResult.IncludeAndContinue
                        : GraphTraversalEvaluatorResult.ExcludeAndContinue
                    )
                    .GetPaths(new NodeInfo(new Identity("test", "1"), metadata.Id)).Count();

            Assert.Equal(3, cx);

            var p2 = domain.Traversal.OnEveryPath(p => p.EndElement.Key == "7" ? GraphTraversalEvaluatorResult.IncludeAndExit : GraphTraversalEvaluatorResult.ExcludeAndContinue)
                                     .PathTraverser(new GraphDepthFirstTraverser())
                                     .GetPaths(new NodeInfo(new Identity("test", "1"), metadata.Id)).First();
            Assert.Equal(2, p2.Length);
        }


        [Fact]
        
        public async Task GraphPathTests()
        {
            var store = await StoreBuilder.New().CreateAsync();
            await store.Schemas.New<TestDomainDefinition>().CreateAsync();
            var domain = await store.DomainModels.New().CreateAsync("Test");

            var start = new Identity("test", "1");
            var p = new GraphPath(domain, start);
            var p2 = new GraphPath(domain, start);

            Assert.Equal(0, p.Length);
            Assert.Equal(1, p.Elements.Count());
            Assert.Equal(0, p.Relationships.Count());
            Assert.Equal(start, p.StartElement);
            Assert.Equal(start, p.EndElement);

            var end = new Identity("test", "3");
            var edge = new EdgeInfo(new Identity("test", "2"), new Identity("test", "Has"), end);

            p = p.Create(end, edge);
            p2 = p2.Create(end, edge);

            Assert.Equal(1, p.Length);
            Assert.Equal(2, p.Elements.Count());
            Assert.Equal(1, p.Relationships.Count());
            Assert.Equal(start, p.StartElement);
            Assert.Equal(end, p.EndElement);


            end = new Identity("test", "5");
            edge = new EdgeInfo(new Identity("test", "4"), new Identity("test", "Has"), end);

            p = p.Create(end, edge);
            p2 = p2.Create(end, edge);

            Assert.Equal(2, p.Length);
            Assert.Equal(3, p.Elements.Count());
            Assert.Equal(2, p.Relationships.Count());
            Assert.Equal(start, p.StartElement);
            Assert.Equal(end, p.EndElement);

            Assert.Equal(p, p2);

            Assert.Equal("[test:1] -- test:has --> [test:3] -- test:has --> [test:5]", p.ToString());

        }
    }
}
