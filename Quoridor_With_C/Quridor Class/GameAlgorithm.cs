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
using Quoridor;

namespace QuoridorGameAlgorithm
{
    /// <summary>
    /// 步步为营决策系统
    /// </summary>
    class QuoridorDecisionSystem
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
            public ABPurningPara(int DepthMax_Set = 2, bool SortActionList_Set = true, bool UseTT_Set = false
                ,double AlphaInit_Set = -50, double BetaInit_Set = 50
                ,bool UseGoodExtension_Set = true, int GoodExtension_Depth_Set = 4, double ScoreLower_Set = 0)
            {
                DepthMax = DepthMax_Set;
                SortActionList = SortActionList_Set;
                UseTT = UseTT_Set;
                AlphaInit = AlphaInit_Set;
                BetaInit = BetaInit_Set;
                UseGoodExtension = UseGoodExtension_Set;
                GoodExtension_Depth = GoodExtension_Depth_Set;
                GoodExtension_ScoreLower = ScoreLower_Set;
            }
            public ABPurningPara() { }
        }
        #endregion
        public ABPurningPara ThisABPurningPara= new ABPurningPara();
        public QuoridorEvalution NowQuoridor = new QuoridorEvalution();
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
            RootNode.NodePlayer = NowQuoridor.ReversePlayer(PolicyPlayer);

            int P2Dis = NowQuoridor.AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player2
                , ThisChessBoard.Player2Location.X, ThisChessBoard.Player2Location.Y);
            int P1Dis = NowQuoridor.AstarEngine.AstarRestart(ThisChessBoard, EnumNowPlayer.Player1
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
                GameTreeNode.CreateGameTree(RootNode, ThisChessBoard, ThisABPurningPara.DepthMax, false);

                NextPolicy = RootNode.NodeAction;
            }

            return NextPolicy;
        }
        public QuoridorDecisionSystem(Enum_DecisionAlgorithm UsedAlgorithm_Set, ABPurningPara ABPurningPara_Set)
        {
            UsedAlgorithm = UsedAlgorithm_Set;
            if (UsedAlgorithm == Enum_DecisionAlgorithm.AlphaBetaPurning)
            {
                ThisABPurningPara = ABPurningPara_Set;
            }
        }
    }
}
