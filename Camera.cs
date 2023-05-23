using Basler.Pylon;
using System;




namespace Test_MachineLearning
{
    public partial class MAIN
    {
        byte[] buffer_Image; // Immagine Grabbata
        int buffer_Image_sizeX;
        int buffer_Image_sizeY;


        public void Print_Camera_Info() // Stampa su consolle info telecamera + setta dimensioni ROI
        {
            try
            {
                // Create a camera object that selects the first camera device found.
                // More constructors are available for selecting a specific camera device.
                using (Basler.Pylon.Camera camera = new Basler.Pylon.Camera())
                {
                    // Before accessing camera device parameters, the camera must be opened.
                    camera.Open();

                    // DeviceVendorName, DeviceModelName, and DeviceFirmwareVersion are string parameters.
                    Console.WriteLine("Camera Device Information");
                    Console.WriteLine("=========================");
                    Console.WriteLine("Vendor           : {0}", camera.Parameters[PLCamera.DeviceVendorName].GetValue());
                    Console.WriteLine("Model            : {0}", camera.Parameters[PLCamera.DeviceModelName].GetValue());
                    Console.WriteLine("Firmware version : {0}", camera.Parameters[PLCamera.DeviceFirmwareVersion].GetValue());
                    Console.WriteLine("");
                    Console.WriteLine("Camera Device Settings");
                    Console.WriteLine("======================");


                    // Setting the ROI. OffsetX, OffsetY, Width, and Height are integer parameters.
                    // On some cameras, the offsets are read-only. If they are writable, set the offsets to min.
                    if (camera.Parameters[PLCamera.OffsetX].TrySetToMinimum())
                    {
                        Console.WriteLine("OffsetX          : {0}", camera.Parameters[PLCamera.OffsetX].GetValue());
                    }
                    if (camera.Parameters[PLCamera.OffsetY].TrySetToMinimum())
                    {
                        Console.WriteLine("OffsetY          : {0}", camera.Parameters[PLCamera.OffsetY].GetValue());
                    }

                    // Some parameters have restrictions. You can use GetIncrement/GetMinimum/GetMaximum to make sure you set a valid value.
                    // Here, we let pylon correct the value if needed.
                    //camera.Parameters[PLCamera.Width].SetValue(800, IntegerValueCorrection.Nearest);
                    //camera.Parameters[PLCamera.Height].SetValue(600, IntegerValueCorrection.Nearest);
                    Console.WriteLine("Width            : {0}", camera.Parameters[PLCamera.Width].GetValue());
                    Console.WriteLine("Height           : {0}", camera.Parameters[PLCamera.Height].GetValue());


                    // Set an enum parameter.
                    string oldPixelFormat = camera.Parameters[PLCamera.PixelFormat].GetValue(); // Remember the current pixel format.
                    Console.WriteLine("Old PixelFormat  : {0} ({1})", camera.Parameters[PLCamera.PixelFormat].GetValue(), oldPixelFormat);

                    // Set pixel format to Mono8 if available.
                    if (camera.Parameters[PLCamera.PixelFormat].TrySetValue(PLCamera.PixelFormat.Mono8))
                    {
                        Console.WriteLine("New PixelFormat  : {0} ({1})", camera.Parameters[PLCamera.PixelFormat].GetValue(), oldPixelFormat);
                    }

                    // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                    // the Gain Auto function must be disabled first (if gain auto is available).
                    camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.
                    camera.Close();
                }
            }
            catch
            {
                Console.Error.WriteLine("Exception");

            }

        }

        public void Camera_Grab_One() // salva nella vaiabile buffer_Image il grab dell'immagine
        {


            using (Basler.Pylon.Camera camera = new Basler.Pylon.Camera())
            {

                camera.CameraOpened += Configuration.AcquireContinuous;  // imposta in acquisizione continua non appena la camera è aperta
                camera.Open();
                camera.StreamGrabber.Start();

                IGrabResult grabResult = camera.StreamGrabber.RetrieveResult(2000, TimeoutHandling.ThrowException);

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
                }

                camera.StreamGrabber.Stop();
                camera.Close();
            }
        }

