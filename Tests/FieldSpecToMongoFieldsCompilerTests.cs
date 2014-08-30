﻿using NUnit.Framework;
using System;
using Rql;
using Rql.MongoDB;
using System.Reflection;
using MongoDB.Driver.Builders;

namespace Rql.MongoDB.Tests
{
    [TestFixture]
    public class FieldSpecToMongoFieldsCompilerTests
    {
        [Test()]
        public void TestFields()
        {
            var pairs = new[] 
            {
                new { Fields = "", Mongo = "" },
                new { Fields = "name(1),age(1)", Mongo = "{ \"name\" : 1, \"age\" : 1 }" },
                new { Fields = "id(0),age(1)", Mongo = "{ \"_id\" : 0, \"age\" : 1 }" },
            };

            for (int i = 0; i < pairs.Length; i++)
            {
                var pair = pairs[i];
                var fields = new FieldSpecToMongoFieldsCompiler().Compile(pair.Fields);
                string mongo = (fields == Fields.Null ? "" : fields.ToString());

                Assert.AreEqual(pair.Mongo, mongo, String.Format("Iteration {0}", i));
            }
        }

        [Test]
        public void TestBadFields()
        {
            var pairs = new[] 
            {
                new { Fields = "name()" },
                new { Fields = "name(2)" },
            };

            for (int i = 0; i < pairs.Length; i++)
            {
                var pair = pairs[i];

                Assert.Throws<FieldSpecParserException>(() => new FieldSpecToMongoFieldsCompiler().Compile(pair.Fields));
            }
        }
    }
}
