using System.IO;

namespace ArchEngine.GUI.Editor
{
    using System.Runtime.Loader;
    using System.Collections.Concurrent;
    using System.Reflection;

    public class AssemblyManager
    {
        private static readonly ConcurrentDictionary<string, AssemblyLoadContext> _loadContexts = new ConcurrentDictionary<string, AssemblyLoadContext>();
        private static readonly ConcurrentDictionary<string, Assembly> _assemblies = new ConcurrentDictionary<string, Assembly>();

        public static Assembly Load(string assemblyPath)
        {
            assemblyPath = Path.GetFullPath(assemblyPath);
            if (_assemblies.TryGetValue(assemblyPath, out var assembly))
            {
                return assembly;
            }

            var loadContext = new AssemblyLoadContext(assemblyPath, isCollectible: true);
            assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
            _loadContexts.TryAdd(assemblyPath, loadContext);
            _assemblies.TryAdd(assemblyPath, assembly);
            return assembly;
        }

        public static void Unload(string assemblyPath)
        {
            if (_loadContexts.TryRemove(assemblyPath, out var loadContext))
            {
                _assemblies.TryRemove(assemblyPath, out var _);
                loadContext.Unload();
            }
        }
    }
}