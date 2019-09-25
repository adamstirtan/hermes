namespace Hermes.Commands
{
    public interface ICommand
    {
        string Trigger { get; set; }

        void Execute();
    }
}