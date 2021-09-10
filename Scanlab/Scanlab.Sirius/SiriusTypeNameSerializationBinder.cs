
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;


namespace Scanlab.Sirius
{
    internal sealed class SiriusTypeNameSerializationBinder : SerializationBinder
    {
        public string TypeFormat { get; private set; }

        public SiriusTypeNameSerializationBinder(string typeFormat) => this.TypeFormat = typeFormat;

        public override void BindToName(
          System.Type serializedType,
          out string assemblyName,
          out string typeName)
        {
            assemblyName = (string)null;
            typeName = serializedType.Name;
        }

        public override System.Type BindToType(string assemblyName, string typeName)
        {
            if (typeName.Equals("Vector2"))
                return typeof(Vector2);
            if (typeName.Equals("Vector2[]"))
                return typeof(Vector2[]);
            if (typeName.Equals("Vector3"))
                return typeof(Vector3);
            if (typeName.Equals("Vector3[]"))
                return typeof(Vector3[]);
            if (typeName.Equals("InsertVertex"))
                return typeof(InsertVertex);
            if (typeName.Equals("InsertVertex[]"))
                return typeof(InsertVertex[]);
            string typeName1 = string.Format(this.TypeFormat, (object)typeName);
            try
            {
                return System.Type.GetType(typeName1, true);
            }
            catch (Exception ex)
            {
                string name = Assembly.GetEntryAssembly().GetName().Name;
                return System.Type.GetType(typeName1 + ", " + name, true);
            }
        }
    }

    internal sealed class SiriusTypeNameSerializationBinder2 : ISerializationBinder
    {
        public IList<System.Type> KnownTypes { get; set; }

        public SiriusTypeNameSerializationBinder2() => this.KnownTypes = (IList<System.Type>)new List<System.Type>()
    {
      typeof (DocumentDefault),
      typeof (Vector2),
      typeof (Vector2[]),
      typeof (InsertVertex),
      typeof (InsertVertex[]),
      typeof (BoundRect)
    };

        public System.Type BindToType(string assemblyName, string typeName) => this.KnownTypes.SingleOrDefault<System.Type>((Func<System.Type, bool>)(t => t.Name == typeName));

        public void BindToName(System.Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = (string)null;
            typeName = serializedType.Name;
        }
    }
}
