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

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> run)
        {
            var index = 0;
            foreach (var s in source)
            {
                run(s, index);
                index++;
            }
        }
    }
}
