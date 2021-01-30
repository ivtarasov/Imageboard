
namespace Imageboard.Markup
{
    public enum Mark
    {
        /* The paired marks. */
        Monospace,
        Bold,
        Italic,
        Spoler,

        NewLine,

        /* The new line marks. They are non paired and must be at the beginning of a new line. */
        OList, // ordered list
        UnList, // unordered list
        ListElem,
        Quote,
        Link, // link to another post 

        End,
        None
    }
}

