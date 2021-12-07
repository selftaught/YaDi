using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Structs
{
    internal class Config
    {
        private bool rememberLastDllPath;
        private bool rememberLastMethod;

        private String configPath;
        private String lastDllPath;

        private ushort lastInjectionMeth;

        public Config()
        {
            // defaults
            rememberLastDllPath = true;
            rememberLastMethod = true;
            lastDllPath = "";
            lastInjectionMeth = (ushort)Enums.InjectionMethod.LoadLibrary;
            configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\YADI.Config.json";

            Load();
        }

        public bool Load()
        {
            if (!File.Exists(configPath))
            {
                File.Create(configPath);
                Save();
            }

            using (var sr = new StreamReader(configPath))
            {
                String configJsonText = sr.ReadToEnd();
#if DEBUG
                Console.WriteLine("Read text from config.json: " + configJsonText);
#endif
                try
                {
                    JObject o = JObject.Parse(configJsonText);

                    rememberLastDllPath = (bool)o.SelectToken(".RememberLastDllPath");
                    rememberLastMethod = (bool)o.SelectToken(".RememberLastInjectionMethod");

                    lastDllPath = (String)o.SelectToken(".LastDllPath");
                    lastInjectionMeth = (ushort)o.SelectToken(".LastInjectionMethod");
                }
                catch (Newtonsoft.Json.JsonReaderException e)
                {
#if DEBUG
                    Console.WriteLine("Caught (Netwonsoft.Json.JsonReaderException): " + e.Message);
#endif
                    return false;
                }
            }

#if DEBUG
            Console.WriteLine("config.rememberLastDllPath: " + rememberLastDllPath);
            Console.WriteLine("config.lastDllPath: " + lastDllPath);
#endif
            return true;
        }

        public bool Save()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("RememberLastDllPath");
                writer.WriteValue(rememberLastDllPath);
                writer.WritePropertyName("LastDllPath");
                writer.WriteValue(lastDllPath);
                writer.WritePropertyName("RememberLastInjectionMethod");
                writer.WriteValue(rememberLastMethod);
                writer.WritePropertyName("LastInjectionMethod");
                writer.WriteValue(lastInjectionMeth);
                writer.WriteEndObject();
            }

#if DEBUG
            Console.WriteLine("Writing to " + configPath);
            Console.WriteLine(sb.ToString());
#endif

            File.WriteAllText(configPath, sb.ToString());

            return true;
        }

        /**
         * Getters
         */
        public bool RememberLastDllPath()
        {
            return rememberLastDllPath;
        }

        public bool RememberLastMethod()
        {
            return rememberLastMethod;
        }

        public String LastDllPath()
        {
            return lastDllPath;
        }

        public ushort LastInjectionMethod()
        {
            return lastInjectionMeth;
        }

        /**
         * Setters
         */
        public void SetRememberLastDllPath(bool b)
        {
            rememberLastDllPath = b;
            Save();
        }

        public void SetRememberLastMethod(bool b)
        {
            rememberLastMethod = b;
            Save();
        }

        public void SetLastDllPath(String path)
        {
            lastDllPath = path;
            Save();
        }

        public void SetLastMethod(ushort m)
        {
            lastInjectionMeth = m;
            Save();
        }
    }
}
