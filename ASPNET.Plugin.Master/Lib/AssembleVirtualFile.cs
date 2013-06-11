using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ASPNET.Plugin.Master.Lib
{
    public class AssembleVirtualFile : System.Web.Hosting.VirtualFile
    {
        string path;
        public AssembleVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            path = VirtualPathUtility.ToAppRelative(virtualPath);
        }
        public override System.IO.Stream Open()
        {
            //string[] parts = path.Split('/');
            
            string assemblyName = "ASPNET.Plugin.PluginOne.dll";
            string[] parts = path.Split('/');
            string resourceName = "ASPNET.Plugin.PluginOne";

            foreach (string part in parts)
            {
                if (part == "~" || part == "") continue;
                resourceName += ".";
                resourceName += part;
            }

            assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(assemblyName);
            if (assembly != null)
            {
                Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
                return resourceStream;
            }
          
            return null;

        }
    }
}