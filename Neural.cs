using Accord.MachineLearning.VectorMachines;
using AForge;
using AForge.Math.Geometry;
using System.Collections.Generic;
using System.Drawing;
using Gaussian = Accord.Statistics.Kernels.Gaussian;

namespace Test_MachineLearning
{
    public partial class MAIN
    {

        private List<(Bitmap, string)> imageList;
        private SupportVectorMachine<Accord.Statistics.Kernels.Gaussian> svm;
        private bool isLearningMode;
        Dictionary<string, SupportVectorMachine<Gaussian>> trainedModels = new Dictionary<string, SupportVectorMachine<Gaussian>>();


        Bitmap _bitmapEdgeImage, _bitmapBinaryImage, _bitmapGreyImage;

        System.Drawing.Font _font = new System.Drawing.Font("Times New Roman", 32, FontStyle.Regular);
        System.Drawing.SolidBrush _brush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

        Pen _PictureboxPen = new Pen(Color.Red, 4);

        int ipenWidth = 5, iFeatureWidth;
        int iThreshold = 40, iRadius = 40;



        #region MyMethods

        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        private double FindDistance(int _pixel)
        {
            ///
            /// distance(D): distance of object from the camera
            /// _focalLength(F): focal length of camera
            /// _pixel(P): apparent width in pixel
            /// _ObjectWidth(W): width of object
            /// 
            /// F = (P*D)/W
            ///     -> D = (W*F)/P
            ///
            double _distance;
            double _ObjectWidth = 10, _focalLength = 604.8;

            //_distance = Convert.ToInt16((_ObjectWidth * _focalLength) / _pixel);
            _distance = (_ObjectWidth * _focalLength) / _pixel;

            return _distance;
        }
        #endregion

