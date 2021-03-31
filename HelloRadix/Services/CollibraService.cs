using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloRadix.BusinessDataTypes;
using HelloRadix.Converters;


namespace HelloRadix.Controllers
{
    public interface ICollibraService
    {
        Task<IEnumerable<Community>> Communities(string authorizationHeaderValue, CancellationToken cancellationToken);
    }

    public class CollibraService : ICollibraService
    {
        private readonly ICollibraHttpClient _collibraHttpClient;
        private readonly ICommunitiesConverter _communitiesConverter;
        private readonly string _communitiesPath = "rest/2.0/communities";

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