namespace Test_MachineLearning
{
    partial class MAIN
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_CameraInfo = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_FrameGrab = new System.Windows.Forms.Button();
            this.buttonOpenCVTest = new System.Windows.Forms.Button();
            this.button_StopContinuous = new System.Windows.Forms.Button();
            this.button_StartContinuous = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.buttonAddShape = new System.Windows.Forms.Button();
            this.shapeNameTextBox = new System.Windows.Forms.TextBox();
            this.buttonLearning = new System.Windows.Forms.Button();
            this.buttonFindShape = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // button_CameraInfo
            // 
            this.button_CameraInfo.Location = new System.Drawing.Point(12, 12);
            this.button_CameraInfo.Name = "button_CameraInfo";
            this.button_CameraInfo.Size = new System.Drawing.Size(106, 49);
            this.button_CameraInfo.TabIndex = 0;
            this.button_CameraInfo.Text = "Set parameters Print camera info";
            this.button_CameraInfo.UseVisualStyleBackColor = true;
            this.button_CameraInfo.Click += new System.EventHandler(this.button_CameraInfo_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(11, 114);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 584);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // button_FrameGrab
            // 
            this.button_FrameGrab.Location = new System.Drawing.Point(124, 12);
            this.button_FrameGrab.Name = "button_FrameGrab";
            this.button_FrameGrab.Size = new System.Drawing.Size(100, 49);
            this.button_FrameGrab.TabIndex = 2;
            this.button_FrameGrab.Text = "Visulizza_Immagine_buffer_image()";
            this.button_FrameGrab.UseVisualStyleBackColor = true;
            this.button_FrameGrab.Click += new System.EventHandler(this.button_FrameGrab_Click);
            // 
            // buttonOpenCVTest
            // 
            this.buttonOpenCVTest.Location = new System.Drawing.Point(230, 12);
            this.buttonOpenCVTest.Name = "buttonOpenCVTest";
            this.buttonOpenCVTest.Size = new System.Drawing.Size(65, 49);
            this.buttonOpenCVTest.TabIndex = 3;
            this.buttonOpenCVTest.Text = "Start ONE";
            this.buttonOpenCVTest.UseVisualStyleBackColor = true;
            this.buttonOpenCVTest.Click += new System.EventHandler(this.buttonOpenCVTest_Click);
            // 
            // button_StopContinuous
            // 
            this.button_StopContinuous.Location = new System.Drawing.Point(378, 12);
            this.button_StopContinuous.Name = "button_StopContinuous";
            this.button_StopContinuous.Size = new System.Drawing.Size(69, 49);
            this.button_StopContinuous.TabIndex = 4;
            this.button_StopContinuous.Text = "Stop Continuous";
            this.button_StopContinuous.UseVisualStyleBackColor = true;
            this.button_StopContinuous.Click += new System.EventHandler(this.button_StopContinuous_Click);
            // 
            // button_StartContinuous
            // 
            this.button_StartContinuous.Location = new System.Drawing.Point(301, 12);
            this.button_StartContinuous.Name = "button_StartContinuous";
            this.button_StartContinuous.Size = new System.Drawing.Size(71, 49);
            this.button_StartContinuous.TabIndex = 5;
            this.button_StartContinuous.Text = "Start Continuous";
            this.button_StartContinuous.UseVisualStyleBackColor = true;
            this.button_StartContinuous.Click += new System.EventHandler(this.button_StartContinuous_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(453, 12);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(140, 45);
            this.trackBar1.TabIndex = 6;
            this.trackBar1.TickFrequency = 20;
            this.trackBar1.Value = 30;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(453, 63);
            this.trackBar2.Maximum = 255;
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(140, 45);
            this.trackBar2.TabIndex = 7;
            this.trackBar2.TickFrequency = 20;
            this.trackBar2.Value = 30;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // buttonAddShape
            // 
            this.buttonAddShape.Location = new System.Drawing.Point(742, 12);
            this.buttonAddShape.Name = "buttonAddShape";
            this.buttonAddShape.Size = new System.Drawing.Size(69, 49);
            this.buttonAddShape.TabIndex = 8;
            this.buttonAddShape.Text = "Add Shape";
            this.buttonAddShape.UseVisualStyleBackColor = true;
            this.buttonAddShape.Click += new System.EventHandler(this.buttonAddShape_Click);
            // 
            // shapeNameTextBox
            // 
            this.shapeNameTextBox.Location = new System.Drawing.Point(817, 12);
            this.shapeNameTextBox.Name = "shapeNameTextBox";
            this.shapeNameTextBox.Size = new System.Drawing.Size(125, 20);
            this.shapeNameTextBox.TabIndex = 9;
            // 
            // buttonLearning
            // 
            this.buttonLearning.Location = new System.Drawing.Point(742, 63);
            this.buttonLearning.Name = "buttonLearning";
            this.buttonLearning.Size = new System.Drawing.Size(69, 28);
            this.buttonLearning.TabIndex = 10;
            this.buttonLearning.Text = "Learning";
            this.buttonLearning.UseVisualStyleBackColor = true;
            this.buttonLearning.Click += new System.EventHandler(this.buttonLearning_Click);
            // 
            // buttonFindShape
            // 
            this.buttonFindShape.Location = new System.Drawing.Point(817, 38);
            this.buttonFindShape.Name = "buttonFindShape";
            this.buttonFindShape.Size = new System.Drawing.Size(125, 53);
            this.buttonFindShape.TabIndex = 11;
            this.buttonFindShape.Text = "Find Shape";
            this.buttonFindShape.UseVisualStyleBackColor = true;
            this.buttonFindShape.Click += new System.EventHandler(this.buttonFindShape_Click);
            // 
            // MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 721);
            this.Controls.Add(this.buttonFindShape);
            this.Controls.Add(this.buttonLearning);
            this.Controls.Add(this.shapeNameTextBox);
            this.Controls.Add(this.buttonAddShape);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.button_StartContinuous);
            this.Controls.Add(this.button_StopContinuous);
            this.Controls.Add(this.buttonOpenCVTest);
            this.Controls.Add(this.button_FrameGrab);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_CameraInfo);
            this.Name = "MAIN";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_CameraInfo;
        private System.Windows.Forms.Button button_FrameGrab;
        private System.Windows.Forms.Button buttonOpenCVTest;
        private System.Windows.Forms.Button button_StopContinuous;
        private System.Windows.Forms.Button button_StartContinuous;
        private System.Windows.Forms.TrackBar trackBar1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Button buttonAddShape;
        private System.Windows.Forms.TextBox shapeNameTextBox;
        private System.Windows.Forms.Button buttonLearning;
        private System.Windows.Forms.Button buttonFindShape;
    }
}

