using System.Text;

namespace Core.Serialization
{
	public interface ISerializer
	{
		Encoding Encoding { get; set; }
		
		string SerializeToJson<T>(T data);
		byte[] SerializeToBinary<T>(T data);
		T DeserializeFromJson<T>(string jsonData);
		T DeserializeFromBinary<T>(byte[] binaryData);
	}
}