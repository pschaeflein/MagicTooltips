namespace MagicTooltips.Providers
{
  public interface IProvider
  {
    string ProviderKey { get; }
    string DefaultCommands { get; }
    string DefaultNounPrefixes { get; }
    string DefaultFgColor { get; }
    string DefaultBgColor { get; }
    string DefaultTemplate { get; }
    string GetValue();
  }
}
