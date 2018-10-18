using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DotConnect.Memory
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args[0];
            var service = new ServiceImpl();
            var response = service.Execute(new Request {Payload = filename});
            var actualData = BinaryDataContractSerializer.Deserialize<DownloadFileResponse>(response);
                   
            Console.WriteLine($"Congratulation! You have just downloaded {actualData.Data.Length} bytes.");
            Console.WriteLine("How about the total memory consumed?");
            Console.Read();
        }
    }

    public static class BinaryDataContractSerializer
    {
        public static byte[] Serialize<T>(T obj) where T : class
        {
            if (obj == null)
                return null;
            var serializer = new DataContractSerializer(typeof(T));
            using (var memStream = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateBinaryWriter(memStream))
                {
                    serializer.WriteObject(writer, obj);
                }
                return memStream.ToArray();
            }
        }

        public static T Deserialize<T>(Response response) where T : class
        {
            if (response == null)
                return default(T);
            if (response.LargeObject != null)
                return response.LargeObject as T;
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream(response.SerializedData))
            {
                using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    return (T)serializer.ReadObject(reader);
                }
            }
        }
    }

    [DataContract]
    public class Request
    {
        [DataMember]
        public string Payload { get; set; }
    }

    [DataContract]
    public class DownloadFileResponse : LargeObject
    {
        [DataMember]
        public byte[] Data { get; set; }
    }

    [DataContract]
    public class Response
    {
        [DataMember]
        public byte[] SerializedData { get; set; }
        [DataMember]
        public string QualifiedAssemblyDataType { get; set; }
        [DataMember]
        public LargeObject LargeObject { get; set; }
    }

    /// <summary>
    /// Contracts inherit from it will not pay the serialization cost.
    /// </summary>
    [DataContract]
    [KnownType(typeof(DownloadFileResponse))]
    public abstract class LargeObject
    {

    }

    public class ServiceImpl
    {
        public Response Execute(Request request)
        {
            // Assuming the request is to read data from a file
            byte[] data = File.ReadAllBytes(request.Payload);
            var responseData = new DownloadFileResponse
            {
                Data = data
            };
            var lo = responseData as LargeObject;
            if (lo != null)
            {
                return new Response{LargeObject = lo};
            }
            return new Response
            {
                QualifiedAssemblyDataType = typeof(DownloadFileResponse).AssemblyQualifiedName,
                SerializedData = BinaryDataContractSerializer.Serialize(responseData)
            };
        }
    }
}
