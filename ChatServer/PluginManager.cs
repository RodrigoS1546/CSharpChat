using System;
using System.IO;
using System.Collections.Generic;

namespace ChatServer
{
    public class PluginManager
    {
        protected List<Plugin> _plugins = new List<Plugin>();

        public void LoadPlugins()
        {
            // Check if plugins directory exists
            if (!Directory.Exists("plugins"))
                return;

            // Gets the files inside plugins
            string[] pluginFiles = Directory.GetFiles("plugins");

            // Goes through every single file
            foreach (string pluginFile in pluginFiles)
            {
                FileInfo info = new FileInfo(pluginFile); // Gets file info
                // Skips the file if extension is not .dll
                if (info.Extension != ".dll")
                    continue;
                _plugins.Add(new Plugin(info.FullName)); // Registers the plugin
            }
                
        }

        public void EmitEvent(string eventname, object[] args)
        {
            // Goes through every plugin
            foreach (Plugin plugin in _plugins)
                plugin.EmitEvent(eventname, args);
            // Emits the event on every plugin
        }
    }
}
