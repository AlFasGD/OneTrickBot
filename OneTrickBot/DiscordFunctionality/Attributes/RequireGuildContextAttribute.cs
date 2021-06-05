using System;
using Discord.Commands;

namespace OneTrickBot.DiscordFunctionality.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequireGuildContextAttribute : RequireContextAttribute
    {
        public RequireGuildContextAttribute()
            : base(ContextType.Guild) { }
    }
}
