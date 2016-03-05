using EnvDTE;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PackageInstaller
{
    class Typings : BasePackageProvider
    {
        private static string _urlBase = "https://api.typings.org/";
        private static ImageSource _icon = BitmapFrame.Create(new Uri("pack://application:,,,/PackageInstaller;component/Resources/typings.png", UriKind.RelativeOrAbsolute));

        public override string Name
        {
            get { return "Typings"; }
        }

        public override ImageSource Icon
        {
            get { return _icon; }
        }

        public override async Task<IEnumerable<string>> GetPackages(string term = null)
        {
            string url = $"{_urlBase}search?query={Uri.EscapeUriString(term)}";

            using(var client = new WebClient())
            {
                string json = await client.DownloadStringTaskAsync(url);
                return ToList(json);
            }
        }

        private static IEnumerable<string> ToList(string json)
        {
            try
            {
                var root = JObject.Parse(json);
                var array = (JArray)root["results"];

                return array.Select(a => a["name"].ToString());
            }
            catch(Exception ex)
            {
                Logger.Log(ex);
                return Enumerable.Empty<string>();
            }
        }

        public async override Task<IEnumerable<string>> GetVersion(string packageName)
        {
            if (string.IsNullOrEmpty(packageName))
                return Enumerable.Empty<string>();

            string searchUrl = $"{_urlBase}search?name={Uri.EscapeUriString(packageName)}";
            string versionJson = "{}";

            using(var client = new WebClient())
            {
                var json = await client.DownloadStringTaskAsync(searchUrl);

                var searchRoot = JObject.Parse(json);
                var searchArray = (JArray)searchRoot["results"];
                var searchResult = searchArray.First();

                var source = searchResult["source"];
                var versionUrl = $"{_urlBase}entries/{source}/{Uri.EscapeUriString(packageName)}/versions";

                versionJson = await client.DownloadStringTaskAsync(versionUrl);
            }

            var versionRoot = JObject.Parse(versionJson);
            var versionArray = versionRoot["versions"];

            if (versionArray == null)
                return Enumerable.Empty<string>();

            return from version in versionArray
                   orderby version["version"].ToString() descending
                   select version["version"].ToString();
        }

        public override async Task<bool> InstallPackage(Project project, string packageName, string version)
        {
            if (!string.IsNullOrEmpty(version))
                packageName += $"@{version}";

            string arg = $"/c typings install {packageName} {VSPackage.Settings.TypingsArguments}";
            string cwd = project.GetRootFolder();

            return await CallCommand(arg, cwd);
        }
    }
}
