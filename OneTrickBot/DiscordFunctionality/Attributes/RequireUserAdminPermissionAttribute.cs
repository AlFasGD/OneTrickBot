using System;
using Discord.Commands;
using static Discord.GuildPermission;

namespace OneTrickBot.DiscordFunctionality.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequireUserAdminPermissionAttribute : RequireUserPermissionAttribute
    {
        public RequireUserAdminPermissionAttribute()
            : base(Administrator) { }
    }
}
