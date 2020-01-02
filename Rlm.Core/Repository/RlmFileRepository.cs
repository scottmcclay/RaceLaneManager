using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ChaoticRotation.Core;

namespace Rlm.Core
{
    public class RlmFileRepository : IRlmRepository
    {
        private ILogger _logger;

        private Object _lock = new Object();
        private string _dataPath;
        private readonly string _tournamentsDirectoryName = "RlmTournaments";

        public RlmFileRepository()
        {
            string environmentVariableName = "LocalAppData";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                _logger?.LogMessage(this, "Platform is Unix");
                environmentVariableName = "HOME";
            }

            string rootPath = Environment.GetEnvironmentVariable(environmentVariableName);
            _logger?.LogMessage(this, $"RlmFileRepository: Using environment variable {environmentVariableName}: {rootPath}");

            CreateTournamentsDirectory(rootPath);
        }

        private void CreateTournamentsDirectory(string rootPath)
        {
            lock (_lock)
            {
                this._dataPath = Path.Combine(rootPath, this._tournamentsDirectoryName);
                _logger?.LogMessage(this, string.Format("Data path: {0}", this._dataPath));

                if (!Directory.Exists(_dataPath))
                {
                    Directory.CreateDirectory(_dataPath);
                }
            }
        }

        public IEnumerable<int> GetAllTournamentIDs()
        {
            List<int> result = new List<int>();

            lock (_lock)
            {
                DirectoryInfo di = new DirectoryInfo(this._dataPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    string nameWithoutExtension = fi.Name.Replace(".json", "");
                    result.Add(int.Parse(nameWithoutExtension));
                }
            }

            return result;
        }

        public Tournament LoadTournament(int tournamentID)
        {
            string fileName = string.Format("{0}.json", tournamentID);
            string filePath = Path.Combine(this._dataPath, fileName);
            string json = string.Empty;

            lock (_lock)
            {
                json = File.ReadAllText(filePath);
            }

            return JsonConvert.DeserializeObject(json, typeof(Tournament)) as Tournament;
        }

        public void SaveTournament(Tournament tournament)
        {
            string fileName = string.Format("{0}.json", tournament.ID);
            string filePath = Path.Combine(this._dataPath, fileName);
            //if ((overwrite == false) && File.Exists(filePath))
            //{
            //    throw new ArgumentException(string.Format("Tournament with ID {0} already exists.", tournament.ID));
            //}

            string json = JsonConvert.SerializeObject(tournament, Formatting.Indented);

            lock (_lock)
            {
                File.WriteAllText(filePath, json);
            }
        }

    }
}
