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
                Point PlayerLocation = new Point();
                EnumNowPlayer CheckPlayer = EnumNowPlayer.Player1;
                if(Player == EnumNowPlayer.Player1)
                {
                    PlayerLocation.X = ThisChessBoard.Player2Location.X;
                    PlayerLocation.Y = ThisChessBoard.Player2Location.Y;
                    CheckPlayer = EnumNowPlayer.Player2;                    
                }
                else
                {
                    PlayerLocation.X = ThisChessBoard.Player1Location.X;
                    PlayerLocation.Y = ThisChessBoard.Player1Location.Y;
                    CheckPlayer = EnumNowPlayer.Player1;
                    SelfDis = RoadDis_Player2;
                    OpponentDis = RoadDis_Player1;
                }
                int distance_effect = CheckBoardEffect(Action_Once.PlayerAction
                                                        , CheckPlayer
                                                        , Action_Once.ActionPoint.X
                                                        , Action_Once.ActionPoint.Y);

                Action_Once.OpponentScore = Convert.ToDouble(distance_effect) - OpponentDis;

                distance_effect = CheckBoardEffect(Action_Once.PlayerAction
                                                        , Player
                                                        , Action_Once.ActionPoint.X
                                                        , Action_Once.ActionPoint.Y);

                Action_Once.SelfScore = Convert.ToDouble(distance_effect) - SelfDis;
            }
        }
        /// <summary>
        /// 对整个动作列表的每个动作评分
        /// </summary>
        public void ActionListEvaluation(ChessBoard ThisChessBoard, ref List<QuoridorAction> ActionList, EnumNowPlayer Player_ToEva)
        {
            int dis_player1 = 0, dis_player2 = 0;
            dis_player1 = AstarEngine.AstarRestart(ThisChessBoard,EnumNowPlayer.Player1
                        , ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y);
            dis_player2 = AstarEngine.AstarRestart(ThisChessBoard,EnumNowPlayer.Player2
                        , ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);

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
                //移动
                if (Action.PlayerAction == NowAction.Action_Move_Player1
                    || Action.PlayerAction == NowAction.Action_Move_Player2)
                {
                    if (Action.PlayerAction == NowAction.Action_Move_Player1)
                    { 
                        Action.WholeScore = Action.SelfScore + 0;
                        if (Action.ActionPoint.X >= 6)
                            Action.WholeScore = 99;
                    }
                    else
                    {
                        Action.WholeScore = Action.SelfScore + 0;
                        if (Action.ActionPoint.X <= 0)
                            Action.WholeScore = 99;
                    }

                    #region 检测是前走还是后走
                    if (Action.PlayerAction == NowAction.Action_Move_Player1)
                    {
                        if (Action.ActionPoint.X < ThisChessBoard.Player1Location.X)
                            Action.WholeScore -= 0.5;
                    }
                    else
                    {
                        if (Action.ActionPoint.X > ThisChessBoard.Player1Location.X)
                            Action.WholeScore -= 0.5;
                            
                    }
                    //Action.WholeScore += 1;
                    #endregion
                    #region 检测是左走还是右走
                    if(Action.ActionPoint.Y > ThisChessBoard.Player1Location.Y)//右走
                        Action.WholeScore -= 0.1;
                    #endregion
                }
                //放挡板
                if (Action.PlayerAction == NowAction.Action_PlaceHorizontalBoard
                || Action.PlayerAction == NowAction.Action_PlaceVerticalBoard)
                {
                    double disbuff_1 = 0, disbuff_2 = 0, disbuff_3 = 0, disbuff_4 = 0;
                    if (Player_Now == EnumNowPlayer.Player1)
                    { 
                        disbuff_1 = Action.OpponentScore - 1;
                        disbuff_2 = Action.SelfScore;
                        if (Action.PlayerAction == NowAction.Action_PlaceHorizontalBoard)
                        {
                            disbuff_3 = Math.Abs(Action.ActionPoint.X - ThisChessBoard.Player1Location.X);

                            if (Action.ActionPoint.X > ThisChessBoard.Player1Location.X)//下方
                            {
                                disbuff_3 = 6 - ThisChessBoard.Player1Location.X - disbuff_3;
                            }

                            disbuff_4 = Math.Abs(Action.ActionPoint.Y - ThisChessBoard.Player1Location.Y);
                        }
                        else
                        {
                            disbuff_3 = Math.Abs(Action.ActionPoint.Y - ThisChessBoard.Player1Location.Y);
                            disbuff_4 = Math.Abs(Action.ActionPoint.X - ThisChessBoard.Player1Location.X);
                        }


                        Action.WholeScore = disbuff_1 - disbuff_2 + disbuff_3 + disbuff_4; 
                    }
                    else
                    {
                        disbuff_1 = Action.OpponentScore - 1;
                        disbuff_2 = Action.SelfScore;
                        if (Action.PlayerAction == NowAction.Action_PlaceHorizontalBoard)
                        {
                            disbuff_3 = Math.Abs(Action.ActionPoint.X - ThisChessBoard.Player2Location.X);

                            if (Action.ActionPoint.X <= ThisChessBoard.Player2Location.X)//上方
                            {
                                disbuff_3 = ThisChessBoard.Player2Location.X - disbuff_3;
                            }

                            disbuff_4 = Math.Abs(Action.ActionPoint.Y - ThisChessBoard.Player2Location.Y);
                        }
                        else
                        {
                            disbuff_3 = Math.Abs(Action.ActionPoint.Y - ThisChessBoard.Player2Location.Y);
                            disbuff_4 = Math.Abs(Action.ActionPoint.X - ThisChessBoard.Player2Location.X);
                        }

                        Action.WholeScore = disbuff_1 - disbuff_2 +disbuff_3 + disbuff_4;
                    }
                }

                #endregion

                #region 根据评分剪枝
                //if (Action.PlayerAction == NowAction.Action_Move_Player2 || Action.PlayerAction == NowAction.Action_Move_Player1)
                //{
                //    if (Action.WholeScore <= 0)
                //    { 
                //        ActionList.Remove(ActionList[i]);
                //        continue;
                //    }
                //}
                #endregion
            }
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
        public static bool ActionListIfSort = false;
        /// <summary>
        /// 创建可选动作列表（目前只是用挡住对手的最短路径上的挡板动作为主）
        /// </summary>
        public List<QuoridorAction> CreateActionList(ChessBoard ThisChessBoard)
        {
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

            #region 创建移动Action
            NowAction MoveAction = NowAction.Action_Move_Player1;
            if(Player_Now == EnumNowPlayer.Player2)
                MoveAction = NowAction.Action_Move_Player2;

            int row = PlayerLocation.X, col = PlayerLocation.Y;

            ///检测附近的12个可能点
            if (row >= 2 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 2, col, MoveAction) == "OK")
            { 
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row - 2, col)));
                if (Player_Now == EnumNowPlayer.Player1)
                    ActionListBuff.Last().SkipChessScore = -1;
                else
                    ActionListBuff.Last().SkipChessScore = 1;
            }

            if (row >= 1 && col >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 1, col - 1, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row - 1, col - 1)));
            if (row >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 1, col, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row - 1, col)));
            if (row >= 1 && col <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row - 1, col + 1, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row - 1, col + 1)));

            if (col >= 2 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col - 2, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row, col - 2)));
            if (col >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col - 1, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row, col - 1)));
            if (col <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col + 1, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row, col + 1)));
            if (col <= 4 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row, col + 2, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row, col + 2)));

            if (row <= 5 && col >= 1 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 1, col - 1, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row + 1, col - 1)));
            if (row <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 1, col, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row + 1, col)));
            if (row <= 5 && col <= 5 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 1, col + 1, MoveAction) == "OK")
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row + 1, col)));

            if (row <= 4 && QuoridorRule.CheckMove_NoChange(ThisChessBoard, row + 2, col, MoveAction) == "OK")
            { 
                ActionListBuff.Add(new QuoridorAction(MoveAction, new Point(row + 2, col)));
                if (Player_Now == EnumNowPlayer.Player1)
                    ActionListBuff.Last().SkipChessScore = 1;
                else
                    ActionListBuff.Last().SkipChessScore = -1;
            }

            #endregion

            #region 创建放置挡板Action
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
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceHorizontalBoard
                                                            ,new Point(row_2, col_2)));
                        if(col_2 >= 1 && !ThisChessBoard.ChessBoardAll[row_2, col_2 - 1].IfUpBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceHorizontalBoard
                                                            , new Point(row_2, col_2 - 1)));
                    }
                    else//上
                    {
                        if (col_1 <= 5 && !ThisChessBoard.ChessBoardAll[row_1, col_1 + 1].IfUpBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceHorizontalBoard
                                                            , new Point(row_1, col_1)));
                        if(col_1 >= 1 && !ThisChessBoard.ChessBoardAll[row_1,col_1 - 1].IfUpBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceHorizontalBoard
                                                            , new Point(row_1, col_1 - 1)));

                    }
                }
                else if (row_1 == row_2 && col_1 != col_2)//左或右
                {
                    if (col_1 < col_2)//右
                    {
                        if(row_2 <= 5 && !ThisChessBoard.ChessBoardAll[row_2 + 1,col_2].IfLeftBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceVerticalBoard
                                                            , new Point(row_2, col_2)));
                        if(row_2 >= 1 && !ThisChessBoard.ChessBoardAll[row_2 - 1,col_2].IfLeftBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceVerticalBoard
                                                            , new Point(row_2 - 1, col_2)));
                    }
                    else//上
                    {
                        if (row_1 <= 5 && !ThisChessBoard.ChessBoardAll[row_1 + 1, col_1].IfLeftBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceVerticalBoard
                                                            , new Point(row_1, col_1)));
                        if (row_1 >= 1 && !ThisChessBoard.ChessBoardAll[row_1 - 1, col_1].IfLeftBoard)
                            ActionListBuff.Add(new QuoridorAction(
                                                            NowAction.Action_PlaceVerticalBoard
                                                            , new Point(row_1 - 1, col_1)));
                    } 
                }
            }
            #endregion
            #region 校验生成的ActionList是否合法

            foreach (QuoridorAction QA in ActionListBuff)
            {
                if (QuoridorRule.CheckBoard(ThisChessBoard, QA.PlayerAction, Player_Now,
                    QA.ActionPoint.X, QA.ActionPoint.Y) == "OK")
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
            ActionListEvaluation(ThisChessBoard, ref ActionList, Player_Now);
            #endregion
            #region 对动作列表按照评分排序
            
            if (ActionListIfSort)
            {
                //Console.WriteLine("排序前：");
                //PrintActionList(ActionList);
                //对动作列表按整体评分升序排列,因为后续是倒序遍历OrderByDescending
                ActionList = ActionList.OrderByDescending(a => a.WholeScore).ToList();
                //Console.WriteLine("排序后：");
                //PrintActionList(ActionList);
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
        public void TestEvaluation()
        {
            List<QuoridorAction> QABuff = ActionList;
            QABuff = CreateActionList(ThisChessBoard);
            ActionListEvaluation(ThisChessBoard, ref QABuff, Player_Now);
            PrintActionList(QABuff);
        }
        /// <summary>
        /// AI落子，使用贪婪算法
        /// </summary>
        /// <param name="AIPlayer"></param>
        /// <returns></returns>
        public QuoridorAction AIAction_Greedy(EnumNowPlayer AIPlayer)
        {
            List<QuoridorAction> QABuff = ActionList;

            ///暂存一些量以便恢复
            EnumNowPlayer PlayerSave = Player_Now;

            Player_Now = AIPlayer;

            QABuff = CreateActionList(ThisChessBoard);
            ActionListEvaluation(ThisChessBoard, ref QABuff, Player_Now);

            #region 贪婪思想，找最好的一步
            QuoridorAction BestAction = ActionList.First();
            double MinScore = 99;
            foreach (QuoridorAction AL in ActionList)
            {
                if (MinScore > AL.WholeScore)
                {
                    BestAction = AL;
                    MinScore = AL.WholeScore;
                }
            }
            #endregion

            Player_Now = PlayerSave;
            return BestAction;
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

}
