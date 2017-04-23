using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Repository
{
    public static class RepositoryManager
    {
        private static IRaceEventRepository _repo = null;
        public static IRaceEventRepository GetDefaultRepository()
        {
            if (_repo == null)
            {
                _repo = new InMemoryRaceEventRepository();
            }

            return _repo;
        }
    }
}
