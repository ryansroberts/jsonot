using System;
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
            var p = PathArray(transform);

            return p.First().Value<string>();
        }

        static JArray PathArray(JObject transform)
        {
            var p = transform.SelectToken("p") as JArray;

            if (p == null || !p.Any())
                throw new TransformException("Cannot locate valid path propery in transform");
            return p;
        }

        T1 PathParams<T1>(JObject transform)
        {
            var p = PathArray(transform);

            if(p.Count != 2)
                throw new TransformException("Incorrect number of parameters in path" + p);

            return p.ElementAt(1).Value<T1>();
        }
        
        T1 PayloadParams<T1>(JObject transform,string name)
        {
            return transform.SelectToken(name).Value<T1>();
        }

        

        JObject NumberAdd(JObject json, JObject transform)
        {
            var toAdd = PayloadParams<double>(transform,"na");
            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => v.Replace(
                        new JValue(v.Value<double>() + toAdd))));

            return transform;
        }



        JObject StringInsert(JObject json, JObject transform)
        {
            var offset = PathParams<int>(transform);
            var toInsert = PayloadParams<string>(transform, "si");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => v.Replace(
                        new JValue(v.Value<string>().Insert(offset,toInsert)))));

            return json;
        }

        public Transformer()
        {
            dispatchMap["na"] = NumberAdd;
            dispatchMap["si"] = StringInsert;
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
