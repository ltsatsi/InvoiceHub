using IMS_DomainLayer.Dtos.Users;
using IMS_DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this ApplicationUser applicationUser)
        {
            return new UserDto
            {
                Id = applicationUser.Id,
                FullName = applicationUser.FullName,
                AddressLine  = applicationUser.AddressLine,
                Email = applicationUser.Email!,
            };
        }
    }
}
