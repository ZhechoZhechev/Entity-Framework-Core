namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            throw new NotImplementedException();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int durationInSeconds)
        {
            var targetedSongs = context.Songs
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > durationInSeconds)
                .Select(s => new
                {
                    SongName = s.Name,
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album!.Producer!.Name,
                    SongDuration = s.Duration.ToString("c"),
                    Performers = s.SongPerformers
                    .Select(p => new
                    {
                        PerformerFullName = string.Concat(p.Performer.FirstName, " ", p.Performer.LastName)
                    })
                    .OrderBy(n => n.PerformerFullName)
                    .ToArray()
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var counter = 1;
            foreach (var s in targetedSongs) 
            {
                sb.AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {s.SongName}")
                    .AppendLine($"---Writer: {s.WriterName}");

                if (s.Performers.Any())
                {
                    foreach (var p in s.Performers) 
                    {
                        sb.AppendLine($"---Performer: {p.PerformerFullName}");

                    }
                }

                sb.AppendLine($"---AlbumProducer: {s.AlbumProducer}")
                    .AppendLine($"---Duration: {s.SongDuration}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
