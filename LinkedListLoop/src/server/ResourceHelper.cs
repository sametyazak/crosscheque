using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListLoop.src.server
{
    public class ResourceHelper
    {
        public static string GetString(string key, CultureInfo culture)
        {
            ResourceManager resourceMgr = GetResourceManger();

            string resource = resourceMgr.GetString(key, culture);

            return resource != null ? resource : key;

        }

        public static string GetString(string key)
        {
            try
            {
                ResourceManager resourceMgr = GetResourceManger();

                string resource = resourceMgr.GetString(key, GlobalConfiguration.CurrentCulture);

                return resource != null ? resource : key;
            }
            catch (Exception ex)
            {
                LogManager.InsertExceptionLog(ex);
                return key;
            }
        }

        public static string GetResourcePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"resources");
        }

        public static ResourceManager GetResourceManger()
        {
            return ResourceManager.CreateFileBasedResourceManager("strings", GetResourcePath(), null);
        }

        public static ResourceSet GetResourceSet(CultureInfo culture)
        {
            return GetResourceManger().GetResourceSet(culture, false, false);
        }
    }
}
