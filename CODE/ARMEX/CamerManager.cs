using Compunet.YoloSharp;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _ARMEX
{
    public class CameraManager
    {
        private VideoCapture? capture;
        private Thread? cameraThread;
        private bool isRunning = false;
        public bool IsRunning => isRunning;
        private PictureBox? mirrorBox = null;
        private double exposure = 0.0;
        private YoloPredictor? predictor;
        private bool useYolo;


        // YOLO 객체 탐지 기능 활성화
        public void EnableYolo(YoloPredictor yolo) { predictor = yolo; useYolo = true; }

        public event Action<string>? OnYoloDetection;  // ADD YOLO DETECTION EVENT

        // 카메라 시작 및 PictureBox에 영상 출력 (별도 스레드)
        public void Start(int camIndex, PictureBox pictureBox)
        {
            if (isRunning)
            {
                MessageBox.Show($" [카메라 {camIndex}] 이미 실행 중입니다.");
                return;
            }

            int waitMs = 0;
            while (!pictureBox.IsHandleCreated && waitMs < 500)
            {
                Thread.Sleep(50);
                waitMs += 50;
            }

            if (!pictureBox.IsHandleCreated)
            {
                MessageBox.Show($" PictureBox 핸들이 아직 준비되지 않았습니다.");
                return;
            }

            capture = new VideoCapture(camIndex);
            if (!capture.IsOpened())
            {
                MessageBox.Show($"카메라 {camIndex}를 열 수 없습니다.");
                return;
            }

            isRunning = true;
            cameraThread = new Thread(() => CaptureLoop(pictureBox))
            {
                IsBackground = true
            };
            cameraThread.Start();

        }

        /// <summary>
        // 영상 캡처 및 YOLO 처리 루프 (스레드용, 내부에서만 호출)
        /// </summary>
        private void CaptureLoop(PictureBox pictureBox)
        {
            using var mat = new Mat();

            while (isRunning)
            {
                try
                {
                    if (capture == null || !capture.Read(mat) || mat.Empty()) continue;

                    if (useYolo && predictor != null)
                    {
                        using Bitmap bmp = BitmapConverter.ToBitmap(mat);
                        using var ms = new MemoryStream();
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        var results = predictor.Detect(ms.ToArray());


                        float threshold = 0.95f; 

                        foreach (var result in results)
                        {
                            if (result.Confidence >= threshold)
                            {
                                var rect = new OpenCvSharp.Rect(result.Bounds.X, result.Bounds.Y, result.Bounds.Width, result.Bounds.Height);
                                Cv2.Rectangle(mat, rect, new Scalar(0, 255, 0), 2);

                                string label = $"{result.Name.ToString()} ({result.Confidence:P1})";
                                Cv2.PutText(mat, label, new OpenCvSharp.Point(rect.X, rect.Y - 5),
                                    HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 255, 0), 1);
                                OnYoloDetection?.Invoke(result.Name.ToString());  
                                ;
                            }               
                        }
                    }

                    var image = BitmapConverter.ToBitmap(mat);
                    var clone = (Bitmap)image.Clone();
                    image.Dispose();

                    pictureBox.Invoke((MethodInvoker)(() =>
                    {
                        pictureBox.Image?.Dispose();
                        pictureBox.Image = (Bitmap)clone.Clone();

                        if (mirrorBox != null && mirrorBox.IsHandleCreated && !mirrorBox.IsDisposed)
                        {
                            mirrorBox.Image?.Dispose();
                            mirrorBox.Image = (Bitmap)clone.Clone();
                        }
                    }));

                    clone.Dispose();
                }
                catch { break; }
            }
        }



        // 카메라 노출값 설정
        public void SetExposure(double value)
        {
            if (capture != null && capture.IsOpened())
            {
                exposure = value;
                capture.Set(VideoCaptureProperties.Exposure, value);
            }
        }
        // 현재 카메라 노출값 반환
        public double GetExposure()
        {
            return exposure;
        }

        // 미러(PictureBox) 지정(영상 복제)
        public void SetMirror(PictureBox mirror)
        {
            mirrorBox = mirror;
        }

        // 카메라 영상 및 스레드 정지
        public void Stop()
        {
            if (!isRunning) return;

            isRunning = false;
            if (cameraThread != null && cameraThread.IsAlive)
            {
                cameraThread.Join(300);
            }
            capture?.Release();
            capture?.Dispose();

        }
        // 현재 프레임(Mat) 캡처 반환
        public Mat? CaptureCurrentFrame()
        {
            if (capture == null || !capture.IsOpened()) return null;

            var mat = new Mat();
            capture.Read(mat);
            return mat;
        }




    }
}
