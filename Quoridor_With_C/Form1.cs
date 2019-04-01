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
using Quoridor;

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

        public string GameMode = "DoublePlay";
        public enum EnumNowPlayer
        {
            Player1,
            Player2
        }
        EnumNowPlayer NowPlayer = EnumNowPlayer.Player1;
        public enum NowAction
        {
            Action_PlaceVerticalBoard,
            Action_PlaceHorizontalBoard,
            Action_Move_Player1,
            Action_Move_Player2,
            Action_Wait
        }

        NowAction PlayerNowAction = NowAction.Action_Move_Player1;

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
                    y1 = Convert.ToInt16(y0 + CB_BlockWidth);

                    Gr.DrawLine(pen, x0, y0, x1, y1);
                }
                else if(NA == NowAction.Action_PlaceHorizontalBoard)
                {
                    Pen pen = new Pen(BoardColor, CB_LineWidth);//定义画笔，里面的参数为画笔的颜色

                    int x0 = StartLocation_X, y0 = StartLocation_Y;
                    int x1 = StartLocation_X, y1 = StartLocation_Y;

                    x0 = Convert.ToInt16(x0 + CB_LineWidth / 2 + col * CB_BlockWidth);
                    y0 = Convert.ToInt16(y0 + CB_LineWidth / 2 + row * CB_BlockWidth);
                    x1 = Convert.ToInt16(x0 + CB_BlockWidth);
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
            public Point DrawChess(Graphics Gr, int row, int col, Color ChessColor)
            {
                int size_Chess = Convert.ToInt16(_FormDraw.CB_BlockWidth * 0.7);
                //Gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //Brush bush = new SolidBrush(ChessColor);//填充的颜色
                int x = _FormDraw.StartLocation_X + Convert.ToInt16(col * _FormDraw.CB_BlockWidth + size_Chess / 2 - size_Chess / 5);
                int y = _FormDraw.StartLocation_Y + Convert.ToInt16(row * _FormDraw.CB_BlockWidth + size_Chess / 2 - size_Chess / 5);

                return new Point(x, y);

                //Gr.FillEllipse(bush, x, y, size_Chess, size_Chess);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50 
            }

        }
        public Form1()
        {
            InitializeComponent();

        }
        QuoridorGame NowQuoridor = new QuoridorGame();
        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
            Gr = Graphics.FromImage(ChessBoardPB.Image);
            _FormDraw FD = new _FormDraw();

            FD.DrawChessBoard(Gr, 630, 630, 10, 83.5F);
            ChessBoardPB.Size = new Size(630, 630);

            ChessWhitePB.Size = new Size(58, 58);
            ChessBoardPB.Controls.Add(ChessWhitePB);
            ChessWhitePB.Location = new Point(0, 0);
            ChessWhitePB.BackColor = Color.Transparent;
            ChessBlackPB.Size = new Size(58, 58);
            ChessBoardPB.Controls.Add(ChessBlackPB);
            ChessBlackPB.Location = new Point(100, 100);
            ChessBlackPB.BackColor = Color.Transparent;

            ChessWhitePB.Location = FD.DrawChess(Gr, 0, 3, Color.White);
            ChessBlackPB.Location = FD.DrawChess(Gr, 6, 3, Color.Black);

            //刷新初始棋盘
            NowQuoridor.ThisChessBoard.DrawNowChessBoard(ref Gr, ChessWhitePB, ChessBlackPB);
            ChessBoardPB.Refresh();

            TestTB.Text = "当前行动玩家：白子";
            TestTB.Text += System.Environment.NewLine;
            TestTB.Text += "白子剩余挡板：" + NowQuoridor.NumPlayer1Board.ToString();
            TestTB.Text += System.Environment.NewLine;
            TestTB.Text += "黑子剩余挡板：" + NowQuoridor.NumPlayer2Board.ToString();

        }

        bool IfPlaceBoard = false;

        private void PlaceBoardBTN_Click(object sender, EventArgs e)
        {
            PlayerNowAction = NowAction.Action_PlaceVerticalBoard;
            PlaceVerticalBoardBTN.Enabled = false;
            PlaceHorizontalBoardBTN.Enabled = false;
            IfPlaceBoard = true;
        }

        private void MoveBTN_Click(object sender, EventArgs e)
        {
            if(NowPlayer == EnumNowPlayer.Player1) 
                PlayerNowAction = NowAction.Action_Move_Player1;
            else
                PlayerNowAction = NowAction.Action_Move_Player2;
        }
        public System.Drawing.Point TP = new System.Drawing.Point();

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

    
        }
        private void ChessBoardPB_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void ChessWhitePB_Click(object sender, EventArgs e)
        {
            string Hint1 = "This is Not Empty!";
            Console.WriteLine(Hint1);

        }

        private void PlaceHorizontalBoardBTN_Click(object sender, EventArgs e)
        {
            PlayerNowAction = NowAction.Action_PlaceHorizontalBoard;
            PlaceVerticalBoardBTN.Enabled = false;
            PlaceHorizontalBoardBTN.Enabled = false;
            IfPlaceBoard = true;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            FreeConsole();
            Application.Exit();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void ChessBoardPB_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)//右键取消正在执行的操作
            {
                if (IfPlaceBoard)
                {
                    IfPlaceBoard = false;
                    if (NowPlayer == EnumNowPlayer.Player1)
                    {
                        PlayerNowAction = NowAction.Action_Move_Player1;
                    }
                    if (NowPlayer == EnumNowPlayer.Player2)
                    {
                        PlayerNowAction = NowAction.Action_Move_Player2;
                    }
                    PlaceVerticalBoardBTN.Enabled = true;
                    PlaceHorizontalBoardBTN.Enabled = true;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)//左键执行某个行动
            {
                string Hint = "OK";

                TP = e.Location;
                # region 计算相关操作对应的操作位置的行和列

                int col = 0, row = 0;//0~7行列
                if (PlayerNowAction == NowAction.Action_PlaceVerticalBoard)
                {
                    col = Convert.ToInt16((TP.X - _FormDraw.StartLocation_X) / _FormDraw.CB_BlockWidth);
                    row = Convert.ToInt16((TP.Y + _FormDraw.CB_BlockWidth / 2 - _FormDraw.StartLocation_Y) / _FormDraw.CB_BlockWidth) - 1;
                }
                else if (PlayerNowAction == NowAction.Action_PlaceHorizontalBoard)
                {
                    col = Convert.ToInt16((TP.X + _FormDraw.CB_BlockWidth / 2 - _FormDraw.StartLocation_X) / _FormDraw.CB_BlockWidth) - 1;
                    row = Convert.ToInt16((TP.Y - _FormDraw.StartLocation_Y) / _FormDraw.CB_BlockWidth);                
                }
                else if (PlayerNowAction == NowAction.Action_Move_Player1 || PlayerNowAction == NowAction.Action_Move_Player2)
                {
                    col = Convert.ToInt16(TP.X / _FormDraw.CB_BlockWidth) - 1;
                    row = Convert.ToInt16(TP.Y / _FormDraw.CB_BlockWidth) - 1;
                }

                # endregion

                if (!(row >= 0 && row <= 6 && col >= 0 && col <= 6))
                {
                    Hint = "点击越界！";
                    MessageBox.Show(Hint);
                    return;
                }
                string Hint1 = "";
                string Hint2 = "";
                Hint1 = NowQuoridor.CheckBoard(PlayerNowAction, EnumNowPlayer.Player1, row, col);
                Hint2 = NowQuoridor.CheckBoard(PlayerNowAction, EnumNowPlayer.Player2, row, col);


                if (Hint1 == "Player1 No Board")
                {
                    if (PlayerNowAction == NowAction.Action_PlaceHorizontalBoard
                        || PlayerNowAction == NowAction.Action_PlaceVerticalBoard)
                    {
                        MessageBox.Show(Hint1);
                        return;
                    }
                }
                else if (Hint2 == "Player2 No Board")
                {
                    if (PlayerNowAction == NowAction.Action_PlaceHorizontalBoard
                        || PlayerNowAction == NowAction.Action_PlaceVerticalBoard)
                    {
                        MessageBox.Show(Hint1);
                        return;
                    } 
                }

                if ((Hint1 != "Player1 No Board" && Hint2 != "Player2 No Board")
                    && (Hint1 != "OK" || Hint2 != "OK"))
                {
                    MessageBox.Show(Hint1 + Hint2);
                    return;
                }

                Hint = NowQuoridor.ThisChessBoard.Action(row, col, PlayerNowAction);
                if (Hint != "OK")
                {
                    MessageBox.Show(Hint);
                    return;
                }
                if (NowPlayer == EnumNowPlayer.Player1)
                {
                    if(PlayerNowAction == NowAction.Action_PlaceVerticalBoard 
                        || PlayerNowAction == NowAction.Action_PlaceHorizontalBoard)
                        NowQuoridor.NumPlayer1Board -= 2;
                    NowPlayer = EnumNowPlayer.Player2;
                }
                else if (NowPlayer == EnumNowPlayer.Player2)
                {
                    if (PlayerNowAction == NowAction.Action_PlaceVerticalBoard
                        || PlayerNowAction == NowAction.Action_PlaceHorizontalBoard)
                        NowQuoridor.NumPlayer2Board -= 2;
                    NowPlayer = EnumNowPlayer.Player1;
                }

                NowQuoridor.ThisChessBoard.DrawNowChessBoard(ref Gr, ChessWhitePB, ChessBlackPB);
                ChessBoardPB.Refresh();

                int[,] disbuff = new int[7, 7];
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        disbuff[i, j] = NowQuoridor.CalDistance(EnumNowPlayer.Player1, i, j);
                        Console.Write(disbuff[i, j].ToString() + " ");
                    }
                    Console.WriteLine();
                }

                int dis = 0;
                if (NowPlayer == EnumNowPlayer.Player1)
                    dis = NowQuoridor.LookupRoad_Greedy(NowPlayer, NowQuoridor.ThisChessBoard.Player1Location.X, NowQuoridor.ThisChessBoard.Player1Location.Y, new List<Point>());
                else
                    dis = NowQuoridor.LookupRoad_Greedy(NowPlayer, NowQuoridor.ThisChessBoard.Player2Location.X, NowQuoridor.ThisChessBoard.Player2Location.Y, new List<Point>());
                Console.WriteLine("dis = " + dis.ToString());

                string result = NowQuoridor.CheckResult();
                if (result != "No success")
                {
                    MessageBox.Show(result);
                }

                if (NowPlayer == EnumNowPlayer.Player1)
                {
                    MessageBox.Show("现在轮到玩家1操作！");
                    TestTB.Text = "当前行动玩家：白子";
                    TestTB.Text += System.Environment.NewLine;
                    TestTB.Text += "白子剩余挡板：" + NowQuoridor.NumPlayer1Board.ToString();
                    TestTB.Text += System.Environment.NewLine;
                    TestTB.Text += "黑子剩余挡板：" + NowQuoridor.NumPlayer2Board.ToString();

                    PlayerNowAction = NowAction.Action_Move_Player1;
                }
                if (NowPlayer == EnumNowPlayer.Player2)
                {
                    MessageBox.Show("现在轮到玩家2操作！");
                    TestTB.Text = "当前行动玩家：黑子";
                    TestTB.Text += System.Environment.NewLine;
                    TestTB.Text += "白子剩余挡板：" + NowQuoridor.NumPlayer1Board.ToString();
                    TestTB.Text += System.Environment.NewLine;
                    TestTB.Text += "黑子剩余挡板：" + NowQuoridor.NumPlayer2Board.ToString();

                    PlayerNowAction = NowAction.Action_Move_Player2;
                }

                PlaceVerticalBoardBTN.Enabled = true;
                PlaceHorizontalBoardBTN.Enabled = true; 
            }
        }

        private void TestBTN_Click(object sender, EventArgs e)
        {
            int rowbuff = NowQuoridor.ThisChessBoard.Player1Location.X;
            int colbuff = NowQuoridor.ThisChessBoard.Player1Location.Y;
            int player1dis = NowQuoridor.AstarRestart(EnumNowPlayer.Player1, rowbuff, colbuff);
            Console.WriteLine("玩家1最短路径长度：");
            Console.WriteLine(player1dis.ToString());

            rowbuff = NowQuoridor.ThisChessBoard.Player2Location.X;
            colbuff = NowQuoridor.ThisChessBoard.Player2Location.Y;
            int player2dis = NowQuoridor.AstarRestart(EnumNowPlayer.Player2, rowbuff, colbuff);
            Console.WriteLine("玩家2最短路径长度：");
            Console.WriteLine(player2dis.ToString());

            List<Point> Roadbuff = NowQuoridor.Player1MinRoad;
            Console.WriteLine("Player1最短路径：");
            for (int i = Roadbuff.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(Roadbuff[i].X.ToString()+ ", " +Roadbuff[i].Y.ToString());
            }
            Roadbuff = NowQuoridor.Player2MinRoad;
            Console.WriteLine("Player2最短路径：");
            for (int i = Roadbuff.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(Roadbuff[i].X.ToString() + ", " + Roadbuff[i].Y.ToString());
            }
        }

    }
}
