using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quoridor_With_C;
using System.Drawing;

namespace Quoridor
{

    /// <summary>
    /// 棋盘格类，定义了棋盘的一个格子这个对象
    /// </summary>
    public class Grid
    {
        public enum GridInsideStatus
        {
            Have_Player1,
            Have_Player2,
            Empty
        }
        public GridInsideStatus GridStatus = GridInsideStatus.Empty;
        public bool IfUpBoard = false;
        public bool IfLeftBoard = false;

        public Grid(){}
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
        public Grid[,] ChessBoardAll = new Grid[7, 7];
        public static Color Player1ChessColor = Color.White;
        public static Color Player2ChessColor = Color.Black;

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
                        FD.DrawBoard(Gr, Form1.NowAction.Action_PlaceHorizontalBoard, i, j, Color.Red);
                    }
                    if (ChessBoardAll[i, j].IfLeftBoard)
                    {
                        FD.DrawBoard(Gr, Form1.NowAction.Action_PlaceVerticalBoard, i, j, Color.Red);
                    }
                }
            } 
        }
        public string Action(int row, int col, Form1.NowAction NA)
        {
            switch (NA)
            {
                case Form1.NowAction.Action_PlaceVerticalBoard:
                    if (col <= 0 || col >= 7 || row >= 6) return "VerticalBoardPlaceError!";
                    if (ChessBoardAll[row, col].IfLeftBoard || ChessBoardAll[row+1, col].IfLeftBoard) return "This has a VerticalBoard!";
                    ChessBoardAll[row, col].IfLeftBoard = true;
                    ChessBoardAll[row+1, col].IfLeftBoard = true;
                    return "OK";
                case Form1.NowAction.Action_PlaceHorizontalBoard:
                    if (row <= 0 || row >= 7 || col >= 6) return "HorizontalBoardPlaceError!";
                    if (ChessBoardAll[row, col].IfUpBoard || ChessBoardAll[row, col+1].IfUpBoard) return "This has a HorizontalBoard!";
                    ChessBoardAll[row, col].IfUpBoard = true;
                    ChessBoardAll[row, col+1].IfUpBoard = true;
                    return "OK";
                case Form1.NowAction.Action_Move_Player1:
                    if (ChessBoardAll[row, col].GridStatus != Grid.GridInsideStatus.Empty) return "This Not Empty";
                    //前扫一格
                    if (row >= 1  
                        && !(ChessBoardAll[row, col].IfUpBoard))//上扫
                    {
                        if (ChessBoardAll[row - 1, col].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            ChessBoardAll[row - 1, col].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                            return "OK";
                        }
                        else if (ChessBoardAll[row - 1, col].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            if (row >= 2
                                && ChessBoardAll[row - 2, col].GridStatus == Grid.GridInsideStatus.Have_Player1
                                && !(ChessBoardAll[row - 2, col].IfUpBoard))
                            {
                                ChessBoardAll[row - 2, col].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                                return "OK"; 
                            }
                        }
                    }
                    if (row <= 5
                        && !(ChessBoardAll[row + 1, col].IfUpBoard))//下扫
                    {
                        if (ChessBoardAll[row + 1, col].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            ChessBoardAll[row + 1, col].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                            return "OK";
                        }
                        else if (ChessBoardAll[row + 1, col].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            if (row <= 4
                                && ChessBoardAll[row + 2, col].GridStatus == Grid.GridInsideStatus.Have_Player1
                                && !(ChessBoardAll[row + 2, col].IfUpBoard))
                            {
                                ChessBoardAll[row + 2, col].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                                return "OK"; 
                            }
                        }
                    }
                    if (col >= 1 
                        && !(ChessBoardAll[row, col].IfLeftBoard))//左扫
                    {
                        if(ChessBoardAll[row, col - 1].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        { 
                            ChessBoardAll[row, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                            return "OK";
                        }
                        else if (ChessBoardAll[row, col - 1].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            if (col >= 2
                                && ChessBoardAll[row, col - 2].GridStatus == Grid.GridInsideStatus.Have_Player1
                                && !(ChessBoardAll[row, col - 1].IfLeftBoard))
                            {
                                ChessBoardAll[row, col - 2].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                                return "OK"; 
                            }
                        }
                    }
                    if (col <= 5 
                        && !(ChessBoardAll[row, col + 1].IfLeftBoard))//右扫
                    {
                        if (ChessBoardAll[row, col + 1].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            ChessBoardAll[row, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                            return "OK";
                        }
                        else if (ChessBoardAll[row, col + 1].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            if(col <= 4
                                && ChessBoardAll[row, col + 2].GridStatus == Grid.GridInsideStatus.Have_Player1
                                && !(ChessBoardAll[row, col + 2].IfLeftBoard))
                            {
                                ChessBoardAll[row, col + 2].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player1;
                                return "OK";
                            }
                        }
                    }

                    return "MoveError";
                case Form1.NowAction.Action_Move_Player2:
                    if (ChessBoardAll[row, col].GridStatus != Grid.GridInsideStatus.Empty) return "This Not Empty";
                    if (row >= 1  
                        && !(ChessBoardAll[row, col].IfUpBoard))//上扫
                    {
                        if (ChessBoardAll[row - 1, col].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            ChessBoardAll[row - 1, col].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                            return "OK";
                        }
                        else if (ChessBoardAll[row - 1, col].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            if (row >= 2
                                && ChessBoardAll[row - 2, col].GridStatus == Grid.GridInsideStatus.Have_Player2
                                && !(ChessBoardAll[row - 2, col].IfUpBoard))
                            {
                                ChessBoardAll[row - 2, col].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                                return "OK"; 
                            }
                        }
                    }
                    if (row <= 5
                        && !(ChessBoardAll[row + 1, col].IfUpBoard))//下扫
                    {
                        if (ChessBoardAll[row + 1, col].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            ChessBoardAll[row + 1, col].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                            return "OK";
                        }
                        else if (ChessBoardAll[row + 1, col].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            if (row <= 4
                                && ChessBoardAll[row + 2, col].GridStatus == Grid.GridInsideStatus.Have_Player2
                                && !(ChessBoardAll[row + 2, col].IfUpBoard))
                            {
                                ChessBoardAll[row + 2, col].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                                return "OK"; 
                            }
                        }
                    }
                    if (col >= 1 
                        && !(ChessBoardAll[row, col].IfLeftBoard))//左扫
                    {
                        if(ChessBoardAll[row, col - 1].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        { 
                            ChessBoardAll[row, col - 1].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                            return "OK";
                        }
                        else if (ChessBoardAll[row, col - 1].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            if (col >= 2
                                && ChessBoardAll[row, col - 2].GridStatus == Grid.GridInsideStatus.Have_Player2
                                && !(ChessBoardAll[row, col - 1].IfLeftBoard))
                            {
                                ChessBoardAll[row, col - 2].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                                return "OK"; 
                            }
                        }
                    }
                    if (col <= 5 
                        && !(ChessBoardAll[row, col + 1].IfLeftBoard))//右扫
                    {
                        if (ChessBoardAll[row, col + 1].GridStatus == Grid.GridInsideStatus.Have_Player2)
                        {
                            ChessBoardAll[row, col + 1].GridStatus = Grid.GridInsideStatus.Empty;
                            ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                            return "OK";
                        }
                        else if (ChessBoardAll[row, col + 1].GridStatus == Grid.GridInsideStatus.Have_Player1)
                        {
                            if(col <= 4
                                && ChessBoardAll[row, col + 2].GridStatus == Grid.GridInsideStatus.Have_Player2
                                && !(ChessBoardAll[row, col + 2].IfLeftBoard))
                            {
                                ChessBoardAll[row, col + 2].GridStatus = Grid.GridInsideStatus.Empty;
                                ChessBoardAll[row, col].GridStatus = Grid.GridInsideStatus.Have_Player2;
                                return "OK";
                            }
                        }
                    }
                    return "MoveError";
                case Form1.NowAction.Action_Wait:
                    return "OK";
                default:
                    break;
            }
            return "未知……";
        }
    }
    /// <summary>
    /// 步步为营游戏类
    /// </summary>
    public class QuoridorGame
    {
        public ChessBoard ThisChessBoard = new ChessBoard();
        /// <summary>
        /// 检测游戏是否结束
        /// </summary>
        /// <returns>代表游戏状态</returns>
        public string CheckResult()
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
    }
}
