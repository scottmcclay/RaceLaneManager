namespace ChaoticRotation.Core
{
    public interface ILogger
    {
        void LogMessage(string message);
        void LogMessage(object sender, string message);
    }
}
