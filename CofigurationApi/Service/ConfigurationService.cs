using CofigurationApi.Data;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;

/// <summary>
/// Summary description for Class1
/// </summary>
public class ConfigurationService
{
    private  BlobProvider _blobStorageProvider;

	public ConfigurationService(BlobProvider blobStorageProvider)
	{
       _blobStorageProvider = blobStorageProvider;
	}

    public  async Task<string> GetConfiguration(string name)
    {
        return await _blobStorageProvider.DownloadBlob(name);
    }

}
