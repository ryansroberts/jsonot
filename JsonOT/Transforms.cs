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
    }
}