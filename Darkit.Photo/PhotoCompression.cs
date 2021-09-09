using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Darkit.Photo
{
    /// <summary>
    /// 图片压缩
    /// </summary>
    public static class PhotoCompression
    {
        public static string ToBase64(string source, int width, int height)
        {
            using (Image si = Image.FromFile(source))
            {
                using (Image ti = Compress(si, width, height))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ti.Save(ms, si.RawFormat);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static void Compress(string source, string target, int width, int height)
        {
            using (Image si = Image.FromFile(source))
            {
                using (Image ti = Compress(si, width, height))
                {
                    ti.Save(target);
                }
            }
        }

        public static Image Compress(Image source, int width, int height)
        {
            // 等比例压缩。
            if (source.Width > width && source.Height > height)
            {
                // 先把宽度调到，看看高度能不能容纳。
                int rw = source.Width / width;
                int nw = width;
                int nh = source.Height / rw;
                if (height < nh) // 无法容纳高度，使用高度。
                {
                    int rh = source.Height / height;
                    nw = source.Width / rh;
                    nh = height;
                }
                // 构建新图，开始绘画。
                Bitmap target = new Bitmap(nw, nh);
                using (Graphics gr = Graphics.FromImage(target))
                {
                    Rectangle rect = new Rectangle(0, 0, nw, nh);
                    gr.Clear(Color.White);
                    gr.CompositingQuality = CompositingQuality.HighQuality;
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.DrawImage(source, rect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
                }
                return target;
            }
            // 质量压缩。
            Bitmap r = new Bitmap(source.Width, source.Height);
            using (Graphics gr = Graphics.FromImage(r))
            {
                Rectangle rect = new Rectangle(0, 0, source.Width, source.Height);
                gr.Clear(Color.White);
                gr.CompositingQuality = CompositingQuality.HighSpeed;
                gr.SmoothingMode = SmoothingMode.HighSpeed;
                gr.InterpolationMode = InterpolationMode.Low;
                gr.DrawImage(source, rect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
            }
            return r;
        }
    }
}
