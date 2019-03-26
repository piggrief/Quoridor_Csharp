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
using System.Runtime.InteropServices;  

namespace Quoridor_With_C
{
    public partial class Form1 : Skin_Metro
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole(); 

        Graphics Gr;
        Bitmap bmp = new Bitmap(1000, 900);

        enum EnumNowPlayer
        {
            Player1,
            Player2
        }
        EnumNowPlayer NowPlayer = EnumNowPlayer.Player1;
        public enum NowAction
        {
            Action_PlaceVerticalBoard,
            Action_PlaceHorizontalBoard,
            Action_Move,
            Action_Wait
        }

        NowAction P1NowAction = NowAction.Action_Wait;
        NowAction P2NowAction = NowAction.Action_Wait;

        public class _FormDraw
        {
            public static float CB_LineWidth = 0;//棋盘框线宽度
            public static float CB_BlockWidth = 0;//棋盘每格的宽度
            public static int CB_size_width = 0;//棋盘宽的像素大小
            public static int CB_size_height = 0;//棋盘高的像素大小
            public static int StartLocation_X = 16;//棋盘框线的起始位置X坐标
            public static int StartLocation_Y = 16;//棋盘框线的起始位置Y坐标

            /// <summary>
            /// 画棋盘框线函数
            /// </summary>
            /// <param name="Gr">绘画类</param>
            /// <param name="size_width">棋盘宽度</param>
            /// <param name="size_height">棋盘高度</param>
            /// <param name="LineColor">框线颜色</param>
            /// <param name="LineWidth">框线宽度</param>
            public void DrawChessBoard(Graphics Gr, int size_width, int size_height, float LineWidth, float BlockWidth)
            {
                CB_LineWidth = LineWidth;
                CB_size_width = size_width;
                CB_size_height = size_height;
                CB_BlockWidth = BlockWidth;
            }
            /// <summary>
            /// 画挡板
            /// </summary>
            /// <param name="Gr">绘画类</param>
            /// <param name="NA">当前动作状态</param>
            /// <param name="row">第row行挡板</param>
            /// <param name="col">第col列挡板</param>
            /// <param name="BoardColor">挡板颜色</param>
            /// <param name="BoardWidth">挡板宽度，最好和棋盘框长度一样</param>
            public void DrawBoard(Graphics Gr, NowAction NA, int row, int col, Color BoardColor)
            {
                int BlockWidth = _FormDraw.CB_size_width / 8;
                if (NA == NowAction.Action_PlaceVerticalBoard)
                {
                    Pen pen = new Pen(BoardColor, CB_LineWidth);//定义画笔，里面的参数为画笔的颜色

                    int x0 = StartLocation_X, y0 = StartLocation_Y;
                    int x1 = StartLocation_X, y1 = StartLocation_Y;

                    x0 = Convert.ToInt16(x0 + CB_LineWidth / 2 + col * CB_BlockWidth);
                    y0 = Convert.ToInt16(y0 + CB_LineWidth / 2 + row * CB_BlockWidth);
                    x1 = x0;
                    y1 = Convert.ToInt16(y0 + 2 * CB_BlockWidth);

                    Gr.DrawLine(pen, x0, y0, x1, y1);
                }
                else if(NA == NowAction.Action_PlaceHorizontalBoard)
                {
                    Pen pen = new Pen(BoardColor, CB_LineWidth);//定义画笔，里面的参数为画笔的颜色

                    int x0 = StartLocation_X, y0 = StartLocation_Y;
                    int x1 = StartLocation_X, y1 = StartLocation_Y;

                    x0 = Convert.ToInt16(x0 + CB_LineWidth / 2 + col * CB_BlockWidth);
                    y0 = Convert.ToInt16(y0 + CB_LineWidth / 2 + row * CB_BlockWidth);
                    x1 = Convert.ToInt16(x0 + 2 * CB_BlockWidth);
                    y1 = y0;

                    Gr.DrawLine(pen, x0, y0, x1, y1);
                }
            }
            /// <summary>
            /// 画棋子圆
            /// </summary>
            /// <param name="Gr">绘画类</param>
            /// <param name="row">第row行棋格</param>
            /// <param name="col">第col行棋格</param>
            /// <param name="ChessColor">棋子颜色</param>
            /// <param name="LineWidth">棋盘线宽</param>
            public void DrawChess(Graphics Gr, int row, int col, Color ChessColor, int LineWidth)
            {
                int BlockWidth = _FormDraw.CB_size_width / 8;
                int size_Chess = Convert.ToInt16(BlockWidth * 0.6)-2;
                Gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Brush bush = new SolidBrush(ChessColor);//填充的颜色

                Gr.FillEllipse(bush, col* BlockWidth + size_Chess / 2 + 2, row * BlockWidth + size_Chess / 2 + 2, size_Chess, size_Chess);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50 
            }
            /// <summary>
            /// 清除棋子圆
            /// </summary>
            /// <param name="Gr">绘画类</param>
            /// <param name="row">第row行棋格</param>
            /// <param name="col">第col行棋格</param>
            /// <param name="ClearColor">清除颜色</param>
            /// <param name="LineWidth">棋盘线宽</param>
            public void ClearChess(Graphics Gr, int row, int col, Color ClearColor, int LineWidth)
            {
                int BlockWidth = _FormDraw.CB_size_width / 8;
                double size_Chess = Convert.ToDouble(BlockWidth) * 0.6;
                Gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Brush bush = new SolidBrush(ClearColor);//填充的颜色
                int x = Convert.ToInt16(col * BlockWidth + size_Chess / 2);
                int y = Convert.ToInt16(row * BlockWidth + size_Chess / 2);

                Gr.FillEllipse(bush, x, y, Convert.ToInt16(size_Chess), Convert.ToInt16(size_Chess));//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50 
            }
        }
        public Form1()
        {
            InitializeComponent();
            AllocConsole();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gr = Graphics.FromImage(ChessBoardPB.Image);
            _FormDraw FD = new _FormDraw();

            //Gr.Clear(Color.FromArgb(42, 58, 86));
            FD.DrawChessBoard(Gr, 630, 630, 10, 83.5F);
            ChessBoardPB.Size = new Size(630, 630);

            ChessBoardPB.BackgroundImage = bmp;
        }


