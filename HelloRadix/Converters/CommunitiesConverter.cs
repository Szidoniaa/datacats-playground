using System.Collections.Generic;
using System.Linq;
using HelloRadix.BusinessDataTypes;
using HelloRadix.DataTransferObjects;

namespace HelloRadix.Converters
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