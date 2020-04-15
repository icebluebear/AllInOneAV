using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ImageHelper
    {
        public static void CombinePics(string destPath, List<string> pics, bool keepOld)
        {
            int width = 0;
            int height = 0;
            int drawWidth = 0;
            int drawHeight = 0;

            List<ImageCombine> images = new List<ImageCombine>();

            foreach (var pStr in pics)
            {
                if (File.Exists(pStr))
                {
                    Image tempImg = Image.FromFile(pStr);
                    Bitmap tempBm = new Bitmap(tempImg);

                    width = Math.Max(width, tempImg.Width);
                    height += tempImg.Height;

                    images.Add(new ImageCombine
                    {
                        Bitmap = tempBm,
                        Image = tempImg
                    });
                }
            }

            Bitmap bitMap = new Bitmap(width, height);
            Graphics g1 = Graphics.FromImage(bitMap);
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));

            foreach (var img in images)
            {
                g1.DrawImage(img.Bitmap, drawWidth, drawHeight, img.Image.Width, img.Image.Height);

                drawHeight += img.Image.Height;
            }

            Image finalImg = bitMap;
            finalImg.Save(destPath);

            if (!keepOld)
            {
                foreach (var p in pics)
                {
                    File.Delete(p);
                }
            }
        }
    }

    public class ImageCombine
    {
        public Image Image { get; set; }
        public Bitmap Bitmap { get; set; }
    }
}
