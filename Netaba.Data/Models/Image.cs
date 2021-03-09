
namespace Netaba.Data.Models
{
    public class Image
    {
        public int Id { get; }
        public string Path { get; }
        public string Format { get; }
        public string Name { get; }
        public string SizeDesc { get; }
        public int Height { get; }
        public int Width { get; }
        public int ViewHeight { get; }
        public int ViewWidth { get; }

        public Image(int id, string path, string name, string format, string sizeDesc, int height, int width, int vheight, int vwidth) 
            : this(path, name, format, sizeDesc, height, width, vheight, vwidth)
        {
            Id = id;
        }

        public Image(string path, string name, string format, string sizeDesc, int height, int width, int vheight, int vwidth)
        {
            Path = path;
            Name = name;
            Format = format;
            SizeDesc = sizeDesc;
            Height = height;
            Width = width;
            ViewHeight = vheight;
            ViewWidth = vwidth;
        }
    }
}
