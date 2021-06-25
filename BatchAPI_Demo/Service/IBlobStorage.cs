using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI_Demo.Service
{
    public interface IBlobStorage
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string ContainerName);
        Task<string> UploadFileToBlob(string ContainerName);
    }  
}
