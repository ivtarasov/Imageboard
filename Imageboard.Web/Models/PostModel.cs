using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imageboard.Web.Models
{
    public class PostModel
    {
        public Post Post { get; set; }
        public int PostNumber { get; set; }
        public bool IsFromShortcutTread { get; set; }
    }
}
