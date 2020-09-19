using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace ChatServer
{
	public class Plugin
	{
		protected readonly string _pluginFile;
		private readonly object _pluginInst;
		private readonly Type _pluginType;
		
		public Plugin(string pluginFile)
		{
			_pluginFile = pluginFile;

			// Loads the dll file
			Assembly pluginLibrary = Assembly.LoadFile(_pluginFile);

			// Goes through every single exported type
			foreach (Type type in pluginLibrary.GetExportedTypes())
			{
				try
				{
					// If it implements IPlugin it registers the plugin instance
					if (type.GetInterfaces().Contains(typeof(IPlugin)))
                    {
						_pluginType = type;
						_pluginInst = Activator.CreateInstance(type);
						return;
					}
				}
				catch {}
			}
		}
		
		public void EmitEvent(string eventname, object[] args) {
			// Call the event
			try
            {
				// Find a method that has ChatServerEventHandlerAttribute
				// And Invokes it if the event corresponds
				foreach (MethodInfo methodInfo in _pluginType.GetMethods())
                {
					IEnumerable<CustomAttributeData> cads = methodInfo.CustomAttributes;

					foreach (CustomAttributeData cad in cads)
					{
						if (cad.AttributeType != typeof(ChatServerEventHandlerAttribute))
							continue;

						ChatServerEventHandlerAttribute attr = (ChatServerEventHandlerAttribute)methodInfo.GetCustomAttribute(cad.AttributeType);

						if (eventname == attr.EventName)
							methodInfo.Invoke(_pluginInst, args);

						break;
					}
				}
			}
			catch {}
		}
	}
}
