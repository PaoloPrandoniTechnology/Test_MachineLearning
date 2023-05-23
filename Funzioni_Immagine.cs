using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;



namespace Test_MachineLearning
{
    public partial class MAIN
    {

        public void Visulizza_Immagine_buffer_image() // Visulizza immagine da buffer_Image in PictureBox
        {
            using (MemoryStream stream = new MemoryStream(buffer_Image))
            {
                Bitmap bitmapTemp = new Bitmap(buffer_Image_sizeX, buffer_Image_sizeY, PixelFormat.Format8bppIndexed);

                ColorPalette palette = bitmapTemp.Palette;
                for (int p = 0; p < palette.Entries.Length; p++)
                {
                    palette.Entries[p] = Color.FromArgb(p, p, p);
                }
                bitmapTemp.Palette = palette;

                Rectangle rect = new Rectangle(0, 0, buffer_Image_sizeX, buffer_Image_sizeY);
                BitmapData bmpData = bitmapTemp.LockBits(rect, ImageLockMode.WriteOnly, bitmapTemp.PixelFormat);
                IntPtr scan0 = bmpData.Scan0;
                Marshal.Copy(buffer_Image, 0, scan0, buffer_Image.Length);
                bitmapTemp.UnlockBits(bmpData);

                pictureBox1.Image = bitmapTemp;

            }
        }

        public Bitmap BufferToBitmapRGB(byte[] buffer, int bufferSizeX, int bufferSizeY) // Da buffer a Bitmap RGB
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                Bitmap bitmapTemp = new Bitmap(bufferSizeX, bufferSizeY, PixelFormat.Format8bppIndexed);

                ColorPalette palette = bitmapTemp.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    palette.Entries[i] = Color.FromArgb(255, i, i, i);
                }
                bitmapTemp.Palette = palette;

                Rectangle rect = new Rectangle(0, 0, bufferSizeX, bufferSizeY);
                BitmapData bmpData = bitmapTemp.LockBits(rect, ImageLockMode.WriteOnly, bitmapTemp.PixelFormat);
                IntPtr scan0 = bmpData.Scan0;
                Marshal.Copy(buffer, 0, scan0, buffer.Length);
                bitmapTemp.UnlockBits(bmpData);

                return bitmapTemp;

            }
        }

        public Bitmap MatToBitmapGray(Mat mat)
        {
            // Crea un'immagine bitmapTemp
            Bitmap bitmapTemp = new Bitmap(mat.Width, mat.Height, PixelFormat.Format8bppIndexed); //Format24bppRgb  Format8bppIndexed  Format1bppIndexed

            ColorPalette palette = bitmapTemp.Palette;
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            bitmapTemp.Palette = palette;

            // Copia i valori della matrice nella bitmap
            BitmapData bitmapData = bitmapTemp.LockBits(new Rectangle(0, 0, bitmapTemp.Width, bitmapTemp.Height), ImageLockMode.WriteOnly, bitmapTemp.PixelFormat);
            int stride = bitmapData.Stride;
            byte[] bytes = new byte[mat.Width * mat.Height];
            Marshal.Copy(mat.Data, bytes, 0, bytes.Length);

            unsafe
            {
                byte* p = (byte*)bitmapData.Scan0.ToPointer();
                for (int row = 0; row < mat.Rows; row++)
                {
                    for (int col = 0; col < mat.Cols; col++)
                    {
                        int index = row * mat.Cols + col;
                        p[row * stride + col] = BitConverter.GetBytes(bytes[index])[0];
                    }
                }
            }
            bitmapTemp.UnlockBits(bitmapData);

            return bitmapTemp;
        }

        public Bitmap MatToBitmapRGB(Mat mat)
        {
            // Crea un'immagine bitmapTemp
            Bitmap bitmapTemp = new Bitmap(mat.Width, mat.Height, PixelFormat.Format24bppRgb); //Format24bppRgb  Format8bppIndexed  Format1bppIndexed

            // Copia i valori della matrice nella bitmap
            BitmapData bitmapData = bitmapTemp.LockBits(new Rectangle(0, 0, bitmapTemp.Width, bitmapTemp.Height), ImageLockMode.WriteOnly, bitmapTemp.PixelFormat);
            int stride = bitmapData.Stride;
            byte[] bytes = new byte[mat.Width * mat.Height];
            Marshal.Copy(mat.Data, bytes, 0, bytes.Length);

            unsafe
            {
                byte* ptrMat = (byte*)mat.DataPointer;
                byte* ptrBitmap = (byte*)bitmapData.Scan0;

                for (int y = 0; y < bitmapData.Height; ++y)
                {
                    for (int x = 0; x < bitmapData.Width; ++x)
                    {
                        ptrBitmap[2] = ptrMat[2];
                        ptrBitmap[1] = ptrMat[1];
                        ptrBitmap[0] = ptrMat[0];

                        ptrMat += 3;
                        ptrBitmap += 3;
                    }
                    ptrMat += mat.Step() - (bitmapData.Width * 3);
                    ptrBitmap += bitmapData.Stride - (bitmapData.Width * 3);
                }
            }
            bitmapTemp.UnlockBits(bitmapData);

            return bitmapTemp;
        }

        public Mat BufferToMat(byte[] buffer, int bufferSizeX, int bufferSizeY)
        {
            int width = bufferSizeX;
            int height = bufferSizeY;
            byte[] ImageBytes = new byte[height * width];

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                Bitmap bitmapTemp = new Bitmap(bufferSizeX, bufferSizeY, PixelFormat.Format8bppIndexed);

                ColorPalette palette = bitmapTemp.Palette;
                for (int p = 0; p < palette.Entries.Length; p++)
                {
                    palette.Entries[p] = Color.FromArgb(p, p, p);
                }
                bitmapTemp.Palette = palette;

                Rectangle rect = new Rectangle(0, 0, bufferSizeX, bufferSizeY);
                BitmapData bmpDat = bitmapTemp.LockBits(rect, ImageLockMode.WriteOnly, bitmapTemp.PixelFormat);
                IntPtr scan0 = bmpDat.Scan0;
                Marshal.Copy(buffer, 0, scan0, buffer.Length);
                bitmapTemp.UnlockBits(bmpDat);

                BitmapData bmpData = bitmapTemp.LockBits(new Rectangle(0, 0, buffer_Image_sizeX, buffer_Image_sizeY), ImageLockMode.ReadOnly, bitmapTemp.PixelFormat);
                Marshal.Copy(bmpData.Scan0, ImageBytes, 0, ImageBytes.Length);
                bitmapTemp.UnlockBits(bmpData);

                Mat mat = new Mat(bufferSizeY, bufferSizeX, MatType.CV_8UC1, ImageBytes);

                return mat;
            }
        }

        public Mat ScriviOnMat(Mat img, string testo, Font font, SolidBrush brush, float x, float y)
        {
            Bitmap image_scritta = MatToBitmapRGB(img);

            using (Graphics g = Graphics.FromImage(image_scritta))
            {
                Font fnt = font;
                SolidBrush brsh = brush;

                // Scrive il testo sull'immagine alle coordinate (x, y)
                float x1 = x;
                float y1 = y;
                g.DrawString(testo, fnt, brsh, x1, y1);

                BitmapConverter.ToMat(image_scritta, img);

                return img;
            }
        }



    }
}