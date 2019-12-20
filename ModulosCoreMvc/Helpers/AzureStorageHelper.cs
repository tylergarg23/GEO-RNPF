using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SERFOR.Component.DTEntities.General;
using System;
using System.IO;

namespace Modulos_Core_MVC.Helpers
{
    public static class AzureStorageHelper
    {

        public enum ContainerFlolder
        {
            Anexos,
            Coordenadas,
            Logos
        }

        public static string UploadFile(Stream file,  string fileName, ContainerFlolder folder)
        {
            var blockBlob = getBlockBlob(fileName, folder);

            blockBlob.UploadFromStream(file, file.Length);

            return blockBlob.Uri.AbsoluteUri;
        }

        public static bool DeleteFile(string fileName, ContainerFlolder folder)
        {
            bool exito = true;
            try
            {
                var blockBlob = getBlockBlob(fileName, folder);
                
                blockBlob.Delete();

            }
            catch { exito = false; }

            return exito;
        }


       
        private static CloudBlockBlob getBlockBlob(string fileName, ContainerFlolder folder)
        {
            CloudStorageAccount storageAccount;

            try
            {
                storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            }
            catch (StorageException e)
            {
                throw e;
            }

            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("rnpfiles");

            if (container.CreateIfNotExists())
            {
                container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            string blob = string.Empty;
            switch (folder)
            {
                case ContainerFlolder.Anexos:
                    blob = $"anexos/{fileName}";
                    break;
                case ContainerFlolder.Coordenadas:
                    blob = $"coordenadas/{fileName}";
                    break;
                case ContainerFlolder.Logos:
                    blob = $"logoautoridad/{fileName}";
                    break;

            }
            
            return container.GetBlockBlobReference(blob);
        }
    }
}