        private void PlaceBoardBTN_Click(object sender, EventArgs e)
        {
            if (NowPlayer == EnumNowPlayer.Player1)
            {
                P1NowAction = NowAction.Action_PlaceVerticalBoard;
            }
        }

        private void MoveBTN_Click(object sender, EventArgs e)
        {
            if (NowPlayer == EnumNowPlayer.Player1)
            {
                P1NowAction = NowAction.Action_Move;
            }
        }
        public System.Drawing.Point TP = new System.Drawing.Point();

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

    
        }
        bool flag_clear = true;
        private void ChessBoardPB_MouseClick(object sender, MouseEventArgs e)
        {
            TP = e.Location;
            //Console.WriteLine("MouseClick!");
            if (NowPlayer == EnumNowPlayer.Player1)
            {
                if (P1NowAction == NowAction.Action_PlaceVerticalBoard)
                {
                    int col = 0, row = 0;//0~7行列
                    col = Convert.ToInt16((TP.X  - _FormDraw.StartLocation_X) / _FormDraw.CB_BlockWidth);
                    row = Convert.ToInt16((TP.Y + _FormDraw.CB_BlockWidth / 2 - _FormDraw.StartLocation_Y) / _FormDraw.CB_BlockWidth) - 1;
                    Console.WriteLine("挡板位置：" + col.ToString() + "," + row.ToString());

                    if (row >= 6 || col <= 0 || col >= 7)
                    {
                        MessageBox.Show("挡板放置错误请重放！");
                        return;
                    }
                    _FormDraw FD = new _FormDraw();
                    FD.DrawBoard(Gr, NowAction.Action_PlaceVerticalBoard, row, col, Color.Red);
                    //ChessBoardPB.Size = new Size(630, 630);
                    //ChessBoardPB.BackgroundImage = bmp;
                    ChessBoardPB.Refresh();
                }
                else if (P1NowAction == NowAction.Action_PlaceHorizontalBoard)
                {
                    int col = 0, row = 0;//0~7行列
                    col = Convert.ToInt16((TP.X + _FormDraw.CB_BlockWidth / 2  - _FormDraw.StartLocation_X) / _FormDraw.CB_BlockWidth) - 1;
                    row = Convert.ToInt16((TP.Y - _FormDraw.StartLocation_Y) / _FormDraw.CB_BlockWidth);
                    Console.WriteLine("挡板位置：" + col.ToString() + "," + row.ToString()); 
                    if (col >= 6 || row <= 0 || row >= 7)
                    {
                        MessageBox.Show("挡板放置错误请重放！");
                        return;
                    }
                    _FormDraw FD = new _FormDraw();
                    FD.DrawBoard(Gr, NowAction.Action_PlaceHorizontalBoard, row, col, Color.Red);
                    ChessBoardPB.Refresh();

                }
                else if (P1NowAction == NowAction.Action_Move)
                {
                    int col = 0, row = 0;//0~7行列
                    int BlockWidth = (_FormDraw.CB_size_width - 32) / 8;
                    col = (TP.X) / BlockWidth;
                    row = (TP.Y) / BlockWidth;
                    Console.WriteLine("挡板位置：" + TP.X.ToString() + "," + TP.Y.ToString()); 
                    _FormDraw FD = new _FormDraw();
                    flag_clear = !flag_clear;

                    if (flag_clear)
                        FD.ClearChess(Gr, row, col, Color.FromArgb(42, 58, 86), 10);
                    else
                        FD.DrawChess(Gr, row, col, Color.Green,10);
                    ChessBoardPB.Refresh(); 
                }
            }
        }

    }
}
