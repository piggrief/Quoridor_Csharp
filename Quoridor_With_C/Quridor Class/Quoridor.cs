using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quoridor_With_C;
using System.Drawing;
using LookupRoad;
using QuoridorRule;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;
using System.Collections;
using MathNet.Numerics.Random;
using System.IO;

namespace Quoridor
{
    /// <summary>
    /// 动作类，包含双方评分
    /// </summary>
    public class QuoridorAction
    {
        public NowAction PlayerAction = NowAction.Action_Move_Player1;
        public double SkipChessScore = 0;
        public Point ActionPoint = new Point(-1, -1);

        public double SelfScore = 50;
        public double OpponentScore = 50;
        public double WholeScore = 50;
        public QuoridorRuleEngine.CheckBoardResult ActionCheckResult = new QuoridorRuleEngine.CheckBoardResult();
        public QuoridorAction(NowAction PA, Point AP)
        {
            PlayerAction = PA;
            ActionPoint.X = AP.X;
            ActionPoint.Y = AP.Y;
        }
    }
    /// <summary>
    /// 步步为营AI类 
    /// </summary>
    public class QuoridorAI
    {
        public ChessBoard ThisChessBoard = new ChessBoard();
        public LookupRoadAlgorithm AstarEngine = new LookupRoadAlgorithm();
        public QuoridorRuleEngine QuoridorRule = new QuoridorRuleEngine();
        public List<QuoridorAction> ActionList = new List<QuoridorAction>();
        public EnumNowPlayer Player_Now = EnumNowPlayer.Player1;

        public class RunTime
        {
            public bool IfMeassureTime = false;
            public double Astar_s = 0;
            public long AstarNum = 0;
        }
        public static RunTime AIRunTime = new RunTime();
        /// <summary>
        /// 检测该挡板对该玩家有何影响
        /// </summary>
        /// <param name="WhichBoard">放置哪种挡板</param>
        /// <param name="Player">检测哪个玩家会被堵死</param>
        /// <param name="Location_row">玩家的位置行</param>
        /// <param name="Location_col">玩家的位置列</param>
        /// <returns>影响了该玩家多少步数</returns>
        public int CheckBoardEffect(NowAction WhichBoard, EnumNowPlayer Player, int Location_row, int Location_col)
        {
            if (WhichBoard == NowAction.Action_Move_Player1 || WhichBoard == NowAction.Action_Move_Player2)
                return 993;
            //if (Player == EnumNowPlayer.Player1 && QuoridorRuleEngine.NumPlayer1Board <= 0)
            //    return 993;
            //else if (Player == EnumNowPlayer.Player2 && QuoridorRuleEngine.NumPlayer2Board <= 0)
            //    return 993;

            ///为了不改变原状态而暂存原状态以便后续恢复
            ChessBoard ChessBoardBuff = new ChessBoard();
            ChessBoard.SaveChessBoard(ref ChessBoardBuff, ThisChessBoard);

            //假设能放挡板
            string Hint = QuoridorRule.Action(ref ThisChessBoard, Location_row, Location_col, WhichBoard);
            if (Hint != "OK")
            {
                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);
                return 995;
            }

            int disbuff = 0;
            List<LookupRoadAlgorithm.AstarList> InitAList = new List<LookupRoadAlgorithm.AstarList>();
            AstarEngine.Astar_Stop = false;
            AstarEngine.Min_DistanceLength = 0;
            if (Player == EnumNowPlayer.Player1)
            {
                //disbuff = LookupRoad_Greedy(Player
                //    , ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y
                //    , Moved);
                LookupRoadAlgorithm.AstarList InitGrid = new LookupRoadAlgorithm.AstarList(6, 0, 6, ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y);

                InitAList.Add(InitGrid);
                disbuff = AstarEngine.LookupRoad_Astar(ThisChessBoard,EnumNowPlayer.Player1
                    , InitGrid
                    , 1, new List<LookupRoadAlgorithm.AstarList>()
                    , InitAList);
            }
            else
            {
                //disbuff = LookupRoad_Greedy(Player
                //    , ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y
                //    , Moved); 
                LookupRoadAlgorithm.AstarList InitGrid = new LookupRoadAlgorithm.AstarList(6, 0, 6, ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);

                InitAList.Add(InitGrid);
                disbuff = AstarEngine.LookupRoad_Astar(ThisChessBoard,EnumNowPlayer.Player2
                    , InitGrid
                    , 1, new List<LookupRoadAlgorithm.AstarList>()
                    , InitAList);
            }

            ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);