        #region Blob Detection
        /// <summary> Blob Detection    
        /// This method for color object detection by Blob counter algorithm.
        /// If you using this method, then you can detecting as follows:
        /// </summary>
        private Bitmap BlobDetection(Bitmap _bitmapBinaria, Bitmap _bitmapGrayscale)
        {

            AForge.Imaging.BlobCounter _blobCounter = new AForge.Imaging.BlobCounter();

            _blobCounter.MinWidth = 20;
            _blobCounter.MinHeight = 20;
            _blobCounter.FilterBlobs = true;

            _blobCounter.ProcessImage(_bitmapBinaria);

            AForge.Imaging.Blob[] _blobPoints = _blobCounter.GetObjectsInformation();
            Graphics _g = Graphics.FromImage(_bitmapGrayscale);
            SimpleShapeChecker _shapeChecker = new SimpleShapeChecker();

            for (int i = 0; i < _blobPoints.Length; i++)
            {
                List<IntPoint> _edgePoint = _blobCounter.GetBlobsEdgePoints(_blobPoints[i]);
                List<IntPoint> _corners = null;
                AForge.Point _center;
                float _radius;


                /* #region detecting Rectangle
                 ///
                 /// _corners: the corner of Quadrilateral
                 /// 
                 if (_shapeChecker.IsQuadrilateral(_edgePoint, out _corners))
                 {
                     //Drawing the reference point of the picturebox
                     _g.DrawEllipse(_PictureboxPen, (float)(pictureBox1.Size.Width),
                                                    (float)(pictureBox1.Size.Height),
                                                    (float)10, (float)10);

                     // Drawing setting for outline of detected object
                     Rectangle[] _rects = _blobCounter.GetObjectsRectangles();
                     System.Drawing.Point[] _coordinates = ToPointsArray(_corners);
                     Pen _pen = new Pen(Color.Blue, ipenWidth);
                     int _x = _coordinates[0].X;
                     int _y = _coordinates[0].Y;

                     // Drawing setting for centroid of detected object
                     int _centroid_X = (int)_blobPoints[0].CenterOfGravity.X;
                     int _centroid_Y = (int)_blobPoints[0].CenterOfGravity.Y;

                     //Drawing the centroid point of object
                     _g.DrawEllipse(_pen, (float)(_centroid_X), (float)(_centroid_Y), (float)10, (float)10);
                     //Degree calculation
                     int _deg_x = (int)_centroid_X - pictureBox1.Size.Width;
                     int _deg_y = pictureBox1.Size.Height - (int)_centroid_Y;
                     //textBox1.Text = ("Degree: (" + _deg_x + ", " + _deg_y + ")");

                     ///
                     /// Drawing outline of detected object
                     /// 
                     if (_coordinates.Length == 4)
                     {
                         string _shapeString = "" + _shapeChecker.CheckShapeType(_edgePoint);
                         _g.DrawString(_shapeString, _font, _brush, _x, _y);
                         _g.DrawPolygon(_pen, ToPointsArray(_corners));
                     }

                     //size of rectange
                     foreach (Rectangle rc in _rects)
                     {
                         ///for debug
                         //System.Diagnostics.Debug.WriteLine(
                         //    string.Format("Rect size: ({0}, {1})", rc.Width, rc.Height));

                         iFeatureWidth = rc.Width;
                         //check the FindDistance method.
                         double dis = FindDistance(iFeatureWidth);
                        // _g.DrawString(dis.ToString("N2") + "cm", _font, _brush, _x, _y + 60);
                     }
                 }
                 #endregion
                 */

                #region detecting Circle
                ///
                /// _center: the center of circle
                /// _radius: the radius of circle
                ///
                if (_shapeChecker.IsCircle(_edgePoint, out _center, out _radius))
                {
                    //Drawing the reference point
                    _g.DrawEllipse(_PictureboxPen, (float)(pictureBox1.Size.Width),
                                                   (float)(pictureBox1.Size.Height),
                                                   (float)10, (float)10);

                    // Drawing setting for outline of detected object
                    Rectangle[] _rects = _blobCounter.GetObjectsRectangles();
                    Pen _pen = new Pen(Color.Red, ipenWidth);
                    string _shapeString = "" + _shapeChecker.CheckShapeType(_edgePoint);
                    int _x = (int)_center.X;
                    int _y = (int)_center.Y;
                    ///
                    /// Drawing outline of detected object
                    ///
                    _g.DrawString(_shapeString, _font, _brush, _x, _y);
                    _g.DrawEllipse(_pen, (float)(_center.X - _radius),
                                         (float)(_center.Y - _radius),
                                         (float)(_radius * 2),
                                         (float)(_radius * 2));

                    //Drawing the centroid point of object
                    int _centroid_X = (int)_blobPoints[0].CenterOfGravity.X;
                    int _centroid_Y = (int)_blobPoints[0].CenterOfGravity.Y;
                    _g.DrawEllipse(_pen, (float)(_centroid_X), (float)(_centroid_Y), (float)10, (float)10);

                    //Scrivi centro ellisse
                    double center = _center.X * 5 / 1280; // Pixel camera : 5mm FOW = centro : X
                    _g.DrawString(center.ToString("N4") + " mm", _font, _brush, _x, _y + 60);


                }
                #endregion

                /* #region detecting Triangle
                 ///
                 /// _corners: the corner of Triangle
                 ///
                 if (_shapeChecker.IsTriangle(_edgePoint, out _corners))
                 {
                     //Drawing the reference point
                     _g.DrawEllipse(_PictureboxPen, (float)(pictureBox1.Size.Width),
                                                    (float)(pictureBox1.Size.Height),
                                                    (float)10, (float)10);

                     // Drawing setting for outline of detected object
                     Rectangle[] _rects = _blobCounter.GetObjectsRectangles();
                     Pen _pen = new Pen(Color.Green, ipenWidth);
                     string _shapeString = "" + _shapeChecker.CheckShapeType(_edgePoint);
                     int _x = (int)_center.X;
                     int _y = (int)_center.Y;

                     // Drawing setting for centroid of detected object
                     int _centroid_X = (int)_blobPoints[0].CenterOfGravity.X;
                     int _centroid_Y = (int)_blobPoints[0].CenterOfGravity.Y;
                     ///
                     /// Drawing outline of detected object
                     ///
                     _g.DrawString(_shapeString, _font, _brush, _x, _y);
                     _g.DrawPolygon(_pen, ToPointsArray(_corners));

                     //Drawing the centroid point of object
                     _g.DrawEllipse(_pen, (float)(_centroid_X), (float)(_centroid_Y), (float)10, (float)10);
                     //Degree calculation
                     int _deg_x = (int)_centroid_X - pictureBox1.Size.Width;
                     int _deg_y = pictureBox1.Size.Height - (int)_centroid_Y;
                     //textBox1.Text = ("Degree: (" + _deg_x + ", " + _deg_y + ")");
                     //size of rectange
                     foreach (Rectangle rc in _rects)
                     {
                         ///for debug
                         //System.Diagnostics.Debug.WriteLine(
                         //    string.Format("Triangle Size: ({0}, {1})", rc.Width, rc.Height));

                         iFeatureWidth = rc.Width;
                         double dis = FindDistance(iFeatureWidth);
                         //_g.DrawString(dis.ToString("N2") + "cm", _font, _brush, _x, _y + 60);
                     }
                 }
                 #endregion*/

            }

            return _bitmapGrayscale;



        }
        #endregion

















    }
}