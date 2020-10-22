using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Discord.Commands;

namespace Nerdomat.Tools
{
    public static class CommandsCrawler
    {
        private static IList<Command> _commands;
        static CommandsCrawler()
        {
            _commands = new List<Command>();
            Crawl();

            var alignLength = _commands.Max(x => x.FullName.Length < 40 ? 40 : x.FullName.Length);
            foreach (var cmd in _commands)
                cmd.AlignLength = alignLength;
        }

        private static void Crawl()
        {
            var classes = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .Where(x => x.IsClass)
                                  .Where(x => x.IsSubclassOf(typeof(ModuleBase<SocketCommandContext>)))
                                  .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ModuleActive)))
                                  .ToList();

            foreach (var cmdClass in classes)
            {
                var active = (bool)(cmdClass.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(ModuleActive))?.ConstructorArguments.FirstOrDefault().Value ?? false);
                var moduleName = cmdClass.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(ModuleName))?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? cmdClass.Name;
                if (!active) continue;

                var methods = cmdClass.GetMethods()
                                      .Where(x => x.IsPublic)
                                      .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(CommandAttribute)))
                                      .ToList();

                foreach (var cmd in methods)
                {
                    var cmdParams = cmd.GetParameters()
                                       .Select(x => new CommandParams { Type = x.ParameterType.TypeNameOrAlias(), Name = x.Name })
                                       .ToList();

                    var name = cmd.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(CommandAttribute))?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;
                    var summary = cmd.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(SummaryAttribute))?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;
                    var adminOlny = cmd.CustomAttributes.Any(x => x.AttributeType == typeof(MethodAdmin));

                    var cmdDescription = new Command
                    {
                        Module = moduleName,
                        Method = cmd.Name,
                        Name = $"!{name}",
                        Description = summary,
                        Params = cmdParams,
                        AdminOnly = adminOlny
                    };

                    _commands.Add(cmdDescription);
                }
            }
        }

        public static string GetCommandsList(bool admin)
        {
            var sb = new StringBuilder();
            var modules = admin
                ? _commands.GroupBy(x => x.Module)
                : _commands.Where(x => !x.AdminOnly).GroupBy(x => x.Module);

            foreach (var mod in modules)
            {
                sb.AppendLine($"{mod.Key}:".Decorate(Decorator.Underline_bold));
                foreach (var cmd in mod)
                {
                    sb.AppendLine(cmd.ToString());
                }

                sb.AppendLine(string.Empty);
            }

            var res = sb.ToString();
            return string.IsNullOrEmpty(res) ? "Brak komend" : res;
        }
    }

    public class Command
    {
        public string Module { get; set; }
        public string Method { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<CommandParams> Params { get; set; }
        public bool AdminOnly { get; set; }

        public int? AlignLength { get; set; }

        public string FullName
        {
            get
            {
                var p = string.Empty;
                foreach (var param in Params)
                    p += $" {param.ToString()}";

                return Name + p;
            }
        }

        public override string ToString()
        {
            return AdminOnly
                ? $"{FullName.AlginText(AlignLength).Decorate(Decorator.Inline_code, true)} - {Description} {"(admin)".Decorate(Decorator.Bold)}"
                : $"{FullName.AlginText(AlignLength).Decorate(Decorator.Inline_code, true)} - {Description}";
        }
    }

    public struct CommandParams
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public override string ToString() => $"[{Type} {Name}]";
    }
}