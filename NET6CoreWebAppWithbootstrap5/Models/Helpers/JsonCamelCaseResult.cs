
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NET6CoreWebAppWithbootstrap5.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.Dynamic;

namespace NET6CoreWebAppWithbootstrap5.Models.Helpers
{
    public static class DynamicExtensions
    {
        public static object? Merge(this object? item1, object? item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? null;

            HybridDictionary item = new HybridDictionary();
            if(item1 is IDictionary dicItem1)
            {
                foreach (var key in dicItem1.Keys)
                {
                    item.TryAdd(key, dicItem1[key]);
                }
            }
            else
            {
                foreach (System.Reflection.PropertyInfo fi in item1.GetType().GetProperties())
                {
                    if (fi.GetValue(item1, null) is object value)
                    {
                        item.TryAdd(fi.Name, value);
                    }

                }
            }
            if (item2 is IDictionary dicItem2)
            {
                foreach (var key in dicItem2.Keys)
                {
                    item.TryAdd(key, dicItem2[key]);
                }
            }
            else
            {
                foreach (System.Reflection.PropertyInfo fi in item2.GetType().GetProperties())
                {
                    if (fi.GetValue(item2, null) is object value)
                    {
                        item.TryAdd(fi.Name, value);
                    }
                }
            }
            
            return item;
        }
    }
}