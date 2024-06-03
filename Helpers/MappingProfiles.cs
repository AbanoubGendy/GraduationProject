using AutoMapper;
using Graduation.DAL.Models;
using Graduation.PL.ViewModels;

namespace Graduation.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserViewModel, User>().ReverseMap();
        }
    }
}
