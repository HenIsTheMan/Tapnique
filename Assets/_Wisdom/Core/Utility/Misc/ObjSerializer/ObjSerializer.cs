using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Genesis.Wisdom {
    internal static class ObjSerializer {
		internal static byte[] Serialize(this object obj) {
			if(obj == null) {
				return null;
			}

			BinaryFormatter binFormatter = new BinaryFormatter();
			using MemoryStream memStream = new MemoryStream();

			binFormatter.Serialize(memStream, obj);

			return memStream.ToArray();
		}

		internal static T Deserialize<T>(this byte[] byteArr) where T:
			class
		{
			if(byteArr == null) {
				return null;
			}

			using MemoryStream memStream = new MemoryStream();
			BinaryFormatter binFormatter = new BinaryFormatter();

			memStream.Write(byteArr, 0, byteArr.Length);
			memStream.Seek(0, SeekOrigin.Begin);

			return (T)binFormatter.Deserialize(memStream);
		}
	}
}