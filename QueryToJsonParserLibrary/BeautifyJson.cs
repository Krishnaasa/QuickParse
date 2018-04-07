using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Text;

namespace QueryToJsonParserLibrary
{
    public class BeautifyJson
    {
        private Profile m_Profile;
        
        public BeautifyJson(Profile profile)
        {
            m_Profile = profile;
        }
        public string BeautifyJsonToString()
        {
            if (m_Profile != null)
            {
                return JsonConvert.SerializeObject(m_Profile, CreateJsonSerializerSettings());
            }
            else return null;
        }

        private static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            return settings;
        }

        public string FileWriteHelper(string payload)
        {
            if (File.Exists(ConfigurationManager.AppSettings["FileStoragePath"]))
            {
                File.Delete(ConfigurationManager.AppSettings["FileStoragePath"]);
            }

            //Create the file.
            using (FileStream fs = File.Create(ConfigurationManager.AppSettings["FileStoragePath"]))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(payload);
                fs.Write(info, 0, info.Length);

            }
            return Path.GetFullPath(ConfigurationManager.AppSettings["FileStoragePath"]);
        }
    }
    
}
