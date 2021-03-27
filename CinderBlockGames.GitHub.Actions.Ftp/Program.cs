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
                .WithParsed(Run)
                .WithNotParsed(Err);
        }

        private static void Run(Options options)
        {
            // Get source files info.
            var source = Directory.GetFiles(options.SourcePath, "*", SearchOption.AllDirectories)
                                  .Select(src => new Item(src, options.SourcePath, File.GetLastWriteTime(src).ToUniversalTime()));

            using (var client = new FtpClient(options.Server, options.Port, options.Username, options.Password))
            {
                client.Connect();

                // Get destination files info.
                var destination = client.GetListing(options.DestinationPath, FtpListOption.Recursive)
                                        .Where(dest => dest.Type == FtpFileSystemObjectType.File)
                                        .Select(dest => new Item(dest.FullName, options.DestinationPath, dest.Modified));

                // Delete any files that don't exist in source.
                var delete = destination.Except(source, ItemComparer.Default);
                foreach (var file in delete)
                {
                    client.DeleteFile(file.FullPath);
                }

                // Update any files that have changed.
                var update = (from src in source
                              from dest in destination
                              where ItemComparer.Default.Equals(src, dest)
                                 && src.Modified > dest.Modified
                              select src);
                Upload(client, update);

                // Upload any files that are new.
                var upload = source.Except(destination, ItemComparer.Default);
                Upload(client, upload);
            }
        }

        private static void Upload(FtpClient client, IEnumerable<Item> files)
        {
            var grouped = files.GroupBy(file => file.Directory, StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in grouped)
            {
                client.UploadFiles(kvp.Select(file => file.FullPath), kvp.Key);
            }
        }

        private static void Err(IEnumerable<Error> errors)
        {
            //handle errors
        }

    }
}