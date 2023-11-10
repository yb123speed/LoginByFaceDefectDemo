namespace FaceDetectDemo
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			btnCaptureFace = new Button();
			SuspendLayout();
			// 
			// btnCaptureFace
			// 
			btnCaptureFace.Location = new Point(475, 43);
			btnCaptureFace.Name = "btnCaptureFace";
			btnCaptureFace.Size = new Size(75, 23);
			btnCaptureFace.TabIndex = 0;
			btnCaptureFace.Text = "录入人脸";
			btnCaptureFace.UseVisualStyleBackColor = true;
			btnCaptureFace.Click += btnCaptureFace_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(btnCaptureFace);
			Name = "MainForm";
			Text = "MainForm";
			ResumeLayout(false);
		}

		#endregion

		private Button btnCaptureFace;
	}
}