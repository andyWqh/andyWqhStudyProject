using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace HelpClassLib.Web
{
    /// <summary>
    /// Class Image.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// 彩色图片转换成黑白图片
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap ToGrayscale(Bitmap bitmap)
        {
            var bm = new Bitmap(bitmap.Width, bitmap.Height);
            for (int y= 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    var c = bitmap.GetPixel(x, y);
                    var rgb = (int)(c.R*0.3 + c.G*0.59 + c.B*0.11);
                    bm.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            }
            return bm;
        }

        /// <summary>
        /// 放大缩小图片尺寸
        /// </summary>
        /// <param name="picPath"></param>
        /// <param name="reSizePicPath"></param>
        /// <param name="iSize"></param>
        /// <param name="format"></param>
        public void PicSized(string picPath, string reSizePicPath, int iSize, ImageFormat format)
        {
            var originBmp = new Bitmap(picPath);

            var w = originBmp.Width * iSize;
            var h = originBmp.Height * iSize;

            var resizedBmp = new Bitmap(w, h);
            var g = Graphics.FromImage(resizedBmp);

            //设置高质量插值法   
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度   
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //消除锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawImage(originBmp, new Rectangle(0, 0, w, h), new Rectangle(0, 0, originBmp.Width, originBmp.Height),
                        GraphicsUnit.Pixel);

            resizedBmp.Save(reSizePicPath, format);

            g.Dispose();
            resizedBmp.Dispose();
            originBmp.Dispose();
        }
    }
}
