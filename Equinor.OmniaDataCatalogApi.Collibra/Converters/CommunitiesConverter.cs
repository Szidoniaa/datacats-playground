using System.Collections.Generic;
using System.Linq;
using Equinor.OmniaDataCatalogApi.Collibra.BusinessDataTypes;
using Equinor.OmniaDataCatalogApi.Collibra.Dtos;

namespace Equinor.OmniaDataCatalogApi.Collibra.Converters
{
    public interface ICommunitiesConverter
    {
        IEnumerable<Community> Convert(CommunitiesResultDto communityDtos);
    }
    public class CommunitiesConverter : ICommunitiesConverter
    {
        public IEnumerable<Community> Convert(CommunitiesResultDto communityDtos)
        {
            return communityDtos.Communities.Select(communityDto => new Community {Id = communityDto.Id, Name = communityDto.Name}).ToList();
        }
    }
}