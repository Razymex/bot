using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmmands;
using Gommon;
using Volte.Commands.Results;

namespace Volte.Commands.Modules
{
    public sealed class HelpModule : VolteModule
    {
        [Command("Help", "H")]
        [Description("Shows the commands used for module listing, command listing, and command info.")]
        [Remarks("help")]
        public Task<ActionResult> HelpAsync(string moduleOrCommand = null)
        {
            if (moduleOrCommand is null)
            {
                return Ok(new StringBuilder()
                    .AppendLine("Hey, I'm GearBot! Here's a list of my Commands to help you out.")
                    .AppendLine(
                        $"Use `{Context.GuildData.Configuration.CommandPrefix}help {{Commandname}}` to list all commands")
                    .AppendLine()
                    .AppendLine(
                        $"Available Commands: `{CommandService.GetAllCommands().Select(x => x.Name).Join("`, `")}`")
                    .ToString());
            }

            var module = GetTargetModule(moduleOrCommand);
            var command = GetTargetCommand(moduleOrCommand);

            if (module is null && command is null)
            {
                return BadRequest($"{EmojiService.X} No matching Module/Command was found.");
            }

            if (module != null && command is null)
            {
                var commands = $"`{module.Commands.Select(x => x.FullAliases.First()).Join("`, `")}`";
                return Ok(Context.CreateEmbedBuilder().WithDescription(commands)
                    .WithTitle($"Commands for {module.SanitizeName()}"));
            }

            if (module is null && command != null)
            {
                return Ok(new StringBuilder()
                    .AppendLine($"**Command**: {command.Name}")
                    .AppendLine($"**Module**: {command.Module.SanitizeName()}")
                    .AppendLine($"**Description**: {command.Description ?? "No summary provided."}")
                    .AppendLine($"**Usage**: {command.GetUsage(Context)}")
                    .ToString());
            }

            if (module != null && command != null)
            {
                return BadRequest(new StringBuilder()
                    .AppendLine($"{EmojiService.X} Found more than one Module or Command. Results:")
                    .AppendLine($"**{module.SanitizeName()}**")
                    .AppendLine($"**{command.Name}**")
                    .ToString());
            }

            return None();
        }

        private Module GetTargetModule(string input)
            => CommandService.GetAllModules().FirstOrDefault(x => x.SanitizeName().EqualsIgnoreCase(input));

        private Command GetTargetCommand(string input)
            => CommandService.GetAllCommands().FirstOrDefault(x => x.FullAliases.ContainsIgnoreCase(input));
    }
}