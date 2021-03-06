﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using System.Runtime.InteropServices;
using QuoridorEva;
using Queen;
using DebugToolForm;
using QuoridorRule;
using GameTree;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;
using System.Diagnostics;
using MCTS;
using System.IO;
using System.Drawing.Drawing2D;
using QuoridorGameAlgorithm;
using QuoridorGameClass;

namespace Quoridor_With_C
{
    public partial class Form1 : Skin_Metro
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole(); 

        public static Graphics Gr;
        Graphics Gr_Chess1;

        Bitmap bmp = new Bitmap(1000, 900);
        public enum GameModeStatus
        {
            SinglePlay,
            DoublePlay,
            Queen8
        }
        public GameModeStatus GameMode;

        EnumNowPlayer NowPlayer = EnumNowPlayer.Player1;

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
            /// <summary>
            /// 获得八皇后棋子绘制位置
            /// </summary>
            /// <param name="row">行</param>
            /// <param name="col">列</param>
            /// <returns>位置坐标</returns>      
            public Point GetQueenChessLocation(int row, int col)
            {
                int size_Chess = Convert.ToInt16(_FormDraw.CB_BlockWidth * 0.7);
                //Gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //Brush bush = new SolidBrush(ChessColor);//填充的颜色
                int x = 14 + Convert.ToInt16(col * _FormDraw.CB_BlockWidth  
                    + size_Chess / 2 - size_Chess / 4);
                int y = 12 + Convert.ToInt16(row * _FormDraw.CB_BlockWidth 
                    + size_Chess / 2 - size_Chess / 4);

                return new Point(x, y);

                //Gr.FillEllipse(bush, x, y, size_Chess, size_Chess);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50 
            }

        }
        public static Form1 form1;
        public Form1()
        {
           InitializeComponent();
           form1 = this;

        }
        public Form1(GameModeStatus GM)
        {
            InitializeComponent();
            GameMode = GM;
            form1 = this;

        }
        public void ConfigTabPageInit()
        {
            P1TypeSetCB.Items.Add(QuoridorGame.Enum_PlayerType.AI);
            P1TypeSetCB.Items.Add(QuoridorGame.Enum_PlayerType.Human);
            P1TypeSetCB.SelectedIndex = 1;
            P2TypeSetCB.Items.Add(QuoridorGame.Enum_PlayerType.AI);
            P2TypeSetCB.Items.Add(QuoridorGame.Enum_PlayerType.Human);
            P2TypeSetCB.SelectedIndex = 0;
            for (int i = 2; i <= 6; i += 2)
            {
                ABPDepthSelectCB.Items.Add(i);
            }
            ABPDepthSelectCB.SelectedItem = 2;
            for (int i = 0; i <= 12; i+=2)
			{
                OE_DepthSelectCB.Items.Add(i);			 
			}
            OE_DepthSelectCB.SelectedItem = 6;
            ABPConfigCB.SelectedIndex = 0;
            GameTabControl.SelectedTab = GameSetTabPage;
        }
        /// <summary>
        /// 显示参数配置提示字符串
        /// </summary>
        /// <param name="ConfigGame">配置好的游戏对象</param>
        public void ShowParaConfigStr(QuoridorGame ConfigGame)
        {
            string HintStr = "";
            if (ConfigGame.P1Type == QuoridorGame.Enum_PlayerType.Human)
            {
                HintStr = "P1:Human";
            }
            else
            {
                HintStr = "P1:AI,算法：";
                HintStr += ConfigGame.P1DecisionSystem.UsedAlgorithm.ToString();
                HintStr += System.Environment.NewLine;
                if (ConfigGame.P1DecisionSystem.UsedAlgorithm == QuoridorDecisionSystem.Enum_DecisionAlgorithm.AlphaBetaPurning)
                {                    
                    QuoridorDecisionSystem.ABPurningPara ParaConfiged = ConfigGame.P1DecisionSystem.ThisABPurningPara;
                    HintStr += ParaConfiged.DepthMax.ToString() + "层剪枝，"
                        + ParaConfiged.GoodExtension_Depth.ToString() + "层算杀";
                    HintStr += System.Environment.NewLine;
                    if (ParaConfiged.SortActionList)
                        HintStr += "结点有序;";
                    if (ParaConfiged.UseTT)
                        HintStr += "置换表;";
                    if (ParaConfiged.UseFormulae)
                        HintStr += "分局势;";
                    HintStr += System.Environment.NewLine;
                    if (ParaConfiged.UseGoodExtension)
                        HintStr += "单步延伸评分下限" + ParaConfiged.GoodExtension_ScoreLower.ToString();                                  
                }
            }
            P1ParaShowTB.Text = HintStr;
            if (ConfigGame.P2Type == QuoridorGame.Enum_PlayerType.Human)
            {
                HintStr = "P2:Human";
            }
            else
            {
                HintStr = "P2:AI,算法：";
                HintStr += ConfigGame.P2DecisionSystem.UsedAlgorithm.ToString();
                HintStr += System.Environment.NewLine;
                if (ConfigGame.P2DecisionSystem.UsedAlgorithm == QuoridorDecisionSystem.Enum_DecisionAlgorithm.AlphaBetaPurning)
                {
                    QuoridorDecisionSystem.ABPurningPara ParaConfiged = ConfigGame.P2DecisionSystem.ThisABPurningPara;
                    HintStr += ParaConfiged.DepthMax.ToString() + "层剪枝，"
                        + ParaConfiged.GoodExtension_Depth.ToString() + "层算杀";
                    HintStr += System.Environment.NewLine;
                    if (ParaConfiged.SortActionList)
                        HintStr += "结点有序;";
                    if (ParaConfiged.UseTT)
                        HintStr += "置换表;";
                    if (ParaConfiged.UseFormulae)
                        HintStr += "分局势;";
                    HintStr += System.Environment.NewLine;
                    if (ParaConfiged.UseGoodExtension)
                        HintStr += "单步延伸评分下限" + ParaConfiged.GoodExtension_ScoreLower.ToString();
                }
            }
            P2ParaShowTB.Text = HintStr;
            QuoridorDecisionSystem DS = ConfigGame.P1DecisionSystem;
            
        }
        public DebugView DV;
        public DebugTool DT;
        QuoridorEvalution NowQuoridor = new QuoridorEvalution();
        List<CCWin.SkinControl.SkinPictureBox> QueenChessList = new List<CCWin.SkinControl.SkinPictureBox>();
        List<CCWin.SkinControl.SkinPictureBox> QueenList = new List<CCWin.SkinControl.SkinPictureBox>();      
        public static List<Point> QueenChessLocation = new List<Point>();
        public Thread GameThread;
        private void Form1_Load(object sender, EventArgs e)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            AllocConsole();
            this.Size = new System.Drawing.Size(1024, 720);
            _FormDraw FD = new _FormDraw();

            QueenList.Add(Queen1PB);
            QueenList.Add(Queen2PB);
            QueenList.Add(Queen3PB);
            QueenList.Add(Queen4PB);
            QueenList.Add(Queen5PB);
            QueenList.Add(Queen6PB);
            QueenList.Add(Queen7PB);
            QueenList.Add(Queen8PB);

            foreach (CCWin.SkinControl.SkinPictureBox SPB in QueenList)
            {
                SPB.Size = new Size(58, 58);
                ChessBoardPB.Controls.Add(SPB);
                SPB.Visible = false;
                SPB.BackColor = Color.Transparent;
                QueenChessLocation.Add(new Point(-1, -1));
            }

            QueenChessList.Add(QueenChess1PB);
            QueenChessList.Add(QueenChess2PB);
            QueenChessList.Add(QueenChess3PB);
            QueenChessList.Add(QueenChess4PB);
            QueenChessList.Add(QueenChess5PB);
            QueenChessList.Add(QueenChess6PB);
            QueenChessList.Add(QueenChess7PB);
            QueenChessList.Add(QueenChess8PB);

            foreach (CCWin.SkinControl.SkinPictureBox SPB in QueenChessList)
            {
                SPB.Size = new Size(58, 58);
                ChessBoardPB.Controls.Add(SPB);
                SPB.Visible = false;
                SPB.BackColor = Color.Transparent;
            }

            if (GameMode == GameModeStatus.DoublePlay || GameMode == GameModeStatus.SinglePlay)
            {
                //this.Text = "步步为营游戏_" + GameMode.ToString() + "模式_上海海事大学";
                this.Text = "步步为营游戏_" + "单人游戏" + "模式_上海海事大学";

                RandomPlaceBTN.Dispose();
                CustomPlaceBTN.Dispose();
                Test2BTN.Dispose();

                ChessBoardPB.Size = new Size(630, 630);
                ChessBoardPB.Image = Resource1.qipan;
                Gr = Graphics.FromImage(ChessBoardPB.Image);
                FD.DrawChessBoard(Gr, 630, 630, 10, 83.5F);

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

                VBoardPB.Size = new System.Drawing.Size(10, 170);
                HBoardPB.Size = new System.Drawing.Size(170, 10);

                HideQueen(QueenList);

                TestTB.Location = new Point(ChessBoardPB.Location.X, ChessBoardPB.Size.Height + ChessBoardPB.Location.Y);
                #region 打开DebugView窗体
                //IfOpenDebugViewForm = true;
                Rectangle rect = new Rectangle();
                rect = Screen.GetWorkingArea(this);

                DV = new DebugView();
                DV.Size = new System.Drawing.Size(rect.Width, rect.Height);
                DV.Show();
                #endregion

                #region 配置算法选择控件
                ConfigTabPageInit();
                #endregion
                Console.WriteLine("棋盘HashCode为：" + GameTreeNode.InitChessBoardHashCode.ToString());
            }
            else
            {
                this.Text = "八皇后路径寻优仿真平台_上海海事大学";

                FD.DrawChessBoard(Gr, 630, 630, 7, 74.5F);

                ChessBoardPB.Size = new Size(630, 630);
                ChessBoardPB.Image = Resource1.qipan2019;
                Gr = Graphics.FromImage(ChessBoardPB.Image);

                ChessWhitePB.Dispose();
                ChessBlackPB.Dispose();
                PlaceVerticalBoardBTN.Dispose();
                PlaceHorizontalBoardBTN.Dispose();
                TestBTN.Dispose();

                ConfigTabControl.Visible = false;
                ConfigTabControl.Enabled = false;
                TestTB.Visible = false;
                skinToolStrip1.Visible = false;
                skinToolStrip1.Enabled = false;

                ChessBoardPB.Refresh();

                //this.Location = new Point(0, 0);
                DT = new DebugTool(this);
                //DT1.Location = new Point(this.Size.Width, 0);
                DT.Show();
            }

        }

        bool IfPlaceBoard = false;

        private void PlaceBoardBTN_Click(object sender, EventArgs e)
        {
            PlayerNowAction = NowAction.Action_PlaceVerticalBoard;
            PlaceVerticalBoardBTN.Enabled = false;
            PlaceHorizontalBoardBTN.Enabled = false;
            IfPlaceBoard = true;
            RestartFollow();
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
            RestartFollow();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            FreeConsole();
            Application.Exit();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }
        long count_AIAction = 0;
        bool PlayerFirstAction = true;
        bool AIFirstAction = true;
        GameTreeNode Root = new GameTreeNode();
        MonteCartoTreeNode MTRoot = new MonteCartoTreeNode();
        QuoridorAction OpponentAction = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));
        QuoridorAction SelfAction = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));

        public ChessManual ThisCM = new ChessManual();
        public QuoridorGame NowGame = new QuoridorGame(
            QuoridorGame.Enum_PlayerType.Human, QuoridorGame.Enum_PlayerType.AI,
                new QuoridorDecisionSystem(EnumNowPlayer.Player1, QuoridorDecisionSystem.Enum_DecisionAlgorithm.AlphaBetaPurning, new QuoridorDecisionSystem.ABPurningPara())
                , new QuoridorDecisionSystem(EnumNowPlayer.Player2, QuoridorDecisionSystem.Enum_DecisionAlgorithm.AlphaBetaPurning, new QuoridorDecisionSystem.ABPurningPara()));
        public void QuoridorGame_Do()
        {
            #region 配置初始棋盘
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[0, 3].GridStatus = Grid.GridInsideStatus.Empty;
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[1, 3].GridStatus = Grid.GridInsideStatus.Have_Player1;
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[6, 3].GridStatus = Grid.GridInsideStatus.Empty;
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[5, 3].GridStatus = Grid.GridInsideStatus.Have_Player2;

            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[1, 3].IfUpBoard = true;
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[1, 4].IfUpBoard = true;
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[6, 2].IfUpBoard = true;
            //NowGame.QuoridorEva.ThisChessBoard.ChessBoardAll[6, 3].IfUpBoard = true;

            ////NowQuoridor.ThisChessBoard.ChessBoardAll[2, 3].IfLeftBoard = true;
            ////NowQuoridor.ThisChessBoard.ChessBoardAll[3, 3].IfLeftBoard = true;
            ////NowQuoridor.ThisChessBoard.ChessBoardAll[2, 4].IfLeftBoard = true;
            ////NowQuoridor.ThisChessBoard.ChessBoardAll[3, 4].IfLeftBoard = true;

            //NowGame.QuoridorEva.ThisChessBoard.NumPlayer1Board = 16 - 2;
            //NowGame.QuoridorEva.ThisChessBoard.NumPlayer2Board = 16 - 2;


            //NowGame.QuoridorEva.ThisChessBoard.Player1Location = new Point(1, 3);
            //NowGame.QuoridorEva.ThisChessBoard.Player2Location = new Point(5, 3);
            #endregion
            BlackBoardNumLB.Text = NowGame.QuoridorEva.ThisChessBoard.NumPlayer2Board.ToString();
            WhiteBoardNumLB.Text = NowGame.QuoridorEva.ThisChessBoard.NumPlayer1Board.ToString();

            //刷新初始棋盘
            NowGame.QuoridorEva.ThisChessBoard.DrawNowChessBoard(ref Gr, ChessWhitePB, ChessBlackPB);
            ChessBoardPB.Refresh();

            EnumNowPlayer NowPlayer = EnumNowPlayer.Player1;
            while (true)
            {
                string Hint = NowGame.DoOncePolicy(ref NowPlayer);
                if (Hint == "Player1 Success!" || Hint == "Player2 Success!")
	            {
                    NowGame.QuoridorEva.ThisChessBoard.DrawNowChessBoard(ref Gr, ChessWhitePB, ChessBlackPB);
                    ChessBoardPB.Refresh();
                    MessageBox.Show(Hint);
                    System.Environment.Exit(0);                   
		            break;
	            }
                else if(Hint != "OK")
                {
                    TestTB.Text = Hint;
                    #region 取消放置挡板
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

                    IfShowFollow = false;
                    VBoardPB.Visible = false;
                    VBoardPB.Location = new Point(837, 569);
                    HBoardPB.Visible = false;
                    HBoardPB.Location = new Point(837, 569);
                    #endregion
                    continue;
                }
                #region 取消放置挡板
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

                IfShowFollow = false;
                VBoardPB.Visible = false;
                VBoardPB.Location = new Point(837, 569);
                HBoardPB.Visible = false;
                HBoardPB.Location = new Point(837, 569);
                #endregion
                if (NowPlayer == EnumNowPlayer.Player2 && NowGame.P1Type == QuoridorGame.Enum_PlayerType.AI)
                {
                    NowGame.PolicyRootNodeList.Add(NowGame.NodeToTreeView);                    
                }
                else if (NowPlayer == EnumNowPlayer.Player1 && NowGame.P2Type == QuoridorGame.Enum_PlayerType.AI)
                {
                    NowGame.PolicyRootNodeList.Add(NowGame.NodeToTreeView);                    
                }
                TestTB.Text = Hint;
                NowGame.QuoridorEva.ThisChessBoard.DrawNowChessBoard(ref Gr, ChessWhitePB, ChessBlackPB);
                ChessBoardPB.Refresh();
                #region 更新状态提示界面
                if (NowPlayer == EnumNowPlayer.Player1)
                {
                    ActionPlayerLabel.Text = "白子";
                    BlackBoardNumLB.Text = NowGame.QuoridorEva.ThisChessBoard.NumPlayer2Board.ToString();
                    WhiteBoardNumLB.Text = NowGame.QuoridorEva.ThisChessBoard.NumPlayer1Board.ToString();
                }
                if (NowPlayer == EnumNowPlayer.Player2)
                {
                    ActionPlayerLabel.Text = "黑子";
                    BlackBoardNumLB.Text = NowGame.QuoridorEva.ThisChessBoard.NumPlayer2Board.ToString();
                    WhiteBoardNumLB.Text = NowGame.QuoridorEva.ThisChessBoard.NumPlayer1Board.ToString();
                }
                #endregion
            }
        }
        /// <summary>
        /// 根据点击坐标执行下棋操作
        /// </summary>
        /// <param name="TP">点击坐标</param>
        /// <returns>行动提示字符串</returns>
        string MouseAction_PlayChess(Point TP)
        {
            if (!NowGame.HumanPolicyFinish)
            {
                NowGame.HumanPolicyAction = PlayerNowAction;
                NowGame.HumanMouseClickPoint = TP;
                NowGame.HumanPolicyFinish = true;
            }
            string Hint = "OK";

            PlaceVerticalBoardBTN.Enabled = true;
            PlaceHorizontalBoardBTN.Enabled = true;

            return Hint;
        }
        int Place_Index = 0;
        /// <summary>
        /// 根据点击坐标生成棋子初始位置
        /// </summary>
        /// <param name="TP"></param>
        void MouseAction_PlaceQueen(Point TP)
        {
            string Hint = "OK";
            # region 计算相关操作对应的操作位置的行和列           
            int col = 0, row = 0;//0~7行列

            col = Convert.ToInt16((TP.X - 14 + _FormDraw.CB_BlockWidth / 2) / _FormDraw.CB_BlockWidth) - 1;
            row = Convert.ToInt16((TP.Y - 12 + _FormDraw.CB_BlockWidth / 2) / _FormDraw.CB_BlockWidth) - 1;

            # endregion

            Console.WriteLine("row = " + row.ToString() + " col = " + col.ToString());

            if (!(row >= 0 && row <= 7 && col >= 0 && col <= 7))
            {
                Hint = "点击越界！";
            }

            if (Hint == "OK")
            {
                _FormDraw FDBuf = new _FormDraw();
                QueenChessList[Place_Index].Visible = true;
                QueenChessList[Place_Index].Location = FDBuf.GetQueenChessLocation(row, col);
                QueenChessLocation[Place_Index] = new Point(row, col);
                Place_Index++;
                if(Place_Index >= 8)
                {
                    Place_Index = 0;
                    CustomPlaceBTN.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 重启挡板跟随鼠标移动显示
        /// </summary>
        public void RestartFollow()
        {
            IfShowFollow = true;
            if (PlayerNowAction == NowAction.Action_PlaceHorizontalBoard)
                HBoardPB.Visible = true;
            else if(PlayerNowAction == NowAction.Action_PlaceVerticalBoard)
                VBoardPB.Visible = true;
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

                    IfShowFollow = false;
                    VBoardPB.Visible = false;
                    VBoardPB.Location = new Point(837, 569);
                    HBoardPB.Visible = false;
                    HBoardPB.Location = new Point(837, 569);
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)//左键执行某个行动
            {
                TP = e.Location;

                if (GameMode == GameModeStatus.SinglePlay || GameMode == GameModeStatus.DoublePlay)
                {
                    string Result = MouseAction_PlayChess(TP);
                    if (Result == "OK")
                    {
                        IfShowFollow = false;
                        VBoardPB.Visible = false;
                        VBoardPB.Location = new Point(837, 569);
                        HBoardPB.Visible = false;
                        HBoardPB.Location = new Point(837, 569);
                    }
                    else 
                    {
                        RestartFollow();
                    }
                }
                else
                {
                    Console.WriteLine(TP.X.ToString() + "," + TP.Y.ToString());
                    MouseAction_PlaceQueen(TP);
                }
            }

        }
        /// <summary>
        /// 用于步步为营AI的一些算法测试
        /// </summary>
        private void TestBTN_Click(object sender, EventArgs e)
        {
            int dis_player1 = NowQuoridor.AstarEngine.AstarRestart(NowQuoridor.ThisChessBoard, EnumNowPlayer.Player1
, NowQuoridor.ThisChessBoard.Player1Location.X, NowQuoridor.ThisChessBoard.Player1Location.Y);
            int dis_player2 = NowQuoridor.AstarEngine.AstarRestart(NowQuoridor.ThisChessBoard, EnumNowPlayer.Player2
    , NowQuoridor.ThisChessBoard.Player2Location.X, NowQuoridor.ThisChessBoard.Player2Location.Y);

            Console.WriteLine("P1的最短路程:" + dis_player1.ToString() + "步");
            Console.WriteLine("P2的最短路程:" + dis_player2.ToString() + "步");
            List<QuoridorAction> QABuff = new List<QuoridorAction>();
            //QABuff = NowQuoridor.CreateActionList(NowQuoridor.ThisChessBoard, EnumNowPlayer.Player1, dis_player1, dis_player2);
            //foreach (QuoridorAction QA in QABuff)
            //{
            //    Console.WriteLine(QA.PlayerAction.ToString() + QA.ActionPoint.ToString());
            //}
            //NowQuoridor.TestEvaluation(NowQuoridor.ThisChessBoard, dis_player1, dis_player2);
            Console.WriteLine("Stop!");
            if (IfUseViewFormDebug)
            {
                for (int i = 0; i < NowGame.PolicyRootNodeList.Count; i++)
                {
                    if (DV.treeView1.Nodes[DV.treeView1.Nodes.Count - 1].Text != "Root")
                        DV.treeView1.Nodes.Add(new TreeNode("第" + (i + 1).ToString() + "次落子:"));
                    else
                        DV.treeView1.Nodes[0] = new TreeNode("第" + 1.ToString() + "次落子:");
                    GameTreeNode.GameTreeView(NowGame.PolicyRootNodeList[i], DV.treeView1.Nodes[DV.treeView1.Nodes.Count - 1]);               
                }
                NowGame.PolicyRootNodeList.Clear();
            } 
            
        }
        bool IfShowFollow = false;
        /// <summary>
        /// 用于在Form窗体上实现挡板跟随效果
        /// </summary>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IfShowFollow)
            {
                if (PlayerNowAction == NowAction.Action_PlaceHorizontalBoard)
                {
                    HBoardPB.Location = e.Location;
                }
                else if (PlayerNowAction == NowAction.Action_PlaceVerticalBoard)
                {
                    VBoardPB.Location = e.Location;
                }
            }
        }

        private void VBoardPB_Click(object sender, EventArgs e)
        {

        }

        int delaycount = 0;//用于挡板跟随延迟计数
        const int DelayTime_Follow = 3;//挡板跟随延迟总数,可以控制挡板跟随效果的延迟
        /// <summary>
        /// 用于挡板跟随
        /// </summary>
        private void ChessBoardPB_MouseMove(object sender, MouseEventArgs e)
        {
            if (IfShowFollow)
            {
                delaycount++;

                if (delaycount >= DelayTime_Follow)
                {
                    delaycount = 0;
                    int L_X = e.Location.X;
                    int L_Y = e.Location.Y;

                    Point MovePoint = new Point(L_X - 5, L_Y + 5);

                    if (PlayerNowAction == NowAction.Action_PlaceHorizontalBoard)
                    {
                        HBoardPB.Location = MovePoint;
                    }
                    else if (PlayerNowAction == NowAction.Action_PlaceVerticalBoard)
                    {
                        VBoardPB.Location = MovePoint;
                    }
                }
            }
        }

        private void skinPictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void CustomPlaceBTN_Click(object sender, EventArgs e)
        {
            CustomPlaceBTN.Enabled = false;
            RandomPlaceBTN.Enabled = false;
        }

        public static QueenSolve ThisQueenSolve = new QueenSolve(QueenSolve.DistanceCalMethod.EuclideanDistance
                                                         ,QueenSolve.InitResultMethod.Dijkstra, 92);
        /// <summary>
        /// 显示八皇后位置（在棋盘上显示）
        /// </summary>
        /// <param name="QueenLocation">皇后位置（行列表示）</param>
        /// <param name="QueenPB">皇后位置图片列表</param>
        public void ShowQueenLocation(List<Point> QueenLocation, List<CCWin.SkinControl.SkinPictureBox> QueenPB)
        {
            _FormDraw FDBuff = new _FormDraw();
            for (int i = 0; i < QueenPB.Count; i++)
            {
                QueenPB[i].Location = FDBuff.GetQueenChessLocation(QueenLocation[i].X,QueenLocation[i].Y);
                QueenPB[i].Visible = true;
            }
        }
        /// <summary>
        /// 显示八皇后位置（在棋盘上显示）
        /// </summary>
        /// <param name="MoveSequence">移动序列</param>
        /// <param name="QueenPB">皇后位置图片列表</param>
        public void ShowQueenLocation(List<int> MoveSequence, List<CCWin.SkinControl.SkinPictureBox> QueenPB)
        {
            _FormDraw FDBuff = new _FormDraw();
            for (int i = 1; i < MoveSequence.Count; i+=2)
            {
                int index = (i - 1) / 2;
                QueenPB[index].Location = FDBuff.GetQueenChessLocation(((MoveSequence[i] / 10) % 10), MoveSequence[i] % 10);
                QueenPB[index].Visible = true;
            }
        }
        /// <summary>
        /// 隐藏皇后位置
        /// </summary>
        /// <param name="QueenPB">皇后图片列表</param>
        public void HideQueen(List<CCWin.SkinControl.SkinPictureBox> QueenPB)
        {
            foreach (CCWin.SkinControl.SkinPictureBox SPB in QueenPB)
            {
                SPB.Visible = false;
                SPB.Location = new Point(808, 348);
            }
        }
        /// <summary>
        /// 将最短路径转换成GraphpicsPath，用于绘制路径
        /// </summary>
        /// <param name="MoveSequence">最短路径序列</param>
        public GraphicsPath ChangeMinRoadToPath(List<int> MoveSequence)
        {
            GraphicsPath Pth = new GraphicsPath();
            _FormDraw FDBuff = new _FormDraw();
            //Pth.StartFigure();
            int x0 = (MoveSequence[0] / 10) % 10;
            int y0 = MoveSequence[0] % 10;
            Point X0 = FDBuff.GetQueenChessLocation(x0, y0);
            X0.X += 30;
            X0.Y += 30;
            Point XStart = new Point();
            if (X0.X < X0.Y)
            {
                XStart.X = 0;
                XStart.Y = X0.Y;
            }
            else
            {
                XStart.Y = 0;
                XStart.X = X0.X; 
            }
            Pth.AddLine(XStart, X0);
            for (int i = 0; i < MoveSequence.Count - 1; i++)
			{
                Point x1 = new Point(), x2 = new Point();
                x1 = FDBuff.GetQueenChessLocation(((MoveSequence[i] / 10) % 10), MoveSequence[i] % 10);
                x2 = FDBuff.GetQueenChessLocation(((MoveSequence[i + 1] / 10) % 10), MoveSequence[i + 1] % 10);
                x1.X += 30;
                x1.Y += 30;
                x2.X += 30;
                x2.Y += 30;
                Pth.AddLine(x1, x2);
			}
            int xe = (MoveSequence[MoveSequence.Count - 1] / 10) % 10;
            int ye = MoveSequence[MoveSequence.Count - 1] % 10;
            Point XE = FDBuff.GetQueenChessLocation(xe, ye);
            XE.X += 30;
            XE.Y += 30;
            Point XEnd = new Point();
            if (XE.X < XE.Y)
            {
                XEnd.X = 0;
                XEnd.Y = XE.Y;
            }
            else
            {
                XEnd.Y = 0;
                XEnd.X = XE.X;
            }
            Pth.AddLine(XEnd, XE);
            //Pth.CloseFigure();
            return Pth;
        }
        bool IsShowQueenFlag = true;
       /// <summary>
       /// 主要用于测试八皇后寻优
       /// </summary>
 
        private void Test2BTN_Click(object sender, EventArgs e)
        {
            ThisQueenSolve.ChessLocationList = QueenChessLocation;

            ThisQueenSolve.QueenLocationList = new List<Point>();
            for (int i = 0; i < 8; i++)
            {
                ThisQueenSolve.QueenLocationList.Add(new Point(i, ThisQueenSolve.EightQueenResult[0, i] - 1));
            }

            if (IsShowQueenFlag)
            {
                Test2BTN.Text = "取消显示";
                //ShowQueenLocation(ThisQueenSolve.QueenLocationList, QueenList);
                Queen.QueenSolve.SelectedQueenResultNum = 10;
                ///1000 0.96 64
                //ThisQueenSolve.InitSA(1000, 0.96, 64, 0, SimulateAnneal.Annealing.SAMode.SA);
                //ThisQueenSolve.InitSA(1000, 0.9, 90, 0.1, SimulateAnneal.Annealing.SAMode.FastSA);

                List<Point> BestResult_QueenLocation = new List<Point>();
                List<int> MoveSequence = new List<int>();
                double disall = 0;

                #region 算法测试
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); //  开始监视代码运行时间
                //  需要测试的代码 
                //MoveSequence = ThisQueenSolve.SearchResult_ForOverall(ref disall, ref BestResult_QueenLocation, SearchPB);
                //ThisQueenSolve.Test_SAParameter(1000, 0.9, 90, 0.1, SimulateAnneal.Annealing.SAMode.FastSA, 50);
                
                //ThisQueenSolve.Test_SAParameter(1000, 0.96, 64, 0, SimulateAnneal.Annealing.SAMode.SA, 10);

                double disbuff = 0;
                ThisQueenSolve.InitSA(1000, 0.99, 64, 0, SimulateAnneal.Annealing.SAMode.SA);
                MoveSequence = ThisQueenSolve.SearchResult_ForOverall(ref disbuff, ref BestResult_QueenLocation, SearchPB);
                
                string SendBuff = ThisQueenSolve.CreateSendCMDStr(MoveSequence);
                Console.WriteLine(SendBuff);
                DT.SendTB.Text = SendBuff;
                //ThisQueenSolve.InitSA(200, 0.9, 32, 0, SimulateAnneal.Annealing.SAMode.SA);
                //ThisQueenSolve.Test_SAParameter_Auto(ThisQueenSolve.ThisSAMode, ThisQueenSolve.ThisSA
                //    , QueenSolve.WhichSAParameter.Lenght, 2, 4, 50, 20);

                stopwatch.Stop(); //  停止监视
                TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
                double hours = timespan.TotalHours; // 总小时
                double minutes = timespan.TotalMinutes;  // 总分钟
                double seconds = timespan.TotalSeconds;  //  总秒数
                double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数

                //MoveSequence = ThisQueenSolve.CreateInitResult(ThisQueenSolve.ChessLocationList
                //                                              ,ThisQueenSolve.QueenLocationList
                //                                              ,ref disall);

                ThisQueenSolve.PrintMoveSequence(MoveSequence, ThisQueenSolve.ChessLocationList, ThisQueenSolve.QueenLocationList);
                Console.WriteLine("总路径长度为" + disbuff.ToString());
                Console.WriteLine("用时(s) ：" + seconds.ToString() + "秒");
                Console.WriteLine("用时(ms)：" + milliseconds.ToString() + "毫秒");
                
                ShowQueenLocation(MoveSequence, QueenList);
                Gr.DrawPath(new Pen(Color.Red, 6.5F), ChangeMinRoadToPath(MoveSequence));
                ChessBoardPB.Refresh();
                #endregion
            }
            else
            {
                HideQueen(QueenList);
                Test2BTN.Text = "生成移动序列";
            }
            IsShowQueenFlag = !IsShowQueenFlag;
        }

        private void Queen3PB_Click(object sender, EventArgs e)
        {

        }

        private void SearchPB_Click(object sender, EventArgs e)
        {
            
        }

        private void skinToolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
        public bool IfUseViewFormDebug = true;
        private void OpenDebugFormBTN_Click(object sender, EventArgs e)
        {
        }

        private void DepthCompareCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void skinLabel4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void IfUseTT_CopareCB_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    GameThread.Abort();
                    GameThread = new Thread(Form1.form1.QuoridorGame_Do);
                    GameThread.Start();
                    break;
                default:
                    break;
            }
        }

        private void skinTextBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConfigABP_BTN_Click(object sender, EventArgs e)
        {
            if (P1TypeSetCB.SelectedIndex == 0)//AI
                NowGame.P1Type = QuoridorGame.Enum_PlayerType.AI;
            else
                NowGame.P1Type = QuoridorGame.Enum_PlayerType.Human;
            if (P2TypeSetCB.SelectedIndex == 0)//AI
                NowGame.P2Type = QuoridorGame.Enum_PlayerType.AI;
            else
                NowGame.P2Type = QuoridorGame.Enum_PlayerType.Human;
            QuoridorDecisionSystem QDS = NowGame.P2DecisionSystem;
            if (ABPConfigCB.SelectedIndex == 0)//P1
                QDS = NowGame.P1DecisionSystem;

            int Depth = Convert.ToInt16(ABPDepthSelectCB.SelectedItem);
            bool SortNode = IfSortedCB.Checked;
            bool UseTT = IfUseTTCB.Checked;
            bool UseSituation = IsUseGameSituationCB.Checked;
            double AlphaInit = Convert.ToDouble(AlphaSetTB.Text);
            double BetaInit = Convert.ToDouble(BetaSetTB.Text);
            int GE_Depth = Convert.ToInt16(OE_DepthSelectCB.Text);
            double ScoreLimit = Convert.ToDouble(ScoreLimitSetTB.Text);
            bool UseGE = false;
            if (GE_Depth > 0)
                UseGE = true;
            QDS.ThisABPurningPara = new QuoridorDecisionSystem.ABPurningPara(Depth
                , SortNode, UseTT, AlphaInit, BetaInit, UseSituation, UseGE, GE_Depth, ScoreLimit);

            ShowParaConfigStr(NowGame);
        }

        private void StartGameBtn_Click(object sender, EventArgs e)
        {
            if (StartGameBtn.Text == "开始游戏")
            {
                if (P1TypeSetCB.SelectedIndex == 0)//AI
                    NowGame.P1Type = QuoridorGame.Enum_PlayerType.AI;
                else
                    NowGame.P1Type = QuoridorGame.Enum_PlayerType.Human;
                if (P2TypeSetCB.SelectedIndex == 0)//AI
                    NowGame.P2Type = QuoridorGame.Enum_PlayerType.AI;
                else
                    NowGame.P2Type = QuoridorGame.Enum_PlayerType.Human;
                ShowParaConfigStr(NowGame);
                # region 单独给棋盘对弈一个线程
                GameThread = new Thread(Form1.form1.QuoridorGame_Do);
                GameThread.Start();
                # endregion
                StartGameBtn.Text = "重开游戏";
            }
            else
            {
                GameThread.Abort();
                GameThread = new Thread(Form1.form1.QuoridorGame_Do);
                GameThread.Start();
                StartGameBtn.Text = "开始游戏";
            }
        }

    }
}
