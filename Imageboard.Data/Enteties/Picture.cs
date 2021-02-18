using System.ComponentModel.DataAnnotations.Schema;

namespace Imageboard.Data.Enteties
{
    [Table("Picture")]
    public class Picture
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Format { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ViewHeight { get; set; }
        public int ViewWidth { get; set; }

        public Picture() { }

        public Picture(string path, string name, string format, int length, int height, int width, int vheight, int vwidth)
        {
            Path = path;
            Name = name;
            Format = format;
            Size = length;
            Height = height;
            Width = width;
            ViewHeight = vheight;
            ViewWidth = vwidth;
        }
    }
}
