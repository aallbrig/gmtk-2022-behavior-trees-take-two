namespace Model.Player
{
    public interface IPlayerActionFSM
    {
        public IPlayerActionState CurrentState { get; }
    }
}