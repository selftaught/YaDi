using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Reflection;

using YaDi.Enums;
using System.Windows.Forms;

namespace YaDi.Structs
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
            rememberLastDllPath = true;
            rememberLastMethod = true;
            lastDllPath = "";
            lastInjectionMeth = (ushort)InjectionMethod.LoadLibrary;
            configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\yadi.json";

            Load();
        }

        public bool Load()
        {
            if (!File.Exists(configPath))
            {
                File.Create(configPath);
                Save();
            }

            using (var streadReader = new StreamReader(configPath))
            {
                String configJsonText = streadReader.ReadToEnd();
                try
                {
                    JObject o = JObject.Parse(configJsonText);

                    lastDllPath = (String)o.SelectToken(".LastDllPath");
                    lastInjectionMeth = (ushort)o.SelectToken(".LastInjectionMethod");
                    rememberLastDllPath = (bool)o.SelectToken(".RememberLastDllPath");
                    rememberLastMethod = (bool)o.SelectToken(".RememberLastInjectionMethod");
                }
                catch (Newtonsoft.Json.JsonReaderException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            return true;
        }

        public bool Save()
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);

            using (JsonWriter writer = new JsonTextWriter(stringWriter))
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

            using (StreamWriter file = new StreamWriter(configPath))
            {
                file.Write(stringBuilder.ToString());
            }

            File.WriteAllText(configPath, stringBuilder.ToString());

            return true;
        }

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
