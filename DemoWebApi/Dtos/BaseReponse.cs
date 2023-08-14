using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace DemoWebApi.Dtos
{
    [DataContract]
    public class BaseReponse
    {
        [DataMember(Name = "responseCode")]
        public int ResponseCode { get; set; }
        [DataMember(Name = "responseText")]
        public string ResponseText { get; set; }
        [DataMember(Name = "data")]
        public object Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
