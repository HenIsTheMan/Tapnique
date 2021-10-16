using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Genesis.Wisdom {
	internal static class ObjCopier {
		internal static T DeepCopy<T>(this T src) {
			if(!typeof(T).IsSerializable) {
				throw new ArgumentException("T is not serializable!", nameof(src));
			}

			if(src is null) {
				return default; //default(T) also can
			}

			IFormatter binFormatter = new BinaryFormatter();
			Stream memStream = new MemoryStream();

			using(memStream) {
				binFormatter.Serialize(memStream, src);
				memStream.Seek(0, SeekOrigin.Begin);
				return (T)binFormatter.Deserialize(memStream);
			}
		}
	}
}