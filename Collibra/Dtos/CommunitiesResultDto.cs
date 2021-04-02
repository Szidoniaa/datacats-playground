using System.Collections.Generic;
using Newtonsoft.Json;

namespace Collibra.Dtos
{
    public class CommunitiesResultDto
    {
        [JsonProperty("results")]
        public IEnumerable<CommunityDto> Communities { get; set; }
    }
}