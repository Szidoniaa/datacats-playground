using System.Collections.Generic;
using System.Linq;
using Collibra.BusinessDataTypes;
using Collibra.Dtos;

namespace Collibra.Converters
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