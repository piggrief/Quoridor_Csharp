using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorEva;
using System.Drawing;
using Quoridor_With_C;
using LookupRoad;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;

namespace QuoridorRule
{
    /// <summary>
    /// 棋盘格类，定义了棋盘的一个格子这个对象
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// 棋盘格占用状态
        /// </summary>
        public enum GridInsideStatus
        {
            Have_Player1,
            Have_Player2,
            Empty
        }
        public GridInsideStatus GridStatus = GridInsideStatus.Empty;
        public bool IfUpBoard = false;
        public bool IfLeftBoard = false;

        public Grid() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Gs">格子状态</param>
        /// <param name="UpBoard">是否有上挡板</param>
        /// <param name="DownBoard">是否有下挡板</param>
        /// <param name="LeftBoard">是否有左挡板</param>
        /// <param name="RightBoard">是否有右挡板</param>
        public Grid(GridInsideStatus Gs, bool UpBoard, bool LeftBoard)
        {
            GridStatus = Gs;
            IfUpBoard = UpBoard;
            IfLeftBoard = LeftBoard;
        }
    }
    /// <summary>
    /// 棋盘类
    /// </summary>
    public class ChessBoard
    {
        public Grid[,] ChessBoardAll = new Grid[7, 7];//当前7x7的棋盘
        public Int64 VerticalBoardHashCode = 0;//竖挡板地图哈希值
        public Int64 HorizontalBoardHashCode = 0;//横挡板地图哈希值
        public static Color Player1ChessColor = Color.White;//玩家1棋子颜色
        public static Color Player2ChessColor = Color.Black;//玩家2棋子颜色
        public Point Player1Location = new Point(0, 3);//玩家1位置
        public Point Player2Location = new Point(6, 3);//玩家2位置
        public int NumPlayer1Board = 16;//玩家1剩余挡板数
        public int NumPlayer2Board = 16;//玩家2剩余挡板数

        public ChessBoard()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    ChessBoardAll[i, j] = new Grid();
                }
            }
            ChessBoardAll[0, 3].GridStatus = Grid.GridInsideStatus.Have_Player1;
            ChessBoardAll[6, 3].GridStatus = Grid.GridInsideStatus.Have_Player2;

        }
        /// <summary>
        /// 根据棋盘类的棋盘数组画整个棋盘
        /// </summary>
        /// <param name="Gr"></param>
        public void DrawNowChessBoard(ref Graphics Gr, CCWin.SkinControl.SkinPictureBox ChessWhite, CCWin.SkinControl.SkinPictureBox ChessBlack)
        {
            Gr.Clear(Color.Transparent);
            Gr.DrawImage(Resource1.qipan, 0, 0, Resource1.qipan2019.Width, Resource1.qipan2019.Height);
            Form1._FormDraw FD = new Form1._FormDraw();
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (ChessBoardAll[i, j].GridStatus == Grid.GridInsideStatus.Have_Player1)
                    {
                        ChessWhite.Location = FD.DrawChess(Gr, i, j, Player1ChessColor);
                    }
                    else if (ChessBoardAll[i, j].GridStatus == Grid.GridInsideStatus.Have_Player2)
                    {
                        ChessBlack.Location = FD.DrawChess(Gr, i, j, Player2ChessColor);
                    }
                    if (ChessBoardAll[i, j].IfUpBoard)
                    {
                        FD.DrawBoard(Gr, NowAction.Action_PlaceHorizontalBoard, i, j, Color.Red);
                    }
                    if (ChessBoardAll[i, j].IfLeftBoard)
                    {
                        FD.DrawBoard(Gr, NowAction.Action_PlaceVerticalBoard, i, j, Color.Red);
                    }
                }
            }
        }
        /// <summary>
        /// 保存当前棋盘,将ChessBoardNow暂存到ChessBoardSave中
        /// </summary>
        public static void SaveChessBoard(ref ChessBoard ChessBoardSave, ChessBoard ChessBoardNow)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    ChessBoardSave.ChessBoardAll[i, j].IfLeftBoard = ChessBoardNow.ChessBoardAll[i, j].IfLeftBoard;
                    ChessBoardSave.ChessBoardAll[i, j].IfUpBoard = ChessBoardNow.ChessBoardAll[i, j].IfUpBoard;
                    ChessBoardSave.ChessBoardAll[i, j].GridStatus = ChessBoardNow.ChessBoardAll[i, j].GridStatus;
                }
            }
            ChessBoardSave.Player1Location.X = ChessBoardNow.Player1Location.X;
            ChessBoardSave.Player1Location.Y = ChessBoardNow.Player1Location.Y;
            ChessBoardSave.Player2Location.X = ChessBoardNow.Player2Location.X;
            ChessBoardSave.Player2Location.Y = ChessBoardNow.Player2Location.Y;
            ChessBoardSave.NumPlayer1Board = ChessBoardNow.NumPlayer1Board;
            ChessBoardSave.NumPlayer2Board = ChessBoardNow.NumPlayer2Board;
            ChessBoardSave.HorizontalBoardHashCode = ChessBoardNow.HorizontalBoardHashCode;
            ChessBoardSave.VerticalBoardHashCode = ChessBoardNow.VerticalBoardHashCode;
        }
        /// <summary>
        /// 从ChessBoardSave中恢复保存的棋盘至ChessBoard_Resumed
        /// </summary>        
        public static void ResumeChessBoard(ref ChessBoard ChessBoard_Resumed, ChessBoard ChessBoardSave)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    ChessBoard_Resumed.ChessBoardAll[i, j].IfLeftBoard = ChessBoardSave.ChessBoardAll[i, j].IfLeftBoard;
                    ChessBoard_Resumed.ChessBoardAll[i, j].IfUpBoard = ChessBoardSave.ChessBoardAll[i, j].IfUpBoard;
                    ChessBoard_Resumed.ChessBoardAll[i, j].GridStatus = ChessBoardSave.ChessBoardAll[i, j].GridStatus;
                }
            }
            ChessBoard_Resumed.Player1Location.X = ChessBoardSave.Player1Location.X;
            ChessBoard_Resumed.Player1Location.Y = ChessBoardSave.Player1Location.Y;
            ChessBoard_Resumed.Player2Location.X = ChessBoardSave.Player2Location.X;
            ChessBoard_Resumed.Player2Location.Y = ChessBoardSave.Player2Location.Y;
            ChessBoard_Resumed.NumPlayer1Board = ChessBoardSave.NumPlayer1Board;
            ChessBoard_Resumed.NumPlayer2Board = ChessBoardSave.NumPlayer2Board;
            ChessBoard_Resumed.HorizontalBoardHashCode = ChessBoardSave.HorizontalBoardHashCode;
            ChessBoard_Resumed.VerticalBoardHashCode = ChessBoardSave.VerticalBoardHashCode;
        }
    }
    /// <summary>
    /// Quoridor规则引擎类
    /// </summary>
    public class QuoridorRuleEngine
    {
        public enum EnumNowPlayer
        {
            Player1,
            Player2
        }
        public enum NowAction
        {
            Action_PlaceVerticalBoard,
            Action_PlaceHorizontalBoard,
            Action_Move_Player1,
            Action_Move_Player2,
            Action_Wait
        }

        public LookupRoadAlgorithm AstarEngine = new LookupRoadAlgorithm();
        /// <summary>
        /// 检测能否执行移动，No Change代表检测成功后不会执行这次移动，不会改变棋盘ChessBoard_ToCheck
        /// </summary>
        /// <param name="ChessBoard_ToCheck">待检测的棋盘</param>
        /// <param name="row">移动的行</param>
        /// <param name="col">移动的列</param>
        /// <param name="NA">移动类型</param>
        /// <returns></returns>
        public string CheckMove_NoChange(ChessBoard ChessBoard_ToCheck, int row, int col, NowAction NA)
        {
            Grid[,] ChessBoardAll = ChessBoard_ToCheck.ChessBoardAll;

            if (ChessBoardAll[row, col].GridStatus != Grid.GridInsideStatus.Empty) return "This Not Empty";

            Grid.GridInsideStatus ActionPlayer = Grid.GridInsideStatus.Empty;
            Grid.GridInsideStatus AnotherPlayer = Grid.GridInsideStatus.Empty;

            if (NA != NowAction.Action_Move_Player1
                && NA != NowAction.Action_Move_Player2)
                return "Error";

            if (NA == NowAction.Action_Move_Player1)
            {
                ActionPlayer = Grid.GridInsideStatus.Have_Player1;
                AnotherPlayer = Grid.GridInsideStatus.Have_Player2;
            }
            else
            {
                ActionPlayer = Grid.GridInsideStatus.Have_Player2;
                AnotherPlayer = Grid.GridInsideStatus.Have_Player1;
            }

            //前扫一格
            if (row >= 1
                && !(ChessBoardAll[row, col].IfUpBoard))//上扫
            {
                if (ChessBoardAll[row - 1, col].GridStatus == ActionPlayer)
                {
                    return "OK";
                }
                else if (ChessBoardAll[row - 1, col].GridStatus == AnotherPlayer)
                {
                    if (col >= 1
                        && ChessBoardAll[row - 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row - 1, col].IfLeftBoard))//左扫
                    {
                        return "OK";
                    }
                    if (col <= 5
                        && ChessBoardAll[row - 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row - 1, col + 1].IfLeftBoard))//右扫
                    {
                        return "OK";
                    }
                    if (row >= 2
                        && ChessBoardAll[row - 2, col].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row - 1, col].IfUpBoard))//上扫
                    {
                        return "OK";
                    }
                }
            }
            if (row <= 5
                && !(ChessBoardAll[row + 1, col].IfUpBoard))//下扫
            {
                if (ChessBoardAll[row + 1, col].GridStatus == ActionPlayer)
                {
                    return "OK";
                }
                else if (ChessBoardAll[row + 1, col].GridStatus == AnotherPlayer)
                {
                    if (col >= 1
                        && ChessBoardAll[row + 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col].IfLeftBoard))//左扫
                    {
                        return "OK";
                    }
                    if (col <= 5
                        && ChessBoardAll[row + 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col + 1].IfLeftBoard))//右扫
                    {
                        return "OK";
                    }

                    if (row <= 4
                        && ChessBoardAll[row + 2, col].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 2, col].IfUpBoard))//下扫
                    {
                        return "OK";
                    }
                }
            }
            if (col >= 1
                && !(ChessBoardAll[row, col].IfLeftBoard))//左扫
            {
                if (ChessBoardAll[row, col - 1].GridStatus == ActionPlayer)
                {
                    return "OK";
                }
                else if (ChessBoardAll[row, col - 1].GridStatus == AnotherPlayer)
                {
                    if (col >= 2
                        && ChessBoardAll[row, col - 2].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col - 1].IfLeftBoard))//左扫
                    {
                        return "OK";
                    }
                    if (row <= 5
                        && ChessBoardAll[row + 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col - 1].IfUpBoard))//下扫
                    {
                        return "OK";
                    }
                    if (row >= 1
                        && ChessBoardAll[row - 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col - 1].IfUpBoard))//上扫
                    {
                        return "OK";
                    }
                }
            }
            if (col <= 5
                && !(ChessBoardAll[row, col + 1].IfLeftBoard))//右扫
            {
                if (ChessBoardAll[row, col + 1].GridStatus == ActionPlayer)
                {
                    return "OK";
                }
                else if (ChessBoardAll[row, col + 1].GridStatus == AnotherPlayer)
                {
                    if (col <= 4
                        && ChessBoardAll[row, col + 2].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col + 2].IfLeftBoard))//右扫
                    {
                        return "OK";
                    }
                    if (row <= 5
                        && ChessBoardAll[row + 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col + 1].IfUpBoard))//下扫
                    {
                        return "OK";
                    }
                    if (row >= 1
                        && ChessBoardAll[row - 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col + 1].IfUpBoard))//上扫
                    {
                        return "OK";
                    }
                }
            }

            return "MoveError";
        }
        /// <summary>
        /// 改变P1或P2棋子位置
        /// </summary>
        /// <param name="ChessBoard_ToCheck">待检测的棋盘</param>
        /// <param name="row">移动的行</param>
        /// <param name="col">移动的列</param>
        /// <param name="NA">移动类型</param>
        public void ChangeP1P2Location(ref ChessBoard ChessBoard_ToCheck, int row, int col, NowAction NA)
        {
            if (NA == NowAction.Action_Move_Player1)
                ChessBoard_ToCheck.Player1Location = new Point(row, col);
            else
                ChessBoard_ToCheck.Player2Location = new Point(row, col); 
        }
        /// <summary>
        /// 检测能否执行移动，Change代表检测成功后会执行这次移动，改变棋盘ChessBoard_ToCheck
        /// </summary>
        /// <param name="ChessBoard_ToCheck">待检测的棋盘</param>
        /// <param name="row">移动的行</param>
        /// <param name="col">移动的列</param>
        /// <param name="NA">移动类型</param>
        /// <returns></returns>
        public string CheckMove_Change(ref ChessBoard ChessBoard_ToCheck, int row, int col, NowAction NA)
        {
            Grid[,] ChessBoardAll = ChessBoard_ToCheck.ChessBoardAll;
            if (ChessBoard_ToCheck.ChessBoardAll[row, col].GridStatus != Grid.GridInsideStatus.Empty) return "This Not Empty";

            Grid.GridInsideStatus ActionPlayer = Grid.GridInsideStatus.Empty;
            Grid.GridInsideStatus AnotherPlayer = Grid.GridInsideStatus.Empty;

            if (NA != NowAction.Action_Move_Player1
                && NA != NowAction.Action_Move_Player2)
                return "Error";

            if (NA == NowAction.Action_Move_Player1)
            {
                ActionPlayer = Grid.GridInsideStatus.Have_Player1;
                AnotherPlayer = Grid.GridInsideStatus.Have_Player2;
            }
            else
            {
                ActionPlayer = Grid.GridInsideStatus.Have_Player2;
                AnotherPlayer = Grid.GridInsideStatus.Have_Player1;
            }

            //前扫一格
            if (row >= 1
                && !(ChessBoardAll[row, col].IfUpBoard))//上扫
            {
                if (ChessBoardAll[row - 1, col].GridStatus == ActionPlayer)
                {
                    ChessBoardAll[row - 1, col].GridStatus = Grid.GridInsideStatus.Empty;
                    ChessBoardAll[row, col].GridStatus = ActionPlayer;
                    ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                    return "OK";
                }
                else if (ChessBoardAll[row - 1, col].GridStatus == AnotherPlayer)
                {
                    if (col >= 1
                        && ChessBoardAll[row - 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row - 1, col].IfLeftBoard))//左扫
                    {
                        ChessBoardAll[row - 1, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (col <= 5
                        && ChessBoardAll[row - 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row - 1, col + 1].IfLeftBoard))//右扫
                    {
                        ChessBoardAll[row - 1, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (row >= 2
                        && ChessBoardAll[row - 2, col].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row - 1, col].IfUpBoard))//上扫
                    {
                        ChessBoardAll[row - 2, col].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                }
            }

            if (row <= 5
                && !(ChessBoardAll[row + 1, col].IfUpBoard))//下扫
            {
                if (ChessBoardAll[row + 1, col].GridStatus == ActionPlayer)
                {
                    ChessBoardAll[row + 1, col].GridStatus = Grid.GridInsideStatus.Empty;
                    ChessBoardAll[row, col].GridStatus = ActionPlayer;
                    ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                    return "OK";
                }
                else if (ChessBoardAll[row + 1, col].GridStatus == AnotherPlayer)
                {
                    if (col >= 1
                        && ChessBoardAll[row + 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col].IfLeftBoard))//左扫
                    {
                        ChessBoardAll[row + 1, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (col <= 5
                        && ChessBoardAll[row + 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col + 1].IfLeftBoard))//右扫
                    {
                        ChessBoardAll[row + 1, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }

                    if (row <= 4
                        && ChessBoardAll[row + 2, col].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 2, col].IfUpBoard))//下扫
                    {
                        ChessBoardAll[row + 2, col].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                }
            }
            if (col >= 1
                && !(ChessBoardAll[row, col].IfLeftBoard))//左扫
            {
                if (ChessBoardAll[row, col - 1].GridStatus == ActionPlayer)
                {
                    ChessBoardAll[row, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                    ChessBoardAll[row, col].GridStatus = ActionPlayer;
                    ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                    return "OK";
                }
                else if (ChessBoardAll[row, col - 1].GridStatus == AnotherPlayer)
                {
                    if (col >= 2
                        && ChessBoardAll[row, col - 2].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col - 1].IfLeftBoard))//左扫
                    {
                        ChessBoardAll[row, col - 2].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (row <= 5
                        && ChessBoardAll[row + 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col - 1].IfUpBoard))//下扫
                    {
                        ChessBoardAll[row + 1, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (row >= 1
                        && ChessBoardAll[row - 1, col - 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col - 1].IfUpBoard))//上扫
                    {
                        ChessBoardAll[row - 1, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                }
            }
            if (col <= 5
                && !(ChessBoardAll[row, col + 1].IfLeftBoard))//右扫
            {
                if (ChessBoardAll[row, col + 1].GridStatus == ActionPlayer)
                {
                    ChessBoardAll[row, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                    ChessBoardAll[row, col].GridStatus = ActionPlayer;
                    ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                    return "OK";
                }
                else if (ChessBoardAll[row, col + 1].GridStatus == AnotherPlayer)
                {
                    if (col <= 4
                        && ChessBoardAll[row, col + 2].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col + 2].IfLeftBoard))//右扫
                    {
                        ChessBoardAll[row, col + 2].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (row <= 5
                        && ChessBoardAll[row + 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row + 1, col + 1].IfUpBoard))//下扫
                    {
                        ChessBoardAll[row + 1, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                    if (row >= 1
                        && ChessBoardAll[row - 1, col + 1].GridStatus == ActionPlayer
                        && !(ChessBoardAll[row, col + 1].IfUpBoard))//上扫
                    {
                        ChessBoardAll[row - 1, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                        ChessBoardAll[row, col].GridStatus = ActionPlayer;
                        ChangeP1P2Location(ref ChessBoard_ToCheck, row, col, NA);
                        return "OK";
                    }
                }
            }

            return "MoveError";
        }
        /// <summary>
        /// 检测挡板结果类
        /// </summary>
        public class CheckBoardResult
        {
            public string HintStr = "";//错误提示符，能被放下就会返回“OK”
            public int P1Distance = -1;
            public int P2Distance = -1;
        }
        /// <summary>
        /// 检测该挡板是否能被放下
        /// </summary>
        /// <param name="WhichBoard">放置哪种挡板</param>
        /// <param name="Player">检测哪个玩家会被堵死</param>
        /// <param name="Location_row">玩家的位置行</param>
        /// <param name="Location_col">玩家的位置列</param>
        /// <returns>检测挡板结果</returns>
        public CheckBoardResult CheckBoard(ChessBoard ChessBoard_ToCheck, NowAction WhichBoard, EnumNowPlayer Player, int Location_row, int Location_col)
        {
            CheckBoardResult Result = new CheckBoardResult();
            ChessBoard ThisChessBoard = ChessBoard_ToCheck;
            //if (WhichBoard == NowAction.Action_Move_Player1 || WhichBoard == NowAction.Action_Move_Player2)
            //{
            //    Result.HintStr = "OK";
            //    return Result; 
            //}
            if (WhichBoard != NowAction.Action_Move_Player1 && WhichBoard != NowAction.Action_Move_Player2)
            { 
                if (Player == EnumNowPlayer.Player1 && ChessBoard_ToCheck.NumPlayer1Board <= 0)
                {
                    Result.HintStr = "Player1 No Board";
                    return Result; 
                }
                else if (Player == EnumNowPlayer.Player2 && ChessBoard_ToCheck.NumPlayer2Board <= 0)
                {
                    Result.HintStr = "Player2 No Board";
                    return Result; 
                }
            }
            ///为了不改变原状态而暂存原状态以便后续恢复
            ChessBoard ChessBoardBuff = new ChessBoard();
            ChessBoard.SaveChessBoard(ref ChessBoardBuff, ThisChessBoard);

            //假设能放挡板
            string Hint = Action(ref ThisChessBoard, Location_row, Location_col, WhichBoard);
            if (Hint == "OK")
            {
                int disbuff1 = 0, disbuff2 = 0;
                disbuff1 = AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player1
                        , ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y);
                disbuff2 = AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player2
                        , ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);
                Result.P1Distance = disbuff1;
                Result.P2Distance = disbuff2;
                if (disbuff1 >= 999 && disbuff2 < 999)
                {
                    Hint = "Player1 No Road!";
                }
                else if (disbuff2 >= 999 && disbuff1 < 999)
                {
                    Hint = "Player2 No Road!";
                }
                else if (disbuff1 >= 999 && disbuff2 >= 999)
                {
                    Hint = "Player1&Player2 No Road!";
                }
            }
            if (Hint != "OK")
            {
                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);
                Result.HintStr = Hint;
                return Result;
            }

            ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);
            Result.HintStr = "OK";
            return Result;

        }
        /// <summary>
        /// 检测游戏是否结束
        /// </summary>
        /// <returns>代表游戏状态</returns>
        public string CheckResult(ChessBoard ThisChessBoard)
        {
            for (int i = 0; i < 7; i++)
            {
                if (ThisChessBoard.ChessBoardAll[0, i].GridStatus == Grid.GridInsideStatus.Have_Player2)
                    return "Player2 Success!";
            }
            for (int i = 0; i < 7; i++)
            {
                if (ThisChessBoard.ChessBoardAll[6, i].GridStatus == Grid.GridInsideStatus.Have_Player1)
                    return "Player1 Success!";
            }

            return "No success";
        }

        /// <summary>
        /// 行动操作，主要是用来改变棋盘状态数组
        /// </summary>
        /// <param name="row">行动的行</param>
        /// <param name="col">行动的列</param>
        /// <param name="NA">行动操作</param>
        /// <returns>行动结果，可不可行</returns>
        public string Action(ref ChessBoard ChessBoard_ToAction, int row, int col, NowAction NA)
        {
            Grid[,] ChessBoardAll = ChessBoard_ToAction.ChessBoardAll;
            switch (NA)
            {
                case NowAction.Action_PlaceVerticalBoard:
                    if (col <= 0 || col >= 7 || row >= 6) return "VerticalBoardPlaceError!";
                    if (ChessBoardAll[row, col].IfLeftBoard || ChessBoardAll[row + 1, col].IfLeftBoard) return "This has a VerticalBoard!";
                    if (ChessBoardAll[row + 1, col].IfUpBoard && ChessBoardAll[row + 1, col - 1].IfUpBoard)
                        return "十字交叉违规！";
                    ChessBoardAll[row, col].IfLeftBoard = true;
                    ChessBoardAll[row + 1, col].IfLeftBoard = true;
                    LookupRoadAlgorithm.ResultSaveTable.RenewHashCode(
                        ref ChessBoard_ToAction.VerticalBoardHashCode,
                        ref ChessBoard_ToAction.HorizontalBoardHashCode,
                        NowAction.Action_PlaceVerticalBoard, row, col);
                    return "OK";
                case NowAction.Action_PlaceHorizontalBoard:
                    if (row <= 0 || row >= 7 || col >= 6) return "HorizontalBoardPlaceError!";
                    if (ChessBoardAll[row, col].IfUpBoard || ChessBoardAll[row, col + 1].IfUpBoard) return "This has a HorizontalBoard!";
                    if (ChessBoardAll[row, col + 1].IfLeftBoard && ChessBoardAll[row - 1, col + 1].IfLeftBoard)
                        return "十字交叉违规！";
                    ChessBoardAll[row, col].IfUpBoard = true;
                    ChessBoardAll[row, col + 1].IfUpBoard = true;
                    LookupRoadAlgorithm.ResultSaveTable.RenewHashCode(
                        ref ChessBoard_ToAction.VerticalBoardHashCode,
                        ref ChessBoard_ToAction.HorizontalBoardHashCode,
                        NowAction.Action_PlaceHorizontalBoard, row, col);
                    return "OK";
                case NowAction.Action_Move_Player1:
                    return CheckMove_Change(ref ChessBoard_ToAction, row, col, NowAction.Action_Move_Player1);
                case NowAction.Action_Move_Player2:
                    return CheckMove_Change(ref ChessBoard_ToAction, row, col, NowAction.Action_Move_Player2);
                case NowAction.Action_Wait:
                    return "OK";
                default:
                    break;
            }
            return "未知……";
        }

    }
}
