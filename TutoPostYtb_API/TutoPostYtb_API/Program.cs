using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeUpload
{
    class Program
    {
        static async Task Main(string[] args)
        {

            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json",FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("YouTube.Upload.Auth.Store")
                );

            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "YouTube Upload Console App"
            }); 


            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = "Video Tuto Api Youtube";
            video.Snippet.Description = "Description de la vidéo";
            video.Snippet.Tags = new string[] { "Tuto", "API" };
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "private"; // or "private" or "public"


            using (var fileStream = new FileStream("video.mp4", FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                await videosInsertRequest.UploadAsync();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();








        }
    }
}