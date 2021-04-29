using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Animulu.Models
{
    public class AnimuluUser : IdentityUser
    {
        public AnimuluUser()
        {
            ProfilePicName = Guid.NewGuid().ToString();
            ProfilePicExist = false;
        }
        [PersonalData]
        public string ProfilePicName { get; set; }
        [PersonalData]
        public bool ProfilePicExist { get; set; }
    }
}
