﻿using Moq;
using Newtonsoft.Json.Linq;
using SolrExpress.Core;
using SolrExpress.Core.Utility;
using SolrExpress.Solr5.Search.Parameter;
using System;
using Xunit;

namespace SolrExpress.Solr5.UnitTests.Search.Parameter
{
    public class SortParameterTests
    {
        /// <summary>
        /// Where   Using a SortParameter instance
        /// When    Invoking the method "Execute"
        /// What    Create a valid JSON
        /// </summary>
        [Fact]
        public void SortParameter001()
        {
            // Arrange
            var expected = JObject.Parse(@"
            {
              ""sort"": ""_id_ asc""
            }");
            string actual;
            var jObject = new JObject();
            var expressionCache = new ExpressionCache<TestDocument>();
            var expressionBuilder = (IExpressionBuilder<TestDocument>)new ExpressionBuilder<TestDocument>(expressionCache);
            var parameter = new SortParameter<TestDocument>(expressionBuilder);
            parameter.Configure(q => q.Id, true);

            // Act
            parameter.Execute(jObject);
            actual = jObject.ToString();

            // Assert
            Assert.Equal(expected.ToString(), actual);
        }

        /// <summary>
        /// Where   Using a SortParameter instance
        /// When    Create the instance with null
        /// What    Throws ArgumentNullException
        /// </summary>
        [Fact]
        public void SortParameter002()
        {
            // Arrange
            var expressionCache = new ExpressionCache<TestDocument>();
            var expressionBuilder = (IExpressionBuilder<TestDocument>)new ExpressionBuilder<TestDocument>(expressionCache);
            var parameter = new SortParameter<TestDocument>(expressionBuilder);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => parameter.Configure(null, true));
        }

        /// <summary>
        /// Where   Using a SortParameter instance
        /// When    Invoking the method "Validate" using field Indexed=true
        /// What    Valid is true
        /// </summary>
        [Fact]
        public void SortParameter003()
        {
            // Arrange
            bool isValid;
            string errorMessage;
            var expressionCache = new ExpressionCache<TestDocumentWithAttribute>();
            var expressionBuilder = (IExpressionBuilder<TestDocumentWithAttribute>)new ExpressionBuilder<TestDocumentWithAttribute>(expressionCache);
            var parameter = new SortParameter<TestDocumentWithAttribute>(expressionBuilder);
            parameter.Configure(q => q.Indexed, true);

            // Act
            parameter.Validate(out isValid, out errorMessage);

            // Assert
            Assert.True(isValid);
        }

        /// <summary>
        /// Where   Using a SortParameter instance
        /// When    Invoking the method "Validate" using field Indexed=false
        /// What    Valid is true
        /// </summary>
        [Fact]
        public void SortParameter004()
        {
            // Arrange
            bool isValid;
            string errorMessage;
            var expressionCache = new ExpressionCache<TestDocumentWithAttribute>();
            var expressionBuilder = (IExpressionBuilder<TestDocumentWithAttribute>)new ExpressionBuilder<TestDocumentWithAttribute>(expressionCache);
            var parameter = new SortParameter<TestDocumentWithAttribute>(expressionBuilder);
            parameter.Configure(q => q.NotIndexed, true);

            // Act
            parameter.Validate(out isValid, out errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Equal(Resource.FieldMustBeIndexedTrueToBeUsedInASortException, errorMessage);
        }
    }
}
