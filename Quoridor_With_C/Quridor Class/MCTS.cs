using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTree;
using System.Drawing;
using QuoridorRule;
using QuoridorEva;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;
using Quoridor_With_C;

namespace MCTS
{
    /// <summary>
    /// 蒙特卡洛树节点
    /// </summary>
    class MonteCartoTreeNode
    {
        public long _N = 1;//N值
        public double _Q = 0;//Q值
        public double _P = 0;//P值
        public double _UCT = 0;//UCT值
        public static double _C = 0;//折中系数
        public bool IfWin = false;

        public NowAction NodeAction = NowAction.Action_Move_Player1;///当前节点的动作
        public EnumNowPlayer NodePlayer = EnumNowPlayer.Player2;///当前节点的动作的执行玩家
        public Point ActionLocation = new Point(-1, -1);///当前节点动作的执行位置

        public MonteCartoTreeNode FatherNode;
        public List<MonteCartoTreeNode> SonNode = new List<MonteCartoTreeNode>();///子节点列表

        public static EnumNowPlayer JudgePlayer = EnumNowPlayer.Player2;
        public static QuoridorEvalution NowQuoridor = new QuoridorEvalution();
        public static QuoridorRuleEngine RuleEngine = new QuoridorRuleEngine();
        /// <summary>
        /// 计算一个子节点的UCT值，用于Select
        /// </summary>
        /// <param name="FatherNode">父节点</param>
        /// <param name="SonNode">子节点</param>
        /// <returns>SonNode的UCT值</returns>
        public static double CalUCTValue_UCT(MonteCartoTreeNode FatherNode, MonteCartoTreeNode SonNode)
        {
            double UCTBuff = 0;
            double ExploitationComponent = 0, ExplorationComponent = 0;

            /*普通的UCT*/
            //ExploitationComponent = SonNode._Q / SonNode._N;
            //ExplorationComponent = _C * Math.Sqrt((Math.Log10(FatherNode._N) / SonNode._N));
            //UCTBuff = ExploitationComponent + ExplorationComponent;

            /*与P值有关的UCT*/
            UCTBuff = _C * SonNode._P * Math.Sqrt(FatherNode._N / (1 + SonNode._N));

            return UCTBuff;
        }
        /// <summary>
        /// Select操作
        /// </summary>
        /// <param name="FatherNode">父节点</param>
        /// <returns>选择要去拓展的子节点</returns>
        public static MonteCartoTreeNode Select(MonteCartoTreeNode FatherNode)
        {
            MonteCartoTreeNode MTNode = new MonteCartoTreeNode();

            double MaxQUCT = -100;
            foreach (MonteCartoTreeNode NodeBuff in FatherNode.SonNode)
            {
                NodeBuff._UCT = CalUCTValue_UCT(FatherNode, NodeBuff);
                if (MaxQUCT < NodeBuff._UCT + NodeBuff._Q)//选择最大UCT + Q值的节点
                {
                    MaxQUCT = NodeBuff._UCT + NodeBuff._Q;
                    MTNode = NodeBuff;
                }
            }

            return MTNode;
        }
        /// <summary>
        /// 根据子节点Q值更新当前节点Q值
        /// </summary>
        /// <param name="Leaf_Value">子节点Q值</param>
        public void UpdateInfo(double Leaf_Value)
        {
            _N++;
            _Q = 1.0 * (Leaf_Value - _Q) / _N;//滑动平均方法更新Q值
        }
        /// <summary>
        /// 反向传播更新节点信息（回溯）
        /// </summary>
        /// <param name="Leaf_Value">子节点Q值</param>
        public void BackPropagation(double Leaf_Value)
        {
            if (FatherNode != null)//非根节点
            {
                FatherNode.BackPropagation(-Leaf_Value);
            }
            UpdateInfo(Leaf_Value);
        }
        /// <summary>
        /// 根据当前局面拓展一个未拓展节点
        /// </summary>
        /// <param name="ThisChessBoard">当前局面棋盘</param>
        /// <param name="FatherNode">父节点</param>
        public void Expand(ChessBoard ThisChessBoard, MonteCartoTreeNode FatherNode)
        {
            if (SonNode.Count == 0)//未拓展节点
            {
                EnumNowPlayer PlayerSave = NowQuoridor.ReversePlayer(FatherNode.NodePlayer);
                NowQuoridor.Player_Now = PlayerSave;

                List<QuoridorAction> QABuff = NowQuoridor.ActionList;

//                QABuff = NowQuoridor.CreateActionList(ThisChessBoard, EnumNowPlayer.Player2);
                //QABuff = NowQuoridor.CreateActionList_ALL(ThisChessBoard);
                /*完全拓展*/
                foreach (QuoridorAction QA in QABuff)
                {
                    MonteCartoTreeNode MTSonNode = new MonteCartoTreeNode();
                    MTSonNode.NodePlayer = PlayerSave;
                    MTSonNode.NodeAction = QA.PlayerAction;
                    MTSonNode.ActionLocation.X = QA.ActionPoint.X;
                    MTSonNode.ActionLocation.Y = QA.ActionPoint.Y;
                    MTSonNode._P = QA.WholeScore;
                    MTSonNode.FatherNode = FatherNode;
                    SonNode.Add(MTSonNode);
                    //if (QA.PlayerAction == NowAction.Action_PlaceHorizontalBoard || QA.PlayerAction == NowAction.Action_PlaceVerticalBoard)
                    //{
                    //    if (QA.OpponentScore - QA.SelfScore >= 5)
                    //    {
                    //        MTSonNode.IfWin = true;
                    //        SonNode = new List<MonteCartoTreeNode>();
                    //        SonNode.Add(MTSonNode);
                    //        break;
                    //    }
                    //}
                }
            }
        }
        /// <summary>
        /// 进行一次模拟(Simluation)
        /// </summary>
        /// <param name="InitChessBoard">当前决策节点局面</param>
        /// <param name="RootNode">根节点</param>
        public static void SimluationOnce(ChessBoard InitChessBoard, MonteCartoTreeNode RootNode)
        {
            #region 暂存挡板数量
            int Board1Save = InitChessBoard.NumPlayer1Board;
            int Board2Save = InitChessBoard.NumPlayer2Board;
            #endregion

            if (RootNode.SonNode.Count == 0)//初始根节点
            {
                RootNode.Expand(InitChessBoard, RootNode);//先拓展一次		 
            }

            ChessBoard SimluationChessBoard = new ChessBoard();
            ChessBoard.SaveChessBoard(ref SimluationChessBoard, InitChessBoard);//相当于拷贝了
            MonteCartoTreeNode NextExpandNode = RootNode;
            while (true)
            {
                #region 提前终止局面检测
                if (NextExpandNode.SonNode.Count == 1)
                {
                    if (NextExpandNode.SonNode[0].IfWin)
                    {
                        double leaf_value = -1;
                        if (JudgePlayer != NextExpandNode.SonNode[0].NodePlayer)
                        {
                            leaf_value = 1;
                        }
                        NextExpandNode.BackPropagation(leaf_value);
                        break;
                    }
                }
                #endregion
                /*选择*/
                NextExpandNode = Select(NextExpandNode);

                #region 模拟落子
                string Hint = NowQuoridor.QuoridorRule.Action(ref SimluationChessBoard
                    , NextExpandNode.ActionLocation.X, NextExpandNode.ActionLocation.Y, NextExpandNode.NodeAction);
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

                if (NextExpandNode.NodePlayer == EnumNowPlayer.Player1)
                {
                    if (NextExpandNode.NodeAction == NowAction.Action_PlaceVerticalBoard
                        || NextExpandNode.NodeAction == NowAction.Action_PlaceHorizontalBoard)
                        SimluationChessBoard.NumPlayer1Board -= 2;
                }
                else if (NextExpandNode.NodePlayer == EnumNowPlayer.Player2)
                {
                    if (NextExpandNode.NodeAction == NowAction.Action_PlaceVerticalBoard
                        || NextExpandNode.NodeAction == NowAction.Action_PlaceHorizontalBoard)
                        SimluationChessBoard.NumPlayer2Board -= 2;
                }
                #endregion

                //SimluationChessBoard.DrawNowChessBoard(ref Form1.Gr, Form1.form1.ChessWhitePB, Form1.form1.ChessBlackPB);
                //Form1.form1.ChessBoardPB.Refresh();
                //System.Threading.Thread.Sleep(500);
                string SucessHint = RuleEngine.CheckResult(SimluationChessBoard);
                if (SucessHint != "No success")//搜索到胜利节点了
                {
                    double leaf_value = -1;
                    if (JudgePlayer == EnumNowPlayer.Player1 && SucessHint == "Player1 Success!")
                    {
                        leaf_value = 1;
                    }
                    if (JudgePlayer == EnumNowPlayer.Player2 && SucessHint == "Player2 Success!")
                    {
                        leaf_value = 1;
                    }

                    NextExpandNode.BackPropagation(leaf_value);
                    break;
                }

                double dis_player1 = RuleEngine.AstarEngine.AstarRestart(SimluationChessBoard, EnumNowPlayer.Player1
, SimluationChessBoard.Player1Location.X, SimluationChessBoard.Player1Location.Y);
                double dis_player2 = RuleEngine.AstarEngine.AstarRestart(SimluationChessBoard, EnumNowPlayer.Player2
        , SimluationChessBoard.Player2Location.X, SimluationChessBoard.Player2Location.Y);

                EnumNowPlayer Winner = EnumNowPlayer.Player1;
                # region 必赢必输局面检测
                //if (dis_player1 >= 14 || dis_player2 >= 14)//某人步数过大
                //{
                //    if (dis_player2 - dis_player1 >= 5)
                //    {
                //        Winner = EnumNowPlayer.Player2;
                //    }
                //}
                //else if (SimluationChessBoard.NumPlayer2Board == 0 && SimluationChessBoard.NumPlayer1Board == 0)//挡板已用完
                //{
                //    #region 是否存在跳棋检测(未写)
                //    #endregion
                //    if (dis_player2 - dis_player1 > 0)
                //    {
                //        Winner = EnumNowPlayer.Player2;
                //    }                   
                //}

                //double leaf_value2 = -1;
                //if (JudgePlayer == EnumNowPlayer.Player1 && Winner == EnumNowPlayer.Player1)//下一步是P2走
                //{
                //    leaf_value2 = 1;
                //}
                //if (JudgePlayer == EnumNowPlayer.Player2 && Winner == EnumNowPlayer.Player2)//下一步是P1走
                //{
                //    leaf_value2 = 1;
                //}
                //NextExpandNode.BackPropagation(leaf_value2);

                #endregion

                /*拓展*/
                NextExpandNode.Expand(SimluationChessBoard, NextExpandNode);
            }
            #region 恢复挡板数量
            InitChessBoard.NumPlayer1Board = Board1Save;
            InitChessBoard.NumPlayer2Board = Board2Save;
            #endregion
        }
        /// <summary>
        /// 进行SimluationNum次模拟后选择最好的节点落子
        /// </summary>
        /// <param name="InitChessBoard">当前棋盘局面</param>
        /// <param name="RootNode">根节点</param>
        /// <param name="SimluationNum">模拟总次数</param>
        /// <returns>MCTS的搜索结果动作</returns>
        public static QuoridorAction GetMCTSPolicy(ChessBoard InitChessBoard, MonteCartoTreeNode RootNode, int SimluationNum = 10)
        {
            for (int i = 0; i < SimluationNum; i++)//模拟SimluationNum次
            {
                SimluationOnce(InitChessBoard, RootNode);
            }

            MonteCartoTreeNode BestMoveNode = new MonteCartoTreeNode();
            ///选择访问最多的节点
            long MaxVisitNum = -100;
            foreach (MonteCartoTreeNode NodeBuff in RootNode.SonNode)
            {
                if (NodeBuff._N > MaxVisitNum)
                {
                    MaxVisitNum = NodeBuff._N;
                    BestMoveNode = NodeBuff;
                }
            }

            QuoridorAction NextPolicy = new QuoridorAction(BestMoveNode.NodeAction, new Point(BestMoveNode.ActionLocation.X, BestMoveNode.ActionLocation.Y));
            return NextPolicy;
        }
        /// <summary>
        /// 获得下一轮决策的蒙特卡洛树根节点
        /// </summary>
        /// <param name="SelfAction">自己上次的行动</param>
        /// <param name="OpponentAction">对手上次的行动</param>
        /// <param name="MTRootNode">上次决策用的MC树</param>
        /// <returns>下一轮决策用的MC树根节点</returns>
        public static MonteCartoTreeNode GetNextPolicyRootNode(QuoridorAction SelfAction, QuoridorAction OpponentAction, MonteCartoTreeNode MTRootNode)
        {
            MonteCartoTreeNode NextRootNode = new MonteCartoTreeNode();

            foreach (MonteCartoTreeNode NodeBuff in MTRootNode.SonNode)
            {
                if (NodeBuff.NodeAction == SelfAction.PlayerAction)
                {
                    if (NodeBuff.ActionLocation.X == SelfAction.ActionPoint.X
                        && NodeBuff.ActionLocation.Y == SelfAction.ActionPoint.Y)
                    {
                        NextRootNode = NodeBuff;
                        break;
                    }
                }
            }

            bool IfHaveThisNode = false;
            foreach (MonteCartoTreeNode NodeBuff in NextRootNode.SonNode)
            {
                if (NodeBuff.NodeAction == OpponentAction.PlayerAction)
                {
                    if (NodeBuff.ActionLocation.X == OpponentAction.ActionPoint.X
                        && NodeBuff.ActionLocation.Y == OpponentAction.ActionPoint.Y)
                    {
                        NextRootNode = NodeBuff;
                        IfHaveThisNode = true;
                        break;
                    }
                }
            }

            if (!IfHaveThisNode)
            {
                NextRootNode = new MonteCartoTreeNode();
            }
            return NextRootNode;
        }

        public MonteCartoTreeNode() { }
    }
}
