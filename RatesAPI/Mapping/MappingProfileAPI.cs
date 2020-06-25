using AutoMapper;
using Core.Models.Users;
using Core.Models.Users.DTOs;
using System;

namespace RatesAPI.Mapping
{
    public class MappingProfileAPI : Profile
    {
        public MappingProfileAPI()
        {
            CreateMap<User, UserOutputDTO>();

        }
    }
}
