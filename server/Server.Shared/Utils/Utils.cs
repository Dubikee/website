using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Server.Shared.Models.Auth;

namespace Server.Shared.Utils
{
    public static class Utils
    {
        public static bool IsAdmin(this AppUser appUser)
        {
            return appUser.Role.ToLower() == RoleTypes.Admin;
        }

        public static bool IsMaster(this AppUser appUser)
        {
            return appUser.Role.ToLower() == RoleTypes.Master;
        }

        public static string GetJwt(this IHeaderDictionary headers)
        {
            return headers
                .FirstOrDefault(x => x.Key.ToLower() == "authorization").Value
                .FirstOrDefault();
        }

        public static void RemoveJwt(this IHeaderDictionary headers)
        {
            var header = headers.FirstOrDefault(x => x.Key.ToLower() == "authorization");
            headers.Remove(header);
        }
    }
}
