using OpenCvSharp;
using System;
using System.Drawing;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;

namespace Test_MachineLearning
{
    public partial class MAIN
    {

        public void OpenCV_Test() // test OpenCV
        {
            Mat image = BufferToMat(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);
            Mat image_Canny = new Mat();

            Cv2.Threshold(image, image_Canny, ThrValue1, ThrValue2, ThresholdTypes.Binary);

            //Cv2.Canny(image_Canny, image_Canny, ThrValue1, ThrValue2);

            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(image_Canny, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);

            Cv2.CvtColor(image_Canny, image_Canny, ColorConversionCodes.GRAY2RGB);
            Cv2.CvtColor(image, image, ColorConversionCodes.GRAY2RGB);

            Cv2.DrawContours(image_Canny, contours, -1, Scalar.Red, 1);

            // Cerca il contorno con l'area più grande
            double maxArea = 0;
            int maxAreaContourIndex = -1;
            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > maxArea && area < (buffer_Image_sizeX - 2) * (buffer_Image_sizeY - 2))
                {
                    maxArea = area;
                    maxAreaContourIndex = i;
                }
            }

            Cv2.DrawContours(image, contours, maxAreaContourIndex, Scalar.Yellow, 1);

            if (contours.Length > 2)
            {
                // Calcola i momenti del contorno
                Moments moments = Cv2.Moments(contours[maxAreaContourIndex]);

                // Calcola le coordinate x e y del centro del contorno
                double centerX = moments.M10 / moments.M00;
                double centerY = moments.M01 / moments.M00;

                // Calcola la rotondità del contorno
                double perimeter = Cv2.ArcLength(contours[maxAreaContourIndex], true);
                double circularity = (4 * Math.PI * moments.M00) / (perimeter * perimeter);


                Font font = new Font("Lucida Sans Unicode", 25);
                //SolidBrush brush = new SolidBrush(System.Drawing.Color.Green);

                // Verifica se il contorno rappresenta un cerchio
                double threshold = 0.72; // valore di soglia arbitrario
                if (circularity > threshold)
                {
                    RotatedRect ellipse = Cv2.FitEllipse(contours[maxAreaContourIndex]);
                    Cv2.Ellipse(image, ellipse, Scalar.Green, 3);


                    SolidBrush brush = new SolidBrush(System.Drawing.Color.DarkOrange);
                    //image = ScriviOnMat(image, "Larghezza = " + Convert.ToString(ellipse.Size.Width) + " Altezza = " + Convert.ToString(ellipse.Size.Height), font, brush, (float)ellipse.Center.X, (float)ellipse.Center.Y);

                    string txt = "Centro Y = " + Convert.ToString(Math.Round((ellipse.Center.Y - 461) * 5 / 960, 3)) + " mm";

                    image = ScriviOnMat(image, txt, font, brush, (float)(ellipse.Center.X - (txt.Length * 20 / 2)), (float)ellipse.Center.Y + 30);

                }
                else
                {
                    string txt = "Cerchio non trovato!";
                    SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
                    image = ScriviOnMat(image, txt, font, brush, (float)(centerX - (txt.Length * 20 / 2)), (float)centerY);
                }






            }


            pictureBox1.Invoke((MethodInvoker)delegate
            {
                //pictureBox1.Image = MatToBitmapGray(image_Canny);
                pictureBox1.Image = MatToBitmapRGB(image);
                //pictureBox1.Image = MatToBitmapRGB(image_Canny);

                //pictureBox1.Width = buffer_Image_sizeX;
                //pictureBox1.Height = buffer_Image_sizeY;
            });









        }


        public Bitmap OpenCV_Test2() // test OpenCV
        {
            Mat image = BufferToMat(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);
            Mat image2 = BufferToMat(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);

            Mat image_Canny = new Mat();

            Cv2.Threshold(image, image_Canny, ThrValue1, ThrValue2, ThresholdTypes.Binary);

            //Cv2.Canny(image_Canny, image_Canny, ThrValue1, ThrValue2);


            //image2 = image_Canny;

            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(image_Canny, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);

            Cv2.CvtColor(image_Canny, image_Canny, ColorConversionCodes.GRAY2RGB);
            Cv2.CvtColor(image, image, ColorConversionCodes.GRAY2RGB);
            Cv2.CvtColor(image2, image2, ColorConversionCodes.GRAY2RGB);

            Cv2.Threshold(image2, image2, ThrValue1, ThrValue2, ThresholdTypes.Binary);

            Cv2.DrawContours(image_Canny, contours, -1, Scalar.Red, 1);

            // Cerca il contorno con l'area più grande
            double maxArea = 0;
            int maxAreaContourIndex = -1;
            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > maxArea && area < (buffer_Image_sizeX - 2) * (buffer_Image_sizeY - 2))
                {
                    maxArea = area;
                    maxAreaContourIndex = i;
                }
            }


            //Cv2.DrawContours(image, contours, maxAreaContourIndex, Scalar.Yellow, 1);
            Cv2.DrawContours(image, contours, -1, Scalar.Yellow, 1);

            if (contours.Length > 2)
            {
                Font font = new Font("Lucida Sans Unicode", 25);
                //SolidBrush brush = new SolidBrush(System.Drawing.Color.Green);

                //string txt = "Cerchio non trovato!";
                //SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
                //image = ScriviOnMat(image, txt, font, brush, (float)1280 / 2, (float)960 / 2);
            }

            Bitmap Im1, Im2, Im3;

            Im1 = MatToBitmapRGB(image);
            Im2 = MatToBitmapRGB(image2);

            Im3 = BlobDetection(Im2, Im1);



            return Im3;

        }



        public Bitmap OpenCV_Test3() // test OpenCV
        {
            Mat image = BufferToMat(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);
            Mat image2 = BufferToMat(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);

            Cv2.CvtColor(image, image, ColorConversionCodes.GRAY2RGB);
            Cv2.CvtColor(image2, image2, ColorConversionCodes.GRAY2RGB);

            Cv2.Threshold(image2, image2, ThrValue1, ThrValue2, ThresholdTypes.Binary);

            Bitmap Im1 = MatToBitmapRGB(image);
            Bitmap Im2 = MatToBitmapRGB(image2);

            return BlobDetection(Im2, Im1);
        }





    }
}