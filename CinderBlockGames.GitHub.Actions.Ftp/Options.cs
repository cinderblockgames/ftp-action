using CommandLine;

namespace CinderBlockGames.GitHub.Actions.Ftp
{
    public class Options
    {

        // Connection.

        [Option("server",
                Required = true,
                HelpText = "Address for the destination server.")]
        public string Server { get; set; }

        [Option("port",
                Required = false, Default = 21,
                HelpText = "Port for the destination server.")]
        public int Port { get; set; }

        [Option("username",
                Required = true,
                HelpText = "Username for the destination server.")]
        public string Username { get; set; }

        [Option("password",
                Required = true,
                HelpText = "Password for the destination server.")]
        public string Password { get; set; }

        // Data.

        [Option("source",
                Required = false, Default = "/",
                HelpText = "Directory in source from which to upload.")]
        public string SourcePath { get; set; }

        [Option("destination",
                Required = false, Default = "/",
                HelpText = "Directory in destination to which to upload.")]
        public string DestinationPath { get; set; }

        // Options.

        [Option("ignoreUnchanged",
                Required = false, Default = false,
                HelpText = "Do not upload any file that hasn't changed.  Setting to true will be slower than leaving false.")]
        public bool IgnoreUnchanged { get; set; }

    }
}