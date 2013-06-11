using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ASPNET.Plugin.Master.Lib
{
    public class AssembleVirtualPathProvider: System.Web.Hosting.VirtualPathProvider
    {
        public AssembleVirtualPathProvider() { }

        private bool IsAppResourcePath(string virtualPath)
        {
            String checkPath =
               VirtualPathUtility.ToAppRelative(virtualPath);

            string assemblyName = "ASPNET.Plugin.PluginOne.dll";
            string[] parts = virtualPath.Split('/');
            string resourceName = "ASPNET.Plugin.PluginOne";

            foreach (string part in parts)
            {
                if (part == "~" || part == "") continue;
                resourceName += ".";
                resourceName += part;
            }

            

            assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(assemblyName);



            var manifest = assembly.GetManifestResourceInfo(resourceName);

            return null != manifest;

            //return checkPath.StartsWith("~/Views/home/Index.cshtml", StringComparison.InvariantCultureIgnoreCase);
       
        }
        public override bool FileExists(string virtualPath)
        {            
            return base.FileExists(virtualPath) || IsAppResourcePath(virtualPath);
        }
        public override System.Web.Hosting.VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
                return new AssembleVirtualFile(virtualPath);
            else
                return base.GetFile(virtualPath);
        }
        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
                return null;
            else
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

    }
}