            if (disbuff >= 50)
            {
                return 994;
            }
            else
            {
                return disbuff; //- dis_pre;
            }
        }

        /// <summary>
        /// 计算一个动作的评分，评分越大越好
        /// </summary>
        /// <param name="Action_Once">本次动作</param>
        /// <param name="Player">做该动作的玩家</param>
        public void ActionEvaluation(ChessBoard ThisChessBoard, int RoadDis_Player1, int RoadDis_Player2, QuoridorAction Action_Once, EnumNowPlayer Player)
        {
            int SelfDis = RoadDis_Player1;
            int OpponentDis = RoadDis_Player2;
            
            ChessBoard ChessBoardSave = new ChessBoard();
            ChessBoard.SaveChessBoard(ref ChessBoardSave, ThisChessBoard);
            if (Action_Once.PlayerAction == NowAction.Action_Move_Player1)
            {
                QuoridorRule.Action(ref ThisChessBoard, Action_Once.ActionPoint.X, Action_Once.ActionPoint.Y, Action_Once.PlayerAction);
                Action_Once.SelfScore = RoadDis_Player1 - Convert.ToDouble(AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player1, ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y));
                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardSave);
            }
            else if (Action_Once.PlayerAction == NowAction.Action_Move_Player2)
            {
                QuoridorRule.Action(ref ThisChessBoard, Action_Once.ActionPoint.X, Action_Once.ActionPoint.Y, Action_Once.PlayerAction);
                Action_Once.SelfScore = RoadDis_Player2 - Convert.ToDouble(AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player2, ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y));
                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardSave);
            }
            else if (Action_Once.PlayerAction == NowAction.Action_PlaceHorizontalBoard
                || Action_Once.PlayerAction == NowAction.Action_PlaceVerticalBoard)
            {
                int SelfEffectDis = Action_Once.ActionCheckResult.P1Distance;
                int OpponentEffectDis = Action_Once.ActionCheckResult.P2Distance;
                Point PlayerLocation = new Point();
                if(Player == EnumNowPlayer.Player1)
                {
                    PlayerLocation.X = ThisChessBoard.Player2Location.X;
                    PlayerLocation.Y = ThisChessBoard.Player2Location.Y;
                }
                else
                {
                    PlayerLocation.X = ThisChessBoard.Player1Location.X;
                    PlayerLocation.Y = ThisChessBoard.Player1Location.Y;
                    SelfDis = RoadDis_Player2;
                    OpponentDis = RoadDis_Player1;
                    SelfEffectDis = Action_Once.ActionCheckResult.P2Distance;
                    OpponentEffectDis = Action_Once.ActionCheckResult.P1Distance;
                }

                Action_Once.OpponentScore = Convert.ToDouble(OpponentEffectDis) - OpponentDis;
                Action_Once.SelfScore = Convert.ToDouble(SelfEffectDis) - SelfDis;
                //Action_Once.OpponentScore = Convert.ToDouble(OpponentEffectDis);
                //Action_Once.SelfScore = Convert.ToDouble(SelfEffectDis);
            }
        }
        
        /// <summary>
        /// 对整个动作列表的每个动作评分
        /// </summary>
        /// <param name="ThisChessBoard">当前局面棋盘</param>
        /// <param name="ActionList">待评估的动作列表</param>
        /// <param name="Player_ToEva">执行动作的玩家</param>
        /// <param name="NowP1Dis">当前局面玩家1最短路程</param>
        /// <param name="NowP2Dis">当前局面玩家2最短路程</param>
        /// <param name="ScoreLowerLimit">评分下限(不包含)，-50以下代表不使用评分剪枝（用于算杀）</param>
        public void ActionListEvaluation(ChessBoard ThisChessBoard, ref List<QuoridorAction> ActionList, EnumNowPlayer Player_ToEva, int NowP1Dis, int NowP2Dis, int ScoreLowerLimit)
        {
            int dis_player1 = 0, dis_player2 = 0;
            dis_player1 = NowP1Dis;
            dis_player2 = NowP2Dis;
            //dis_player1 = AstarEngine.AstarRestart(ThisChessBoard,EnumNowPlayer.Player1
            //            , ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y);
            //dis_player2 = AstarEngine.AstarRestart(ThisChessBoard,EnumNowPlayer.Player2
            //            , ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);

            QuoridorAction BestAction = new QuoridorAction(NowAction.Action_Wait, new Point(0, 0));
            BestAction.WholeScore = -1000;
            for (int i = ActionList.Count - 1; i >= 0 ; i--)
            {
                QuoridorAction Action = ActionList[i];
                ActionEvaluation(ThisChessBoard, dis_player1, dis_player2, Action, Player_ToEva);

                if (Action.SelfScore >= 100 || Action.OpponentScore >= 100)
                {
                    ActionList.Remove(ActionList[i]);
                    continue;
                }
                # region 计算整体评分
                //放挡板
                if (Action.PlayerAction == NowAction.Action_PlaceHorizontalBoard
                || Action.PlayerAction == NowAction.Action_PlaceVerticalBoard)
                {
                    double disbuff_1 = 0, disbuff_2 = 0;
                    if (Player_Now == EnumNowPlayer.Player1)
                    {
                        disbuff_1 = Action.OpponentScore - 1;
                        disbuff_2 = Action.SelfScore;
                        Action.WholeScore = disbuff_1 - disbuff_2;//
                    }
                    else
                    {
                        disbuff_1 = Action.OpponentScore - 1;
                        disbuff_2 = Action.SelfScore;

                        Action.WholeScore = disbuff_1 - disbuff_2;//
                    }
                }
                else
                {
                    Action.WholeScore = Action.SelfScore - 1;
                }
                #region 随机因子
                double RandomScore = 0;
                CryptoRandomSource rnd = new CryptoRandomSource();
                RandomScore = rnd.NextDouble();
                //Action.WholeScore += RandomScore;
                #endregion
                #endregion
                #region 根据评分剪枝
                if (Action.PlayerAction == NowAction.Action_Move_Player2 || Action.PlayerAction == NowAction.Action_Move_Player1)
                {
                    if (Action.WholeScore > BestAction.WholeScore)
                    {
                        BestAction = Action;
                    }
                }
                if (ScoreLowerLimit > -50)
                {
                    if (Action.WholeScore < ScoreLowerLimit)
                    {
                        ActionList.Remove(ActionList[i]);
                    }                    
                }
                #endregion
            }
            #region 移动动作中只选最佳移动！
            for (int i = ActionList.Count - 1; i >= 0; i--)
            {
                QuoridorAction Action = ActionList[i];
                if (Action.PlayerAction == NowAction.Action_Move_Player2 || Action.PlayerAction == NowAction.Action_Move_Player1)
                {
                    ActionList.Remove(ActionList[i]);
                }
            }
            ActionList.Add(BestAction);
            #endregion
        }
        /// <summary>
        /// 动作列表按WholeScore降序排序
        /// </summary>
        /// <param name="ActionListToSort">待排序动作列表</param>
        public void SortActionList(ref List<QuoridorAction> ActionListToSort)
        {
            SortedList SL1 = new SortedList();
            int SortedIndex = 0;

            foreach (QuoridorAction QA in ActionListToSort)
            {
                SL1.Add(QA.WholeScore, SortedIndex);
                SortedIndex++;
            }
            List<QuoridorAction> SortedActionList = new List<QuoridorAction>();
            ICollection ScoreList = SL1.Keys;
            foreach (double Score in ScoreList)
            {               
                SortedActionList.Add(ActionListToSort[Convert.ToInt16(SL1[Score])]);
            }
            ActionListToSort = SortedActionList;
        }
        /// <summary>
        /// 在ActionListToAdd列表中添加所有移动序列
        /// </summary>
        /// <param name="ActionListToAdd">待添加的动作列表</param>
        /// <param name="ThisChessBoard">当前局面棋盘</param>
        /// <param name="ToCreatePlayer">添加的是哪个玩家的移动动作</param>
        /// <param name="PlayerLocation">该玩家的位置</param>
        public void AddMoveAction(ref List<QuoridorAction> ActionListToAdd, ChessBoard ThisChessBoard, EnumNowPlayer ToCreatePlayer, Point PlayerLocation)
        {
            NowAction MoveAction = NowAction.Action_Move_Player1;
            if (ToCreatePlayer == EnumNowPlayer.Player2)
                MoveAction = NowAction.Action_Move_Player2;

            int row = PlayerLocation.X, col = PlayerLocation.Y;

            ///检测附近的12个可能点
            if (row >= 2 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 2, col, MoveAction) == "OK")
            {
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row - 2, col)));
                if (Player_Now == EnumNowPlayer.Player1)
                    ActionListToAdd.Last().SkipChessScore = -1;
                else
                    ActionListToAdd.Last().SkipChessScore = 1;
            }

            if (row >= 1 && col >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 1, col - 1, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row - 1, col - 1)));
            if (row >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 1, col, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row - 1, col)));
            if (row >= 1 && col <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 1, col + 1, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row - 1, col + 1)));

            if (col >= 2 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col - 2, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row, col - 2)));
            if (col >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col - 1, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row, col - 1)));
            if (col <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col + 1, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row, col + 1)));
            if (col <= 4 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col + 2, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row, col + 2)));

            if (row <= 5 && col >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 1, col - 1, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row + 1, col - 1)));
            if (row <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 1, col, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row + 1, col)));
            if (row <= 5 && col <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 1, col + 1, MoveAction) == "OK")
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row + 1, col)));

            if (row <= 4 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 2, col, MoveAction) == "OK")
            {
                ActionListToAdd.Add(new QuoridorAction(MoveAction, new Point(row + 2, col)));
                if (Player_Now == EnumNowPlayer.Player1)
                    ActionListToAdd.Last().SkipChessScore = 1;
                else
                    ActionListToAdd.Last().SkipChessScore = -1;
            } 
        }
        /// <summary>
        /// 在ActionListToAdd列表中添加挡板动作序列，后续的几个bool可选项中只要IfAllSearch为True，后面三个就没用了
        /// </summary>
        /// <param name="ActionListToAdd">待添加的动作列表</param>
        /// <param name="ThisChessBoard">当前局面棋盘</param>
        /// <param name="ToCreatePlayer">添加的是哪个玩家的挡板动作</param>
        /// <param name="IfAllSearch">是否搜索所有挡板动作</param>
        /// <param name="IfBoardExtend">是否使用挡板延伸搜索</param>
        /// <param name="IfMinRoadSearch">是否使用最短路径搜索</param>
        /// <param name="IfNeighborSearch">是否使用领域搜索</param>
        public void AddBoardAction(ref List<QuoridorAction> ActionListToAdd, ChessBoard ThisChessBoard, EnumNowPlayer ToCreatePlayer
            , bool IfAllSearch = false, bool IfBoardExtend = true, bool IfMinRoadSearch = false, bool IfNeighborSearch = false)
        {
            if (IfAllSearch)
            {
                #region 遍历所有可能并检测
                /*横挡板扫描*/
                for (int rowindex = 1; rowindex <= 6; rowindex++)
                {
                    for (int colindex = 0; colindex <= 5; colindex++)
                    {
                        QuoridorRuleEngine.CheckBoardResult ResultBuff =
                            QuoridorRule.CheckBoard(ThisChessBoard
                            , NowAction.Action_PlaceHorizontalBoard
                            , Player_Now, rowindex, colindex);
                        string BoardHintStr = ResultBuff.HintStr;

                        if (BoardHintStr == "OK")
                        {
                            ActionListToAdd.Add(
                                new QuoridorAction(NowAction.Action_PlaceHorizontalBoard
                                    , new Point(rowindex, colindex)));
                        }
                    }
                }
                /*竖挡板扫描*/
                for (int rowindex = 0; rowindex <= 5; rowindex++)
                {
                    for (int colindex = 1; colindex <= 6; colindex++)
                    {
                        QuoridorRuleEngine.CheckBoardResult ResultBuff =
                            QuoridorRule.CheckBoard(ThisChessBoard
                            , NowAction.Action_PlaceVerticalBoard
                            , Player_Now, rowindex, colindex);
                        string BoardHintStr = ResultBuff.HintStr;

                        if (BoardHintStr == "OK")
                        {
                            ActionListToAdd.Add(
                                new QuoridorAction(NowAction.Action_PlaceVerticalBoard
                                    , new Point(rowindex, colindex)));
                        }
                    }
                }
                #endregion
            }
            else
            {
                List<int> CheckTable = new List<int>();
                int CheckNum = 0;
                if (IfMinRoadSearch)
                {
                    #region 寻路出最短路径
                    List<Point> MinRoad = new List<Point>();
                    if(Player_Now == EnumNowPlayer.Player1)
                    {
                        AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player2, ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);
                        MinRoad = AstarEngine.Player2MinRoad;
                    }
                    else
                    {
                        AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player1, ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y);

                        MinRoad = AstarEngine.Player1MinRoad;
                    }
                    #endregion
                    #region 根据最短路径生成所有挡板序列（不判断可不可行）
                    for (int i = 0; i < MinRoad.Count - 1; i++)
                    {
                        int row_1 = MinRoad[i].X, col_1 = MinRoad[i].Y;
                        int row_2 = MinRoad[i + 1].X, col_2 = MinRoad[i + 1].Y;


                        /*判断下一格在这一格的哪个方位*/
                        if (col_1 == col_2 && row_1 != row_2)//上或下
                        {
                            if (row_1 < row_2)//下
                            {
                                if (col_2 <= 5 && !ThisChessBoard.ChessBoardAll[row_2, col_2 + 1].IfUpBoard)
                                {
                                    CheckNum = 100 + 10 * row_2 + col_2;
                                    if(!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                           NowAction.Action_PlaceHorizontalBoard
                                                                           , new Point(row_2, col_2)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                                if (col_2 >= 1 && !ThisChessBoard.ChessBoardAll[row_2, col_2 - 1].IfUpBoard)
                                {
                                    CheckNum = 100 + 10 * row_2 + col_2 - 1;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                           NowAction.Action_PlaceHorizontalBoard
                                                                           , new Point(row_2, col_2 - 1)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                            }
                            else//上
                            {
                                if (col_1 <= 5 && !ThisChessBoard.ChessBoardAll[row_1, col_1 + 1].IfUpBoard)
                                {
                                    CheckNum = 100 + 10 * row_1 + col_1;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                        NowAction.Action_PlaceHorizontalBoard
                                                                        , new Point(row_1, col_1)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                                if (col_1 >= 1 && !ThisChessBoard.ChessBoardAll[row_1, col_1 - 1].IfUpBoard)
                                {
                                    CheckNum = 100 + 10 * row_1 + col_1 - 1;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                        NowAction.Action_PlaceHorizontalBoard
                                                                        , new Point(row_1, col_1 - 1)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }

                            }
                        }
                        else if (row_1 == row_2 && col_1 != col_2)//左或右
                        {
                            if (col_1 < col_2)//右
                            {
                                if (row_2 <= 5 && !ThisChessBoard.ChessBoardAll[row_2 + 1, col_2].IfLeftBoard)
                                {
                                    CheckNum = 200 + 10 * row_2 + col_2;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                        NowAction.Action_PlaceVerticalBoard
                                                                        , new Point(row_2, col_2)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                                if (row_2 >= 1 && !ThisChessBoard.ChessBoardAll[row_2 - 1, col_2].IfLeftBoard)
                                {
                                    CheckNum = 200 + 10 * (row_2 - 1) + col_2;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                           NowAction.Action_PlaceVerticalBoard
                                                                           , new Point(row_2 - 1, col_2)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                            }
                            else//上
                            {
                                if (row_1 <= 5 && !ThisChessBoard.ChessBoardAll[row_1 + 1, col_1].IfLeftBoard)
                                {
                                    CheckNum = 200 + 10 * row_1 + col_1;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                        NowAction.Action_PlaceVerticalBoard
                                                                        , new Point(row_1, col_1)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                                if (row_1 >= 1 && !ThisChessBoard.ChessBoardAll[row_1 - 1, col_1].IfLeftBoard)
                                {
                                    CheckNum = 200 + 10 * (row_1 - 1) + col_1;
                                    if (!CheckTable.Contains(CheckNum))
                                    {
                                        ActionListToAdd.Add(new QuoridorAction(
                                                                        NowAction.Action_PlaceVerticalBoard
                                                                        , new Point(row_1 - 1, col_1)));
                                        CheckTable.Add(CheckNum);
                                    }
                                }
                            } 
                        }
                    }
                    # endregion
                }
                if (IfBoardExtend)
                {
                    #region 检索所有横挡板,只检索出放下的位置
                    List<Point> HorizontalBoardList = new List<Point>();
                    for (int rowindex = 1; rowindex <= 6; rowindex++)
                    {
                        for (int colindex = 0; colindex <= 5; colindex++)
                        {
                            if (ThisChessBoard.ChessBoardAll[rowindex, colindex].IfUpBoard)
                            {
                                HorizontalBoardList.Add(new Point(rowindex, colindex));
                                colindex++;
                            }
                        }
                    }
                    foreach (Point HorizontalBoardPoint in HorizontalBoardList)
                    {
                        int BoardRow = HorizontalBoardPoint.X;
                        int BoardCol = HorizontalBoardPoint.Y;
                        #region 八块领域板延伸
                        #region 两块横档板延伸
                        if (BoardCol >= 2)//左板
                        {
                            CheckNum = 100 + 10 * BoardRow + BoardCol - 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow, BoardCol - 2)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardCol <= 3)//右板
                        {
                            CheckNum = 100 + 10 * BoardRow + BoardCol + 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow, BoardCol + 2)));
                                CheckTable.Add(CheckNum);
                            } 
                        }
                        #endregion
                        # region 六块竖挡板延伸
                        if (BoardRow >= 2 && BoardCol >= 1)//左上
                        {
                            CheckNum = 200 + 10 * (BoardRow - 2) + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow - 2, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }                            
                        }
                        if (BoardRow <= 5 && BoardCol >= 1)//左下
                        {
                            CheckNum = 200 + 10 * BoardRow + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }                             
                        }
                        if (BoardRow >= 2)//中上
                        {
                            CheckNum = 200 + 10 * (BoardRow - 2) + BoardCol + 1;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow - 2, BoardCol + 1)));
                                CheckTable.Add(CheckNum);
                            }   
                        }
                        if (BoardRow <= 5)//中下
                        {
                            CheckNum = 200 + 10 * BoardRow + BoardCol + 1;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow, BoardCol + 1)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardRow >= 2 && BoardCol <= 4)//右上
                        {
                            CheckNum = 200 + 10 * (BoardRow - 2) + BoardCol + 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow - 2, BoardCol + 2)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardRow <= 5 && BoardCol <= 4)//右下
                        {
                            CheckNum = 200 + 10 * BoardRow + BoardCol + 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow, BoardCol + 2)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        # endregion
                        #endregion
                    }
                    # endregion
                    # region 检索所有竖挡板,只检索出放下的位置
                    List<Point> VerticalBoardList = new List<Point>();
                    for (int colindex = 1; colindex <= 6; colindex++)
                    {
                        for (int rowindex = 0; rowindex <= 5; rowindex++)
                        {
                            if (ThisChessBoard.ChessBoardAll[rowindex, colindex].IfLeftBoard)
                            {
                                VerticalBoardList.Add(new Point(rowindex, colindex));
                                rowindex++;
                            }
                        }
                    }
                    foreach (Point VerticalBoardPoint in VerticalBoardList)
                    {
                        int BoardRow = VerticalBoardPoint.X;
                        int BoardCol = VerticalBoardPoint.Y;

                        # region 八块邻域板延伸
                        # region 两块竖挡板延伸
                        if (BoardRow >= 2)//上板
                        {
                            CheckNum = 200 + 10 * (BoardRow - 2) + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow - 2, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardRow <= 3)//下板
                        {
                            CheckNum = 200 + 10 * (BoardRow + 2) + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceVerticalBoard
                                    , new Point(BoardRow + 2, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        # endregion
                        # region 六块横档板延伸
                        if (BoardCol >= 2 && BoardRow >= 1)//左上
                        {
                            CheckNum = 100 + 10 * BoardRow + BoardCol - 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow, BoardCol - 2)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardCol <= 4 && BoardRow >= 1)//右上
                        {
                            CheckNum = 100 + 10 * BoardRow + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardCol >= 2)//左中
                        {
                            CheckNum = 100 + 10 * (BoardRow + 1) + BoardCol - 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow + 1, BoardCol - 2)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardCol <= 4)//右中
                        {
                            CheckNum = 100 + 10 * (BoardRow + 1) + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow + 1, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardCol >= 2 && BoardRow <= 4)//左下
                        {
                            CheckNum = 100 + 10 * (BoardRow + 2) + BoardCol - 2;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow + 2, BoardCol - 2)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        if (BoardCol <= 4 && BoardRow <= 4)//右下
                        {
                            CheckNum = 100 + 10 * (BoardRow + 2) + BoardCol;
                            if (!CheckTable.Contains(CheckNum))
                            {
                                ActionListToAdd.Add(new QuoridorAction(
                                    NowAction.Action_PlaceHorizontalBoard
                                    , new Point(BoardRow + 2, BoardCol)));
                                CheckTable.Add(CheckNum);
                            }
                        }
                        # endregion
                        # endregion
                    }
                    # endregion
                }
                if (IfNeighborSearch)
                {
                    # region 确定领域
                    int LocationRow = ThisChessBoard.Player1Location.X;
                    int LocationCol = ThisChessBoard.Player1Location.Y;
                    if (ToCreatePlayer == EnumNowPlayer.Player1)
                    {
                        LocationRow = ThisChessBoard.Player2Location.X;
                        LocationCol = ThisChessBoard.Player2Location.Y;
                    }
                    # endregion
                    # region 领域搜索挡板
                    # region 八块横档板
                    List<Point> BoardLocationList = new List<Point>();
                    BoardLocationList.Add(new Point(LocationRow - 1, LocationCol - 1));
                    BoardLocationList.Add(new Point(LocationRow - 1, LocationCol));
                    BoardLocationList.Add(new Point(LocationRow, LocationCol - 1));
                    BoardLocationList.Add(new Point(LocationRow, LocationCol));
                    BoardLocationList.Add(new Point(LocationRow + 1, LocationCol - 1));
                    BoardLocationList.Add(new Point(LocationRow + 1, LocationCol));
                    BoardLocationList.Add(new Point(LocationRow + 2, LocationCol - 1));
                    BoardLocationList.Add(new Point(LocationRow + 2, LocationCol));
                    //校验
                    foreach (Point BoardLocation in BoardLocationList)
                    {
                        if (BoardLocation.X >= 1 && BoardLocation.X <= 6)
                        {
                            if (BoardLocation.Y >= 0 && BoardLocation.Y <= 5)
                            {
                                CheckNum = 100 + 10 * BoardLocation.X + BoardLocation.Y;
                                if (!CheckTable.Contains(CheckNum))
                                {
                                    ActionListToAdd.Add(new QuoridorAction(
                                        NowAction.Action_PlaceHorizontalBoard
                                        , new Point(BoardLocation.X, BoardLocation.Y)));
                                    CheckTable.Add(CheckNum);
                                }
                            }
                        }
                    }

                    # endregion
                    # region 八块竖挡板
                    BoardLocationList = new List<Point>();
                    BoardLocationList.Add(new Point(LocationRow - 1, LocationCol - 1));
                    BoardLocationList.Add(new Point(LocationRow, LocationCol - 1));
                    BoardLocationList.Add(new Point(LocationRow - 1, LocationCol));
                    BoardLocationList.Add(new Point(LocationRow, LocationCol));
                    BoardLocationList.Add(new Point(LocationRow - 1, LocationCol + 1));
                    BoardLocationList.Add(new Point(LocationRow, LocationCol + 1));
                    BoardLocationList.Add(new Point(LocationRow - 1, LocationCol + 2));
                    BoardLocationList.Add(new Point(LocationRow, LocationCol + 2));
                    //校验
                    foreach (Point BoardLocation in BoardLocationList)
                    {
                        if (BoardLocation.X >= 0 && BoardLocation.X <= 5)
                        {
                            if (BoardLocation.Y >= 1 && BoardLocation.Y <= 6)
                            {
                                CheckNum = 200 + 10 * BoardLocation.X + BoardLocation.Y;
                                if (!CheckTable.Contains(CheckNum))
                                {
                                    ActionListToAdd.Add(new QuoridorAction(
                                        NowAction.Action_PlaceVerticalBoard
                                        , new Point(BoardLocation.X, BoardLocation.Y)));
                                    CheckTable.Add(CheckNum);
                                }
                            }
                        }
                    }
                    # endregion
                    # endregion
                }
            }
        }
        
        public static bool ActionListIfSort = false;
        /// <summary>
        /// 创建可选动作列表（目前只是用挡住对手的最短路径上的挡板动作为主）
        /// </summary>
        public List<QuoridorAction> CreateActionList(ChessBoard ThisChessBoard, EnumNowPlayer MaxPlayer, int NowP1Dis, int NowP2Dis)
        {
            EnumNowPlayer PlayerSave = Player_Now;
            List<QuoridorAction> ActionListBuff = new List<QuoridorAction>();
            ActionList = new List<QuoridorAction>();

            Point PlayerLocation = new Point();
            if(Player_Now == EnumNowPlayer.Player1)
            {
                PlayerLocation.X = ThisChessBoard.Player1Location.X;
                PlayerLocation.Y = ThisChessBoard.Player1Location.Y;
            }
            else
            {
                PlayerLocation.X = ThisChessBoard.Player2Location.X;
                PlayerLocation.Y = ThisChessBoard.Player2Location.Y;
            }
            AddMoveAction(ref ActionListBuff, ThisChessBoard, Player_Now, PlayerLocation);
            AddBoardAction(ref ActionListBuff, ThisChessBoard, Player_Now, false, false, true, false);
            #region 校验生成的ActionList是否合法
            foreach (QuoridorAction QA in ActionListBuff)
            {
                QA.ActionCheckResult =
                    QuoridorRule.CheckBoard(ThisChessBoard, QA.PlayerAction, Player_Now,
                    QA.ActionPoint.X, QA.ActionPoint.Y);

                if (QA.ActionCheckResult.HintStr == "OK")
                {
                    if (QA.PlayerAction == NowAction.Action_Move_Player1 || QA.PlayerAction == NowAction.Action_Move_Player2)
                    {
                        if (QuoridorRule.CheckMove_NoChange(ThisChessBoard, QA.ActionPoint.X, QA.ActionPoint.Y, QA.PlayerAction) == "OK")
                        {                        
                            ActionList.Add(QA);
                        }
                    }
                    else
                        ActionList.Add(QA);
                }
            }

            #endregion

            # region 评估加剪裁列表
            ActionListEvaluation(ThisChessBoard, ref ActionList, Player_Now, NowP1Dis, NowP2Dis, -50);
            #endregion
            #region 对动作列表按照评分排序
            
            if (ActionListIfSort)
            {
                if (PlayerSave == MaxPlayer)
                {
                    ActionList = ActionList.OrderBy(a => a.WholeScore).ToList();
                    //ActionList = ActionList.OrderBy(a => a.WholeScore).ToList();
                }
                else//Min玩家
                {
                    ActionList = ActionList.OrderByDescending(a => a.OpponentScore).ToList();
                    //ActionList = ActionList.OrderByDescending(a => a.WholeScore).ToList();
                }
                //对当前博弈玩家按整体评分降序排列
                //ActionList = ActionList.OrderByDescending(a => a.WholeScore).ToList();
                //对当前对手玩家按整体评分升序排列
                //ActionList = ActionList.OrderBy(a => a.WholeScore).ToList();
            }
            #endregion
            return ActionList;
        }
        /// <summary>
        /// 控制台输出动作列表的信息
        /// </summary>
        public void PrintActionList(List<QuoridorAction> ActionListBuff)
        {
            Console.WriteLine("/**********显示" + (Player_Now).ToString() + "动作评分***********/");
            foreach (QuoridorAction AL in ActionListBuff)
            {
                string actionstr = "";
                switch (AL.PlayerAction)
                {
                    case NowAction.Action_PlaceVerticalBoard:
                        Console.Write("在" + ((AL.ActionPoint.X) * 8 + AL.ActionPoint.Y + 1).ToString() + "点和" +
                            ((AL.ActionPoint.X + 1) * 8 + AL.ActionPoint.Y + 1).ToString() + "点左侧");
                        actionstr = "放置竖挡板"; break;
                    case NowAction.Action_PlaceHorizontalBoard:
                        Console.Write("在" + ((AL.ActionPoint.X) * 8 + AL.ActionPoint.Y + 1).ToString() + "点和" +
                            ((AL.ActionPoint.X) * 8 + AL.ActionPoint.Y + 1 + 1).ToString() + "点上侧");
                        actionstr = "放置横挡板"; break;
                    case NowAction.Action_Move_Player1:
                        Console.Write("在" + ((AL.ActionPoint.X) * 8 + AL.ActionPoint.Y + 1).ToString() + "点处");
                        actionstr = "移动玩家1"; break;
                    case NowAction.Action_Move_Player2:
                        Console.Write("在" + ((AL.ActionPoint.X) * 8 + AL.ActionPoint.Y + 1).ToString() + "点处");
                        actionstr = "移动玩家2"; break;
                    default:
                        actionstr = "异常"; break;
                }
                Console.WriteLine(actionstr);
                Console.Write("该动作评分为：");
                Console.WriteLine("对手：" + AL.OpponentScore.ToString() + ",自己" + AL.SelfScore.ToString() + ",总分" + AL.WholeScore.ToString());

            }
            Console.WriteLine("/****************************************/"); 
        }
        /// <summary>
        /// 测试该动作列表的评分
        /// </summary>
        public void TestEvaluation(ChessBoard ThisChessBoard, int Dis1, int Dis2)
        {
            List<QuoridorAction> QABuff = new List<QuoridorAction>();
            QABuff = CreateActionList(ThisChessBoard, EnumNowPlayer.Player1, Dis1, Dis2);
            ActionListEvaluation(ThisChessBoard, ref QABuff, EnumNowPlayer.Player1, Dis1, Dis2, -50);
            PrintActionList(QABuff);
        }
        public enum SearchLevel
        {
            MaxLevel,
            MinLevel
        }
        public Grid[,] ChessBoardBuff = new Grid[7, 7];
        public EnumNowPlayer PlayerBuff = EnumNowPlayer.Player2;
        public EnumNowPlayer ReversePlayer(EnumNowPlayer NowPlayer)
        {
            if (NowPlayer == EnumNowPlayer.Player1)
                return EnumNowPlayer.Player2;
            else
                return EnumNowPlayer.Player1;
        }
    }
    public class ChessManual
    {
        public List<int> ActionSequence = new List<int>();
        public EnumNowPlayer Winner = EnumNowPlayer.Player1;
        /// <summary>
        /// 根据ActionSequence和Winner生成棋盘保存用的棋谱字符串
        /// </summary>
        /// <returns>一局棋谱的字符串</returns>
        string CreateManual()
        {
            string ManualStr = "";
            ManualStr += DateTime.Now.ToLocalTime().ToString();
            ManualStr += ",";
            switch (Winner)
            {
                case EnumNowPlayer.Player1:
                    ManualStr += "1,";
                    break;
                case EnumNowPlayer.Player2:
                    ManualStr += "2,";
                    break;
                default:
                    break;
            }
            foreach (int Action in ActionSequence)
            {
                ManualStr += Action.ToString();
                ManualStr += ",";
            }

            ManualStr += "End";
            ManualStr += System.Environment.NewLine;

            return ManualStr;
        }
        /// <summary>
        /// 把棋谱字符串写入FilePath文件末尾
        /// </summary>
        /// <param name="FilePath"></param>
        void SaveChessManual(string FilePath)
        {
            FileStream fs = null;
            fs = File.OpenWrite(FilePath);
            
            //StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default);
            string ManualBuff = CreateManual();
            byte[] data = System.Text.Encoding.Default.GetBytes(ManualBuff);
            //设定书写的开始位置为文件的末尾  
            fs.Position = fs.Length;
            //将待写入内容追加到文件末尾  
            fs.Write(data, 0, data.Length);
            //sw.Write(ManualBuff);
        }
        /// <summary>
        /// 录入一局棋谱
        /// </summary>
        /// <param name="OnceGame_ActionSequence">该局的移动序列</param>
        public void EnterChessManual(EnumNowPlayer WinnerPlayer)
        {
            Winner = WinnerPlayer;
            string ManualPath = System.Windows.Forms.Application.StartupPath;
            ManualPath += "\\ChessManual\\CM.txt";
            try
            {
                SaveChessManual(ManualPath);
            }
            catch (Exception)
            {
                Console.WriteLine(ManualPath);
                return;
            }
            foreach (int Action in ActionSequence)
            {
                Console.WriteLine(Action.ToString()); 
            }
            
            Console.WriteLine("Enter Success!");
        }
        /// <summary>
        /// 更新动作序列
        /// </summary>
        /// <param name="NowAction"></param>
        public void RenewActionSequence(NowAction NowAction, int row, int col)
        {
            int ActionIndex = 0;
            switch (NowAction)
            {
                case NowAction.Action_PlaceVerticalBoard:
                    ActionIndex = 0;
                    break;
                case NowAction.Action_PlaceHorizontalBoard:
                    ActionIndex = 1;
                    break;
                case NowAction.Action_Move_Player1:
                    ActionIndex = 2;
                    break;
                case NowAction.Action_Move_Player2:
                    ActionIndex = 3;
                    break;
                case NowAction.Action_Wait:
                    break;
                default:
                    break;
            }
            ActionSequence.Add(ActionIndex * 100 + row * 10 + col);
        }
    }
}
