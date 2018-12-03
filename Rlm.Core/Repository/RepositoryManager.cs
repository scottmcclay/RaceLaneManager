namespace Rlm.Core
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
