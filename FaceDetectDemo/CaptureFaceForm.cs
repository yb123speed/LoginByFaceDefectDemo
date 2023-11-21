using DlibDotNet;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using DlibDotNet.Extensions;

namespace FaceDetectDemo
{
    public partial class CaptureFaceForm : Form
    {
        private readonly VideoCapture capture;
        private readonly ShapePredictor shapePredictor;
        private readonly FrontalFaceDetector faceDetector;
        public CaptureFaceForm()
        {
            InitializeComponent();

            capture = new VideoCapture();
            faceDetector = Dlib.GetFrontalFaceDetector();
            shapePredictor = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat");

        }

        private void CaptureFaceForm_Load(object sender, EventArgs e)
        {
            capture.Open(0, VideoCaptureAPIs.ANY);
            if (!capture.IsOpened())
            {
                Close();
                return;
            }

            backgroundWorker1.RunWorkerAsync();
        }

        private void CaptureFaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            capture.Dispose();
            shapePredictor.Dispose();
            faceDetector.Dispose();
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
                        var image = (frameMat.ToBitmap()).ToArray2D<RgbPixel>();

                        var dets = faceDetector.Operator(image);

                        foreach (var det in dets)
                        {
                            Dlib.DrawRectangle(image, det, new RgbPixel { Green = 255 });
                        }
                        var frameBitmap = image.ToBitmap();
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

        //// 提取人脸特征点的示例方法
        //private List<Point2f> ExtractFacialLandmarks(Mat frameMat, Rect face)
        //{
        //    // 获取灰度图像的像素数据
        //    Cv2.CvtColor(frameMat, frameMat, ColorConversionCodes.BGR2GRAY);
        //    // 在这里实现人脸特征点提取的逻辑，例如使用 DlibDotNet 或其他库来提取关键点坐标
        //    // 这里使用 DlibDotNet 的示例
        //    var landmarks = shapePredictor.Detect((OutputArray)frameMat, face);

        //    // 返回关键点坐标
        //    return landmarks;
        //}

        //// 将人脸信息保存到文件的示例方法
        //private void SaveFacialLandmarks(List<Point2f> landmarks, string filePath)
        //{
        //    using (var writer = new StreamWriter(filePath, append: true))
        //    {
        //        foreach (var landmark in landmarks)
        //        {
        //            writer.WriteLine($"X: {landmark.X}, Y: {landmark.Y}");
        //        }
        //    }
        //}

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var frameBitmap = (Bitmap)e.UserState;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = frameBitmap;
        }

        private void btnRecordFace_Click(object sender, EventArgs e)
        {
            Mat frameMat = new Mat();
            capture.Read(frameMat); // 获取摄像头或图像的帧

            // 将帧转换为灰度图像以提高人脸检测效果
            //Mat grayMat = new Mat();
            //Cv2.CvtColor(frameMat, grayMat, ColorConversionCodes.BGR2GRAY);
            // 检测人脸
            var img = frameMat.ToBitmap().ToArray2D<RgbPixel>();
            var win = new ImageWindow(img);
                var faces = new List<Array2D<RgbPixel>>();
            foreach (var face in faceDetector.Operator(img))
            {
                var shape = shapePredictor.Detect(img, face);
                var faceChipDetail = Dlib.GetFaceChipDetails(shape, 150, 0.25);
                var faceChip = Dlib.ExtractImageChip<RgbPixel>(img, faceChipDetail);
                faces.Add(faceChip);
                win.AddOverlay(face);
            }


            //foreach (var face in faces)
            //{
            //    // 在此处可以执行特征提取逻辑

            //    // 绘制人脸框
            //    Cv2.Rectangle(frameMat, face, Scalar.Red);

            //    // 存储人脸信息（示例中存储关键点坐标）
            //    var landmarks = ExtractFacialLandmarks(frameMat, face);
            //    SaveFacialLandmarks(landmarks, "face_info.txt");
            //}

            // 显示帧（略）
        }
    }
}