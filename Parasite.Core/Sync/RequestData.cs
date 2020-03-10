using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Data;

namespace Parasite.Core.Sync
{
    public  class RequestData :IRequestData
    {
       

        /// <summary>
        /// Requests data (deserializes) from local disk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public  DataContainer RequestDataLocal(string fileName) 
        {

            DataContainer data = null;

            // Get full path, includes fileName
            string pathToFolder = Path.Combine(FolderInfo.dirPath, fileName);

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(pathToFolder, FileMode.Open);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // assign the reference to the local variable.
                data = (DataContainer)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                throw new SerializationException("Failed to deserialize: " + e.Message);

            }

            finally
            {
                fs.Close();
            }

            return data;

        }


        public DataContainer RequestDataServer (string id)
        {
            throw new NotImplementedException();
        }


    }
}
