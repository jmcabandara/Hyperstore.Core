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
using Xunit;
using Hyperstore.Modeling;
using Hyperstore.Modeling.Metadata;
using Hyperstore.Modeling.Metadata.Primitives;
using Hyperstore.Modeling.Utils;
using System.Linq;
using System.Threading.Tasks;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Hyperstore.Tests.Metadata
{
    
    public class PrimitivesTest
    {
        [Fact]
        public async Task BooleanTest()
        {
            var store = await StoreBuilder.New().CreateAsync();
            IDomainModel dm = await store.DomainModels.New().CreateAsync("Test");
            Assert.True((bool)store.PrimitivesSchema.BooleanSchema.Deserialize(new SerializationContext(store.PrimitivesSchema.BooleanSchema, store.PrimitivesSchema.BooleanSchema.Serialize(true))));
            Assert.False((bool)store.PrimitivesSchema.BooleanSchema.Deserialize(new SerializationContext(store.PrimitivesSchema.BooleanSchema, store.PrimitivesSchema.BooleanSchema.Serialize(false))));
            Assert.False((bool)store.PrimitivesSchema.BooleanSchema.Deserialize(new SerializationContext(store.PrimitivesSchema.BooleanSchema, null)));
            Assert.Null(store.PrimitivesSchema.BooleanSchema.Serialize(null));
        }

        [Fact]
        public async Task DateTimeTest()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var dt = DateTime.Today;
            IDomainModel dm = await store.DomainModels.New().CreateAsync("Test");
            Assert.Equal(dt, (DateTime)store.PrimitivesSchema.DateTimeSchema.Deserialize(new SerializationContext(store.PrimitivesSchema.DateTimeSchema, store.PrimitivesSchema.DateTimeSchema.Serialize(dt))));
            Assert.Null(store.PrimitivesSchema.DateTimeSchema.Serialize(null));
        }

        [Fact]
        public async Task DoubleTest()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var dt = 10.2;
            IDomainModel dm = await store.DomainModels.New().CreateAsync("Test");
            Assert.Equal(dt, (Double)store.PrimitivesSchema.DoubleSchema.Deserialize(new SerializationContext( store.PrimitivesSchema.DoubleSchema, store.PrimitivesSchema.DoubleSchema.Serialize(dt))));
            Assert.Null(store.PrimitivesSchema.DoubleSchema.Serialize(null));
        }

        [Fact]
        public async Task TimeSpanTest()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var dt = DateTime.Now.TimeOfDay;
            IDomainModel dm = await store.DomainModels.New().CreateAsync("Test");
            Assert.Equal((TimeSpan)store.PrimitivesSchema.TimeSpanSchema.Deserialize(new SerializationContext( store.PrimitivesSchema.TimeSpanSchema, store.PrimitivesSchema.TimeSpanSchema.Serialize(dt))), dt);
            Assert.Null(store.PrimitivesSchema.TimeSpanSchema.Serialize(null));
        }

        [Fact]
        public async Task FloatTest()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var dt = 10.2;
            IDomainModel dm = await store.DomainModels.New().CreateAsync("Test");
            Assert.True((Single)store.PrimitivesSchema.SingleSchema.Deserialize(new SerializationContext(store.PrimitivesSchema.SingleSchema, store.PrimitivesSchema.SingleSchema.Serialize(dt))) - dt < 0.01);
            Assert.Null(store.PrimitivesSchema.SingleSchema.Serialize(null));
        }

        private enum X { A, B,C };
        //[Fact]
        //public void EnumTest()
        //{
        //    var store = await StoreBuilder.Init().CreateStore();
        //    IDomainModel dm = store.GetDomainModels(ModelType.Model).First();
        //    using (var s = store.BeginSession())
        //    {
        //        ISerializableModelElement ser = new EnumPrimitive<X>(dm.Schema);
        //        // TODO accès aux méthodes privees
        //        Assert.Equal(X.B, (X)ser.Deserialize(new SerializationContext(dm, Identity.Empty, store.PrimitivesSchema.SchemaEntitySchema, ser.Serialize(X.B))));
        //        Assert.Equal(X.B | X.C, (X)ser.Deserialize(new SerializationContext(dm, Identity.Empty, store.PrimitivesSchema.SchemaEntitySchema, ser.Serialize(X.C | X.B))));
        //        Assert.Null(store.PrimitivesSchema.SingleSchema.Serialize(null));
        //        s.AcceptChanges();
        //    }
        //}

        [Fact]
        public void DefaultValue()
        {
            Assert.Equal((double)0, ReflectionHelper.GetDefaultValue(typeof(double)));
            Assert.Equal(0, ReflectionHelper.GetDefaultValue(typeof(int)));
            var x = ReflectionHelper.GetDefaultValue(typeof(Nullable<int>));
            Assert.Equal((Nullable<int>)null, ReflectionHelper.GetDefaultValue(typeof(Nullable<int>)));
            Assert.Equal(X.A, ReflectionHelper.GetDefaultValue(typeof(X)));
            Assert.Equal(default(DateTime), ReflectionHelper.GetDefaultValue(typeof(DateTime)));
            Assert.Equal((uint)0, ReflectionHelper.GetDefaultValue(typeof(uint)));
            Assert.Equal((byte)0, ReflectionHelper.GetDefaultValue(typeof(byte)));
            Assert.Equal(false, ReflectionHelper.GetDefaultValue(typeof(bool)));
        }
    }
}
