using System.Text;

namespace Core.Serialization
{
	public static class Serialization
	{
		private static ISerializer serializer;

		private static ISerializer Serializer
		{
			get
			{
				if (serializer == null) Init();
				return serializer;
			}
		}

		public static Encoding Encoding
		{
			get => Serializer.Encoding;
			set => Serializer.Encoding = value;
		}

		public static void Init(SerializerType type = SerializerType.UnitySerializer)
		{
			switch (type)
			{
				case SerializerType.UnitySerializer:
					serializer = new UnitySerializer();
					break;
				case SerializerType.FsSerializer:
					serializer = new FsSerializer();
					break;
				default:
					serializer = new UnitySerializer();
					break;
			}
		}

		public static string SerializeToJson<T>(T data) => Serializer.SerializeToJson(data);
		public static byte[] SerializeToBinary<T>(T data) => Serializer.SerializeToBinary(data);
		public static T DeserializeFromJson<T>(string jsonData) => Serializer.DeserializeFromJson<T>(jsonData);
		public static T DeserializeFromBinary<T>(byte[] binaryData) => Serializer.DeserializeFromBinary<T>(binaryData);
	}
}