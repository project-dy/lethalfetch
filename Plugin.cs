using BepInEx;
using TerminalApi;
using static TerminalApi.TerminalApi;

namespace LethalFetch
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("atomic.terminalapi", MinimumDependencyVersion: "1.2.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            var text = ""OS: FORTUNE OS
Kernel: v0.04
Uptime: ???
Packages: 5 (apt)
Shell: quota
Terminal: VT-10
CPU: BORSON 300 @ 2500 MHz
GPU: N/A
Memory:  12 MiB / 431 MiB
"";
            AddCommand("neofetch", text);
        }
    }
}
