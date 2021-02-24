using System;

namespace Imageboard.Services.ImageHandling
{
    static class SizeReformer
    {
        static public string ToReadableForm(long size)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB"};

            int mag = (int)Math.Log(size, 1024);
            long adjustedSize = size / (1L << (mag * 10));

            return $"{adjustedSize} {SizeSuffixes[mag]}";
        }
    }
}