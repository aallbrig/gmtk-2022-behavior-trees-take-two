namespace Model.Interfaces
{
    public interface IMonobehaviourDebugLogger
    {
        public void DebugLog(string logMessage);
        public bool DebugEnabled { get; set; }
    }
}