using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TFS.Core
{
    public class BitmapConverter
    {
        public static Bitmap ToTransparent(Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color h = image.GetPixel(i, j);
                    if (h.A >= 230 && h.R >= 230 && h.G >= 230 && h.B >= 230)
                    {
                        image.SetPixel(i, j, Color.Red);
                    }
                }
            }
            image.MakeTransparent(Color.Red);
            return image;
        }

        public static byte[] ToJpeg(byte[] image)
        {
            using (Bitmap bmp1 = new Bitmap(new MemoryStream(image)))
            {
                using (MemoryStream mstream = new MemoryStream())
                {
                    bmp1.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] byData = new Byte[mstream.Length];
                    mstream.Position = 0;
                    mstream.Read(byData, 0, byData.Length);
                    mstream.Close();
                    bmp1.Dispose();
                    return byData;
                }
            }
        }

        public static void SaveAs(string fileName, byte[] image, ImageFormat imageFormat)
        {
            using (Bitmap bmp = new Bitmap(new MemoryStream(image)))
            {
                bmp.Save(fileName, imageFormat);
                bmp.Dispose();
            }
        }

        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static byte[] ToEncoder(byte[] image, long value, ImageFormat imageFormat)
        {
            try
            {
                if (image == null)
                    return null;
                using (Bitmap bmp1 = new Bitmap(new MemoryStream(image)))
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(imageFormat);

                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, value);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    MemoryStream ms = new MemoryStream();
                    bmp1.Save(ms, jpgEncoder, myEncoderParameters);
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                return image;
            }
        }

    }
}
