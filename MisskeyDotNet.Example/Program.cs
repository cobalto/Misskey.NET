using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MisskeyDotNet.Exceptions;
using MisskeyDotNet.Models.Authorization;
using MisskeyDotNet.Models.HttpApi;

namespace MisskeyDotNet.Example
{
    class Program
    {
        static async Task Main()
        {
            Misskey io;
            if (File.Exists("credential"))
            {
                io = Misskey.Import(await File.ReadAllTextAsync("credential"));
            }
            else
            {
                var authorizer = new Authorizer(new InstanceSettings("misskey.io", "Misskey.NET", null, null, Permission.All));
                if (!authorizer.TryOpenBrowser())
                {
                    Console.WriteLine("Open the following URL in your web browser to complete the authentication.");
                    Console.WriteLine(authorizer.InstanceSettings.Url);
                }
                Console.WriteLine("Press ENTER when authorization is complete.");
                Console.ReadLine();

                io = await authorizer.CheckAsync();
                await File.WriteAllTextAsync("credential", io.Export());
            }

            // await FetchReactions(io);
            // await SummonError(io);
            // await GetMeta(io);
            await FetchInstances();
            Console.ReadLine();
        }

        private static async ValueTask FetchReactions(Misskey mi)
        {
            var note = await mi.ApiAsync<Note>("notes/show", new
            {
                noteId = "7zzafqsm9a",
            });

            Console.WriteLine("Note ID: " + note.Id);
            Console.WriteLine("Note Created At: " + note.CreatedAt);
            Console.WriteLine("CW: " + note.Cw ?? "null");
            Console.WriteLine("Body: " + note.Text ?? "null");
            Console.WriteLine("Reactions: ");
            int c = 0;
            foreach (var kv in note.Reactions)
            {
                Console.Write(" {0}: {1}", kv.Key, kv.Value);
                c++;
                if (c == 5)
                {
                    c = 0;
                    Console.WriteLine();
                }
            }
        }
        private static async ValueTask SummonError(Misskey mi)
        {
            try
            {
                var note = await mi.ApiAsync<Note>("notes/show", new
                {
                    noteId = "m",
                });
            }
            catch (MisskeyApiException e)
            {
                Console.WriteLine(e.Error.Message);
            }
        }
        private static async ValueTask GetMeta(Misskey mi)
        {
            try
            {
                var meta = await mi.ApiAsync<Meta>("meta");
                Console.WriteLine($"Instance Name: {meta.Name}");
                Console.WriteLine($"Version: {meta.Version}");
                Console.WriteLine($"Description: {meta.Description}");
                Console.WriteLine($"Administrator: {meta.MaintainerName}");
                Console.WriteLine($"Contact: {meta.MaintainerEmail}");
                Console.WriteLine($"Local Timeline: {(meta.DisableLocalTimeline ? "No" : "Yes")}");
                Console.WriteLine($"Global Timeline: {(meta.DisableGlobalTimeline ? "No" : "Yes")}");
                Console.WriteLine($"Registration Open: {(meta.DisableRegistration ? "No" : "Yes")}");
                Console.WriteLine($"E-mail enabled: {(meta.EnableEmail ? "Yes" : "No")}");
                Console.WriteLine($"Twitter Integration: {(meta.EnableTwitterIntegration ? "Yes" : "No")}");
                Console.WriteLine($"Discord Integration: {(meta.EnableDiscordIntegration ? "Yes" : "No")}");
                Console.WriteLine($"GitHub Integration: {(meta.EnableGithubIntegration ? "Yes" : "No")}");
            }
            catch (MisskeyApiException e)
            {
                Console.WriteLine(e.Error.Message);
            }
        }
        private static async ValueTask FetchInstances()
        {
            var res = await Misskey.JoinMisskeyInstancesApiAsync();
            Console.WriteLine($"Last Update: {res.Date}");
            Console.WriteLine($"Number of Instances: {res.Stats.InstancesCount}");
            Console.WriteLine($"Instance List:");
            res.InstancesInfos.Select(meta => " " + meta.Url).ToList().ForEach(Console.WriteLine);
        }
    }
}
