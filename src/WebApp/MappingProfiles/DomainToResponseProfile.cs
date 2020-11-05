using System.Linq;
using AutoMapper;

using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Domain;

namespace WebApplicationAPI.MappingProfiles {
    public class DomainToResponseProfile : Profile {
        public DomainToResponseProfile() {
            this.CreateMap<Tag, TagResponse>();

            this.CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => new TagResponse { Name = tag.TagName })));
        }
    }
}
