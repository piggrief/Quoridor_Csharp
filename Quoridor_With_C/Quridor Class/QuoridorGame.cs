using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using LookupRoad;
using QuoridorRule;
using GameTree;
using QuoridorEva;
using Quoridor_With_C;
using QuoridorGameAlgorithm;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;

namespace QuoridorGameClass
{
    public class QuoridorGame
    {
        public QuoridorEvalution QuoridorEva = new QuoridorEvalution();
        /// <summary>
        /// 玩家类型
        /// </summary>
        public enum Enum_PlayerType
        {
            Human,
            AI
        }
        public Enum_PlayerType P1Type = Enum_PlayerType.Human;
        public Enum_PlayerType P2Type = Enum_PlayerType.AI;
        public QuoridorDecisionSystem P1DecisionSystem = new QuoridorDecisionSystem();
        public QuoridorDecisionSystem P2DecisionSystem = new QuoridorDecisionSystem();

        public bool HumanPolicyFinish = false;//人类玩家决策是否完成
        public Point HumanMouseClickPoint = new Point();
        public Point HumanPolicyLocation = new Point();
        public NowAction HumanPolicyAction = new NowAction();
        /// <summary>
        /// 将鼠标点击坐标转换成玩家决策位置
        /// </summary>
        /// <param name="MousePoint">鼠标点击坐标</param>
        /// <returns>错误提示符，没错返回“OK”</returns>
        public string MousePointToPolicyPoint(Point MousePoint)
        {
            int col = 0, row = 0;//0~7行列
            if (HumanPolicyAction == NowAction.Action_PlaceVerticalBoard)
            {
                col = Convert.ToInt16((MousePoint.X - Form1._FormDraw.StartLocation_X) / Form1._FormDraw.CB_BlockWidth);
                row = Convert.ToInt16((MousePoint.Y + Form1._FormDraw.CB_BlockWidth / 2 - Form1._FormDraw.StartLocation_Y) / Form1._FormDraw.CB_BlockWidth) - 1;
            }
            else if (HumanPolicyAction == NowAction.Action_PlaceHorizontalBoard)
            {
                col = Convert.ToInt16((MousePoint.X + Form1._FormDraw.CB_BlockWidth / 2 - Form1._FormDraw.StartLocation_X) / Form1._FormDraw.CB_BlockWidth) - 1;
                row = Convert.ToInt16((MousePoint.Y - Form1._FormDraw.StartLocation_Y) / Form1._FormDraw.CB_BlockWidth);
            }
            else if (HumanPolicyAction == NowAction.Action_Move_Player1 || HumanPolicyAction == NowAction.Action_Move_Player2)
            {
                col = Convert.ToInt16(MousePoint.X / Form1._FormDraw.CB_BlockWidth) - 1;
                row = Convert.ToInt16(MousePoint.Y / Form1._FormDraw.CB_BlockWidth) - 1;
            }
            if (!(row >= 0 && row <= 6 && col >= 0 && col <= 6))
            {
                HumanPolicyFinish = false;
                return "点击越界！";
            }
            HumanPolicyLocation = new Point(row, col);
            return "OK";
        }
        public GameTreeNode NodeToTreeView = new GameTreeNode();
        public List<GameTreeNode> PolicyRootNodeList = new List<GameTreeNode>();
        public QuoridorAction GetNextPolicy(Enum_PlayerType PlayerType, EnumNowPlayer Player, out string ErrorHint)
        {
            ErrorHint = "";
            QuoridorAction NextPolicy = new QuoridorAction(NowAction.Action_Wait, new Point(-1, -1));
            #region 玩家决策
            if (PlayerType == Enum_PlayerType.Human)
            {
                while (!HumanPolicyFinish)
                {
                }
                ErrorHint = MousePointToPolicyPoint(HumanMouseClickPoint);
                if (ErrorHint == "OK")
                {
                    NextPolicy.ActionPoint = HumanPolicyLocation;
                    if (HumanPolicyAction == NowAction.Action_Move_Player1 || HumanPolicyAction == NowAction.Action_Move_Player2)
                    {
                        if (Player == EnumNowPlayer.Player1)
                        {
                            HumanPolicyAction = NowAction.Action_Move_Player1;
                        }
                        else
                        {
                            HumanPolicyAction = NowAction.Action_Move_Player2; 
                        }
                    }
                    NextPolicy.PlayerAction = HumanPolicyAction;
                    HumanPolicyFinish = false;
                }
            }
            #endregion
            # region AI决策
            else if (PlayerType == Enum_PlayerType.AI)
            {
                QuoridorDecisionSystem DSBuff = P1DecisionSystem;
                if (Player == EnumNowPlayer.Player2)
                    DSBuff = P2DecisionSystem;
                GameTreeNode RootNode = new GameTreeNode();
                NextPolicy = DSBuff.GetNextPolicy(QuoridorEva.ThisChessBoard, out RootNode);
                NodeToTreeView = RootNode;
                ErrorHint = "OK";
            }
            # endregion
            return NextPolicy;
        }
        /// <summary>
        /// 检测该决策是否可行
        /// </summary>
        /// <param name="PolicyToCheck">待检测的决策动作</param>
        /// <param name="PlayerToCheck">待检测的玩家</param>
        /// <returns>错误提示字符串，“OK”代表可行</returns>
        public string CheckPolicy(QuoridorAction PolicyToCheck, EnumNowPlayer PlayerToCheck)
        {
            if (PolicyToCheck.PlayerAction == NowAction.Action_Wait)
            {
                return "决策有误";
            }
            QuoridorRuleEngine.CheckBoardResult RuleCheckResult = new QuoridorRuleEngine.CheckBoardResult();

            RuleCheckResult = QuoridorEva.QuoridorRule.CheckBoard(QuoridorEva.ThisChessBoard, PolicyToCheck.PlayerAction, PlayerToCheck
                , PolicyToCheck.ActionPoint.X, PolicyToCheck.ActionPoint.Y);
            string RuleHint = RuleCheckResult.HintStr;

            if ((RuleHint == "Player1 No Board" && PlayerToCheck == EnumNowPlayer.Player1)
                || (RuleHint == "Player2 No Board" && PlayerToCheck == EnumNowPlayer.Player2))
            {
                if (PolicyToCheck.PlayerAction == NowAction.Action_PlaceHorizontalBoard
                    || PolicyToCheck.PlayerAction == NowAction.Action_PlaceVerticalBoard)
                {
                    return RuleHint;
                }
            }
            if (RuleHint != "OK")
            {
                return RuleHint;
            }
            return "OK";
        }
        /// <summary>
        /// 处理一次决策（主要是更新游戏状态）
        /// </summary>
        /// <param name="Player">当前决策玩家</param>
        /// <returns>错误或胜利提示符，“OK”为决策完成，带有“Succes”的代表胜利</returns>
        public string DoOncePolicy(ref EnumNowPlayer Player)
        {
            Enum_PlayerType PlayerType = P1Type;
            if (Player == EnumNowPlayer.Player2)
            { 
                PlayerType = P2Type; 
            }

            string GetPolicyErrorHint = "";
            QuoridorAction NextPolicy = GetNextPolicy(PlayerType, Player, out GetPolicyErrorHint);
            if (GetPolicyErrorHint != "OK")
            {
                return GetPolicyErrorHint;
            }
            string CheckResultHintStr = CheckPolicy(NextPolicy, Player);
            if (CheckResultHintStr != "OK")
            {
                return CheckResultHintStr;
            }
            string ActionErrorHint = "";
            ActionErrorHint = QuoridorEva.QuoridorRule.Action(ref QuoridorEva.ThisChessBoard
                , NextPolicy.ActionPoint.X, NextPolicy.ActionPoint.Y, NextPolicy.PlayerAction);
            if (ActionErrorHint != "OK")
            {
                return ActionErrorHint;
            }

            string result = QuoridorEva.QuoridorRule.CheckResult(QuoridorEva.ThisChessBoard);
            if (result != "No success")
            {
                return result;
            }

            #region 更新对局信息
            if (Player == EnumNowPlayer.Player1)
            {
                if (NextPolicy.PlayerAction == NowAction.Action_PlaceVerticalBoard
                    || NextPolicy.PlayerAction == NowAction.Action_PlaceHorizontalBoard)
                    QuoridorEva.ThisChessBoard.NumPlayer1Board -= 2;
            }
            else
            {
                if (NextPolicy.PlayerAction == NowAction.Action_PlaceVerticalBoard
                    || NextPolicy.PlayerAction == NowAction.Action_PlaceHorizontalBoard)
                    QuoridorEva.ThisChessBoard.NumPlayer2Board -= 2;
            }
            Player = QuoridorEva.ReversePlayer(Player);
            #endregion
            return "OK";
        }

        public QuoridorGame() { }
        public QuoridorGame(Enum_PlayerType Player1Type, Enum_PlayerType Player2Type
            , QuoridorDecisionSystem P1DS, QuoridorDecisionSystem P2DS)
        {
            P1Type = Player1Type;
            P2Type = Player2Type;
            P1DecisionSystem = P1DS;
            P2DecisionSystem = P2DS;
        }
    }
}
