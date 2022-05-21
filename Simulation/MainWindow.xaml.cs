using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulation
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private int applyTemp;
        private int applyHumi;
        private int applyDust;

        private float currentTemp;
        private float currentHumi;
        private float currentDust;
        private float currentElect;
        private float currentRuntime;

        private const string off = "중지";
        private const string on = "가동";

        private const string humiDeviceText = "가습기 ";
        private const string deHumiDeviceText = "제습기 ";
        private const string dustDeviceText = "공기청정기 ";
        private const string windowDeviceText = "창문 ";
        private const string airDeviceText = "에어컨 ";
        private const string heatDeviceText = "히터 ";

        private Brush red;
        private Brush green;

        Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            var converter = new System.Windows.Media.BrushConverter();
            var redBrush = (Brush)converter.ConvertFromString("#FFFF4D4D");
            red = redBrush;
            var greenBrush = (Brush)converter.ConvertFromString("#FF4DFF65");
            green = greenBrush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(timer != null)
                if (timer.Enabled)
                    timer.Stop();


            applyTemp = Convert.ToInt32(TempTextbox.Text);
            applyHumi = Convert.ToInt32(HumiTextbox.Text);
            applyDust = Convert.ToInt32(DustTextbox.Text);
            currentTemp = applyTemp;
            currentHumi = applyHumi;
            currentDust = applyDust;
            currentRuntime = 0;
            currentElect = 0;
            startSimulation();
        }

        private void startSimulation()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; 
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            currentRuntime++;
            CalculateInf();
        }

        void CalculateInf()
        {
            if(currentTemp > 25)
            {
                PowerStatus(4, true);
                currentTemp -= 0.2f;
                currentElect += 0.05f;
            } else
            {
                PowerStatus(4, false);
            }
            if (currentHumi > 45)
            {
                PowerStatus(1, true);
                currentHumi -= 1;
                currentElect += 0.007f;
            }
            else
            {
                PowerStatus(1, false);
            }
            if(currentDust > 15)
            {
                PowerStatus(2, true);
                currentElect += 0.4f;
                currentDust -= 0.003f;
            } else
            {
                PowerStatus(2, false);
            }

            currentTempText.Text = "온도 " + Math.Round(currentTemp,1).ToString() + "°C";
            currentHumiText.Text = "습도 " + Math.Round(currentHumi,1).ToString() + "%";
            currentDustText.Text = "미세먼지 농도 " + Math.Round(currentDust,1).ToString() + "㎍/㎥";
            currentElectText.Text = "총 전력사용량 " + Math.Round(currentElect, 1) + "wh";
            currentElectText2.Text = "한달 예상사용량 " + (Math.Round(currentElect * currentRuntime * 30)) + "wh";
            runtime.Text = "실행시간 " + currentRuntime.ToString() + "분";

            //현시간 사용량 * 실행 시간 * 한달
        }

        void PowerStatus(int type, bool enabled)
        {
            switch (type)
            {
                case 0:
                    if (enabled)
                    {
                        humiDevice.Text = humiDeviceText + on;
                        ChangeColor(humiDevice, green);
                    }
                    else
                    {
                        humiDevice.Text = humiDeviceText + off;
                        ChangeColor(humiDevice, red);
                    }
                     
                    break;
                case 1:
                    if (enabled)
                    {
                        dehumiDevice.Text = deHumiDeviceText+ on;
                        ChangeColor(dehumiDevice, green);
                    }
                    else
                    {
                        dehumiDevice.Text = deHumiDeviceText + off;
                        ChangeColor(dehumiDevice, red);
                    }

                    break;
                case 2:
                    if (enabled)
                    {
                        dustDevice.Text = dustDeviceText + on;
                        ChangeColor(dustDevice, green);
                    }
                    else
                    {
                        dustDevice.Text = dustDeviceText + off;
                        ChangeColor(dustDevice, red);
                    }

                    break;
                case 3:
                    if (enabled)
                    {
                        winDevice.Text = windowDeviceText + "열림";
                        ChangeColor(winDevice, green);
                    }
                    else
                    {
                        winDevice.Text = windowDeviceText + "닫힘";
                        ChangeColor(winDevice, red);
                    }

                    break;
                case 4:
                    if (enabled)
                    {
                        airDevice.Text = airDeviceText + on;
                        ChangeColor(airDevice, green);
                    }
                    else
                    {
                        airDevice.Text = airDeviceText + off;
                        ChangeColor(airDevice, red);
                    }

                    break;
                case 5:
                    if (enabled)
                    {
                        Device.Text = heatDeviceText + on;
                        ChangeColor(Device, green);
                    }
                    else
                    {
                        Device.Text = heatDeviceText + off;
                        ChangeColor(Device, red);
                    }

                    break;
            }
        }

        void ChangeColor(TextBlock t, Brush color)
        {
            t.Foreground = color;
        }

    }
}
