using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using ext;
using OCP.Files.ObjectStore;

namespace OC.Files.ObjectStore
{
class S3ConnectionTrait {
	/** @var array */
	protected IDictionary<string,object>  _params;

	/** @var S3Client */
	protected IAmazonS3 connection;

	/** @var string */
	protected string id;

	/** @var string */
	protected string bucket;

	/** @var int */
	protected int timeout;

	protected bool test;

	protected void parseParams(IDictionary<string,object> paramList) {
		if (!paramList.ContainsKey("key")|| ((string)paramList["key"]).IsEmpty() || !paramList.ContainsKey("secret")|| ((string)paramList["secret"]).IsEmpty() || !paramList.ContainsKey("bucket")|| ((string)paramList["bucket"]).IsEmpty())
		{
			throw new Exception("Access Key, Secret and Bucket have to be configured.");
		}

		this.id = "amazon::" + paramList["bucket"];

		this.test = paramList.ContainsKey("test") && (bool)paramList["test"];
		this.bucket = (string)paramList["bucket"];
		this.timeout = paramList.ContainsKey("timeout") ? Convert.ToInt32(paramList["timeout"]) : 15;
			paramList["region"] = paramList.ContainsKey("region") ? paramList["region"] : "eu-west-1";
			paramList["hostname"] = paramList.ContainsKey("hostname") ? paramList["region"] : "s3." + paramList["region"]+".amazonaws.com";
			paramList["region"] = paramList.ContainsKey("region") ? paramList["region"] : "eu-west-1";
		if (!paramList.ContainsKey("port") || ((string)paramList["port"]).IsEmpty()) {
			paramList["port"] = paramList.ContainsKey("use_ssl") && ((bool)paramList["use_ssl"] == false) ? 80 : 443;
		}
		this._params = paramList;
	}

	public string getBucket() {
		return this.bucket;
	}

	/**
	 * Returns the connection
	 *
	 * @return S3Client connected client
	 * @throws \Exception if connection could not be made
	 */
	public IAmazonS3 getConnection() {
		if (this.connection != null) {
			return this.connection;
		}

		var scheme = _params.ContainsKey("use_ssl") && ((bool) _params["use_ssl"] == false) ? "http" : "https";
		var base_url = scheme + "://" + _params["hostname"] + ":" + _params["port"] + "/";

		AWSCredentials credentials = new BasicAWSCredentials((string)_params["key"], (string)_params["secret"]);
		RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName((string) _params["region"]);
		AmazonS3Config amazonS3Config = new AmazonS3Config( );
		amazonS3Config.AuthenticationRegion = (string) _params["region"];
		amazonS3Config.RegionEndpoint = regionEndpoint;
		if (_params.ContainsKey("proxy"))
		{
			amazonS3Config.ProxyHost = (string)_params["proxy"];
			amazonS3Config.ProxyPort = (int) _params["proxy_port"];
			if (_params.ContainsKey("proxy_auth") && ((bool)_params["proxy_auth"]) == true)
			{
				var credential = new NetworkCredential();
				credential.UserName = (string) _params["proxy_username"];
				credential.Password = (string) _params["proxy_password"];
				amazonS3Config.ProxyCredentials = credential;
			}
		}
// https://docs.aws.amazon.com/zh_cn/AmazonS3/latest/dev/create-bucket-get-location-example.html#create-bucket-get-location-dotnet
		amazonS3Config.ForcePathStyle =
			_params.ContainsKey("use_path_style") ? (bool) _params["use_path_style"] : false;
		this.connection = new AmazonS3Client(credentials, amazonS3Config);
//		if (!this.connection.isBucketDnsCompatible(this.bucket)) {
//			var logger = OC.server.getLogger();
//			logger.debug("Bucket "' . this.bucket . '" This bucket name is not dns compatible, it may contain invalid characters.',
//					 ["app" => 'objectstore']);
//		}
		if (!AmazonS3Util.DoesS3BucketExistV2Async(this.connection, this.bucket).Result)
		{
			logger = OC.server.getLogger();
			try {
				logger.info("Bucket \"" + this.bucket + "\" does not exist - creating it.", ['app' => 'objectstore']);
//				if (!this.connection.isBucketDnsCompatible(this.bucket)) {
//					throw new Exception("The bucket will not be created because the name is not dns compatible, please correct it: " + this.bucket);
//				}
				var putBucketRequest = new PutBucketRequest
				{
					BucketName = this.bucket,
					UseClientRegion = true
				};

				PutBucketResponse putBucketResponse = this.connection.PutBucketAsync(putBucketRequest).Result;
				this.testTimeout();
			} catch (Exception e) {
				logger.logException(e, [
					'message' => 'Invalid remote storage.',
				'level' => ILogger::DEBUG,
				'app' => 'objectstore',
					]);
				throw new Exception("Creation of bucket \"" + this.bucket + "\" failed. " + e.Message);
			}
		}


		// google cloud's s3 compatibility doesn't like the EncodingType parameter
		if (base_url.IndexOf("storage.googleapis.com") != -1) {
//			this.connection.getHandlerList().remove('s3.auto_encode');
		}

		return this.connection;
	}

	/**
	 * when running the tests wait to let the buckets catch up
	 */
	private void testTimeout() {
		if (this.test) {
			Thread.Sleep(this.timeout);
		}
	}

	public static S3Signature legacySignatureProvider(string version, object service, object region) {
		switch (version) {
			case "v2":
			case "s3":
				return new S3Signature();
			default:
				return null;
		}
	}
}

}