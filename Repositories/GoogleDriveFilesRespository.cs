using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using WebAppContentieux.Models;
using static Google.Apis.Drive.v3.DriveService;
using static System.Net.WebRequestMethods;

namespace WebAppContentieux.Repositories
{
    public class GoogleDriveFilesRespository
    {
        private static DriveService GetService()
        {
            var tokenResponse = new TokenResponse
            {
                AccessToken = "ya29.a0Aa4xrXOcaaEFHuHOex_r15vJbt4OE0_hMydSd79D9ax7oI9_eeU_hmowG1GqOADLRqJEfDKTKCI0oNu5ema7DRPQKjBsXC2xbAUSw90ircgo8Ui-GmMrab_xZ0Tfkzs3Bv3AU5VILkDaboXD00_BICKAygKsaCgYKATASARASFQEjDvL99jWnNZ733pvHVpMl2Fs5NA0163",
                RefreshToken = "1//04YxtBkpcIpdICgYIARAAGAQSNwF-L9IrpoY7DYRn7iBOVbNfYEWBX6fiOwo7GfAcDCaBKNztbHfsgqFretRs5saEJo-nUdMa1F4"
            };

            var applicationName = "WebAppContentieux";// Use the name of the project in Google Cloud
            var username = "pfe.proj.2022@gmail.com"; // Use your email

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "374213054486-m8cic2hmrpi8oihk0tfpc33n080v7kaa.apps.googleusercontent.com",
                    ClientSecret = "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"
                },
                Scopes = new[] { Scope.Drive },
                DataStore = new FileDataStore(applicationName)
            });

            var credential = new UserCredential(apiCodeFlow, username, tokenResponse);
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });
            return service;
        }

        public string CreateFolder(string? parent, string? folderName)
        {
            var service = GetService();
            var driveFolder = new Google.Apis.Drive.v3.Data.File();
            driveFolder.Name = folderName;
            driveFolder.MimeType = "application/vnd.google-apps.folder";
            driveFolder.Parents = new string[] { parent };
            var command = service.Files.Create(driveFolder);
            var file = command.Execute();
            return file.Id;
        }

        public string UploadFile(Stream file, string fileName, string fileMime)
        {
            DriveService service = GetService();
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
            //driveFile.Description = fileDescription;
            driveFile.MimeType = fileMime;
            //driveFile.Parents = new string[] { folder };

            var request = service.Files.Create(driveFile, file, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            return request.ResponseBody.Id;
        }

        public string UploadImage(string _uploadFile, string _descrp = "Uploaded with .NET!")
        {
            DriveService _service = GetService();
            
                var body = new Google.Apis.Drive.v3.Data.File();
                //File body = new File();

                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = _descrp;
                body.MimeType = GetMimeType(_uploadFile);
                // body.Parents = new List<ParentReference>() { new ParentReference() { Id = _parent } };
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                FilesResource.CreateMediaUpload request;
                Google.Apis.Drive.v3.Data.Permission permission = new Google.Apis.Drive.v3.Data.Permission();
                permission.Type = "anyone";
                permission.Role = "reader";
                permission.AllowFileDiscovery = true;



            using (var stream1 = new System.IO.FileStream(_uploadFile, System.IO.FileMode.Open))
                    {
                        request = _service.Files.Create(body, stream1, GetMimeType(_uploadFile));
                        request.Fields = "id";
                var result = request.Upload();
                    }
             
            PermissionsResource.CreateRequest request1 = _service.Permissions.Create(permission, request.ResponseBody.Id);
            request1.Fields = "id";
            request1.Execute();
            return request.ResponseBody.Id;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public void DeleteFile(string? fileId)
        {
            var service = GetService();
            var command = service.Files.Delete(fileId);
            var result = command.Execute();
        }
    }
}

