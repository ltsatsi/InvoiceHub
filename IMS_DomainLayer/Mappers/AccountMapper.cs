using IMS_DomainLayer.Dtos.Account;
using IMS_DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Mappers
{
    public static class AccountMapper
    {
        public static ApplicationUser ToUserFromSignUp(this SignUpDto signUpDto)
        {
            return new ApplicationUser
            {
                FullName = signUpDto.FullName,
                UserName = signUpDto.Email,
                Email = signUpDto.Email,
                AddressLine = signUpDto.AddressLine,
            };
        }
    }
}
