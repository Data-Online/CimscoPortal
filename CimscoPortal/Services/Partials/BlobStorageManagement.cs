using CimscoPortal.Extensions;
using CimscoPortal.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CimscoPortal.Services
{

    partial class PortalService
    {

        //public string GetBlobAdHocSharedAccessSignatureUrl(string containerName, string blobName)
        //{
        //    CloudBlobClient _blobClient = CreateBlobClient();

        //    SharedAccessBlobPolicy adHocPolicy = new SharedAccessBlobPolicy
        //    {
        //        Permissions = SharedAccessBlobPermissions.Read,
        //        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30)
        //    };

        //    CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
        //    CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

        //    string zz = container.GetSharedAccessSignature(adHocPolicy);

        //    string sharedAccessSignature = blob.GetSharedAccessSignature(adHocPolicy);

        //    return container.Uri + zz;
        //    //return blob.Uri + zz;
        //}

        public string GetBlobStorageSharedAccessSignature(string containerName, string blobName)
        {
            // Blob level
            CloudBlobClient _blobClient = CreateBlobClient();
            CloudBlobContainer _container = _blobClient.GetContainerReference(containerName);
            BlobContainerPermissions _permissions = _container.GetPermissions();

            // Clear any existing access policies on container
            _permissions.SharedAccessPolicies.Clear();
            _permissions.SharedAccessPolicies.Add("blobpolicy1", new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(10)
            });
            _container.SetPermissions(_permissions);

            CloudBlockBlob _blob = _container.GetBlockBlobReference(blobName);
            string _sharedAccessSignature = _blob.GetSharedAccessSignature(null, "blobpolicy1");
            return _blob.Uri + _sharedAccessSignature;
        }

        public string GetBlobStorageSharedAccessSignature(string containerName)
        {
            // Container level

            //CloudBlobClient _blobClient = CreateBlobClient();
            //CloudBlobContainer _container = _blobClient.GetContainerReference(containerName);
            CloudBlobContainer _container = GetContainer(containerName);
            BlobContainerPermissions _permissions = _container.GetPermissions();

            // Clear any existing access policies on container
            _permissions.SharedAccessPolicies.Clear();
            _permissions.SharedAccessPolicies.Add("blobpolicy1", new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(10)
            });
            _container.SetPermissions(_permissions);
            string _sharedAccessSignature = _container.GetSharedAccessSignature(null, "blobpolicy1");
            return _container.Uri + _sharedAccessSignature;
        }

        public object[]  GetBlobStorageSharedAccessSignature_(string containerName)
        {
            // Container level
            int _keyLife = Convert.ToInt32(GetConfigValue("BlobPdfKeyLifeMins"));
            //CloudBlobClient _blobClient = CreateBlobClient();
            //CloudBlobContainer _container = _blobClient.GetContainerReference(containerName);
            CloudBlobContainer _container = GetContainer(containerName);
            BlobContainerPermissions _permissions = _container.GetPermissions();

            // Clear any existing access policies on container
            _permissions.SharedAccessPolicies.Clear();
            _permissions.SharedAccessPolicies.Add("blobpolicy1", new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(_keyLife)
            });
            _container.SetPermissions(_permissions);
            string _sharedAccessSignature = _container.GetSharedAccessSignature(null, "blobpolicy1");
            return new[] { _container.Uri.ToString(), _sharedAccessSignature };
        }


        private  CloudBlobClient CreateBlobClient()
        {
            string _key = GetConfigValue("BlobPdfStoreKey");
            string _account = GetConfigValue("BlobPdfStoreAccount");
            string connectionString = string.Format(@"DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                     _account, _key);
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            return cloudStorageAccount.CreateCloudBlobClient();
        }

        public CloudBlobContainer GetContainer(string containerName)
        {
            CloudBlobClient _blobClient = CreateBlobClient();
            CloudBlobContainer _container = _blobClient.GetContainerReference(containerName);
            return _container;
        }

    }
}
