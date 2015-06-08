using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Crowd.Service.Common
{
    public class SvcUtil
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string MakeUploadFolder(string folderName)
        {
            string uploadDir = HostingEnvironment.MapPath("~/Uploads/");
            var dest = Path.Combine(uploadDir, folderName);
            if (Directory.Exists(dest))
                Directory.Delete(dest, true);
            Directory.CreateDirectory(dest);
            return dest;
        }

        public static string DownloadZip(string zipUrl, string destFolder, string zipName)
        {
            string zipPath = "";
            if (!string.IsNullOrWhiteSpace(destFolder))
            {
                //string zipFileName = Guid.NewGuid().ToString();
                //string fName = Path.GetFileName(Path.GetDirectoryName(destFolder + "\\tmp.x"));//Extract the folder name to be used for the zip file name
                zipPath = Path.Combine(destFolder, zipName + ".zip");
                using (WebClient Client = new WebClient())
                {
                    Client.Credentials = ConfidentialData.GetStorageCredentials();
                    Client.DownloadFile(zipUrl, zipPath);
                }
            }
            return zipPath;
        }

        public static async Task<List<string>> DownloadAndExtractZip(string zipUrl, string jobId, string userId,
            string activityId)
        {
            string uFolder = MakeUploadFolder(jobId);
            string zipFilePath = DownloadZip(zipUrl, uFolder, jobId);
            List<string> lstFiles = new List<string>();
            if (!string.IsNullOrWhiteSpace(zipFilePath))
            {
                lstFiles.AddRange(ExtractZip(zipFilePath, uFolder));
            }

            // Connect to Azure cloud storage, creating uploads container (folder) if necessary
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Storage"].ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("speechinguploads");
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            List<string> remoteFiles = new List<string>();

            // Create a storage blob for each file
            foreach (string path in lstFiles)
            {
                string blobName = userId + "/" + activityId + "/" + jobId + "/" + Path.GetFileName(path);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

                // Create or overwrite the blob with contents from a local file.
                using (var fileStream = File.OpenRead(@path))
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }

                remoteFiles.Add(blockBlob.Uri.AbsoluteUri);

                // Remove local version
                File.Delete(path);
            }

            File.Delete(zipFilePath);

            return remoteFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipPath"></param>
        /// <param name="destination"></param>
        /// <returns>List of files path</returns>
        public static List<string> ExtractZip(string zipPath, string destination)
        {
            ZipFile zf = null;
            //bool ret = false;
            List<string> retLst = new List<string>();
            try
            {
                var fs = File.OpenRead(zipPath);
                zf = new ZipFile(fs);
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }
                    string entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096]; // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    string fullZipToPath = Path.Combine(destination, entryFileName);
                    //string directoryName = Path.GetDirectoryName(fullZipToPath);
                    //if (directoryName.Length > 0)
                    //    Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                    //ret = true;
                    retLst.Add(fullZipToPath);
                }
            }
            catch (Exception ex)
            {
                //ret = false;
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
            return retLst;
        }
    }
}