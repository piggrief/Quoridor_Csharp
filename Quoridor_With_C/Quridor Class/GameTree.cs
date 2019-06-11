using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quoridor_With_C;
using System.Drawing;
using LookupRoad;
using QuoridorRule;
using Quoridor;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;
using System.Windows.Forms;

namespace GameTree
{
    /// <summary>
    /// 博弈树节点类
    /// </summary>
    public class GameTreeNode
    {
        public NowAction NodeAction;///当前节点的动作
        public EnumNowPlayer NodePlayer;///当前节点的动作的执行玩家
        public Point ActionLocation = new Point(-1, -1);///当前节点动作的执行位置
        public List<GameTreeNode> SonNode = new List<GameTreeNode>();///子节点列表

        public int depth = 0;///该节点深度

        public double alpha = -10000;///该节点的alpha值
        public double beta = 10000;///该节点的beta值
        public double score = 10000;///该节点的评分值

        public static QuoridorAI NowQuoridor = new QuoridorAI();
        public static long NodeNum = 0;
        public GameTreeNode() { }
        /// <summary>
        /// 构造函数,用来设定该博弈树节点的信息
        /// </summary>
        public GameTreeNode(NowAction Action_set, Point ActionLocation_set, EnumNowPlayer Player_set, int depth_set, double alpha_set, double beta_set, double score_set)
        {
            NodeAction = Action_set;
            NodePlayer = Player_set;
            depth = depth_set;
            alpha = alpha_set;
            beta = beta_set;
            score = score_set;
            ActionLocation = ActionLocation_set;
        }
        public GameTreeNode(GameTreeNode ThisNode)
        {

        }
        /// <summary>
        /// 给该节点添加新的子节点
        /// </summary>
        /// <param name="NewNode">待添加的子节点</param>
        public void CreateNewSon(GameTreeNode FatherNode, GameTreeNode NewNode)
        {
            FatherNode.SonNode.Add(NewNode);
        }
        public static int DepthMax = 1000;///博弈树最大深度
        /// <summary>
        /// 以极大极小搜索框架生成博弈树
        /// </summary>
        /// <param name="ThisChessBoard">当前棋盘状态</param>
        /// <param name="ThisNode">当前博弈树节点</param>
        public void ExpandNode_MinMax(ChessBoard ThisChessBoard, GameTreeNode ThisNode)
        {
            ///暂存一些量以便恢复
            EnumNowPlayer PlayerSave = NowQuoridor.ReversePlayer(ThisNode.NodePlayer);
            NowQuoridor.Player_Now = PlayerSave;

            List<QuoridorAction> QABuff = NowQuoridor.ActionList;

            QABuff = NowQuoridor.CreateActionList(ThisChessBoard);

            if (ThisNode.depth > DepthMax)
            {
                //NowQuoridor.ActionListEvaluation(ThisChessBoard, ref QABuff, ThisNode.NodePlayer);

                if (NowQuoridor.ActionList.Count <= 0)
                {
                    NowQuoridor.Player_Now = PlayerSave;
                    double score = 999999;
                    ThisNode.CreateNewSon(ThisNode, new GameTreeNode(NowAction.Action_Wait, new Point(-1, -1)
                        , PlayerSave, ThisNode.depth + 1, score, score, score));
                    ThisNode.score = score;
                    return;
                }
                #region 贪婪思想，找最好的一步
                QuoridorAction BestAction = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));
                BestAction = QABuff.First();
                double MaxScore = -100;
                foreach (QuoridorAction AL in QABuff)
                {
                    if (MaxScore < AL.WholeScore)
                    {
                        BestAction = AL;
                        MaxScore = AL.WholeScore;
                    }
                }
                #endregion
                NowQuoridor.Player_Now = PlayerSave;
                double alphabetabuff = MaxScore;

                ThisNode.CreateNewSon(ThisNode, new GameTreeNode(BestAction.PlayerAction, BestAction.ActionPoint
                , PlayerSave, ThisNode.depth + 1, -alphabetabuff, alphabetabuff, alphabetabuff));
                ThisNode.score = alphabetabuff;
                return;
            }

