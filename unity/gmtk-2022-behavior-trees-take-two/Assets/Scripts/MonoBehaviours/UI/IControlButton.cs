namespace MonoBehaviours.UI
{
    public interface IControlButton
    {
        public string Label { get; }
        public string Description { get; }
        public void Execute(IControlsMenu ctx);
    }
}