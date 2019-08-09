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
using MathNet.Numerics.Random;
using System.Collections;
using System.Collections.Generic;


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

        public static TranslationTable NodeTranslationTable = new TranslationTable();
        public static long InitChessBoardHashCode = 0;
        public static int RootDepth = 1;///根节点的深度，在一步一步落子后需要+2更新
        public static QuoridorAI NowQuoridor = new QuoridorAI();
        public static long NodeNum = 0;
        public static EnumNowPlayer GameTreePlayer = EnumNowPlayer.Player2;
        public static void InitTranslationTable()
        {
            ChessBoard InitCB = new ChessBoard();

            InitChessBoardHashCode = NodeTranslationTable.ZobristList[0, 0, 3];
            InitChessBoardHashCode = InitChessBoardHashCode ^ NodeTranslationTable.ZobristList[1, 6, 3];

            TranslationTable.GameTreeNodeForHash NodeBuff = new TranslationTable.GameTreeNodeForHash();

            //NodeTranslationTable.ChessBoardTT.Add(InitChessBoardHashCode, NodeBuff);
        }
        public GameTreeNode() 
        {
            NodeHashCode = InitChessBoardHashCode;
            //InitTranslationTable();
        }
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
            InitTranslationTable();
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

            QABuff = NowQuoridor.CreateActionList(ThisChessBoard, GameTreePlayer);

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

                if (ThisNode.depth <= DepthMax)
                {
                    CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                        , PlayerSave, ThisNode.depth + 1, ThisNode.alpha, ThisNode.beta, ThisNode.beta));

                    ExpandNode_MinMax(ThisChessBoard, ThisNode.SonNode.Last());
                }
                else
                {
                    CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                        , PlayerSave, ThisNode.depth + 1, QA.WholeScore, QA.WholeScore, QA.WholeScore)); 
                }
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

        public long NodeHashCode = 0;
        public static bool IfUseTanslationTable = true;
        /// <summary>
        /// 以Alpha-Beta剪枝框架并使用TranslationTable生成博弈树
        /// </summary>
        /// <param name="ThisChessBoard">当前棋盘状态</param>
        /// <param name="ThisNode">当前博弈树节点</param>
        public void ExpandNode_ABPruning(ChessBoard ThisChessBoard, GameTreeNode ThisNode, bool IfUseTT = true)
        {
            if (IfUseTT)
            {
                bool IfInTT = false;
                TranslationTable.GameTreeNodeForHash HashNode1 = new TranslationTable.GameTreeNodeForHash();
                HashNode1 = NodeTranslationTable.Search(ThisNode.NodeHashCode, ref IfInTT);
                if (ThisNode.depth != 0 && IfInTT && ThisNode.depth + RootDepth <= HashNode1.depth)
                {
                    ThisNode.alpha = HashNode1.alpha;
                    ThisNode.beta = HashNode1.beta;
                    return;
                }
            }
            ///暂存一些量以便恢复
            EnumNowPlayer PlayerSave = NowQuoridor.ReversePlayer(ThisNode.NodePlayer);
            NowQuoridor.Player_Now = PlayerSave;

            List<QuoridorAction> QABuff = NowQuoridor.ActionList;

            //QABuff = NowQuoridor.CreateActionList_ALL(ThisChessBoard);
            QABuff = NowQuoridor.CreateActionList(ThisChessBoard, GameTreePlayer);

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
                if (QA.PlayerAction == NowAction.Action_PlaceHorizontalBoard || QA.PlayerAction == NowAction.Action_PlaceVerticalBoard)
                {
                    if (PlayerSave == EnumNowPlayer.Player1)
                    {
                        ThisChessBoard.NumPlayer1Board -= 2;
                    }
                    else
                    {
                        ThisChessBoard.NumPlayer2Board -= 2;
                    }
                }
                #endregion

                if (ThisNode.depth <= DepthMax)
                {
                    CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                    , PlayerSave, ThisNode.depth + 1, ThisNode.alpha, ThisNode.beta, ThisNode.score));

                    if (IfUseTT)
                    {
                        long HashCodeBuff = NodeTranslationTable.NodeGetHashCode(ThisNode.NodeHashCode, QA, ChessBoardBuff);//ThisChessBoard已变，不能作为原棋盘传入，只能上一步的棋盘ChessBoardBuff
                        ThisNode.SonNode.Last().NodeHashCode = HashCodeBuff;
                    }
                    ExpandNode_ABPruning(ThisChessBoard, ThisNode.SonNode.Last(),IfUseTT);
                }
                else
                {
                    CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                    , PlayerSave, ThisNode.depth + 1, QA.WholeScore, ThisNode.beta, QA.WholeScore));
                }

                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);

                #region Min层
                if (ThisNode.NodePlayer == NowQuoridor.PlayerBuff)
                {
                    if (ThisNode.SonNode.Last().alpha < ThisNode.beta)
                    {
                        ThisNode.beta = ThisNode.SonNode.Last().alpha;
                        ThisNode.score = ThisNode.SonNode.Last().alpha;
                    }
                }
                #endregion
                #region Max层
                else
                {
                    if (ThisNode.SonNode.Last().beta > ThisNode.alpha)
                    {
                        ThisNode.alpha = ThisNode.SonNode.Last().beta;
                        ThisNode.score = ThisNode.SonNode.Last().beta;
                    }
                }
                #endregion

                if (ThisNode.beta <= ThisNode.alpha)//剪枝
                {
                    #region 存入置换表
                    if (IfUseTT)
                    {
                        /*剪枝break前这个时刻该节点已经遍历完毕，可以加入置换表*/
                        TranslationTable.GameTreeNodeForHash HashNodeBuff = new TranslationTable.GameTreeNodeForHash();
                        HashNodeBuff.alpha = ThisNode.alpha;
                        HashNodeBuff.beta = ThisNode.beta;
                        HashNodeBuff.depth = ThisNode.depth + RootDepth;

                        NodeTranslationTable.Add(ThisNode.NodeHashCode, HashNodeBuff);
                    }
                    #endregion
                    break;
                }
            }
            #region 存入置换表
            if (IfUseTT)
            {
                /*遍历完整个动作列表后可以加入置换表*/
                TranslationTable.GameTreeNodeForHash HashNodeBuff2 = new TranslationTable.GameTreeNodeForHash();
                HashNodeBuff2.alpha = ThisNode.alpha;
                HashNodeBuff2.beta = ThisNode.beta;
                HashNodeBuff2.depth = ThisNode.depth + RootDepth;

                NodeTranslationTable.Add(ThisNode.NodeHashCode, HashNodeBuff2);
            }
            #endregion
        }
        /// <summary>
        /// 以Alpha-Beta剪枝框架并使用TranslationTable生成博弈树
        /// </summary>
        /// <param name="ThisChessBoard">当前棋盘状态</param>
        /// <param name="ThisNode">当前博弈树节点</param>
        public void ExpandNode_ABPruningNew(ChessBoard ThisChessBoard, GameTreeNode ThisNode, bool IfUseTT = true)
        {
            if (IfUseTT)
            {
                bool IfInTT = false;
                TranslationTable.GameTreeNodeForHash HashNode1 = new TranslationTable.GameTreeNodeForHash();
                HashNode1 = NodeTranslationTable.Search(ThisNode.NodeHashCode, ref IfInTT);
                if (ThisNode.depth != 0 && IfInTT && ThisNode.depth + RootDepth <= HashNode1.depth)
                {
                    ThisNode.alpha = HashNode1.alpha;
                    ThisNode.beta = HashNode1.beta;
                    return;
                }
            }
            ///暂存一些量以便恢复
            EnumNowPlayer PlayerSave = NowQuoridor.ReversePlayer(ThisNode.NodePlayer);
            NowQuoridor.Player_Now = PlayerSave;

            List<QuoridorAction> QABuff = NowQuoridor.ActionList;

            //QABuff = NowQuoridor.CreateActionList(ThisChessBoard);
            QABuff = NowQuoridor.CreateActionList(ThisChessBoard, GameTreePlayer);

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
                if (QA.PlayerAction == NowAction.Action_PlaceHorizontalBoard || QA.PlayerAction == NowAction.Action_PlaceVerticalBoard)
                {
                    if (PlayerSave == EnumNowPlayer.Player1)
                    {
                        ThisChessBoard.NumPlayer1Board -= 2;
                    }
                    else
                    {
                        ThisChessBoard.NumPlayer2Board -= 2;
                    }
                }
                #endregion

                if (ThisNode.depth <= DepthMax)
                {
                    CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                    , PlayerSave, ThisNode.depth + 1, ThisNode.alpha, ThisNode.beta, ThisNode.score));

                    if (IfUseTT)
                    {
                        long HashCodeBuff = NodeTranslationTable.NodeGetHashCode(ThisNode.NodeHashCode, QA, ChessBoardBuff);//ThisChessBoard已变，不能作为原棋盘传入，只能上一步的棋盘ChessBoardBuff
                        ThisNode.SonNode.Last().NodeHashCode = HashCodeBuff;
                    }
                    ExpandNode_ABPruning(ThisChessBoard, ThisNode.SonNode.Last(), IfUseTT);
                }
                else
                {
                    CreateNewSon(ThisNode, new GameTreeNode(QA.PlayerAction, QA.ActionPoint
                    , PlayerSave, ThisNode.depth + 1, QA.WholeScore, ThisNode.beta, QA.WholeScore));
                }

                ChessBoard.ResumeChessBoard(ref ThisChessBoard, ChessBoardBuff);

                #region Min层
                if (ThisNode.NodePlayer == NowQuoridor.PlayerBuff)
                {
                    if (ThisNode.SonNode.Last().alpha < ThisNode.beta)
                    {
                        ThisNode.beta = ThisNode.SonNode.Last().alpha;
                        ThisNode.score = ThisNode.SonNode.Last().alpha;
                    }
                }
                #endregion
                #region Max层
                else
                {
                    if (ThisNode.SonNode.Last().beta > ThisNode.alpha)
                    {
                        ThisNode.alpha = ThisNode.SonNode.Last().beta;
                        ThisNode.score = ThisNode.SonNode.Last().beta;
                    }
                }
                #endregion

                if (ThisNode.beta <= ThisNode.alpha)//剪枝
                {
                    #region 存入置换表
                    if (IfUseTT)
                    {
                        /*剪枝break前这个时刻该节点已经遍历完毕，可以加入置换表*/
                        TranslationTable.GameTreeNodeForHash HashNodeBuff = new TranslationTable.GameTreeNodeForHash();
                        HashNodeBuff.alpha = ThisNode.alpha;
                        HashNodeBuff.beta = ThisNode.beta;
                        HashNodeBuff.depth = ThisNode.depth + RootDepth;

                        NodeTranslationTable.Add(ThisNode.NodeHashCode, HashNodeBuff);
                    }
                    #endregion
                    break;
                }
            }
            #region 存入置换表
            if (IfUseTT)
            {
                /*遍历完整个动作列表后可以加入置换表*/
                TranslationTable.GameTreeNodeForHash HashNodeBuff2 = new TranslationTable.GameTreeNodeForHash();
                HashNodeBuff2.alpha = ThisNode.alpha;
                HashNodeBuff2.beta = ThisNode.beta;
                HashNodeBuff2.depth = ThisNode.depth + RootDepth;

                NodeTranslationTable.Add(ThisNode.NodeHashCode, HashNodeBuff2);
            }
            #endregion 
        }

        public enum Enum_GameTreeSearchFrameWork
        {
            MinMax,
            AlphaBetaPurning
        }
        public static Enum_GameTreeSearchFrameWork SearchFrameWork = Enum_GameTreeSearchFrameWork.AlphaBetaPurning;
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
                if (DepthMax_Set % 2 != 0)//必须是偶数
                {
                    throw E;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            DepthMax = DepthMax_Set;

            if (SearchFrameWork == Enum_GameTreeSearchFrameWork.MinMax)
            {
                RootNode.ExpandNode_MinMax(ChessBoard_Init, RootNode);//3W数量级节点数  
                double MaxScore = -1000;
                foreach (GameTreeNode GTN in RootNode.SonNode)
                {
                    if (MaxScore < GTN.score)
                    {
                        MaxScore = GTN.score;
                        RootNode.NodePlayer = GTN.NodePlayer;
                        RootNode.NodeAction = GTN.NodeAction;
                        RootNode.ActionLocation = GTN.ActionLocation;
                        RootNode.score = MaxScore;
                    }
                }
            }
            else
            {


                RootNode.NodeHashCode = GameTreeNode.InitChessBoardHashCode;
                RootNode.ExpandNode_ABPruning(ChessBoard_Init, RootNode, GameTreeNode.IfUseTanslationTable); 

                double MaxScore = -1000;
                foreach (GameTreeNode GTN in RootNode.SonNode)
                {
                    if (MaxScore < GTN.beta)
                    {
                        MaxScore = GTN.beta;
                        RootNode.NodePlayer = GTN.NodePlayer;
                        RootNode.NodeAction = GTN.NodeAction;
                        RootNode.ActionLocation = GTN.ActionLocation;
                        RootNode.score = MaxScore;
                        RootNode.NodeHashCode = GTN.NodeHashCode;
                    }
                }
                InitChessBoardHashCode = RootNode.NodeHashCode;
            }


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
        /// <summary>
        /// 获得博弈树节点在TreeView控件上应有的Text属性字符串
        /// </summary>
        /// <param name="NowNode">当前待生成的节点</param>
        /// <returns></returns>
        public static string GetGameTreeNodeViewText(GameTreeNode NowNode)
        {
            string SonTextbuff = "D:";
            SonTextbuff += (NowNode.depth + RootDepth).ToString() + " P";

            switch (NowNode.NodePlayer)
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
            switch (NowNode.NodeAction)
            {
                case NowAction.Action_PlaceVerticalBoard:
                    SonTextbuff += ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点和" +
                        ((NowNode.ActionLocation.X + 1) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点;";
                    break;
                case NowAction.Action_PlaceHorizontalBoard:
                    SonTextbuff += ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点和" +
                        ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1 + 1).ToString() + "点";
                    break;
                case NowAction.Action_Move_Player1:
                    SonTextbuff += ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点";
                    break;
                case NowAction.Action_Move_Player2:
                    SonTextbuff += ((NowNode.ActionLocation.X) * 8 + NowNode.ActionLocation.Y + 1).ToString() + "点";
                    break;
                case NowAction.Action_Wait:
                    SonTextbuff += "Error";
                    break;
                default:
                    SonTextbuff += "Error";
                    break;
            }

            SonTextbuff += " A:";
            SonTextbuff += NowNode.alpha.ToString();
            SonTextbuff += ",B:";
            SonTextbuff += NowNode.beta.ToString();
            SonTextbuff += ",S:";
            SonTextbuff += NowNode.score.ToString();

            if (GameTreeNode.IfUseTanslationTable)
            {
                SonTextbuff += " ";
                SonTextbuff += NowNode.NodeHashCode.ToString();                
            }

            return SonTextbuff;
        }
        public static void GameTreeView(GameTreeNode NowNode, TreeNode NowTreeNode)
        {
            if (NowNode.SonNode.Count <= 0)//叶节点
            {
                return;
            }
            foreach (GameTreeNode Son in NowNode.SonNode)
            {
                string SonTextbuff = GetGameTreeNodeViewText(Son);
                TreeNode SonTreeNode = new TreeNode(SonTextbuff);

                NowTreeNode.Nodes.Add(SonTreeNode);
                GameTreeView(Son, SonTreeNode);
                if (NowNode.depth == 0)
                {
                    SonTextbuff = GetGameTreeNodeViewText(NowNode);

                    NowTreeNode.Text = SonTextbuff;
                }
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
        /// <summary>
        /// 使用MTD（f）算法搜索博弈树
        /// </summary>
        /// <param name="ChessBoard_Init">当前局面棋盘</param>
        /// <param name="SearchPlayer">待搜索的玩家</param>
        /// <param name="InitGuessValue">初始猜测值</param>
        /// <param name="Depth">搜索深度</param>
        /// <param name="AlphaMax">待搜索的期望窗中的Alpha值</param>
        /// <param name="BetaMax">待搜索的期望窗中的Beta值</param>
        /// <returns>搜索结果保存的子树根节点</returns>
        public static GameTreeNode MTDfSearch(ChessBoard ChessBoard_Init, EnumNowPlayer SearchPlayer, double InitGuessValue, int Depth, double AlphaMax, double BetaMax)
        {
            GameTreeNode SearchRoot = new GameTreeNode();

            double Low = AlphaMax;
            double Upper = BetaMax;
            double Beta = Upper;
            double GuessValue = InitGuessValue;
            while (Low < Upper)
            {
                if (GuessValue == Low)
                    Beta = 0.5 * (Low + Upper);//二分查找
                else
                    Beta = GuessValue;
                #region AB剪枝搜索
                SearchRoot = new GameTreeNode();
                SearchRoot.NodePlayer = SearchPlayer;
                SearchRoot.alpha = Beta - 1;
                SearchRoot.beta = Beta;
                CreateGameTree(SearchRoot, ChessBoard_Init, Depth);
                if (true)
                {
                    if (Form1.form1.DV.treeView1.Nodes[Form1.form1.DV.treeView1.Nodes.Count - 1].Text != "Root")
                        Form1.form1.DV.treeView1.Nodes.Add(new TreeNode("第次落子:"));
                    else
                        Form1.form1.DV.treeView1.Nodes[0] = new TreeNode("第次落子:");
                    GameTreeNode.GameTreeView(SearchRoot, Form1.form1.DV.treeView1.Nodes[Form1.form1.DV.treeView1.Nodes.Count - 1]);
                }
                GuessValue = SearchRoot.alpha;
                //GuessValue = AlphaBetaPruning(NowNode, Depth, Beta - 1, Beta);
                #endregion
                if (GuessValue >= Beta)
                    Low = GuessValue;
                else
                    Upper = GuessValue;
            }
            return SearchRoot;
        }
    }

    /// <summary>
    /// Zobrist哈希算法,目前无视了挡板是谁下的
    /// </summary>
    public class TranslationTable
    {
        public Hashtable ChessBoardTT = new Hashtable();

        CryptoRandomSource rnd = new CryptoRandomSource();
        /// <summary>
        /// Zobrist决策映射表，对于第一维度的数定义为动作索引，具体定义如下：
        /// <para>0 -> P1玩家的移动</para>
        /// <para>1 -> P2玩家的移动</para>
        /// <para>2 -> 横档板（决策点，不是是否有挡板的意思）</para>
        /// <para>3 -> 竖档板（决策点，不是是否有挡板的意思）</para>
        /// </summary>
        public long[, ,] ZobristList = new long[4, 7, 7];
        /// <summary>
        /// 初始化当前置换表
        /// </summary>
        /// <param name="ChessBoardSize_Width">棋盘宽度（格）</param>
        /// <param name="ChessBoardSize_Height">棋盘高度（格）</param>
        /// <param name="ActionNum">行动类型总数</param>
        public TranslationTable()
        {
            ZobristList = new long[4, 7, 7];

            #region 生成随机码
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        ZobristList[i, j, k] = rnd.NextInt64();
                        /*调试用：*/
                        //ZobristList[i, j, k] = 100 * i + 10 * j + k;
                        //ZobristList[i, j, k] = rnd.NextInt32s(1)[0];
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 获得一个哈希值
        /// </summary>
        /// <param name="HashCode">前一步棋盘的哈希值</param>
        /// <param name="QA">动作对象</param>
        /// <param name="IfInitNode">是否是初始节点</param>
        /// <returns></returns>
        public long NodeGetHashCode(long HashCode, QuoridorAction QA, ChessBoard NowCB, bool IfInitNode = false)
        {
            int ActionIndexBuff = 0;
            switch (QA.PlayerAction)
            {
                case NowAction.Action_PlaceVerticalBoard:
                    ActionIndexBuff = 3;
                    break;
                case NowAction.Action_PlaceHorizontalBoard:
                    ActionIndexBuff = 2;
                    break;
                case NowAction.Action_Move_Player1:
                    ActionIndexBuff = 0;
                    break;
                case NowAction.Action_Move_Player2:
                    ActionIndexBuff = 1;
                    break;
                default:
                    ActionIndexBuff = 0;
                    break;
            }
            long HashBuff = ZobristList[ActionIndexBuff, QA.ActionPoint.X, QA.ActionPoint.Y];

            if (IfInitNode)
            {
                return HashBuff;
            }
            else
            {
                #region 撤销玩家的移动位置
                if (QA.PlayerAction == NowAction.Action_Move_Player1)
                {
                    int CancelLocation_X = NowCB.Player1Location.X;
                    int CancelLocation_Y = NowCB.Player1Location.Y;

                    QuoridorAction QABuff = new QuoridorAction(NowAction.Action_Move_Player1, new Point(CancelLocation_X, CancelLocation_Y));
                    long HashCode_Cancel = 
                        ZobristList[0, QABuff.ActionPoint.X, QABuff.ActionPoint.Y];

                    HashBuff = HashBuff ^ HashCode_Cancel;
                }
                else if (QA.PlayerAction == NowAction.Action_Move_Player2)
                {
                    int CancelLocation_X = NowCB.Player2Location.X;
                    int CancelLocation_Y = NowCB.Player2Location.Y;

                    QuoridorAction QABuff = new QuoridorAction(NowAction.Action_Move_Player2, new Point(CancelLocation_X, CancelLocation_Y));

                    long HashCode_Cancel =
                        ZobristList[1, QABuff.ActionPoint.X, QABuff.ActionPoint.Y];

                    HashBuff = HashBuff ^ HashCode_Cancel;
                }
                #endregion

                HashBuff = HashCode ^ HashBuff;
                return HashBuff;
            }
        }
        /// <summary>
        /// 添加一个散列映射
        /// </summary>
        /// <param name="HashCode">要添加的哈希值</param>
        /// <param name="NodeToSave">待保存的节点信息</param>
        /// <param name="IfInitNode">是否是初始节点（即前一步棋盘是未进行任何行动的时候的棋盘）</param>
        public void Add(long HashCode, GameTreeNodeForHash NodeToSave, bool IfInitNode = false)
        {
            long HashCodeBuff = HashCode;
            
            if (IfInitNode)
            {
                ChessBoardTT.Add(HashCodeBuff, NodeToSave);
            }
            else
            {
                #region 置换表中已包含的话替换该节点
                bool IfHaveThisNode = false;
                Search(HashCodeBuff, ref IfHaveThisNode);
                #endregion
                #region 随时替换策略
                if (!IfHaveThisNode)
                    ChessBoardTT.Add(HashCodeBuff, NodeToSave);
                else
                    ChessBoardTT[HashCodeBuff] = NodeToSave;
                #endregion
            }
        }
        /// <summary>
        /// 通过一个哈希值检索出置换表所存的节点，如果没有该哈希值的话则为默认节点，是否包含该哈希值由IfContain变量给出
        /// </summary>
        /// <param name="HashCode">哈希值</param>
        /// <param name="IfContain">是否包含此哈希值</param>
        /// <returns>检索出的节点对象</returns>
        public GameTreeNodeForHash Search(long HashCode, ref bool IfContain)
        {
            GameTreeNodeForHash ReturnNode = new GameTreeNodeForHash();
            if (ChessBoardTT.Contains(HashCode))
            {
                IfContain = true;
                ReturnNode = (GameTreeNodeForHash)ChessBoardTT[HashCode];
            }
            else
            {
                IfContain = false;
            }
            return ReturnNode;
        }
        public class GameTreeNodeForHash
        {
            public double alpha = -10000;
            public double beta = 10000;
            public int depth = -1;
        }
        //class GameTreeNodeForHash
        //{
        //    public int Depth;//该节点的搜索深度
        //    public double Value;//该节点上一层对其给出的估值
        //    public QuoridorAction BestAction;//该节点下一步的最佳决策
        //    public QuoridorAction SecondBestAction;//该节点下一步的次优决策
        //    public int HashNodeType;//该节点类型
        //}
    }
    public class MTD
    {
        public double AlphaBetaPruning(GameTreeNode NowNode, int Depth, double Alpha, double Beta)
        { return 0; }
        public double MTD_f(GameTreeNode NowNode, double GuessValue, int Depth)
        {
            double Low = -60;
            double Upper = 60;
            double Beta = 60;
            while (Low < Upper)
            {
                if (GuessValue == Low)
                    Beta = 0.5 * (Low + Upper);
                else
                    Beta = GuessValue;
                GuessValue = AlphaBetaPruning(NowNode, Depth, Beta - 1, Beta);
                if (GuessValue >= Beta)
                    Low = GuessValue;
                else
                    Upper = GuessValue;
            }
            return GuessValue;
        }
    }

}
