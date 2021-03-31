﻿using Newtonsoft.Json;

namespace HelloRadix.DataTransferObjects
{
    public class CommunityDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }
    }
}