//    //defined scope.
//    public static string[] Scopes = { DriveService.Scope.Drive };
//    //create Drive API service.
//    public static DriveService GetService()
//    {
//        //get Credentials from client_secret.json file 
//        UserCredential credential;
//        using (var stream = new FileStream(@"D:\client_secret.json", FileMode.Open, FileAccess.Read))
//        {
//            String FolderPath = @"D:\";
//            String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
//            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//                GoogleClientSecrets.Load(stream).Secrets,
//                Scopes,
//                "user",
//                CancellationToken.None,
//                new FileDataStore(FilePath, true)).Result;
//        }
//        //create Drive API service.
//        DriveService service = new DriveService(new BaseClientService.Initializer()
//        {
//            HttpClientInitializer = credential,
//            ApplicationName = "GoogleDriveRestAPI-v3",
//        });
//        return service;
//    }
//    //get all files from Google Drive.
//    public static List<GoogleDriveFiles> GetDriveFiles()
//    {
//        DriveService service = GetService();
//        // define parameters of request.
//        FilesResource.ListRequest FileListRequest = service.Files.List();
//        //listRequest.PageSize = 10;
//        //listRequest.PageToken = 10;
//        FileListRequest.Fields = "nextPageToken, files(id, name, size, version, createdTime)";
//        //get file list.
//        IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
//        List<GoogleDriveFiles> FileList = new List<GoogleDriveFiles>();
//        if (files != null && files.Count > 0)
//        {
//            foreach (var file in files)
//            {
//                GoogleDriveFiles File = new GoogleDriveFiles
//                {
//                    Id = file.Id,
//                    Name = file.Name,
//                    Size = file.Size,
//                    Version = file.Version,
//                    CreatedTime = file.CreatedTime
//                };
//                FileList.Add(File);
//            }
//        }
//        return FileList;
//    }
//    //file Upload to the Google Drive. 
//    public static void FileUpload(HttpPostedFileBase file)
//    {
//        if (file != null && file.ContentLength > 0)
//        {
//            DriveService service = GetService();
//            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
//            Path.GetFileName(file.FileName));
//            file.SaveAs(path);
//            var FileMetaData = new Google.Apis.Drive.v3.Data.File();
//            FileMetaData.Name = Path.GetFileName(file.FileName);
//            FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);
//            FilesResource.CreateMediaUpload request;
//            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
//            {
//                request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
//                request.Fields = "id";
//                request.Upload();
//            }
//        }
//    }
//    //Download file from Google Drive by fileId.
//    public static string DownloadGoogleFile(string fileId)
//    {
//        DriveService service = GetService();
//        string FolderPath = System.Web.HttpContext.Current.Server.MapPath("/GoogleDriveFiles/");
//        FilesResource.GetRequest request = service.Files.Get(fileId);
//        string FileName = request.Execute().Name;
//        string FilePath = System.IO.Path.Combine(FolderPath, FileName);
//        MemoryStream stream1 = new MemoryStream();
//        // Add a handler which will be notified on progress changes.
//        // It will notify on each chunk download and when the
//        // download is completed or failed.
//        request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
//        {
//            switch (progress.Status)
//            {
//                case DownloadStatus.Downloading:
//                    {
//                        Console.WriteLine(progress.BytesDownloaded);
//                        break;
//                    }
//                case DownloadStatus.Completed:
//                    {
//                        Console.WriteLine("Download complete.");
//                        SaveStream(stream1, FilePath);
//                        break;
//                    }
//                case DownloadStatus.Failed:
//                    {
//                        Console.WriteLine("Download failed.");
//                        break;
//                    }
//            }
//        };
//        request.Download(stream1);
//        return FilePath;
//    }
//    // file save to server path
//    private static void SaveStream(MemoryStream stream, string FilePath)
//    {
//        using (System.IO.FileStream file = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
//        {
//            stream.WriteTo(file);
//        }
//    }
//    //Delete file from the Google drive
//    public static void DeleteFile(GoogleDriveFiles files)
//    {
//        DriveService service = GetService();
//        try
//        {
//            // Initial validation.
//            if (service == null)
//                throw new ArgumentNullException("service");
//            if (files == null)
//                throw new ArgumentNullException(files.Id);
//            // Make the request.
//            service.Files.Delete(files.Id).Execute();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception("Request Files.Delete failed.", ex);
//        }
//    }
//}
//    public static string[] Scopes = { DriveService.Scope.Drive };
//    //create Drive API service.
//    public static DriveService GetService()
//    {
//        //get Credentials from client_secret.json file 
//        UserCredential credential;
//        using (var stream = new FileStream(@"D:\client_secret.json", FileMode.Open, FileAccess.Read))
//        {
//            String FolderPath = @"D:\";
//            String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
//            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//                GoogleClientSecrets.Load(stream).Secrets,
//                Scopes,
//                "user",
//                CancellationToken.None,
//                new FileDataStore(FilePath, true)).Result;
//        }
//        //create Drive API service.
//        DriveService service = new DriveService(new BaseClientService.Initializer()
//        {
//            HttpClientInitializer = credential,
//            ApplicationName = "GoogleDriveFilesAPI_v3",
//        });
//        return service;
//    }
//    //get all files from Google Drive.
//    public static List<GoogleDriveFiles> GetDriveFiles()
//    {
//        DriveService service = GetService();
//        // define parameters of request.
//        FilesResource.ListRequest FileListRequest = service.Files.List();
//        //listRequest.PageSize = 10;
//        //listRequest.PageToken = 10;
//        FileListRequest.Fields = "nextPageToken, files(id, name, size, version, createdTime)";
//        //get file list.
//        IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
//        List<GoogleDriveFiles> FileList = new List<GoogleDriveFiles>();
//        if (files != null && files.Count > 0)
//        {
//            foreach (var file in files)
//            {
//                GoogleDriveFiles File = new GoogleDriveFiles
//                {
//                    Id = file.Id,
//                    Name = file.Name,
//                    Size = file.Size,
//                    Version = file.Version,
//                    CreatedTime = file.CreatedTime
//                };
//                FileList.Add(File);
//            }
//        }
//        return FileList;
//    }
//    //file Upload to the Google Drive.
//    public string UploadFile(Stream file, string fileName, string fileMime, string folder, string fileDescription)
//    {
//        DriveService service = GetService();
//var driveFile = new Google.Apis.Drive.v3.Data.File();
//        driveFile.Name = fileName;
//        driveFile.Description = fileDescription;
//        driveFile.MimeType = fileMime;
//        driveFile.Parents = new string[] { folder };
//var request = service.Files.Create(driveFile, file, fileMime);
//        request.Fields = "id";
//        var response = request.Upload();
//        if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
//            throw response.Exception;
//        return request.ResponseBody.Id;
//    }
//    // file save to server path
//    private static void SaveStream(MemoryStream stream, string FilePath)
//    {
//        using (System.IO.FileStream file = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
//        {
//            stream.WriteTo(file);
//        }
//    }
//    //Delete file from the Google drive
//    public static void DeleteFile(GoogleDriveFiles files)
//    {
//        DriveService service = GetService();
//        try
//        {
//            // Initial validation.
//            if (service == null)
//                throw new ArgumentNullException("service");
//            if (files == null)
//                throw new ArgumentNullException(files.Id);
//            // Make the request.
//            service.Files.Delete(files.Id).Execute();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception("Request Files.Delete failed.", ex);
//        }