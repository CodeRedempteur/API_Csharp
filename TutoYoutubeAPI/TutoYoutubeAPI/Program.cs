using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TutoYoutubeAPI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Remplacez "YOUR_API_KEY" par votre clé API YouTube.
            string apiKey = "YOUR_API_KEY"; 

            // Initialiser le service YouTube.
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "TutoYoutubeAPI"
            });

            // ID de la chaîne YouTube
            string channelIdOrUsername = "UC4w6n_v3M9KP4-A_JdHjeqQ"; // Peut être un ID de chaîne ou un nom d'utilisateur commençant par '@'

            string channelId = await GetChannelIdAsync(youtubeService, channelIdOrUsername);

            if (channelId != null)
            {
                try
                {
                    // Récupérer les informations de la chaîne en utilisant l'ID de chaîne.
                    var channelRequest = youtubeService.Channels.List("snippet,statistics");
                    channelRequest.Id = channelId;

                    var channelResponse = await channelRequest.ExecuteAsync();

                    if (channelResponse.Items != null && channelResponse.Items.Count > 0)
                    {
                        var channel = channelResponse.Items[0];
                        Console.WriteLine($"Nom de la chaîne: {channel.Snippet.Title}");
                        Console.WriteLine($"Nombre de vidéos: {channel.Statistics.VideoCount}");
                        Console.WriteLine($"Nombre d'abonnés: {channel.Statistics.SubscriberCount}");

                        // Télécharger et enregistrer l'image de profil dans le dossier Téléchargements de l'utilisateur
                        string profileImageUrl = channel.Snippet.Thumbnails.Default__.Url;
                        await DownloadAndSaveImageAsync(profileImageUrl, channel.Snippet.Title+".jpg");
                        Console.WriteLine("Image de profil téléchargée et enregistrée dans le dossier Téléchargements");
                    }
                    else
                    {
                        Console.WriteLine("Aucune chaîne trouvée avec l'ID de chaîne spécifié.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID de chaîne ou nom d'utilisateur non valide.");
            }
        }

        static async Task<string> GetChannelIdAsync(YouTubeService youtubeService, string idOrUsername)
        {
            // Vérifier si l'ID est un nom d'utilisateur commençant par '@'
            if (idOrUsername.StartsWith("@"))
            {
                // Supprimer le '@' du nom d'utilisateur
                string username = idOrUsername.Substring(1);

                try
                {
                    // Récupérer l'ID de chaîne en utilisant le nom d'utilisateur
                    var channelRequest = youtubeService.Channels.List("id");
                    channelRequest.ForUsername = username;

                    var channelResponse = await channelRequest.ExecuteAsync();

                    if (channelResponse.Items != null && channelResponse.Items.Count > 0)
                    {
                        return channelResponse.Items[0].Id;
                    }
                    else
                    {
                        Console.WriteLine($"Aucune chaîne trouvée pour le nom d'utilisateur : {username}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la récupération de l'ID de chaîne pour le nom d'utilisateur : {ex.Message}");
                    return null;
                }
            }
            else
            {
                // L'ID est déjà au format standard (UC4w6n_v3M9KP4-A_JdHjeqQ par exemple)
                return idOrUsername;
            }
        }

        static async Task DownloadAndSaveImageAsync(string imageUrl, string fileName)
        {
            string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadFolderPath = Path.Combine(downloadsFolder, "Downloads");

            // Créer le dossier Téléchargements s'il n'existe pas
            Directory.CreateDirectory(downloadFolderPath);

            string filePath = Path.Combine(downloadFolderPath, fileName);

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }

            Console.WriteLine($"Image téléchargée et enregistrée sous : {filePath}");
        }
    }
}
