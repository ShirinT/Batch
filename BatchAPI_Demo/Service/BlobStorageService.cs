using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BatchAPI_Demo.Service
{
    public class BlobStorageService : IBlobStorage
    {
        private readonly IConfiguration _config;

        public BlobStorageService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> UploadFileToBlob(string ContainerName)
        {
            try
            {
                string file = @"C:\Users\Shirin\source\repos\Final\BatchAPI_Demo\BatchAPI_Demo\wwwroot\TestFile.txt";
                var fileName = Path.GetFileName(file);
                byte[] fileData = new byte[file.Length];

                return Task.Run(() => this.UploadFileToBlobAsync(file, fileData, ContainerName));
                // var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, ContainerName));
                // _task.Wait();
                //  string fileUrl = _task.Result;
                //  return fileUrl;
                // return _task.Result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string ContainerName)
        {
            try
            {
                string AccessKey = _config.GetValue<string>("AppSettings:AccessKey");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AccessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                //   string strContainerName = "batch";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (strFileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(strFileName);
                    //  cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "Success";
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

    }
}
