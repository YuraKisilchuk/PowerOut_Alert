﻿using Microsoft.AspNetCore.Identity;

namespace WebTelegram.Data.Entities.Identity
{
    public class UserRoleEntity : IdentityUserRole<int>
    {
        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
