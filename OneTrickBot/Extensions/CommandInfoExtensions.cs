using System.Linq;
using Discord.Commands;

namespace OneTrickBot.Extensions
{
    public static class CommandInfoExtensions
    {
        public static bool HasPrecondition<T>(this CommandInfo info)
            where T : PreconditionAttribute
        {
            return info.Preconditions.Any(a => a is T) || info.Module.HasPrecondition<T>();
        }
    }
}
