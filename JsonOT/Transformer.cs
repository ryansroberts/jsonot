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

        Tuple<T1,T2> PayloadParams<T1,T2>(JObject transform, string p1,string p2)
        {
            return new Tuple<T1, T2>(
                transform.SelectToken(p1).Value<T1>(),
                transform.SelectToken(p2).Value<T2>());
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

        JObject StringDelete(JObject json, JObject transform)
        {
            var offset = PathParams<int>(transform);
            var toDelete = PayloadParams<string>(transform, "sd");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => v.Replace(
                        new JValue(v.Value<string>().Remove(offset, toDelete.Length)))));

            return json;
        }

        JObject ListInsert(JObject json, JObject transform)
        {
            var index = PathParams<int>(transform);
            var toInsert = PayloadParams<JToken>(transform, "li");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => ((JArray)v)[index].AddAfterSelf(toInsert)));

            return json;
        }

        JObject ListDelete(JObject json, JObject transform)
        {
            var index = PathParams<int>(transform);
            var toDelete = PayloadParams<JToken>(transform, "ld");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => ((JArray)v)[index].Remove()));

            return json;
        }

        JObject ListReplace(JObject json, JObject transform)
        {
            var index = PathParams<int>(transform);
            var replaceParams = PayloadParams<JToken,JToken>(transform, "ld","li");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => ((JArray)v)[index].Replace(replaceParams.Item2)));

            return json;
        }

        JObject ListMove(JObject json, JObject transform)
        {
            var index = PathParams<int>(transform);
            var toIndex = PayloadParams<int>(transform, "lm");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => ((JArray) v)[toIndex].AddBeforeSelf(((JArray) v)[index])));

            return json;
        }
        
        JObject ObjectDelete(JObject json, JObject transform)
        {
            var toDelete = PayloadParams<JToken>(transform,"od");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v =>  v.Parent.Remove()));

            return json;
        }

        JObject ObjectInsert(JObject json, JObject transform)
        {
            var propertyName = PathParams<string>(transform);
            var toInsert = PayloadParams<JToken>(transform, "oi");

            Path(transform)
                .Maybe(p => json.SelectToken(p)
                    .Maybe(v => (v[propertyName] = toInsert)));

            return json;
        }

        JObject ObjectReplace(JObject json, JObject transform)
        {
            var replaceParams = PayloadParams<JToken, JToken>(transform, "od", "oi");


            Path(transform)
                .Maybe(t => json.SelectToken(t)
                    .Maybe(p => p.Values().FirstOrDefault(v => JToken.DeepEquals(v,replaceParams.Item1))
                        .Maybe(o => o.Replace(replaceParams.Item2))));


            return json;
        }

        public Transformer()
        {
            dispatchMap["na"] = NumberAdd;
            dispatchMap["si"] = StringInsert;
            dispatchMap["sd"] = StringDelete;
            dispatchMap["li"] = ListInsert;
            dispatchMap["ld"] = ListDelete;
            dispatchMap["ldli"] = ListReplace;
            dispatchMap["lm"] = ListMove;
            dispatchMap["oi"] = ObjectInsert;
            dispatchMap["od"] = ObjectDelete;
            dispatchMap["odoi"] = ObjectReplace;
        }


        public JObject Transform(JObject json, JObject transform)
        {
            var opCode = OpCode(transform);
            Func<JObject, JObject, JObject> op;
            if (dispatchMap.TryGetValue(opCode, out op))
                op(json, transform); 
            return json;
        }

        static string OpCode(JObject transform)
        {
            return string.Join("", transform.Properties()
                                            .Where(p => p.Name != "p")
                                            .Select(p => p.Name).ToArray());

        }
    }

}
