using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imageboard.Web.Models
{
    public class TreadModel
    {
        public Tread Tread { get; set; }
        public string AboutOmittedPosts { get; set; }
        public int FirstDisplayedPost { get; set; }
        public bool IsShortcut { get; set; }
    }
}
