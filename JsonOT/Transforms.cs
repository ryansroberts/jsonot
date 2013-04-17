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
    }
}