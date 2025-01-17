﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.OmniaDataCatalogApi.Collibra.BusinessDataTypes;
using Equinor.OmniaDataCatalogApi.Collibra.Converters;
using Equinor.OmniaDataCatalogApi.Collibra.HttpClients;

namespace Equinor.OmniaDataCatalogApi.Collibra.Services
{
    public interface ICollibraService
    {
        Task<IEnumerable<Community>> Communities(string authorizationHeaderValue, CancellationToken cancellationToken);
    }

    public class CollibraService : ICollibraService
    {
        private readonly ICollibraHttpClient _collibraHttpClient;
        private readonly ICommunitiesConverter _communitiesConverter;
        private readonly string _communitiesPath = "rest/2.0/communities";//move this to configuration?

        public CollibraService(ICollibraHttpClient collibraHttpClient, ICommunitiesConverter communitiesConverter)
        {
            _collibraHttpClient = collibraHttpClient;
            _communitiesConverter = communitiesConverter;
        }

        public async Task<IEnumerable<Community>> Communities(string authorizationHeaderValue, CancellationToken cancellationToken=default)
        {
            var headers =
                new Dictionary<string, string> {{"Authorization", $"{authorizationHeaderValue}"}};
            
            var communityDtos = await _collibraHttpClient.Get(_communitiesPath, headers, cancellationToken);
            
            return _communitiesConverter.Convert(communityDtos);
        }
    }
}