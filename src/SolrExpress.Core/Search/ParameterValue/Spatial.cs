﻿using SolrExpress.Core.Utility;
using System;
using System.Linq.Expressions;

namespace SolrExpress.Core.Search.ParameterValue
{
    /// <summary>
    /// Result spatial value
    /// </summary>
    public sealed class Spatial<TDocument> : ISearchParameterValue<TDocument>
        where TDocument : IDocument
    {
        /// <summary>
        /// Create a spatial filter parameter
        /// </summary>
        /// <param name="functionType">Function used in the spatial filter</param>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="centerPoint">Center point to spatial filter</param>
        /// <param name="distance">Distance from the center point</param>
        public Spatial(SolrSpatialFunctionType functionType, Expression<Func<TDocument, object>> expression, GeoCoordinate centerPoint, decimal distance)
        {
            Checker.IsNull(expression);

            this.FunctionType = functionType;
            this.Expression = expression;
            this.CenterPoint = centerPoint;
            this.Distance = distance;
        }

        /// <summary>
        /// Execute parameter value generator
        /// </summary>
        /// <returns>Result generated value</returns>
        public string Execute()
        {
            var fieldName = this.ExpressionBuilder.GetFieldNameFromExpression(this.Expression);

            return ExpressionUtility.GetSolrSpatialFormule(
                this.FunctionType,
                fieldName,
                this.CenterPoint,
                this.Distance);
        }

        /// <summary>
        /// Expressions builder
        /// </summary>
        public IExpressionBuilder<TDocument> ExpressionBuilder { get; set; }

        /// <summary>
        /// Function used in the spatial filter
        /// </summary>
        public SolrSpatialFunctionType FunctionType { get; private set; }

        /// <summary>
        /// Expression used to find the property name
        /// </summary>
        public Expression<Func<TDocument, object>> Expression { get; private set; }

        /// <summary>
        /// Center point to spatial filter
        /// </summary>
        public GeoCoordinate CenterPoint { get; private set; }

        /// <summary>
        /// Distance from the center point
        /// </summary>
        public decimal Distance { get; private set; }
    }
}
