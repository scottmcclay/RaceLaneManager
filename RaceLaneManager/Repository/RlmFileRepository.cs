using System;
using System.Collections.Generic;
using RaceLaneManager.Model;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RaceLaneManager.Repository
{
    public class RlmFileRepository : IRlmRepository
    {
        private string dataPath;
        private readonly string tournamentsDirectoryName = "RlmTournaments";

        public RlmFileRepository()
        {
            string environmentVariableName = "LocalAppData";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Debug.WriteLine("Platform is Unix");
                environmentVariableName = "HOME";
            }

            string rootPath = Environment.GetEnvironmentVariable(environmentVariableName);
            Debug.WriteLine("RlmFileRepository: Using environment variable {0}: {1}", environmentVariableName, rootPath);

            CreateTournamentsDirectory(rootPath);
        }

        private void CreateTournamentsDirectory(string rootPath)
        {
            this.dataPath = Path.Combine(rootPath, this.tournamentsDirectoryName);
            Debug.WriteLine("Data path: {0}", this.dataPath);

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
        }

        private IEnumerable<int> ReadAllTournamentIDs()
        {
            List<int> result = new List<int>();

            DirectoryInfo di = new DirectoryInfo(this.dataPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                string nameWithoutExtension = fi.Name.Replace(".json", "");
                result.Add(int.Parse(nameWithoutExtension));
            }

            return result;
        }

        private Tournament ReadTournament(int tournamentID)
        {
            string fileName = string.Format("{0}.json", tournamentID);
            string filePath = Path.Combine(this.dataPath, fileName);
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject(json, typeof(Tournament)) as Tournament;
        }

        private void WriteTournament(Tournament tournament, bool overwrite = true)
        {
            string fileName = string.Format("{0}.json", tournament.ID);
            string filePath = Path.Combine(this.dataPath, fileName);
            if ((overwrite == false) && File.Exists(filePath))
            {
                throw new ArgumentException(string.Format("Tournament with ID {0} already exists.", tournament.ID));
            }

            string json = JsonConvert.SerializeObject(tournament, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public Tournament AddTournament(Tournament tournament)
        {
            // find an ID to assign to this new Tournament
            int maxID = 0;
            foreach (int i in ReadAllTournamentIDs())
            {
                if (i > maxID)
                {
                    maxID = i;
                }
            }

            tournament.ID = maxID + 1;

            WriteTournament(tournament, false);
            return tournament;
        }

        public Tournament DeleteTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public IList<Tournament> GetAllTournaments()
        {
            List<Tournament> result = new List<Tournament>();

            foreach (int i in ReadAllTournamentIDs())
            {
                result.Add(GetTournament(i));
            }

            return result;
        }

        public Tournament GetTournament(int tournamentID)
        {
            return ReadTournament(tournamentID);
        }

        public Tournament UpdateTournament(int tournamentID, string newName, int numLanes)
        {
            Tournament tournament = ReadTournament(tournamentID);
            if (tournament == null)
            {
                throw new ArgumentException(string.Format("Tournament with ID {0} does not exist.", tournamentID));
            }

            tournament.Name = newName;
            tournament.NumLanes = numLanes;
            WriteTournament(tournament);
            return tournament;
        }
    }
}
