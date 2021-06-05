using System;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using OneTrickBot.DiscordFunctionality;
using OneTrickBot.DiscordFunctionality.TypeReaders;

namespace OneTrickBot.Extensions
{
    public static class CommandServiceExtensions
    {
        private static readonly Type[] baseTypeReaders;
        
        static CommandServiceExtensions()
        {
            var allTypes = typeof(TypeReader).Assembly.GetTypes();
            baseTypeReaders = allTypes.Where(FilterTypeReaderType).ToArray();
        }

        private static bool FilterTypeReaderType(Type t)
        {
            return (t.Namespace?.StartsWith(typeof(UserTypeReader<>).Namespace) == true)
                && t.Name.EndsWith(typeof(UserTypeReader<>).Name[("User".Length)..]);
        }

        public static void AddTypeReader<TObject, TReader>(this CommandService service)
            where TReader : TypeReader<TObject>, new()
        {
            service.AddTypeReader<TObject>(new TReader());
        }
        public static void AddTypeReaders(this CommandService service, Assembly assembly)
        {
            var typeReaders = assembly.GetTypes().Where(t => t.GetCustomAttribute<MandatoryTypeReaderAttribute>() != null);
            foreach (var t in typeReaders)
            {
                var typeReaderBaseType = t;

                while (!typeReaderBaseType.IsGenericType || !baseTypeReaders.Contains(typeReaderBaseType.GetGenericTypeDefinition()))
                    typeReaderBaseType = typeReaderBaseType.BaseType;

                service.AddTypeReader(typeReaderBaseType.GenericTypeArguments[0], t.GetConstructor(Type.EmptyTypes).Invoke(null) as TypeReader);
            }
        }
    }
}
