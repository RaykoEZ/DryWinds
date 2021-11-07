namespace Curry.Ai
{
    public delegate void OnAiAlertGroup();
    public interface IMember 
    {
        event OnAiAlertGroup OnAlert;
        void Alert();
    }
}
