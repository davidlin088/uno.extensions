using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Loader
{
    internal class ModuleLoadContext : AssemblyLoadContext
    {
        public ModuleLoadContext(string modulePath)
            : base(isCollectible: true)
        {
            Resolver = new AssemblyDependencyResolver(modulePath);
        }

        private AssemblyDependencyResolver Resolver { get; }

        //This will not check nuget cache so if we need that support for development
        //a developer loadcontext should be created. For production packages we should
        //have rules in place that specify where we will check for dependencies so that
        //we can reduce the surface area of malicious dlls
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = Resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? assemblyPath = Resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (assemblyPath != null)
            {
                return LoadUnmanagedDllFromPath(assemblyPath);
            }

            return IntPtr.Zero;
        }
    }
}
