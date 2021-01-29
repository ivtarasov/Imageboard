
namespace Imageboard.Markup
{
    public enum Mark
    {
        /* PairedMarks. */
        Monospace,
        Bold,
        Italic,
        Spoler,

        /* NotPairedMarks. */
        NewLine,
        OList, // ordered list
        UnList, // unordered list
        Quote,
        Link, // link to another post 

        End,
        None
    }
}

