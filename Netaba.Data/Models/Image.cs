
namespace Netaba.Data.Models
{
    public class Image
    {
        public int Id { get; private set; }
        public string Path { get; private set; }
        public string Format { get; private set; }
        public string Name { get; private set; }
        public string SizeDesc { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int ViewHeight { get; private set; }
        public int ViewWidth { get; private set; }

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
