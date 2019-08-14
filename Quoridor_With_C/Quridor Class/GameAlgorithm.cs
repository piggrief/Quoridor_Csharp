using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;
using LookupRoad;
using QuoridorRule;
using GameTree;
using QuoridorEva;

namespace QuoridorGameAlgorithm
{
    /// <summary>
    /// 步步为营决策系统
    /// </summary>
    public class QuoridorDecisionSystem
    {
        public EnumNowPlayer PolicyPlayer = EnumNowPlayer.Player2;//决策玩家
        #region 算法配置参数
        public enum Enum_DecisionAlgorithm
        {
            MinMax,
            AlphaBetaPurning,
            MTDf,
            MCTS
        }
        public Enum_DecisionAlgorithm UsedAlgorithm = Enum_DecisionAlgorithm.AlphaBetaPurning;
        public class ABPurningPara
        {
            public int DepthMax = 2;//最大深度
            public bool SortActionList = true;//是否排序节点
            public bool UseTT = false;//是否使用置换表
            public double AlphaInit = -50;//初始期望窗Alpha值
            public double BetaInit = 50;//初始期望窗Beta值
            public bool UseGoodExtension = false;//是否使用强棋延伸（算杀）
            public int GoodExtension_Depth = 4;//强棋延伸最大深度
            public double GoodExtension_ScoreLower = 0;//强棋评分下限
            public bool UseFormulae = true;//是否使用开局定式
            public ABPurningPara(int DepthMax_Set = 2, bool SortActionList_Set = true, bool UseTT_Set = false
                , double AlphaInit_Set = -50, double BetaInit_Set = 50, bool UseFormulae_Set = true
                , bool UseGoodExtension_Set = true, int GoodExtension_Depth_Set = 4, double ScoreLower_Set = 0)
            {
                DepthMax = DepthMax_Set;
                SortActionList = SortActionList_Set;
                UseTT = UseTT_Set;
                AlphaInit = AlphaInit_Set;
                BetaInit = BetaInit_Set;
                UseGoodExtension = UseGoodExtension_Set;
                GoodExtension_Depth = GoodExtension_Depth_Set;
                GoodExtension_ScoreLower = ScoreLower_Set;
                UseFormulae = UseFormulae_Set;
            }
            public ABPurningPara() { }
        }
        #endregion
        public enum GameSituation
        {
            InitialSituation,
            PreviousSituation,
            MediumSituation,
            FinalSituation
        }
        GameSituation NowGameSituation = GameSituation.InitialSituation;
        public ABPurningPara ThisABPurningPara= new ABPurningPara();
        public QuoridorEvalution NowEvalution = new QuoridorEvalution();
        /// <summary>
        /// 获得PolicyPlayer下一步的决策
        /// </summary>
        /// <param name="ThisChessBoard">当前局面棋盘</param>
        /// <param name="RootNode">返回的决策树根节点</param>
        /// <returns>下一步决策动作</returns>
        public QuoridorAction GetNextPolicy(ChessBoard ThisChessBoard, out GameTreeNode RootNode)
        {
            QuoridorAction NextPolicy = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));

            RootNode = new GameTreeNode();
            RootNode.NodePlayer = NowEvalution.ReversePlayer(PolicyPlayer);

