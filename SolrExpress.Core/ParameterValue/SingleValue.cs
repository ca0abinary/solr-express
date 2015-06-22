﻿using System;
using System.Linq.Expressions;
using SolrExpress.Core.Helper;
using SolrExpress.Core.Query;

namespace SolrExpress.Core.ParameterValue
{
    /// <summary>
    /// Single value parameter
    /// </summary>
    public sealed class SingleValue<TDocument> : IQueryParameterValue, IValidation
        where TDocument : IDocument
    {
        private readonly Expression<Func<TDocument, object>> _expression;
        private readonly string _value;
        
        /// <summary>
        /// Create a single solr parameter value
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="value">Value of the filter</param>
        public SingleValue(Expression<Func<TDocument, object>> expression, string value)
        {
            this._expression = expression;
            this._value = value;
        }

        /// <summary>
        /// Execute parameter value generator
        /// </summary>
        /// <returns>Result of the value generator</returns>
        public string Execute()
        {
            var fieldName = UtilHelper.GetFieldNameFromExpression(this._expression);

            return string.Concat(fieldName, ":", this._value);
        }

        /// <summary>
        /// Check for the parameter validation
        /// </summary>
        /// <param name="isValid">True if is valid, otherwise false</param>
        /// <param name="errorMessage">The error message, if applicable</param>
        public void Validate(out bool isValid, out string errorMessage)
        {
            isValid = true;
            errorMessage = string.Empty;

            var solrFieldAttribute = UtilHelper.GetSolrFieldAttributeFromPropertyInfo(this._expression);
            
            if (solrFieldAttribute != null && !solrFieldAttribute.Indexed)
            {
                isValid = false;
                errorMessage = "A field must be \"indexed=true\" to be used in a query";
            }
        }
    }
}