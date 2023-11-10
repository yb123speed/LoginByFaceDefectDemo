using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.ComponentModel;
using System.Drawing;
using static OpenCvSharp.FileStorage;

namespace FaceDetectDemo
{
	public partial class Form1 : Form
	{
		private readonly VideoCapture capture;
		private readonly CascadeClassifier cascadeClassifier;
		public Form1()
		{
			InitializeComponent();

			capture = new VideoCapture();
			cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			capture.Open(0, VideoCaptureAPIs.ANY);
			if (!capture.IsOpened())
			{
				Close();
				return;
			}

			ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);

			backgroundWorker1.RunWorkerAsync();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			backgroundWorker1.CancelAsync();
			capture.Dispose();
			cascadeClassifier.Dispose();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			var bgWorker = (BackgroundWorker)sender;

			while (!bgWorker.CancellationPending)
			{
				try
				{
					using (var frameMat = capture.RetrieveMat())
					{
						var rects = cascadeClassifier.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

						if (rects.Length > 0)
						{
							Cv2.Rectangle(frameMat, rects[0], Scalar.Red);
							performLogin();
						}
						var frameBitmap = BitmapConverter.ToBitmap(frameMat);
						bgWorker.ReportProgress(0, frameBitmap);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

				Thread.Sleep(10);
			}
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			var frameBitmap = (Bitmap)e.UserState;
			pictureBox1.Image?.Dispose();
			pictureBox1.Image = frameBitmap;
		}

		private void performLogin()
		{
			// 停止摄像头捕获
			backgroundWorker1.CancelAsync();

			// 在这里添加实际的登录逻辑
			// 您可能需要与数据库交互，验证用户信息等

			// 显示登录成功消息
			MessageBox.Show("人脸识别登录成功！", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// 重新启动摄像头捕获
			backgroundWorker1.RunWorkerAsync();
		}
	}
}