            int P2Dis = NowEvalution.AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player2
                , ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);
            int P1Dis = NowEvalution.AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player1
                , ThisChessBoard.Player1Location.X, ThisChessBoard.Player1Location.Y);
            RootNode.NodeAction.ActionCheckResult.P1Distance = P1Dis;
            RootNode.NodeAction.ActionCheckResult.P2Distance = P2Dis;

            if (UsedAlgorithm == Enum_DecisionAlgorithm.AlphaBetaPurning)
            {
                #region 配置AB剪枝参数
                QuoridorEvalution.ActionListIfSort = ThisABPurningPara.SortActionList;
                GameTreeNode.SearchFrameWork = GameTreeNode.Enum_GameTreeSearchFrameWork.AlphaBetaPurning;
                GameTreeNode.IfUseTanslationTable = ThisABPurningPara.UseTT;
                GameTreeNode.DepthMax = ThisABPurningPara.DepthMax;
                RootNode.alpha = ThisABPurningPara.AlphaInit;
                RootNode.beta = ThisABPurningPara.BetaInit;                 
                #endregion
                if (ThisABPurningPara.UseFormulae && NowGameSituation == GameSituation.InitialSituation)
                {
                    NextPolicy = GetFormulaePolicy(ThisChessBoard);
                }
                else
                {
                    GameTreeNode.CreateGameTree(RootNode, ThisChessBoard, ThisABPurningPara.DepthMax, false);
                    NextPolicy = RootNode.NodeAction;
                }   
            }

            return NextPolicy;
        }
        public QuoridorDecisionSystem(EnumNowPlayer PolicyPlayer_Set, Enum_DecisionAlgorithm UsedAlgorithm_Set, ABPurningPara ABPurningPara_Set)
        {
            UsedAlgorithm = UsedAlgorithm_Set;
            if (UsedAlgorithm == Enum_DecisionAlgorithm.AlphaBetaPurning)
            {
                ThisABPurningPara = ABPurningPara_Set;
            }
            PolicyPlayer = PolicyPlayer_Set;
            FormulaeListInit();
        }
        public QuoridorDecisionSystem() { FormulaeListInit(); }

        public QuoridorAction GetFormulaePolicy(ChessBoard ThisChessBoard)
        {
            QuoridorAction NextPolicy = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));
            string CBString = ChessBoardToString(ThisChessBoard);
            if (FormulaeList.ContainsKey(CBString))
            {
                NextPolicy = FormulaeList[CBString];
            }
            else//直走或者最短路径走
            {
                Point PlayerLocation = ThisChessBoard.Player1Location;
                if (PolicyPlayer == EnumNowPlayer.Player2)
                {
                    PlayerLocation = ThisChessBoard.Player2Location;
                    NextPolicy.PlayerAction = NowAction.Action_Move_Player2;
                    NextPolicy.ActionPoint = new Point(PlayerLocation.X - 1, PlayerLocation.Y);
                }
                else
                {
                    NextPolicy.PlayerAction = NowAction.Action_Move_Player1;
                    NextPolicy.ActionPoint = new Point(PlayerLocation.X + 1, PlayerLocation.Y);
                }
                string ErrorHint = "";
                ErrorHint = NowEvalution.QuoridorRule.CheckMove_NoChange(ThisChessBoard
                    , NextPolicy.ActionPoint.X, NextPolicy.ActionPoint.Y, NextPolicy.PlayerAction);
                if (ErrorHint != "OK")//只能最短路径走
                {
                    List<QuoridorAction> MoveActionList = new List<QuoridorAction>();

                    NowEvalution.AddMoveAction(ref MoveActionList, ThisChessBoard, PolicyPlayer, PlayerLocation);
                    double MinRoad = 999;
                    foreach (QuoridorAction QA in MoveActionList)
                    {
                        if (QA.SelfScore < MinRoad)
                        {
                            MinRoad = QA.SelfScore;
                            NextPolicy = QA;
                        }
                    }
                }
            }
            return NextPolicy;
        }
        /// <summary>
        /// 定式库
        /// </summary>
        public Dictionary<string, QuoridorAction> FormulaeList = new Dictionary<string, QuoridorAction>();
        /// <summary>
        /// 把Grid对象转换成Int数字
        /// </summary>
        public Int16 GridToInt(Grid Gd)
        {
            Int16 Buff = 0;
            if (Gd.IfLeftBoard)
                Buff += 1;
            if (Gd.IfUpBoard)
                Buff += 2;
            switch (Gd.GridStatus)
            {
                case Grid.GridInsideStatus.Have_Player1:
                    Buff += 4;
                    break;
                case Grid.GridInsideStatus.Have_Player2:
                    Buff += 8;
                    break;
                case Grid.GridInsideStatus.Empty:
                    Buff += 16;
                    break;
                default:
                    break;
            }
            return Buff;
        }
        /// <summary>
        /// 将棋盘状态转换成字符串
        /// </summary>
        public string ChessBoardToString(ChessBoard ThisChessBoard)
        {
            string CBStr = "";
            Int16 GridStatus = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    GridStatus = GridToInt(ThisChessBoard.ChessBoardAll[i, j]);
                    CBStr += GridStatus.ToString();
                }
            }
            return CBStr;
        }
        public void FormulaeListInit()
        {
            string CBStr = "";
            # region 手动录入定式库
            ///1
            ChessBoard CBBuff = new ChessBoard();
            QuoridorAction ActionBuff = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));
            CBBuff.ChessBoardAll[6, 1].IfUpBoard = true;
            CBBuff.ChessBoardAll[6, 2].IfUpBoard = true;
            CBBuff.ChessBoardAll[6, 3].IfUpBoard = true;
            CBBuff.ChessBoardAll[6, 4].IfUpBoard = true;
            CBBuff.ChessBoardAll[6, 3].GridStatus = Grid.GridInsideStatus.Empty;
            CBBuff.ChessBoardAll[6, 2].GridStatus = Grid.GridInsideStatus.Have_Player2;
            ActionBuff.PlayerAction = NowAction.Action_PlaceVerticalBoard;
            ActionBuff.ActionPoint = new Point(5, 5);
            ActionBuff.SkipChessScore = 999;
            CBStr = ChessBoardToString(CBBuff);
            FormulaeList.Add(CBStr, ActionBuff);
            ///2
            # endregion
        }
    }
}
