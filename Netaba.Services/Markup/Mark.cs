
namespace Netaba.Services.Markup
{
    public enum Mark
    {
        // Сan be everywhere 
        Monospace,
        Bold,
        Italic,
        Spoiler,

        // Marks that must be at the beginning of a line to be recognized 
        Link,
        OList,
        UnList,
        ListElem,
        Quote,

        // Mark that is used only for parsing
        NewLine,
        Edge,
        None
    }
}

