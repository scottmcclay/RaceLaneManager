using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Repository
{
    public static class RepositoryManager
    {
        private static IRlmRepository _repo = null;
        public static IRlmRepository GetDefaultRepository()
        {
            if (_repo == null)
            {
                _repo = new RlmFileRepository();
            }

            return _repo;
        }
    }
}
