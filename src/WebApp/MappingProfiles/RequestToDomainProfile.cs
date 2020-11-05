using AutoMapper;

using WebApplicationAPI.Domain;
using WebApplicationAPI.Contracts.V1.Requests.Queries;

namespace WebApplicationAPI.MappingProfiles {
    public class RequestToDomainProfile : Profile {
        public RequestToDomainProfile() {
            this.CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}
