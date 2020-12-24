namespace MagicTooltips.Providers
{
    public interface IProvider
    {
        string GetValue();
        string DefaultCommands { get; }
        string DefaultFgColor { get; }
        string DefaultBgColor { get; }
        string DefaultTemplate { get; }
    }
}
