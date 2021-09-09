using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Darkit.Photo.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(string f in Directory.GetFiles("../../data"))
            {
                Console.WriteLine($"compress {f}");
                string d = Path.GetDirectoryName(f);
                string n = Path.GetFileName(f);
                string t = Path.Combine(d, $"PC-{n}");
                string b = Path.Combine(d, $"B64-{n}");
                PhotoCompression.Compress(f, t, 1000, 1000);
                string b64 = PhotoCompression.ToBase64(f, 1000, 1000);
                File.WriteAllBytes(b, Convert.FromBase64String(b64));
                Console.WriteLine($"b64 size: {b64.Length}");
            }
        }
    }
}
