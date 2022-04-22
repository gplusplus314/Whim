using System;
using System.Collections.Generic;

namespace Whim;

public class PluginManager : IPluginManager
{
	private readonly List<IPlugin> _plugins = new();
	private bool disposedValue;

	public IEnumerable<IPlugin> AvailablePlugins => _plugins;

	public void PreInitialize()
	{
		Logger.Debug("Pre-initializing plugin manager...");

		foreach (IPlugin plugin in _plugins)
		{
			plugin.PreInitialize();
		}
	}

	public void PostInitialize()
	{
		Logger.Debug("Post-initializing plugin manager...");

		foreach (IPlugin plugin in _plugins)
		{
			plugin.PostInitialize();
		}
	}

	public T RegisterPlugin<T>(T plugin) where T : IPlugin
	{
		_plugins.Add(plugin);
		return plugin;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				foreach (IPlugin plugin in _plugins)
				{
					if (plugin is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}
			}

			disposedValue = true;
		}
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		System.GC.SuppressFinalize(this);
	}
}
