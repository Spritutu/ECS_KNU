
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

namespace Scanlab.Sirius
{
    public sealed class PensSerializer
    {
        public static IPens Open(string fileName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Binder = (SerializationBinder)new SiriusTypeNameSerializationBinder("SpiralLab.Sirius.{0}")
            };
            TextReader textReader = (TextReader)null;
            try
            {
                textReader = (TextReader)new StreamReader(fileName);
                return JsonConvert.DeserializeObject<IPens>(textReader.ReadToEnd(), settings);
            }
            finally
            {
                textReader?.Close();
            }
        }

        public static bool Save(IPens pens, string fileName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Binder = (SerializationBinder)new SiriusTypeNameSerializationBinder("SpiralLab.Sirius.{0}")
            };
            TextWriter textWriter = (TextWriter)null;
            pens.FileName = fileName;
            try
            {
                string str = JsonConvert.SerializeObject((object)pens, Formatting.Indented, settings);
                textWriter = (TextWriter)new StreamWriter(fileName, false);
                textWriter.Write(str);
            }
            finally
            {
                textWriter?.Close();
            }
            return true;
        }
    }
}
