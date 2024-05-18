using BepInEx;
using TerminalApi;
using System;
using static TerminalApi.TerminalApi;
using static TerminalApi.Events.Events;
// using TerminalApi.Events;

/*
Welcome to the FORTUNE-9 OS
	Courtesy of the Company

Happy [currentDay].

Type "Help" for a list of commands.

[TooManyEmotes]
Type "Emotes" for a list of commands.





 */

namespace LethalFetch
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("atomic.terminalapi", MinimumDependencyVersion: "1.2.0")]
    public class Plugin : BaseUnityPlugin
    {
        private string orgtemplate = @"
Welcome to the FORTUNE-9 OS
	Courtesy of the Company

Happy [currentDay].

Type ""Help"" for a list of commands.

[TooManyEmotes]
Type ""Emotes"" for a list of commands.




";
        private string template = @"
+---------+ OS: Fortune OS
|         | Kernel: GNU/Hurd v45
| ({0}) | Uptime: {1}
|         | Packages: 6 (bracken) 9 (rpm)
+---------+ Shell: quota
            Terminal: VT-33000
            CPU: BORSON 300 @ 2500 MHz
            GPU: Nvidia Tesla V900
            Memory:  {2} MiB / 448 MiB"; // 431, 448 is the closest one that makes sense

        private int ramUsed;
        private const int ramMax = 448;
        private Random rand;
        private TerminalKeyword keyword;
        private DateTime boot;
        private string theText;

        /*private string getSpecialNodeList()
        {
            string result = "";
            foreach (var node in SpecialNodes)
            {
                result += node.displayText + ", ";
            }
            return result;
        }*/

        private void NeoFetchOnInit(object sender, TerminalEventArgs e)
        {
            Logger.LogMessage("Terminal is awake");

            updateKeyword(sender, null);
            // show it
            TerminalNode node = new TerminalNode();
            node.displayText = theText;
            //node.displayText = SpecialNodes[1].displayText;
            node.clearPreviousText = true;
            //SpecialNodes.Add(node);
            SpecialNodes[13] = node;
            Logger.LogMessage("Added node");
        }

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // subscribe to events
            TerminalBeginUsing += RecordBootTime;
            TerminalBeginUsing += NeoFetchOnInit;
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

        //private 


        private void updateKeyword(Object sender, TerminalTextChangedEventArgs e) {
            Logger.LogInfo(SpecialNodes[1].displayText);
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
            theText = text;
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
