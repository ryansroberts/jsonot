using Newtonsoft.Json.Linq;

namespace JsonOT
{
    public static class Transforms
    {
        public static JObject AddToNumber(string path, int number)
        {
            return JObject.FromObject(new{
                p = new[]{path},na = number
            });
        }

        public static JObject StringInsert(string path,string toinsert, int offset)
        {
            return JObject.FromObject(new {
                p = new []{path,offset.ToString()},
                si = toinsert
            });
        }

        public static JObject StringDelete(string path, string todelete, int offset)
        {
            return JObject.FromObject(new {
                p = new[] { path, offset.ToString() },
                sd = todelete
            });
        }

        public static JObject ListInsert(string path, int index, JToken toInsert)
        {
            return JObject.FromObject(new {
                p = new[] {path,index.ToString()},
                li = toInsert
            });
        }

        public static JObject ListDelete(string path, int index, JToken toDelete)
        {
            return JObject.FromObject(new
            {
                p = new[] { path, index.ToString() },
                ld = toDelete
            });
        }

        public static JObject ListReplace(string path, int index, JValue toReplace, JValue replaceWith)
        {
            return JObject.FromObject(new
            {
                p = new[] { path, index.ToString()},
                ld = toReplace,
                li = replaceWith
            });
        }

        public static JObject ListMove(string path, int fromIndex, int toIndex)
        {
            return JObject.FromObject(new
            {
                p = new[] { path, fromIndex.ToString() },
                lm = toIndex
            });
        }

        public static JObject ObjectInsert(string path, string key, JToken toInsert)
        {
            return JObject.FromObject(new {
                 p = new[] {path,key},
                 oi = toInsert            
            });
        }
    }
}