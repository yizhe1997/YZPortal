using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using YZPortal.API.Controllers.ViewModel.Auditable;
using YZPortal.Core.Error;

namespace YZPortal.API.Infrastructure.AzureStorage
{
    public static class AzureStorageExtensions
	{
		#region Azure Blob

		/// <summary>
		///     Access the azure cloud blob given the container and object name. Returns null if exception caught
		/// </summary>
		public static async Task<BlobClient> ReferenceCloudBlob(this BlobServiceClient BlobClient, string containerName, string fileName, bool toUpload = false)
		{
			try
			{
				// Container reference
				BlobContainerClient containerClient = BlobClient.GetBlobContainerClient(containerName);

				if (await containerClient.ExistsAsync())
				{
					// Blob reference
					BlobClient latestBlob = containerClient.GetBlobClient(fileName);

					if (toUpload == false)
						if (await latestBlob.ExistsAsync())
							return latestBlob;

					if (latestBlob != null && toUpload == true)
					{
						var uri = latestBlob.GenerateSasUri(BlobSasPermissions.Write | BlobSasPermissions.Create, DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc));
						return new BlobClient(uri);
					}
				}

				return null;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		///     Return the string content type for the azure cloud blob given the container and object name.
		///     Content type defaults to application/octet-stream if exception thrown
		/// </summary>
		public static async Task<string> GetBlobContentType(this BlobServiceClient BlobClient, string containerName, string fileName)
		{
			BlobClient blob = await BlobClient.ReferenceCloudBlob(containerName, fileName);

			try
			{
				var provider = new FileExtensionContentTypeProvider();

				var getBlobPropertiesResult = await blob.GetPropertiesAsync();
				var contentType = getBlobPropertiesResult.Value.ContentType;

				if (contentType == null && !provider.TryGetContentType(fileName, out contentType))
				{
					contentType = "application/octet-stream";
				}

				return contentType;
			}
			catch
			{
				return "application/octet-stream";
			}
		}

		public static async Task UploadAttachmentAsBlob(this BlobServiceClient BlobClient, string containerName, string fileName, IFormFile file)
		{
			// Reference a blob from azure container
			BlobClient blob = await BlobClient.ReferenceCloudBlob(containerName, fileName, true);

			if (blob != null)
				try
				{
					await blob.UploadAsync(file.OpenReadStream());
				}
				catch (Exception err)
				{
					throw new RestException(HttpStatusCode.InternalServerError, err.Message);
				}
			else
				throw new RestException(HttpStatusCode.InternalServerError, $"Error referring blob {fileName} from container {containerName}");
		}

		public static async Task DeleteAttachmentAsBlob(this BlobServiceClient BlobClient, string containerName, string fileName)
		{
			// Reference a blob from azure container
			BlobClient blob = await BlobClient.ReferenceCloudBlob(containerName, fileName);

			if (blob != null)
				try
				{
					await blob.DeleteIfExistsAsync();
				}
				catch (Exception err)
				{
					throw new RestException(HttpStatusCode.InternalServerError, err.Message);
				}

			// dont throw exception if blob was dlted unknowingly before user gets to delete the file through the UI - just proceed to dlt the local attachment
		}

		public static async Task DwnloadAttachmentFromBlob(this MemoryStream stream, BlobServiceClient BlobClient, string containerName, string fileName)
		{
			// Reference a blob from azure container
			BlobClient blob = await BlobClient.ReferenceCloudBlob(containerName, fileName);

			if (blob != null)
				try
				{
					await blob.DownloadToAsync(stream);
					stream.Seek(0, SeekOrigin.Begin);
				}
				catch (Exception err)
				{
					throw new RestException(HttpStatusCode.InternalServerError, err.Message);
				}
			else
				throw new RestException(HttpStatusCode.InternalServerError, $"Error referring blob {fileName} from container {containerName}");
		}

		#endregion
	}

	/// <summary>
	///     View model used for non-attachment type entities that may have a list of attachments 
	/// </summary>
	public class NonAttachmentEntityTypeAttachmentViewModel
	{
		public string? FileName { get; set; }
		public string? DisplayName { get; set; }
		public Guid Id { get; set; }
		public string? FileType { get; set; }
		public string? Notes { get; set; }
		public string? Description { get; set; }
	}

	/// <summary>
	///     View model derived from NonAttachmentEntityTypeAttachmentViewModel to map display name using mapper 
	///     profile. For some reasone using the base class isnt working for mapping display name.
	/// </summary>
	public class NonAttachmentEntityTypeAttachmentReservedViewModel : NonAttachmentEntityTypeAttachmentViewModel
	{
	}

	/// <summary>
	///     View model used for attachment type entities 
	/// </summary>
	public class AttachmentViewModel : AuditableViewModel
	{
		public string? FileName { get; set; }
		public string? DisplayName { get; set; }
		public string? FileType { get; set; }
	}

	/// <summary>
	///     View model when downloading attachments from AZ
	/// </summary>
	public class AttachmentDownloadFileModel
	{
		public MemoryStream? Stream { get; set; }
		public string? ContentType { get; set; }
	}
}
