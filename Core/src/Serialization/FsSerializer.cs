using System.Linq;
using System.Text;
using FullSerializer;

namespace Core.Serialization
{
	public sealed class FsSerializer : ISerializer
	{
		private static readonly fsSerializer Serializer = new fsSerializer();
		
		public Encoding Encoding { get; set; } = Encoding.UTF8;
		public string SerializeToJson<T>(T data)
		{
			Serializer.TrySerialize(data, out var fsData);
			StripDeserializationMetadata(ref fsData);
			return fsJsonPrinter.CompressedJson(fsData);
		}

		public byte[] SerializeToBinary<T>(T data) => Encoding.GetBytes(SerializeToJson(data));
		public T DeserializeFromJson<T>(string jsonData)
		{
			var fsData = fsJsonParser.Parse(jsonData);
			T data = default;
			Serializer.TryDeserialize(fsData, ref data);
			return data;
		}

		public T DeserializeFromBinary<T>(byte[] binaryData) => DeserializeFromJson<T>(Encoding.GetString(binaryData));

		private static void StripDeserializationMetadata(ref fsData data)
		{
			if (data.IsDictionary && data.AsDictionary.ContainsKey("$content"))
				data = data.AsDictionary["$content"];
			if (!data.IsDictionary)
				return;
			var asDictionary = data.AsDictionary;
			asDictionary.Remove("$ref");
			asDictionary.Remove("$id");
			asDictionary.Remove("$type");
			asDictionary.Remove("$version");
			for (var i = 0; i < asDictionary.Count; i++)
			{
				var pair = asDictionary.ElementAt(i);
				var child = pair.Value;
				StripDeserializationMetadata(ref child);
				asDictionary[pair.Key] = child;
			}
		}
	}
}