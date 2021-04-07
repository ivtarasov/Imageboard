# Netaba

Netaba is yet another imageboard written in ASP.NET Core and Entity Framework Сore. It implements basic imageboard functionality.

# Features

* Attaching images (one per post)
* Thumbnail images
* Deleting posts by password + ip
* Server-side paging
* Simple markup:
    * Monospaced —  `` `example1` ``
    * Bold — `*example2*`
    * Italic — `_example3_`
    * Spoiler — `#example4#`
    * Ordered list —
                      №Item1
                      №Item2
                      №Item3
    * Unordered list —
                        +Item1
                        +Item2
                        +Item3
    * Quote (must be at the beginning of a line) — `>example5`
    * Link to post (must be at the beginning of a line and post must be on the same board)  — `>>example6`
* Administrative functionality:
    * Deleting posts and treads
    * Adding/deleting boards
    * Adding/deleting admins
* Docker

