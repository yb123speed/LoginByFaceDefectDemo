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
			// ֹͣ����ͷ����
			backgroundWorker1.CancelAsync();

			// ���������ʵ�ʵĵ�¼�߼�
			// ��������Ҫ�����ݿ⽻������֤�û���Ϣ��

			// ��ʾ��¼�ɹ���Ϣ
			MessageBox.Show("����ʶ���¼�ɹ���", "��¼�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// ������������ͷ����
			backgroundWorker1.RunWorkerAsync();
		}
	}
}