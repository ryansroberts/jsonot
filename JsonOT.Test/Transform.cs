using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Should;

namespace JsonOT.Test
{
     public class OperationalTransformsHappy
     {
        JObject Transform(JObject toTranform, JObject transform)
        {
            var tranny = new Transformer();
            return tranny.Transform(toTranform, transform);
        }

        [Fact]
        public void NumericAdditionAdds()
        {
            Transform(JObject.FromObject(new
            {
                somenumber = 1.0f
            }), Transforms.AddToNumber("somenumber", 2))
            ["somenumber"].Value<int>().ShouldEqual(3);
        }

         [Fact]
         public void StringInsertInserts()
         {
             Transform(JObject.FromObject(new
             {
                somestring = "lol"                                     
             }),Transforms.StringInsert("somestring","lol",3))
             ["somestring"].Value<string>().ShouldEqual("lollol");
         }
     }
}
