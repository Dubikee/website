using System;
using System.Collections.Generic;
using System.Text;
using Server.Shared.Models.Auth;

namespace Server.Shared.Utils
{
    public static class Utils
    {
        public static bool IsAdmin(this User user)
        {
            return user.Role.ToLower() == RoleTypes.Admin;
        }

        public static bool IsMaster(this User user)
        {
            return user.Role.ToLower() == RoleTypes.Master;
        }
    }
}
