using System;
using System.IO;
using System.Xml.Serialization;

namespace Model.XMLParser
{
    class XMLManager<T>
    {
        public void DeserealizeObj(string filename, ref T deserealize)
        {

            if (!File.Exists(filename))
            {
                Console.WriteLine("Settings not existend, creating new Settings File");
                SeralizeObj(filename, deserealize);
            }

            XmlSerializer serializer = new XmlSerializer(deserealize.GetType());

            // A FileStream is needed to read the XML document.
            StreamReader reader = new StreamReader(filename);


            // Declare an object variable of the type to be deserialized.


            // Use the Deserialize method to restore the object's state.
            try
            {
                deserealize = (T)serializer.Deserialize(reader);
            }catch(System.InvalidOperationException xmlE)
            {
                Console.WriteLine(xmlE);
                reader.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                const string SettingsWarning = "Settings malformed, creating new default Settings File";
                Console.WriteLine(SettingsWarning);
                Console.ResetColor();
                SeralizeObj(filename, deserealize);
                return;
            }
            reader.Close();

        }
        public void SeralizeObj(string filename, T serealize)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, serealize);
            }
        }


    }
}


