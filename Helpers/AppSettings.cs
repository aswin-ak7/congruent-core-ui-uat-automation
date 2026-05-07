using Microsoft.Playwright;
using static System.Net.Mime.MediaTypeNames;

namespace Helpers
{
    public class AppSettings
    {
        public AppSettings()
        {
            ApplicationURL = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Args = new List<string>();
        }

        public const string Name = "AppSettings";
        internal static string? _appSettings;

        public string ApplicationURL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ftp_user { get; set; }
        public string ftp_password { get; set; }
        public string ftp_host { get; set; }
        public bool Headless { get; set; }
        public int SlowMotion { get; set; }

        public List<string> Args { get; set; }

        public static IPage Page { get; set; }

    }
}
