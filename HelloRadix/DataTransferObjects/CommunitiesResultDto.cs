using System.Collections.Generic;
using Newtonsoft.Json;

namespace HelloRadix.DataTransferObjects
{
    public class CommunitiesResultDto
    {
        [JsonProperty("results")]
        public IEnumerable<CommunityDto> Communities { get; set; }
    }
}