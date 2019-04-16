using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using System.IO.Ports;
using System.IO;
using System.Threading;
using UartComunication;

namespace Quoridor_With_C
{
    public partial class DebugTool : Skin_Metro
    {
        int Size_x = 640;
        int Size_y = 480;
        public DebugTool()
        {
            InitializeComponent();
        }

        public void SerialPortSetInit()
        {
            BaudCB.SelectedIndex = 0;
            DataBitsCB.SelectedIndex = 0;
            StopBitsCB.SelectedIndex = 0;
            ParityCB.SelectedIndex = 0;
            PortCBFresh();
            if (PortCB.Items.Count > 0)
                PortCB.SelectedIndex = 0;
        }
        /// <summary>
        /// 串口端口号Combox刷新
        /// </summary>
        public void PortCBFresh()
        {
            string[] ports = SerialPort.GetPortNames();
            PortCB.Items.Clear();
            foreach (string port in ports)
            {
                PortCB.Items.Add(port);
            }
            if (PortCB.Items.Count == 0)
            {
                PortCB.Text = "";
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void DebugTool_Load(object sender, EventArgs e)
        { 
            Control.CheckForIllegalCrossThreadCalls = false;

            SerialPortSetInit();
            this.Size = new System.Drawing.Size(640, 480);
            Size_x = this.Size.Width;
            Size_y = this.Size.Height;
            skinTabControl1.Location = new Point(skinMenuStrip1.Location.X, skinMenuStrip1.Size.Height + skinMenuStrip1.Location.Y);
            skinTabControl1.Size = new Size(this.Size.Width - skinMenuStrip1.Location.X * 2, this.Size.Height - skinTabControl1.Location.Y - skinMenuStrip1.Location.Y);
        }

        private void PortCB_Click(object sender, EventArgs e)
        {
            PortCBFresh();
        }
        System.Drawing.Size ChangeSize(System.Drawing.Size S, double rate_x, double rate_y)
        {

            int h = S.Height;
            int w = S.Width;

            int newh = Convert.ToInt32(h * rate_y);
            int neww = Convert.ToInt32(w * rate_x);

            System.Drawing.Size S_new = new System.Drawing.Size(neww, newh);

            return S_new;
        }
        private void DebugTool_Resize(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 控件尺寸跟随改变
        /// </summary>
        private void DebugTool_SizeChanged(object sender, EventArgs e)
        {
            int NewSize_x = this.Size.Width;
            int NewSize_y = this.Size.Height;
            double rate_x = Convert.ToDouble(NewSize_x) / Convert.ToDouble(Size_x);
            double rate_y = Convert.ToDouble(NewSize_y) / Convert.ToDouble(Size_y);

            skinTabControl1.Location = new Point(skinMenuStrip1.Location.X, skinMenuStrip1.Size.Height + skinMenuStrip1.Location.Y);
            skinTabControl1.Size = new Size(this.Size.Width - skinMenuStrip1.Location.X * 2,this.Size.Height - skinTabControl1.Location.Y - skinMenuStrip1.Location.Y);

            Size_x = NewSize_x;
            Size_y = NewSize_y;
        }
        /// <summary>
        /// 控件尺寸跟随改变
        /// </summary>
        private void skinTabControl1_SizeChanged(object sender, EventArgs e)
        {
            ReceiveGB.Size = new Size(skinTabControl1.Size.Width - ReceiveGB.Location.X, Convert.ToInt32(skinTabControl1.Size.Height / 3.0 * 2.0 - ReceiveGB.Location.Y));
            SendGB.Location = new Point(ReceiveGB.Location.X, ReceiveGB.Size.Height + ReceiveGB.Location.Y);
            SendGB.Size = new Size(skinTabControl1.Size.Width - ReceiveGB.Location.X, Convert.ToInt32(skinTabControl1.Size.Height - ReceiveGB.Size.Height*1.15));

            ReceiveClearBTN.Location = new Point(Convert.ToInt32(ReceiveGB.Size.Width - ReceiveClearBTN.Size.Width*1.1)
                                                , Convert.ToInt32(ReceiveGB.Size.Height - ReceiveClearBTN.Size.Height*1.1));
            SaveDataBTN.Location = new Point(Convert.ToInt32(ReceiveGB.Size.Width - SaveDataBTN.Size.Width * 1.1)
                                                , Convert.ToInt32(ReceiveClearBTN.Location.Y - SaveDataBTN.Size.Height * 1.1));
            SwitchReceiveBTN.Location = new Point(Convert.ToInt32(ReceiveGB.Size.Width - SwitchReceiveBTN.Size.Width * 1.1)
                                                , Convert.ToInt32(SaveDataBTN.Location.Y - SwitchReceiveBTN.Size.Height * 1.1));

            HexCB.Location = new Point(Convert.ToInt32(ReceiveGB.Size.Width - HexCB.Size.Width * 1.1)
                                                , Convert.ToInt32(SwitchReceiveBTN.Location.Y - HexCB.Size.Height * 1.1));

            ReceiveTB.Size = new Size(Convert.ToInt32(ReceiveGB.Size.Width - ReceiveClearBTN.Size.Width * 1.1 - ReceiveClearBTN.Size.Height * 0.5)
                                    , ReceiveGB.Size.Height - ReceiveTB.Location.Y - 5);
        }
        class TimerExampleState
        {
            public int counter = 0;
            public System.Threading.Timer tmr;
        }

        public UART Uart1 = new UART();
        public enum UARTStatus
        {
            SerialPortClosed,
            SerialPortOpen,
            DataReceiving,
            DataReceiveFinish
        }
        UARTStatus NowUartStatus = UARTStatus.SerialPortClosed;
        /// <summary>是否正在进行接收标志位</summary>
        Thread getRecevice;
        string strRecieve;
        TimerExampleState s = new TimerExampleState();
        private void SwitchReceiveBTN_Click(object sender, EventArgs e)
        {
            if (SwitchReceiveBTN.Text == "停止接收")
            {
                NowUartStatus = UARTStatus.DataReceiveFinish;
            }
            if (NowUartStatus == UARTStatus.SerialPortClosed)
            {
                Uart1.SetSerialPort(PortCB.Text, int.Parse(BaudCB.Text), int.Parse(DataBitsCB.Text), int.Parse(StopBitsCB.Text));

                if (Uart1.SwtichSP(true) == true)//打开成功
                {
                    NowUartStatus = UARTStatus.SerialPortOpen;
                }
                else
                {
                    PortCBFresh();
                }
            }
            if (NowUartStatus == UARTStatus.SerialPortOpen)
            {
                Uart1.sp.Encoding = Encoding.GetEncoding("GB2312");
                //使用委托以及多线程进行
                #region 打开多线程定时器
                //创建代理对象TimerCallback，该代理将被定时调用
                TimerCallback timerDelegate = new TimerCallback(TimerRecive);
                //创建一个时间间隔为1s的定时器
                System.Threading.Timer timer = new System.Threading.Timer(timerDelegate, s, 0, 100);
                s.tmr = timer;
                #endregion

                SwitchReceiveBTN.Text = "停止接收";
                NowUartStatus = UARTStatus.DataReceiving;
            }
        }
        void TimerRecive(Object state)
        {
            TimerExampleState s = (TimerExampleState)state;
            if (NowUartStatus == UARTStatus.DataReceiveFinish)
            {
                if (Uart1.SwtichSP(false) == true)//关闭成功
                {

                }

                SwitchReceiveBTN.Text = "开始接收";
                NowUartStatus = UARTStatus.SerialPortClosed;

                s.tmr.Dispose();
                s.tmr = null;
                return;
            }
            try
            {
                strRecieve = Uart1.sp.ReadExisting();
                string newstr = "";
                if (HexCB.Checked == true)
                {
                    byte[] b = Uart1.sp.Encoding.GetBytes(strRecieve);
                    newstr += b.ToString();
                    //strRecieve = strRecieve.ToString("X2");//StringToHexString(strRecieve, Encoding.GetEncoding("GB2312"));
                }
                Console.WriteLine(strRecieve);
                ReceiveTB.Text += strRecieve;

                if (ReceiveTB.Text.Length >= 10000)
                {
                    NowUartStatus = UARTStatus.DataReceiveFinish;
                    SwitchReceiveBTN.Text = "开始接收";
                }
            }
            catch (Exception ex) { }
            //}
        }
        private string StringToHexString(string s,Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%"+Convert.ToString(b[i], 16);
            }
            return result;
        }
    }
}
