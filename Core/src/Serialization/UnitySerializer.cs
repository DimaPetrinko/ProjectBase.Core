using System.Text;
using UnityEngine;

namespace Core.Serialization
{
	public sealed class UnitySerializer : ISerializer
	{
		public Encoding Encoding { get; set; } = Encoding.UTF8;
		public string SerializeToJson<T>(T data) => JsonUtility.ToJson(data);
		public byte[] SerializeToBinary<T>(T data) => Encoding.UTF8.GetBytes(SerializeToJson(data));
		public T DeserializeFromJson<T>(string jsonData) => JsonUtility.FromJson<T>(jsonData);
		public T DeserializeFromBinary<T>(byte[] binaryData) => DeserializeFromJson<T>(Encoding.UTF8.GetString(binaryData));
	}
}