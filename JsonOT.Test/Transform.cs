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
        [Fact]
        public void NumericAdditionAdds()
        {
            var tranny = new Transformer();
            var res = tranny.Transform(JObject.FromObject(new
            {
                somenumber = 1.0f
            }), Transforms.AddToNumber("somenumber", 2));

            res["somenumber"].Value<int>().ShouldEqual(3);
        }
     }
}
