using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using FluentFTP;

namespace CinderBlockGames.GitHub.Actions.Ftp
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }

        private static void Run(Options options)
        {
            // Get source files info.
            Console.WriteLine("...Finding source files...");
            var source = Directory.GetFiles(options.SourcePath, "*", SearchOption.AllDirectories)
                                  .Select(src => new Item(src, options.SourcePath, File.GetLastWriteTime(src)));

            using (var client = new FtpClient(options.Server, options.Port, options.Username, options.Password))
            {
                Console.WriteLine("...Connecting to remote server...");
                client.Connect();

                // Get destination files info.
                Console.WriteLine("...Finding destination files...");
                var destination = client.GetListing(options.DestinationPath, FtpListOption.Recursive)
                                        .Where(dest => dest.Type == FtpFileSystemObjectType.File)
                                        .Select(dest => new Item(dest.FullName, options.DestinationPath, dest.Modified));

                // Delete any files that don't exist in source.
                var delete = destination.Except(source, ItemComparer.Default);
                Console.WriteLine($"...Deleting {delete.Count()} files...");
                foreach (var file in delete)
                {
                    Console.WriteLine(file.FullPath);
                    client.DeleteFile(file.FullPath);
                }
                Console.WriteLine();

                // Update any files that have changed.
                var update = (from src in source
                              from dest in destination
                              where ItemComparer.Default.Equals(src, dest)
                                 && src.Modified > dest.Modified
                              select src);
                Console.WriteLine($"...Updating {update.Count()} files...");
                Upload(client, update);
                Console.WriteLine();

                // Upload any files that are new.
                var upload = source.Except(destination, ItemComparer.Default);
                Console.WriteLine($"...Uploading {upload.Count()} new files...");
                Upload(client, upload);
                Console.WriteLine();

                Console.WriteLine("Complete!");
            }
        }

        private static void Upload(FtpClient client, IEnumerable<Item> files)
        {
            var grouped = files.GroupBy(file => file.Directory, StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in grouped)
            {
                Console.WriteLine($"[Directory: {kvp.Key}]");
                Console.WriteLine(string.Join("\r\n", kvp.Select(file => file.FullPath)));
                client.UploadFiles(kvp.Select(file => file.FullPath), kvp.Key);
            }
        }

    }
}