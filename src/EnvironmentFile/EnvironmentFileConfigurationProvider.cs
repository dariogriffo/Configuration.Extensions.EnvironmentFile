using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Configuration.Extensions.EnvironmentFile
{
    internal class EnvironmentFileConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly string _fileName;
        private readonly bool _trim;
        private readonly bool _removeWrappingQuotes;
        private readonly string _prefix;
        private readonly FileSystemWatcher _watcher;

        internal EnvironmentFileConfigurationProvider(
            string fileName,
            bool trim,
            bool removeWrappingQuotes,
            string prefix,
            bool reloadOnChange
        )
        {
            _fileName = fileName;
            _trim = trim;
            _removeWrappingQuotes = removeWrappingQuotes;
            _prefix = prefix;
            if (!reloadOnChange)
            {
                return;
            }
            
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(_fileName));
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Load();
            OnReload();
        }
        
        public override void Load()
        {
            LoadData();
        }

        private void LoadData()
        {
            var nonCommentLinesWithPropertyValues =
                File
                    .ReadAllLines(_fileName)
                    .Select(x => x.TrimStart())
                    .Select(x => string.IsNullOrWhiteSpace(_prefix) ? x : x.Replace(_prefix, string.Empty))
                    .Where(x => !x.StartsWith("#") && x.Contains("="));

            string ParseQuotes(string line)
            {
                if (!_removeWrappingQuotes)
                {
                    return line;
                }

                var parts = line.Split('=');
                line = string.Join("=", parts.Skip(1));
                return $"{parts[0]}={line.Trim('"')}";
            }

            string RemoveCommentsAtTheEndAndTrimIfNecessary(string line)
            {
                return _trim ? line.Trim() : line;
            }

            var configuration =
                nonCommentLinesWithPropertyValues
                    .Select(ParseQuotes)
                    .Select(RemoveCommentsAtTheEndAndTrimIfNecessary)
                    .Select(x => x.Split('='))
                    .Select(x =>
                        new KeyValuePair<string, string>(x[0].Replace("__", ":"), string.Join("=", x.Skip(1))));
            Data = configuration.ToDictionary(x => x.Key, x => x.Value);
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}
