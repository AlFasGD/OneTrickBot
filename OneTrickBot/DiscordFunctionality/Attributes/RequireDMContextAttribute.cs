using System;
using Discord.Commands;

namespace OneTrickBot.DiscordFunctionality.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequireDMContextAttribute : RequireContextAttribute
    {
        public RequireDMContextAttribute()
            : base(ContextType.DM) { }
    }
}
