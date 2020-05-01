using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Newtonsoft.Json;
using ParasiteIO.Core.Collections;


namespace ParasiteIO.Core.Sync
{
    public  class PushData:IPushData
    {

        public  void PushDataLocal (DataContainer data, string fileName)
        {

            // Check if folder exists, if not create it.
            if (!Directory.Exists(FolderInfo.dirPath))
            {
                Directory.CreateDirectory(FolderInfo.dirPath);
            }

            string pathToFolder = Path.Combine(FolderInfo.dirPath, fileName);

            FileStream fs = new FileStream(pathToFolder, FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, data);
                
                
            }
            catch (SerializationException e)
            {
                throw new SerializationException("Failed to serialize: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
        }


   


        private static byte[] GetAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


        public void PushDataServer(DataContainer data, string url)
        {
            throw new NotImplementedException();
        }


        public async void PushDataServerAsync(DataContainer data, string url)
        {
            throw new NotImplementedException();
        }




        private  async void ReadResponse(HttpResponseMessage response)
        {
            //byte[] dataResponse = await response.Content.ReadAsByteArrayAsync();

            //using (var memStream = new MemoryStream())
            //{
            //    var binForm = new BinaryFormatter();
            //    memStream.Write(dataResponse, 0, dataResponse.Length);
            //    memStream.Seek(0, SeekOrigin.Begin);
            //    var obj = binForm.Deserialize(memStream);
            //    return obj;
            //}
        }


        //private DataContainer ByteArrayToObject(byte[] arrBytes)
        //{
        //    using (var memStream = new MemoryStream())
        //    {
        //        var binForm = new BinaryFormatter();
        //        memStream.Write(arrBytes, 0, arrBytes.Length);
        //        memStream.Seek(0, SeekOrigin.Begin);
        //        DataContainer obj = binForm.Deserialize(memStream);
        //        return obj;
        //    }
        //}


        private byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

    }


}
