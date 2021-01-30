
namespace Imageboard.Markup
{
    public enum Mark
    {
        /* The paired marks. */
        Monospace,
        Bold,
        Italic,
        Spoler,

        /* The new line marks. They are non paired and must be at the beginning of a new line. */
        NewLine,
        OList, // ordered list
        UnList, // unordered list
        Quote,
        Link, // link to another post 

        End,
        None
    }
}

