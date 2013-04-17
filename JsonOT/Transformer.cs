﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonOT
{

    public static class Extensions
    {
        public static T Maybe<T>(this T item, Func<T,T> action) where T:class
        {
            return item != null ? action(item) : item;
        }

        public static void Maybe<T>(this T item, Action<T> action) where T : class
        {
            if (item != null)
                action(item);
        }
    }

    public class TransformException : Exception
    {
        public TransformException()
        {
        }

        public TransformException(string message)
            : base(message)
        {
        }
    }

    public class Transformer
    {
        Dictionary<string,Func<JObject,JObject,JObject>> dispatchMap = new Dictionary<string, Func<JObject, JObject, JObject>>();

        static string Path(JObject transform)
        {
            var p = transform.SelectToken("p") as JArray;

            if (p == null || !p.Any())
                throw new TransformException("Cannot locate valid path propery in transform");

            return p.First().Value<string>();
        }

        JObject NumberAdd(JObject json, JObject transform)
        {
            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => v.Replace(
                        new JValue(v.Value<double>() + transform["na"].Value<double>()))));

            return transform;
        }

        public Transformer()
        {
            dispatchMap["na"] = NumberAdd;
        }


        public JObject Transform(JObject json, JObject transform)
        {
            var opCode = (transform.Properties().ElementAtOrDefault(1) ?? new JProperty("",null)).Name;
            Func<JObject, JObject, JObject> op;
            if (dispatchMap.TryGetValue(opCode, out op))
                op(json, transform); 
            return json;
        }


    }

}
