﻿using Hyperstore.Modeling;
using Hyperstore.Modeling.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hyperstore.XTests
{
    public class SchemaTests
    {
        class CultureInfoSchema : SchemaValueObject<CultureInfo>
        {
            public CultureInfoSchema(ISchema schema)
                : base(schema)
            {

            }
            protected override string Serialize(object data, IJsonSerializer serializer)
            {
                if (data == null)
                    return null;
                return ((CultureInfo)data).DisplayName;
            }

            protected override object Deserialize(SerializationContext ctx)
            {
                if (ctx.Value == null)
                    return DefaultValue;
                return new CultureInfo((string)ctx.Value);
            }
        }

        class MySchemaDefinition : SchemaDefinition
        {
            public bool IsSchemaLoaded { get; private set; }

            public MySchemaDefinition()
                : base("Hyperstore.XTests")
            {
            }

            protected override void DefineSchema(ISchema schema)
            {
                new CultureInfoSchema(schema);
            }

            protected override void OnSchemaLoaded(ISchema schema)
            {
                base.OnSchemaLoaded(schema);
                IsSchemaLoaded = true;
            }
        }

        [Fact]
        public async Task CheckPrimitives()
        {
            var store = await StoreBuilder.New().CreateAsync();
            Assert.NotNull(store.GetSchemaInfo<string>());
            Assert.NotNull(store.GetSchemaInfo<int>());
            Assert.NotNull(store.GetSchemaInfo<double>());
        }

        [Fact]
        public async Task SchemaEvents()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var def = new MySchemaDefinition();
            var schema = await store.Schemas.New(def).CreateAsync();
            Assert.True(def.IsSchemaLoaded);
        }

        [Fact]
        public async Task LoadSchema()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var def = new MySchemaDefinition();
            var schema = await store.Schemas.New(def).CreateAsync();
            Assert.NotNull(schema);
            Assert.Equal(1, schema.GetSchemaInfos().Count());
            Assert.Equal(2, store.Schemas.Count());
        }

        [Fact]
        public async Task UnloadSchema()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var def = new MySchemaDefinition();
            var schema = await store.Schemas.New(def).CreateAsync();
            Assert.NotNull(schema);
            store.Schemas.Unload(schema);
            Assert.Equal(1, store.Schemas.Count());
        }

        [Fact]
        public async Task UnloadPrimitivesSchemaMustFailed()
        {
            var store = await StoreBuilder.New().CreateAsync();
            var schema = store.Schemas.First();
            Assert.NotNull(schema);
            Assert.IsType<PrimitivesSchema>(schema);
            Assert.Throws<Exception>( ()=> store.Schemas.Unload(schema));
        }
    }
}