            foreach (QuoridorAction QA in QABuff)
            {
                #region 保存棋盘状态
                ChessBoard ChessBoardBuff = new ChessBoard();
                ChessBoard.SaveChessBoard(ref ChessBoardBuff, ThisChessBoard);
                #endregion
                #region 模拟落子
                string Hint = NowQuoridor.QuoridorRule.Action(ref ThisChessBoard, QA.ActionPoint.X, QA.ActionPoint.Y, QA.PlayerAction);
                try
                {
                    if (Hint != "OK")
                    {
                        Exception e = new Exception();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion

                CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                    , PlayerSave, ThisNode.depth + 1, ThisNode.alpha, ThisNode.beta, ThisNode.beta));

                ExpandNode_MinMax(ThisChessBoard, ThisNode.SonNode.Last());
                #region 恢复棋盘状态
                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);
                #endregion
            }
            if (ThisNode.NodePlayer == NowQuoridor.PlayerBuff)//MIN层
            {
                double minvalue = 99999;
                foreach (GameTreeNode Son in ThisNode.SonNode)
                {
                    if (Son.score < minvalue)
                    {
                        minvalue = Son.score;
                        ThisNode.score = minvalue;
                    }
                }
            }
            else //MAX层
            {
                double maxvalue = -10000;
                foreach (GameTreeNode Son in ThisNode.SonNode)
                {
                    if (Son.score > maxvalue)
                    {
                        maxvalue = Son.score;
                        ThisNode.score = maxvalue;
                        if (ThisNode.depth == 0)//根节点层
                        {
                            ThisNode.ActionLocation = Son.ActionLocation;
                            ThisNode.NodeAction = Son.NodeAction;
                            ThisNode.NodePlayer = Son.NodePlayer;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 以Alpha-Beta剪枝框架生成博弈树
        /// </summary>
        /// <param name="ThisChessBoard">当前棋盘状态</param>
        /// <param name="ThisNode">当前博弈树节点</param>
        public void ExpandNode_ABPruning(ChessBoard ThisChessBoard, GameTreeNode ThisNode)
        {
            ///暂存一些量以便恢复

            EnumNowPlayer PlayerSave = NowQuoridor.ReversePlayer(ThisNode.NodePlayer);
            NowQuoridor.Player_Now = PlayerSave;

            List<QuoridorAction> QABuff = NowQuoridor.ActionList;

            QABuff = NowQuoridor.CreateActionList(ThisChessBoard);

            if (ThisNode.depth > DepthMax)
            {
                //NowQuoridor.ActionListEvaluation(ThisChessBoard, ref QABuff, ThisNode.NodePlayer);
                if (NowQuoridor.ActionList.Count <= 0)
                {
                    NowQuoridor.Player_Now = PlayerSave;
                    double score = 999999;
                    ThisNode.CreateNewSon(ThisNode, new GameTreeNode(NowAction.Action_Wait, new Point(-1, -1)
                        , PlayerSave, ThisNode.depth + 1, score, score, score));
                    ThisNode.score = score;
                    ThisNode.beta = score;
                    return;
                }

                //                QuoridorAction AIAction = NowQuoridor.AIAction_Greedy(PlayerSave);//
                //                double alphabetabuff = AIAction.WholeScore;
                //                ThisNode.CreateNewSon(ThisNode, new GameTreeNode(AIAction.PlayerAction, AIAction.ActionPoint
                //, PlayerSave, ThisNode.depth + 1, -alphabetabuff, alphabetabuff, alphabetabuff));
                //                ThisNode.score = alphabetabuff;


                #region 贪婪思想，找最好的一步
                QuoridorAction BestAction = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));
                BestAction = QABuff.First();
                double MaxScore = -100;
                foreach (QuoridorAction AL in QABuff)
                {
                    if (MaxScore < AL.WholeScore)
                    {
                        BestAction = AL;
                        MaxScore = AL.WholeScore;
                    }
                }
                #endregion
                NowQuoridor.Player_Now = PlayerSave;
                double alphabetabuff = MaxScore;

                ThisNode.CreateNewSon(ThisNode, new GameTreeNode(BestAction.PlayerAction, BestAction.ActionPoint
                , PlayerSave, ThisNode.depth + 1, -alphabetabuff, alphabetabuff, alphabetabuff));
                ThisNode.score = alphabetabuff;
                //ThisNode.alpha = ThisNode.SonNode.Last().alpha;
                ThisNode.beta = ThisNode.SonNode.Last().beta;
                return;
            }

            foreach (QuoridorAction QA in QABuff)
            {
                #region 保存棋盘状态
                ChessBoard ChessBoardBuff = new ChessBoard();
                ChessBoard.SaveChessBoard(ref ChessBoardBuff, ThisChessBoard);
                #endregion
                #region 模拟落子
                string Hint = NowQuoridor.QuoridorRule.Action(ref ThisChessBoard, QA.ActionPoint.X, QA.ActionPoint.Y, QA.PlayerAction);
                try
                {
                    if (Hint != "OK")
                    {
                        Exception e = new Exception();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion

                CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                    , PlayerSave, ThisNode.depth + 1, ThisNode.alpha, ThisNode.beta, ThisNode.score));

                ExpandNode_ABPruning(ThisChessBoard, ThisNode.SonNode.Last());

                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);

                if (ThisNode.NodePlayer == NowQuoridor.PlayerBuff)//MIN层
                {
                    if (ThisNode.SonNode.Last().score < ThisNode.beta)
                    {
                        ThisNode.beta = ThisNode.SonNode.Last().score;
                        ThisNode.score = ThisNode.SonNode.Last().score;
                    }

                    if (ThisNode.beta <= ThisNode.alpha)
                    {
                        //break; 
                    }
                }
                else
                {
                    if (ThisNode.SonNode.Last().score > ThisNode.alpha)
                    {
                        ThisNode.alpha = ThisNode.SonNode.Last().score;
                        ThisNode.score = ThisNode.SonNode.Last().score;
                        if (ThisNode.depth == 0)//根节点层
                        {
                            ThisNode.ActionLocation = ThisNode.SonNode.Last().ActionLocation;
                            ThisNode.NodeAction = ThisNode.SonNode.Last().NodeAction;
                            ThisNode.NodePlayer = ThisNode.SonNode.Last().NodePlayer;
                        }
                    }

                    if (ThisNode.beta <= ThisNode.alpha)
                    {
                        //break;
                    }
                }
            }
        }
        public enum Enum_GameTreeSearchFrameWork
        {
            MinMax,
            ABPurning_Normal,
            ABPurning_ScoreSort
        }
        public static Enum_GameTreeSearchFrameWork SearchFrameWork = Enum_GameTreeSearchFrameWork.ABPurning_ScoreSort;
        /// <summary>
        /// 创建一棵博弈树
        /// </summary>
        /// <param name="RootNode">待生成树的根节点</param>
        /// <param name="ChessBoard_Init">初始棋盘状态</param>
        /// <param name="DepthMax_Set">博弈树深度</param>
        /// <param name="IfShowDebugLog">是否显示调试日志，默认不显示</param>
        public static void CreateGameTree(GameTreeNode RootNode, ChessBoard ChessBoard_Init, int DepthMax_Set, bool IfShowDebugLog = false)
        {
            try
            {
                Exception E = new Exception("最大深度设定错误！请设置为偶数！");
                //if (DepthMax_Set % 2 != 1)
                //{
                //    throw E; 
                //}
            }
            catch (Exception e)
            {
                throw;
            }
            DepthMax = DepthMax_Set;

            if (SearchFrameWork == Enum_GameTreeSearchFrameWork.ABPurning_ScoreSort
                || SearchFrameWork == Enum_GameTreeSearchFrameWork.MinMax)
            {
                QuoridorAI.ActionListIfSort = true;
                RootNode.ExpandNode_ABPruning(ChessBoard_Init, RootNode);//5k数量级节点数
            }
            else
            {
                QuoridorAI.ActionListIfSort = false;
            }

            if (SearchFrameWork == Enum_GameTreeSearchFrameWork.MinMax)
            {
                RootNode.ExpandNode_MinMax(ChessBoard_Init, RootNode);//3W数量级节点数                
            }
            else if (SearchFrameWork == Enum_GameTreeSearchFrameWork.ABPurning_Normal)
            {
                RootNode.ExpandNode_ABPruning(ChessBoard_Init, RootNode);//5k数量级节点数
            }

            //double MaxScore = -1000;
            //foreach (GameTreeNode GTN in RootNode.SonNode)
            //{
            //    if (MaxScore < GTN.score)
            //    {
            //        MaxScore = GTN.score;
            //        RootNode.NodePlayer = GTN.NodePlayer;
            //        RootNode.NodeAction = GTN.NodeAction;
            //        RootNode.ActionLocation = GTN.ActionLocation;
            //        RootNode.score = MaxScore;
            //    }
            //}
            if (IfShowDebugLog)
                PrintGameTree(RootNode);
        }
        /// <summary>
        /// 计算博弈树节点总数量，用于测试剪枝性能
        /// </summary>
        /// <param name="NowNode">博弈树根节点</param>
        public static void CalGameTreeNodeNum(GameTreeNode NowNode)
        {
            if (NowNode.SonNode.Count <= 0)
            {
                return;
            }
            foreach (GameTreeNode Son in NowNode.SonNode)
            {
                NodeNum++;
                CalGameTreeNodeNum(Son);
            }
        }
        public static void GameTreeView(GameTreeNode NowNode, TreeNode NowTreeNode)
        {
            if (NowNode.SonNode.Count <= 0)//叶节点
            {
                return;
            }
            foreach (GameTreeNode Son in NowNode.SonNode)
            {
                string SonTextbuff = "D:";
                SonTextbuff += Son.depth.ToString() + " P";

                switch (Son.NodePlayer)
                {
                    case EnumNowPlayer.Player1:
                        SonTextbuff += "1";
                        break;
                    case EnumNowPlayer.Player2:
                        SonTextbuff += "2";
                        break;
                    default:
                        SonTextbuff += "Error";
                        break;
                }
                switch (Son.NodeAction)
                {
                    case NowAction.Action_PlaceVerticalBoard:
                        SonTextbuff += "PV";
                        break;
                    case NowAction.Action_PlaceHorizontalBoard:
                        SonTextbuff += "PH";
                        break;
                    case NowAction.Action_Move_Player1:
                        SonTextbuff += "M1";
                        break;
                    case NowAction.Action_Move_Player2:
                        SonTextbuff += "M2";
                        break;
                    case NowAction.Action_Wait:
                        SonTextbuff += "Error";
                        break;
                    default:
                        SonTextbuff += "Error";
                        break;
                }

                SonTextbuff += " A:";
                SonTextbuff += Son.alpha.ToString();
                SonTextbuff += ",B:";
                SonTextbuff += Son.beta.ToString();
                SonTextbuff += ",S:";
                SonTextbuff += Son.score.ToString();

                TreeNode SonTreeNode = new TreeNode(SonTextbuff);

                NowTreeNode.Nodes.Add(SonTreeNode);
                GameTreeView(Son, SonTreeNode);
            } 
        }
        /// <summary>
        /// 控制台输出博弈树调试日志（向下遍历）
        /// </summary>
        /// <param name="NowNode"></param>
        public static void PrintGameTree(GameTreeNode NowNode)
        {
            if (NowNode.SonNode.Count <= 0)
            {
                //Console.Write(("第" + NowNode.depth.ToString() + "层 "));
                //Console.Write(NowNode.NodePlayer.ToString());
                //Console.Write((" a = " + NowNode.alpha.ToString()));
                //Console.Write((",b = " + NowNode.beta.ToString()));
                //Console.Write("动作：");
                //Console.Write(NowNode.NodeAction.ToString());
                //Console.Write(("位置：" + NowNode.ActionLocation.ToString()));

                //Console.WriteLine();
                return;
            }
            foreach (GameTreeNode Son in NowNode.SonNode)
            {
                PrintGameTree(Son);
            }
            if (NowNode.depth >= 0 && NowNode.depth <= DepthMax + 1)
            {
                Console.Write(("第" + NowNode.depth.ToString() + "层 "));
                Console.Write(NowNode.NodePlayer.ToString());
                Console.Write((" a = " + NowNode.alpha.ToString()));
                Console.Write((",b = " + NowNode.beta.ToString()));
                Console.Write((",s = " + NowNode.score.ToString()));
                Console.Write("动作：");
                Console.Write(NowNode.NodeAction.ToString());
                if (NowNode.NodeAction == NowAction.Action_PlaceVerticalBoard)
                {
                    Console.Write("位置：" + ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点和" +
                        ((NowNode.ActionLocation.X + 1) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点左侧");
                }
                else if (NowNode.NodeAction == NowAction.Action_PlaceHorizontalBoard)
                {
                    Console.Write("位置：" + ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点和" +
                        ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1 + 1).ToString() + "点上侧");
                }
                else if (NowNode.NodeAction == NowAction.Action_Move_Player1 || NowNode.NodeAction == NowAction.Action_Move_Player2)
                {
                    Console.Write("位置：" + ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点");
                }

                Console.WriteLine();
            }
        }

    }
}
