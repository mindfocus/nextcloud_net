using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ext;
using Microsoft.Azure.Storage;
using OCP.Files.ObjectStore;
using Microsoft.Azure.Storage.Blob;

namespace OC.Files.ObjectStore
{
class Azure : IObjectStore {
	/** @var string */
	private string containerName;
	/** @var string */
	private string accountName;
	/** @var string */
	private string accountKey;
	/** @var BlobRestProxy|null */
	private CloudBlobClient blobClient = null;
	private CloudBlobContainer cloudBlobContainer = null;
	private CloudBlockBlob cloudBlockBlob = null;
	/** @var string|null */
	private string endpoint = String.Empty;
	/** @var bool  */
	private bool autoCreate = false;

	/**
	 * @param array parameters
	 */
	public Azure(IDictionary<string, object> parameters) {
		this.containerName =(string) parameters["container"];
		this.accountName = (string) parameters["account_name"];
		this.accountKey = (string) parameters["account_key"];
		if (parameters.ContainsKey("endpoint")) {
			this.endpoint = (string) parameters["endpoint"];
		}
		if (parameters.ContainsKey("autocreate")) {
			this.autoCreate = (bool) parameters["autocreate"];
		}
	}

	/**
	 * @return BlobRestProxy
	 */
	private CloudBlobClient getBlobClient() {
		if (this.blobClient == null) {
			var protocol = this.endpoint.IsNotEmpty() ? this.endpoint.Substring(0, this.endpoint.IndexOf(":") ) : "https";
			var connectionString = "DefaultEndpointsProtocol=" + protocol + ";AccountName=" + this.accountName + ";AccountKey=" + this.accountKey;
			if (this.endpoint.IsNotEmpty()) {
				connectionString += ";BlobEndpoint=" + this.endpoint;
			}
			CloudStorageAccount storageAccount;
			if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
			{
				this.blobClient = storageAccount.CreateCloudBlobClient();
			}
			else
			{
				throw new Exception();
			}
			if (this.autoCreate) {
				try {

					 cloudBlobContainer = 
						this.blobClient.GetContainerReference(this.containerName);
					cloudBlobContainer.Create();
				} catch (Exception e) {
//					if (e.getCode() === 409) {
//						// already exists
//					} else {
//						throw e;
//					}
				}
			}
		}
		return this.blobClient;
	}

	/**
	 * @return string the container or bucket name where objects are stored
	 */
	public string getStorageId() {
		return "azure::blob::" + this.containerName;
	}

	/**
	 * @param string urn the unified resource name used to identify the object
	 * @return resource stream with the read data
	 * @throws \Exception when something goes wrong, message will be logged
	 */
	public Stream readObject(string urn)
	{
		this.getBlobClient();
		var blockBlobReference = this.cloudBlobContainer.GetBlockBlobReference(urn);
		var stream = new MemoryStream();
		blockBlobReference.DownloadToStream(stream);
		return stream;
	}

	/**
	 * @param string urn the unified resource name used to identify the object
	 * @param resource stream stream with the data to write
	 * @throws \Exception when something goes wrong, message will be logged
	 */
	public void writeObject(string urn, Stream stream) {
		var blockBlobReference = this.cloudBlobContainer.GetBlockBlobReference(urn);
		blockBlobReference.UploadFromStream(stream);
	}

	/**
	 * @param string urn the unified resource name used to identify the object
	 * @return void
	 * @throws \Exception when something goes wrong, message will be logged
	 */
	public void deleteObject(string urn) {
		var blockBlobReference = this.cloudBlobContainer.GetBlockBlobReference(urn);
		blockBlobReference.Delete();
	}

	public bool objectExists(string urn) {
		return this.cloudBlobContainer.GetBlockBlobReference(urn).Exists();
	}
}

}