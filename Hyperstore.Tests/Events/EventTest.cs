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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyperstore.Modeling;
using Hyperstore.Modeling.Commands;
using Hyperstore.Modeling.HyperGraph;
using Hyperstore.Modeling.HyperGraph.Index;
using Hyperstore.Tests.Model;
using Xunit;
using System.Threading.Tasks;
using Hyperstore.Modeling.Domain;

#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Hyperstore.Tests.Commands
{
    
    public class EventTest
    {
        [Fact]
        public async Task CreationEvents()
        {
            var store = await StoreBuilder.New().CreateAsync();
            await store.Schemas.New<TestDomainDefinition>().CreateAsync();
            var domain = await store.DomainModels.New().UsingIdGenerator(r=>new LongIdGenerator()).CreateAsync("Test"); 

            int cx = 0;
            // Abonnements aux events
            domain.Events.CustomEventRaised.Subscribe(e =>
            {
                // Il est le seul en top level
                Assert.True(e.Event.TopEvent);
                cx++;
            });
 
            domain.Events.EntityAdded.Subscribe(e =>
            {
                Assert.False(e.Event.TopEvent);
                cx++;
            });

            domain.Events.PropertyChanged.Subscribe(e =>
            {
                Assert.False(e.Event.TopEvent);
                cx++;
            });

            domain.Events.SessionCompleted.Subscribe(e =>
            {
                Assert.False(e.IsAborted);
                Assert.False(e.IsReadOnly);
                cx++;
            });

            domain.Events.PropertyChanged.Subscribe(e =>
            {
                if (e.Event.Id == new Identity("Test", "1"))
                {
                    Assert.False(e.Event.TopEvent);
                    cx++;
                }
            });

            using (var s = store.BeginSession())
            {
                s.Execute(new MyCommand(domain));
                s.AcceptChanges();
            }

            Assert.Equal(5, cx);
        }

        [Fact]
        public async Task DeletionEvents()
        {
            var store = await StoreBuilder.New().CreateAsync();
            await store.Schemas.New<TestDomainDefinition>().CreateAsync();
            var domain = await store.DomainModels.New().CreateAsync("Test"); 

            int cx = 0;
            // Abonnements aux events
            domain.Events.EntityRemoved.Subscribe(e =>
            {
                Assert.True(e.Event.TopEvent);
                cx++;
            });

            domain.Events.PropertyRemoved.Subscribe(e =>
            {
                Assert.False(e.Event.TopEvent);
                cx++;
            });

            domain.Events.SessionCompleted.Subscribe(e =>
            {
                Assert.False(e.IsAborted);
                Assert.False(e.IsReadOnly);
                cx++;
            });

            IModelElement elem;
            using (var s = store.BeginSession())
            {
                var cmd = new MyCommand(domain);
                s.Execute(cmd);
                elem = cmd.Element;
                s.AcceptChanges();
            }

            using (var s = store.BeginSession())
            {
                elem.Remove();
                s.AcceptChanges();
            }

            Assert.Equal(4, cx);
        }

    }
}
