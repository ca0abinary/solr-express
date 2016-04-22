﻿using Newtonsoft.Json;
using SolrExpress.Core.ParameterValue;
using SolrExpress.Solr5.Parameter;
using SolrExpress.Solr5.Result;
using System;
using System.Collections.Generic;

namespace BasicUse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var ctx = new SolrContext())
            {
                List<TechProduct> documents;

                ctx.TechProducts
                    .Parameter(new QueryParameter<TechProduct>().Configure(new QueryAll()))
                    .Parameter(new LimitParameter().Configure(3));

                var result = ctx.TechProducts.Execute();

                documents = result.Get(new DocumentResult<TechProduct>()).Data;
                
                foreach (var document in documents)
                {
                    var json = JsonConvert.SerializeObject(document, Formatting.Indented);

                    Console.WriteLine(json);
                    Console.WriteLine(new string('-', 50));
                }
            }
            
            Console.Read();
        }
    }
}
