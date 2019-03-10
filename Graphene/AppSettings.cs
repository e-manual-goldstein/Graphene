using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Graphene
{
    public class AppSettings
    {
        private const string settingsPath = @"AppSettings.xml";

        private static AppSettings _instance;

        public static AppSettings Instance
        {
            get
            {
                return _instance ?? (_instance = createAppSettings());
            }
        }

        private static AppSettings createAppSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            var reader = new StreamReader(settingsPath);
            return serializer.Deserialize(reader) as AppSettings ?? new AppSettings();
        }

        public double WindowTop { get; set; }

        public double WindowLeft { get; set; }

        public bool Custom { get; set; }

        public static void SaveSettingsToFile()
        {
            var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            TextWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, Instance);
            writer.Close();
        }
    }
}
