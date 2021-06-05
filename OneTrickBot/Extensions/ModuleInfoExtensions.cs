using System.Linq;
using Discord.Commands;

namespace OneTrickBot.Extensions
{
    public static class ModuleInfoExtensions
    {
        public static bool HasPrecondition<T>(this ModuleInfo info)
            where T : PreconditionAttribute
        {
            return info.Preconditions.Any(a => a is T);
        }
    }
}
