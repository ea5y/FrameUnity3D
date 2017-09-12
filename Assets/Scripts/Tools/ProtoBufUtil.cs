//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-12 11:10
//================================

using System;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime;

namespace Easy.FrameUnity.Util
{
    public class ProtoBufUtil
    {
        private static RuntimeTypeModel _typeModel;

        //For auto compress
        //byte[]' length > _compressOutLength enable compress
        private static int _compressOutLength;

        static ProtoBufUtil()
        {
            _compressOutLength = 10240;
            Initialize();
        }

        private static void Initialize()
        {
            _typeModel = TypeModel.Create();
            _typeModel.UseImplicitZeroDefaults = false;
        }

        //Serialize to binary file
        public static void Serialize(object obj, string serializeFilePath)
        {
            using(var fs = new FileStream(serializeFilePath, FileMode.Create))
            {
                _typeModel.Serialize(fs, obj);
            }
        }

        //Deserialize from file
        public static object Deserialize(string serializeFilePath, Type type)
        {
            using(var fs = new FileStream(serializeFilePath, FileMode.Open))
            {
                return _typeModel.Deserialize(fs, null, type);
            }
        }

        //Serialize to byte[]
        public static Byte[] Serialize(object obj)
        {
            return SerializeAutoGZip(obj);
        }

        private static Byte[] SerializeAutoGZip(object obj, bool autoUseGZip = true) 
        {
            using(var memory = new MemoryStream())
            {
                _typeModel.Serialize(memory, obj);
                Byte[] bytes = memory.ToArray();

                if(autoUseGZip)
                {
                    GZipCompress(ref bytes);
                }
                return bytes;
            }
        }

        //Deserialize from byte[]
        public static T Deserialize<T>(Byte[] data)
        {
            return (T)DeserializeAutoGZip(data, typeof(T), true);
        }

        private static object DeserializeAutoGZip(Byte[] data, Type type, bool isGZip)
        {
            try
            {
                if(!isGZip)
                {
                    return _typeModel.Deserialize(new MemoryStream(data), null, type);
                }
                else
                {
                    return GZipDecompress(data, type);
                }

            }
            catch(Exception ex)
            {
                var msg = string.Format("ProtoBuf deserialize type: {0}\n error: {1}", type.FullName, ex.Message);
                throw new Exception(msg);
            }
        }

        private  static object GZipDecompress(Byte[] bytes, Type type)
        {
            using(MemoryStream stream = new MemoryStream())
            {
            Start:
                if(IsCompress(bytes))
                {
                    using(MemoryStream ms = new MemoryStream(bytes, 4, bytes.Length - 4))
                    using(GZipStream gzs = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        //gzs.CopyTo(stream);
                        CopyTo(gzs, stream);
                        var buf = stream.GetBuffer();
                        if(IsCompress(buf))  //For multi gzip
                        {
                            bytes = stream.ToArray();
                            stream.SetLength(0);
                            goto Start;
                        }
                    }
                }
                else
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                stream.Seek(0, SeekOrigin.Begin);
                return _typeModel.Deserialize(stream, null, type);
            }
        }

        private static long CopyTo(Stream source, Stream dest)
        {
            byte[] buffer = new byte[2048];
            int bytesRead;
            long totalBytes = 0;
            while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, bytesRead);
                totalBytes += bytesRead;
            }
            return totalBytes;
        }

        private static bool IsCompress(Byte[] bytes)
        {
            if(bytes.Length >= 14 && bytes[0] == 0 && bytes[1] == 0 && bytes[2] == 0 && bytes[3] == 0 && bytes[4] == 0x1F && bytes[5] == 0x8B)
            {
                return true;
            }
            return false;
        }

        private static void GZipCompress(ref Byte[] bytes)
        {
            if(_compressOutLength > 0 && bytes.Length > _compressOutLength)
            {
                using(var gms = new MemoryStream())
                using(var gzs = new GZipStream(gms, CompressionMode.Compress, true))
                {
                    gms.Position = 4;
                    gzs.Write(bytes, 0, bytes.Length);
                    gzs.Close();
                    bytes = gms.ToArray();
                }
            }
        }

        //==============
        //@TODO remain
        //==============
    }
}