        public void Camera_Parameters() // Parametri camera
        {
            try
            {
                using (Basler.Pylon.Camera camera = new Basler.Pylon.Camera())
                {
                    camera.Open();

                    //****************************** Analog Control ******************************//
                    camera.Parameters[PLCamera.GainSelector].SetValue("All");
                    camera.Parameters[PLCamera.Gain].SetValue(0.0);
                    camera.Parameters[PLCamera.GainAuto].SetValue("Off"); // "Off" "Once" "Continuous"
                    camera.Parameters[PLCamera.AutoGainLowerLimit].SetValue(0.0);
                    camera.Parameters[PLCamera.AutoGainUpperLimit].SetValue(18.0);
                    camera.Parameters[PLCamera.BlackLevelSelector].SetValue("All");
                    camera.Parameters[PLCamera.BlackLevel].SetValue(0.0);
                    camera.Parameters[PLCamera.Gamma].SetValue(1.0);

                    //****************************** Image Format Control ******************************//
                    camera.Parameters[PLCamera.Width].SetValue(12800, IntegerValueCorrection.Nearest);
                    camera.Parameters[PLCamera.Height].SetValue(9600, IntegerValueCorrection.Nearest);
                    camera.Parameters[PLCamera.OffsetX].SetValue(0, IntegerValueCorrection.Nearest);
                    camera.Parameters[PLCamera.OffsetX].SetValue(0, IntegerValueCorrection.Nearest);
                    camera.Parameters[PLCamera.BinningHorizontalMode].SetValue("Average");
                    camera.Parameters[PLCamera.BinningHorizontal].SetValue(1);
                    camera.Parameters[PLCamera.BinningVerticalMode].SetValue("Average");
                    camera.Parameters[PLCamera.BinningVertical].SetValue(1);
                    camera.Parameters[PLCamera.ReverseX].SetValue(false);
                    camera.Parameters[PLCamera.ReverseY].SetValue(false);
                    camera.Parameters[PLCamera.PixelFormat].SetValue("Mono8");

                    //****************************** Acquisition Control ******************************//
                    camera.Parameters[PLCamera.SensorShutterMode].SetValue("Global");
                    camera.Parameters[PLCamera.OverlapMode].SetValue("Off");
                    camera.Parameters[PLCamera.BslImmediateTriggerMode].SetValue("Off");
                    camera.Parameters[PLCamera.ExposureTime].SetValue(6000.0);
                    camera.Parameters[PLCamera.ExposureAuto].SetValue("Off"); // "Off" "Once" "Continuous"
                    camera.Parameters[PLCamera.AutoExposureTimeLowerLimit].SetValue(10.0);
                    camera.Parameters[PLCamera.AutoExposureTimeUpperLimit].SetValue(1000000.0);
                    camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                    camera.Parameters[PLCamera.TriggerMode].SetValue("Off");
                    camera.Parameters[PLCamera.TriggerSource].SetValue("Software"); // "Software" "Line1" "Line2"
                    camera.Parameters[PLCamera.TriggerActivation].SetValue("RisingEdge"); // "RisingEdge" "FallingEdge"
                    camera.Parameters[PLCamera.ExposureMode].SetValue("Timed");
                    camera.Parameters[PLCamera.AcquisitionFrameRate].SetValue(10.0);

                    //****************************** Image Quality Control ******************************//
                    camera.Parameters[PLCamera.BslContrastMode].SetValue("Linear"); // "Linear" "SCurve"
                    camera.Parameters[PLCamera.BslBrightness].SetValue(0.0);
                    camera.Parameters[PLCamera.BslContrast].SetValue(0.0);
                    camera.Parameters[PLCamera.DefectPixelCorrectionMode].SetValue("Off"); // "Off" "On" "StaticOnly"

                    //****************************** Digital IO Control ******************************//
                    camera.Parameters[PLCamera.LineSelector].SetValue("Line1"); // "Line1" "Line2"
                    camera.Parameters[PLCamera.LineMode].SetValue("Input");  // "Input" "Output"
                    camera.Parameters[PLCamera.LineInverter].SetValue(false);
                    camera.Parameters[PLCamera.LineSource].SetValue("ExposureActive");  // "ExposureActive" "UserOutput1" "UserOutput2"
                    camera.Parameters[PLCamera.LineDebouncerTime].SetValue(0.0);
                    camera.Parameters[PLCamera.UserOutputSelector].SetValue("UserOutput1");  // "UserOutput1" "UserOutput2"
                    camera.Parameters[PLCamera.UserOutputValue].SetValue(false);

                    //****************************** Device Control ******************************//
                    camera.Parameters[PLCamera.DeviceIndicatorMode].SetValue("Active");  // "Active" "Inactive"



                    camera.Close();
                }
            }
            catch
            {
                Console.Error.WriteLine("Exception");

            }

        }





    }
}
