using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Test_MachineLearning
{
    public partial class MAIN : Form
    {
        private Thread continuousThread;
        private bool isGrabbingContinuous = false;
        int ThrValue1 = 30;
        int ThrValue2 = 255;

        public MAIN()
        {
            InitializeComponent();

            imageList = new List<(Bitmap, string)>();
            svm = null;
            isLearningMode = true;

            Console.WriteLine("inizialize");
        }

        private void button_CameraInfo_Click(object sender, EventArgs e)
        {
            Print_Camera_Info();
            Camera_Parameters();
        }

        private void button_FrameGrab_Click(object sender, EventArgs e)
        {

            Camera_Grab_One();
            Visulizza_Immagine_buffer_image();

        }

        private void buttonOpenCVTest_Click(object sender, EventArgs e)
        {
            Camera_Grab_One();
            OpenCV_Test();
        }

        private void button_StartContinuous_Click(object sender, EventArgs e)
        {
            if (continuousThread == null)
            {
                isGrabbingContinuous = true;
                continuousThread = new Thread(new ThreadStart(AcquireContinuous));
                continuousThread.Start();
            }
        }

        private void button_StopContinuous_Click(object sender, EventArgs e)
        {
            if (continuousThread != null)
            {
                continuousThread.Abort();
                isGrabbingContinuous = false;
                continuousThread = null;
            }
        }

        private void AcquireContinuous()
        {
            using (Basler.Pylon.Camera camera = new Basler.Pylon.Camera())
            {
                camera.CameraOpened += Configuration.AcquireContinuous;  // imposta in acquisizione continua non appena la camera è aperta
                camera.Open();
                camera.StreamGrabber.Start();

                while (isGrabbingContinuous)
                {
                    IGrabResult grabResult = camera.StreamGrabber.RetrieveResult(5000, TimeoutHandling.ThrowException);
                    using (grabResult)
                    {
                        if (grabResult.GrabSucceeded)
                        {
                            buffer_Image_sizeX = grabResult.Width;
                            buffer_Image_sizeY = grabResult.Height;
                            buffer_Image = grabResult.PixelData as byte[];
                        }
                        else
                        {
                            Console.WriteLine("Error: {0} {1}", grabResult.ErrorCode, grabResult.ErrorDescription);
                        }

                        // Operazioni da fare non appena acquisito un frame




                        pictureBox1.Invoke((MethodInvoker)delegate
                        {

                            pictureBox1.Image = OpenCV_Test3();

                        });






                    }
                }
                camera.StreamGrabber.Stop();
                camera.Close();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ThrValue1 = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ThrValue2 = trackBar2.Value;
        }

        private void buttonAddShape_Click(object sender, EventArgs e)
        {
            Camera_Grab_One();

            Bitmap currentImage = BufferToBitmapRGB(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);
            string shapeName = shapeNameTextBox.Text;

            pictureBox1.Image = currentImage;

            if (!string.IsNullOrEmpty(shapeName) && currentImage != null)
            {
                imageList.Add((currentImage, shapeName));
                currentImage = null;
            }
            else
            {
                MessageBox.Show("Inserisci un nome e cattura un'immagine prima di aggiungerla alla lista.");
            }
        }

        private void buttonLearning_Click(object sender, EventArgs e)
        {

            Dictionary<string, List<Bitmap>> shapeImageLists = new Dictionary<string, List<Bitmap>>();

            // Suddivide le immagini per nome della forma
            foreach (var imageEntry in imageList)
            {
                string shapeName = imageEntry.Item2;
                Bitmap shapeImage = imageEntry.Item1;

                if (shapeImageLists.ContainsKey(shapeName))
                {
                    // Aggiungi l'immagine alla lista esistente
                    shapeImageLists[shapeName].Add(shapeImage);
                }
                else
                {
                    // Crea una nuova lista per il nome della forma e aggiungi l'immagine
                    shapeImageLists.Add(shapeName, new List<Bitmap> { shapeImage });
                }
            }







            // Crea il modello SVM
            //svm = new SupportVectorMachine<Gaussian>(inputs: shapeImageLists["qwe"].Count, kernel: new Gaussian(sigma: 1));


            // Creazione del modello SVM multiclasse
            var svm = new MulticlassSupportVectorMachine<Gaussian>(inputs: shapeImageLists["qwe"].Count, classes: shapeImageLists.Count + 1, kernel: new Gaussian());

            // Creazione del trainer SVM multiclasse con l'estimazione automatica del kernel
            var teacher = new MulticlassSupportVectorLearning<Gaussian>()
            {
                Learner = (p) => new SequentialMinimalOptimization<Gaussian>()
                {
                    Complexity = 100 // Parametro di complessità del modello
                }
            };







            // Itera attraverso le coppie chiave-valore del Dictionary
            foreach (var shapeEntry in shapeImageLists)
            {
                string shapeName = shapeEntry.Key;
                List<Bitmap> shapeImages = shapeEntry.Value;

                if (shapeImages.Count < 2)
                {
                    MessageBox.Show($"Aggiungi almeno due immagini per la forma {shapeName}.");
                    continue;
                }

                // Preprocessa le immagini e converte in un array bidimensionale
                double[][] inputs = new double[shapeImages.Count][];
                int[] labels = new int[shapeImages.Count];

                for (int i = 0; i < shapeImages.Count; i++)
                {
                    Bitmap shapeImage = shapeImages[i];

                    // Preprocessa l'immagine
                    Bitmap preprocessedImage = shapeImage;

                    // Converte l'immagine in un array di input
                    inputs[i] = ImageToArray(preprocessedImage);

                    // Assegna l'etichetta corrispondente
                    labels[i] = 1;
                }

                // Addestra il modello per la forma corrente
                //var teacher = new SequentialMinimalOptimization<Gaussian>()
                //{
                //    Complexity = 100 // Parametro di complessità del modello
                //};

                MulticlassSupportVectorMachine<Gaussian> trainedModel = teacher.Learn(inputs, labels);




                // Memorizza il modello addestrato per la forma corrente
                trainedModels[shapeName] = trainedModel;

                Console.WriteLine();
            }

            // Cambia modalità in modalità di riconoscimento
            isLearningMode = false;

            MessageBox.Show("Apprendimento delle forme completato.");





        }
        private double[] ImageToArray(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            double[] pixels = new double[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    double grayscale = (pixelColor.R + pixelColor.G + pixelColor.B) / 3.0;
                    pixels[y * width + x] = grayscale;
                }
            }

            return pixels;
        }
        public static double[] ConvertByteArrayToDoubleArray(byte[] byteArray)
        {
            int length = byteArray.Length;
            double[] doubleArray = new double[length];

            for (int i = 0; i < length; i++)
            {
                doubleArray[i] = (double)byteArray[i];
            }

            return doubleArray;
        }
        private void buttonFindShape_Click(object sender, EventArgs e)
        {
            if (isLearningMode)
            {
                MessageBox.Show("Devi completare l'apprendimento delle forme prima di passare alla modalità di riconoscimento.");
                return;
            }



            Camera_Grab_One();


            // Acquisisce l'immagine corrente
            Bitmap currentImage = BufferToBitmapRGB(buffer_Image, buffer_Image_sizeX, buffer_Image_sizeY);
            if (currentImage != null)
            {


                // Effettua la predizione della classe
                double[] testInput = ConvertByteArrayToDoubleArray(buffer_Image);

                Console.WriteLine(trainedModels.Values);

                bool decision = trainedModels["qwe"].Decide(testInput);


                Bitmap directImage = new Bitmap(currentImage.Width, currentImage.Height);
                Graphics g = Graphics.FromImage(directImage);

                if (decision)
                {
                    g.DrawString("Forma Trovata", new Font("Arial", 30), Brushes.Red, 10, 10);
                    //pictureBox1.Image = currentImage;
                }
                else
                {
                    Console.WriteLine("La Bitmap non rappresenta la stessa forma del modello addestrato.");
                }


               
            }
            else
            {
                MessageBox.Show("Cattura un'immagine prima di avviare il riconoscimento.");
            }
        }
       




        

    }
}
