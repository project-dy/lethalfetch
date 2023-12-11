using BepInEx;
using TerminalApi;
using System;
using static TerminalApi.TerminalApi;
using static TerminalApi.Events.Events;

namespace LethalFetch
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("atomic.terminalapi", MinimumDependencyVersion: "1.2.0")]
    public class Plugin : BaseUnityPlugin
    {
        private string template = @"
+---------+ OS: Fortune OS
|         | Kernel: GNU/Hurd 0.1.2-3
| ({0}) | Uptime: {1}
|         | Packages: 6 (braken) 9 (rpm)
+---------+ Shell: quota
            Terminal: VT-33000
            CPU: BORSON 300 @ 2500 MH
            GPU: Nvidia Tesla V900
            Memory:  {2} MiB / 448 MiB"; // 431, 448 is the closest one that makes sense

            private int ramUsed;
        private const int ramMax = 448;
        private Random rand;
        private TerminalKeyword keyword;
        private DateTime boot;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // subscribe to events
            TerminalBeginUsing += RecordBootTime;
            // update the text to contain accurate uptime
            TerminalTextChanged += updateKeyword;

            template.Trim(); // trim staring newline
            template += "\n"; // add newline so the prompt is where expected

            // init random;
            rand = new Random();

            keyword = CreateTerminalKeyword("neofetch", template);
            AddTerminalKeyword(keyword);
            Logger.LogInfo("Added keyword neofetch");
        }

        private void updateKeyword(Object sender, TerminalTextChangedEventArgs e) {
            // find uptime string
            String uptimeStr;
            var uptime = DateTime.Now - boot;
            if (uptime.Minutes == 0) {
                uptimeStr = $"{uptime.Seconds} seconds";
            } else {
                uptimeStr = $"{uptime.Minutes} minutes, {uptime.Seconds} seconds";
            }

            // find the ramUsed string
            ramUsed += Math.Abs(rand.Next(-5, 6));
            String ramUsedStr = $"{ramUsed % ramMax}";

            // find the other string
            String other = rand.Next(10) > 2 ? "image" : "anime";

            // update text
            var text = String.Format(template, other, uptimeStr, ramUsedStr);
            // update keyword
            keyword = CreateTerminalKeyword("neofetch", text, true);
            UpdateKeyword(keyword);
        }

        private void RecordBootTime(object sender, TerminalEventArgs e) {
            Logger.LogInfo("Recorded boot time");
            boot = DateTime.Now;
        }


    }
}
