using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quoridor;
using System.Drawing;
using Quoridor_With_C;
using QuoridorRule;
using NowAction = QuoridorRule.QuoridorRuleEngine.NowAction;
using EnumNowPlayer = QuoridorRule.QuoridorRuleEngine.EnumNowPlayer;
using System.Collections;

namespace LookupRoad
{
    /// <summary>
    /// 寻路算法类
    /// </summary>
    public class LookupRoadAlgorithm
    {
        public static LookupRoadResultTable ResultSaveTable = new LookupRoadResultTable();
        /// <summary>
        /// 计算两点间的曼哈顿距离
        /// </summary>
        /// <param name="P0">第一个点</param>
        /// <param name="P1">第二个点</param>
        /// <returns>曼哈顿距离</returns>
        public int CalDistance_Manhattan(Point P0, Point P1)
        {
            return Math.Abs(P0.X - P1.X) + Math.Abs(P0.Y - P1.Y);
        }
        public int Min_DistanceLength = 0;///最小路径
        public bool Astar_Stop = false;
        public class AstarList
        {
            public int H = 0;
            public int G = 0;
            public int F = 999;
            public int Grid_row = -1;
            public int Grid_col = -1;
            public AstarList(int HH, int GG, int FF, int row, int col)
            {
                H = HH;
                G = GG;
                F = FF;
                Grid_row = row;
                Grid_col = col;
            }
            public AstarList Father;
        }
        /// <summary>
        /// 重启A*寻路，最短路径保存在Player1MinRoad或Player2MinRoad中，返回最短路径长度
        /// </summary>
        /// <param name="Player">寻路玩家</param>
        /// <param name="Location_row">该玩家所在位置行</param>
        /// <param name="Location_col">该玩家所在位置列</param>
        /// <returns>最短路径长度</returns>
        public int AstarRestart(ChessBoard ToAstarSearch, EnumNowPlayer Player, int Location_row, int Location_col)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            /***************待测代码段****************/

            Min_DistanceLength = 999;
            List<AstarList> InitAList = new List<AstarList>();
            Astar_Stop = false;

            AstarList InitGrid = new AstarList(6, 0, 6, Location_row, Location_col);
            InitAList.Add(InitGrid);

            int distance = LookupRoad_Astar(ToAstarSearch, Player, InitGrid, 1,
                new List<AstarList>(), InitAList);

            /***************待测代码段****************/
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间

            if (QuoridorAI.AIRunTime.Astar_s == 0)
            {
                QuoridorAI.AIRunTime.Astar_s = timespan.TotalMilliseconds;
            }
            else
            {
                QuoridorAI.AIRunTime.Astar_s += timespan.TotalMilliseconds;
                QuoridorAI.AIRunTime.Astar_s /= 2;
            }
            QuoridorAI.AIRunTime.AstarNum++;

            return Min_DistanceLength;

        }
        public List<Point> Player1MinRoad = new List<Point>();
        public List<Point> Player2MinRoad = new List<Point>();
        /// <summary>
        /// A*寻路，最短路径保存在Player1MinRoad或Player2MinRoad中，返回最短路径长度
        /// </summary>
        /// <param name="Player">寻路玩家</param>
        /// <param name="NowGrid">当前寻路的格子</param>
        /// <param name="num_renew">迭代次数</param>
        /// <param name="OpenList">Open列表</param>
        /// <param name="CloseList">Close列表</param>
        /// <returns></returns>
        public int LookupRoad_Astar(ChessBoard ThisChessBoard, EnumNowPlayer Player, AstarList NowGrid, int num_renew, List<AstarList> OpenList, List<AstarList> CloseList)
        {
            int Location_row = NowGrid.Grid_row;
            int Location_col = NowGrid.Grid_col;

            if (Astar_Stop == true)
                return Min_DistanceLength;

            int Row_Destination = 0;
            #region 设置目的地行
            switch (Player)
            {
                case EnumNowPlayer.Player1:
                    Row_Destination = 6;
                    break;
                case EnumNowPlayer.Player2:
                    Row_Destination = 0;
                    break;
                default:
                    break;
            }
            #endregion

            #region 检查四周能移动的位置添加进P_List_Enable列表
            //计算四周能移动的位置的距离
            List<Point> P_List_Enable = new List<Point>();
            //左
            if (Location_col > 0
                && !(ThisChessBoard.ChessBoardAll[Location_row, Location_col].IfLeftBoard))
                P_List_Enable.Add(new Point(Location_row, Location_col - 1));
            //右
            if (Location_col < 6
                && !(ThisChessBoard.ChessBoardAll[Location_row, Location_col + 1].IfLeftBoard))
                P_List_Enable.Add(new Point(Location_row, Location_col + 1));
            //上
            if (Location_row > 0
                && !(ThisChessBoard.ChessBoardAll[Location_row, Location_col].IfUpBoard))
                P_List_Enable.Add(new Point(Location_row - 1, Location_col));
            //下
            if (Location_row < 6
                && !(ThisChessBoard.ChessBoardAll[Location_row + 1, Location_col].IfUpBoard))
                P_List_Enable.Add(new Point(Location_row + 1, Location_col));
            #endregion

            #region 上下扫描是否有木板，用来减少搜索空间
            bool flag_NoBoard = true;
            bool flag_UpNowBoard = true;
            bool flag_DownNowBoard = true;

            for (int k = Location_row + 1; k <= Row_Destination; k++)//下扫
            {
                if (ThisChessBoard.ChessBoardAll[k, Location_col].IfUpBoard)
                {
                    flag_DownNowBoard = false;
                    break;
                }
            }
            for (int k = Location_row - 1; k >= Row_Destination; k--)//上扫
            {
                if (ThisChessBoard.ChessBoardAll[k + 1, Location_col].IfUpBoard)
                {
                    flag_UpNowBoard = false;
                    break;
                }
            }
            if (flag_DownNowBoard && flag_UpNowBoard)
                flag_NoBoard = true;
            else
                flag_NoBoard = false;

            if (flag_NoBoard)
            {
                Astar_Stop = true;
                Min_DistanceLength = Math.Abs((Row_Destination - Location_row)) + CloseList.Last().G;

                #region 迭代寻找最短路径
                List<Point> MinRoad;
                if (Player == EnumNowPlayer.Player1)
                {
                    Player1MinRoad = new List<Point>();
                    MinRoad = Player1MinRoad;
                }
                else
                {
                    Player2MinRoad = new List<Point>();
                    MinRoad = Player2MinRoad;
                }

                if (Location_row < Row_Destination)
                {
                    for (int i = Row_Destination; i >= Location_row; i--)
                    {
                        MinRoad.Add(new Point(i, Location_col));
                    }
                }
                else
                {
                    for (int i = Row_Destination; i <= Location_row; i++)
                    {
                        MinRoad.Add(new Point(i, Location_col));
                    }
                }
                AstarList ALBuff = CloseList.Last();
                while (true)
                {
                    if (ALBuff.Father != null)
                    {
                        MinRoad.Add(new Point(ALBuff.Father.Grid_row, ALBuff.Father.Grid_col));
                        ALBuff = ALBuff.Father;
                    }
                    else
                        break;
                }
                #endregion
                return Min_DistanceLength;
            }

            #endregion

            #region 搜索树搜索策略——A*算法
            List<int> P_Dis = new List<int>();
            for (int i = 0; i < P_List_Enable.Count; i++)
            {
                P_Dis.Add(999);
            }
            int minF = 9999;
            int minindex = 0;
            for (int i = 0; i < P_List_Enable.Count && i >= 0; i++)
            {
                int Hbuff = Math.Abs(P_List_Enable[i].X - Row_Destination);
                P_Dis[i] = Hbuff;
                int Gbuff = num_renew;
                int Fbuff = Hbuff + Gbuff;
                bool flag_InClose = false;
                //检测是否在Close列表里
                for (int j = 0; j < CloseList.Count; j++)
                {
                    if (P_List_Enable[i].X == CloseList[j].Grid_row && P_List_Enable[i].Y == CloseList[j].Grid_col)
                    {
                        P_List_Enable.Remove(P_List_Enable[i]);
                        P_Dis.Remove(P_Dis[i]);
                        i--;
                        flag_InClose = true;
                        break;
                    }
                }
                if (flag_InClose)
                {
                    continue;
                }

                bool flag_InOpen = false;
                //检测是否在Open列表里
                for (int j = 0; j < OpenList.Count; j++)
                {
                    if (P_List_Enable[i].X == OpenList[j].Grid_row && P_List_Enable[i].Y == OpenList[j].Grid_col)
                    {
                        P_List_Enable.Remove(P_List_Enable[i]);
                        P_Dis.Remove(P_Dis[i]);
                        i--;
                        flag_InOpen = true;

                        if (Gbuff < OpenList[j].G)
                        {
                            OpenList[j].G = Gbuff;
                            OpenList[j].F = Fbuff;
                            OpenList[j].H = Hbuff;
                            OpenList[j].Father = NowGrid;
                        }
                        break;
                    }
                }

                if (!flag_InOpen && !flag_InClose)
                {
                    AstarList NewGrid = new AstarList(Hbuff, Gbuff, Fbuff, P_List_Enable[i].X, P_List_Enable[i].Y);
                    NewGrid.Father = NowGrid;
                    OpenList.Add(NewGrid);
                }

            }
            AstarList MinFGrid = new AstarList(-1, -1, -1, -1, -1);
            for (int i = 0; i < OpenList.Count; i++)
            {
                int Fbuff = OpenList[i].F;
                if (Fbuff < minF)
                {
                    minF = Fbuff;
                    minindex = i;
                    MinFGrid = OpenList[i];
                }
            }
            CloseList.Add(MinFGrid);

            int dislengthbuff = 0;
            if (MinFGrid.Grid_row == Row_Destination && Astar_Stop == false)
            {
                Min_DistanceLength += MinFGrid.G;
                Astar_Stop = true;
                return Min_DistanceLength;
            }
            else
            {
                if (OpenList.Count > 0)
                {
                    OpenList.Remove(MinFGrid);
                    dislengthbuff = LookupRoad_Astar(ThisChessBoard, Player, MinFGrid, MinFGrid.G + 1, OpenList, CloseList);
                }
                else
                    return 999;
            }
            #endregion

            return dislengthbuff;
        }
        /// <summary>
        /// 寻找路径长度——贪婪方法
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="Location_row"></param>
        /// <param name="Location_col"></param>
        /// <returns></returns>
        public int LookupRoad_Greedy(ChessBoard ThisChessBoard, EnumNowPlayer Player, int Location_row, int Location_col, List<Point> MovedPoint)
        {
            int Row_Destination = 0;
            #region 设置目的地行
            switch (Player)
            {
                case EnumNowPlayer.Player1:
                    Row_Destination = 6;
                    break;
                case EnumNowPlayer.Player2:
                    Row_Destination = 0;
                    break;
                default:
                    break;
            }
            #endregion
            #region 上下扫描是否有木板，用来减少搜索空间
            bool flag_NoBoard = true;
            bool flag_UpNowBoard = true;
            bool flag_DownNowBoard = true;

            for (int i = Location_row + 1; i <= Row_Destination; i++)//下扫
            {
                if (ThisChessBoard.ChessBoardAll[i, Location_col].IfUpBoard)
                {
                    flag_DownNowBoard = false;
                    break;
                }
            }
            for (int i = Location_row - 1; i >= Row_Destination; i--)//上扫
            {
                if (ThisChessBoard.ChessBoardAll[i + 1, Location_col].IfUpBoard)
                {
                    flag_UpNowBoard = false;
                    break;
                }
            }
            if (flag_DownNowBoard && flag_UpNowBoard)
                flag_NoBoard = true;
            else
                flag_NoBoard = false;

            if (flag_NoBoard)
                return Math.Abs((Row_Destination - Location_row));

            #endregion
            #region 检查四周能移动的位置添加进P_List_Enable列表
            //计算四周能移动的位置的距离
            List<Point> P_List_Enable = new List<Point>();
            //左
            if (Location_col > 0
                && !(ThisChessBoard.ChessBoardAll[Location_row, Location_col].IfLeftBoard)
                && !MovedPoint.Contains(new Point(Location_row, Location_col - 1)))
                P_List_Enable.Add(new Point(Location_row, Location_col - 1));
            //右
            if (Location_col < 6
                && !(ThisChessBoard.ChessBoardAll[Location_row, Location_col + 1].IfLeftBoard)
                && !MovedPoint.Contains(new Point(Location_row, Location_col + 1)))
                P_List_Enable.Add(new Point(Location_row, Location_col + 1));
            //上
            if (Location_row > 0
                && !(ThisChessBoard.ChessBoardAll[Location_row, Location_col].IfUpBoard)
                && !MovedPoint.Contains(new Point(Location_row - 1, Location_col)))
                P_List_Enable.Add(new Point(Location_row - 1, Location_col));
            //下
            if (Location_row < 6
                && !(ThisChessBoard.ChessBoardAll[Location_row + 1, Location_col].IfUpBoard)
                && !MovedPoint.Contains(new Point(Location_row + 1, Location_col)))
                P_List_Enable.Add(new Point(Location_row + 1, Location_col));
            if (P_List_Enable.Count == 0)
                return 999;
            #endregion

            #region 搜索树搜索策略——贪婪
            int[] P_Dis = new int[P_List_Enable.Count];
            int mindis = 999;
            int minindex = 0;
            //选择距离最近的点尝试
            for (int i = 0; i < P_List_Enable.Count; i++)
            {
                P_Dis[i] = CalDistance(ThisChessBoard, Player, P_List_Enable[i].X, P_List_Enable[i].Y);
                if (P_Dis[i] < mindis)
                {
                    mindis = P_Dis[i];
                    minindex = i;
                }
            }

            MovedPoint.Add(new Point(P_List_Enable[minindex].X, P_List_Enable[minindex].Y));
            int dissum = 0;
            int disbuff = LookupRoad_Greedy(ThisChessBoard, Player, P_List_Enable[minindex].X, P_List_Enable[minindex].Y, MovedPoint);

            #endregion

            if (disbuff != 999)
            {
                dissum += disbuff;
                return dissum;
            }

            return 999;
        }
        /// <summary>
        /// 计算某点的距离
        /// </summary>
        /// <param name="Player">要检测哪个玩家会被这步挡板堵死</param>
        /// <param name="Location_row">某点的行</param>
        /// <param name="Location_col">某点的列</param>
        /// <returns></returns>
        public int CalDistance(ChessBoard ThisChessBoard, EnumNowPlayer Player, int Location_row, int Location_col)
        {
            int Row_Destination = 0;

            switch (Player)
            {
                case EnumNowPlayer.Player1:
                    Row_Destination = 6;
                    break;
                case EnumNowPlayer.Player2:
                    Row_Destination = 0;
                    break;
                default:
                    break;
            }

            int Num_VBoard = 0, Num_HBoard = 0;
            for (int i = Location_row + 1; i <= Row_Destination; i++)//下扫
            {
                if (ThisChessBoard.ChessBoardAll[i, Location_col].IfUpBoard)
                    Num_HBoard++;
            }
            for (int i = Location_row - 1; i >= Row_Destination; i--)//上扫
            {
                if (ThisChessBoard.ChessBoardAll[i + 1, Location_col].IfUpBoard)
                    Num_HBoard++;
            }
            for (int i = Location_col - 1; i >= 0; i--)//左扫
            {
                if (ThisChessBoard.ChessBoardAll[Location_row, i + 1].IfLeftBoard)
                    Num_VBoard++;
            }
            for (int i = Location_col + 1; i <= 6; i++)//右扫
            {
                if (ThisChessBoard.ChessBoardAll[Location_row, i - 1].IfLeftBoard)
                    Num_VBoard++;
            }

            return Num_HBoard + Num_VBoard;
        }
    }
    /// <summary>
    /// 保存已寻路过的局面的寻路结果存储表（哈希表）
    /// </summary>
    public class LookupRoadResultTable
    {
        /// <summary>
        /// 结果存储表特定的哈希值类型
        /// </summary>
        public class HashCode_RT
        {
            public Int64 VerticalHashCode = 0;//竖挡板地图哈希值
            public Int64 HorizontalHashCode = 0;//横挡板地图哈希值
            public HashCode_RT(Int64 VerticalHash, Int64 HorizontalHash)
            {
                VerticalHashCode = VerticalHash;
                HorizontalHashCode = HorizontalHash;
            }
            public HashCode_RT(HashCode_RT CopyHashCode)
            {
                VerticalHashCode = CopyHashCode.VerticalHashCode;
                HorizontalHashCode = CopyHashCode.HorizontalHashCode;
            }
        }
        public Hashtable ResultTable = new Hashtable();
        /// <summary>
        /// 寻路结果类
        /// </summary>
        public class LookRoadResult
        {
            int Player1Distance = 0;
            int Player2Distance = 0;
        }
        /// <summary>
        /// 添加一个散列映射
        /// </summary>
        /// <param name="HashCode">要添加的哈希值</param>
        /// <param name="Result">寻路结果对象</param>
        public void Add(HashCode_RT HashCode, LookRoadResult Result)
        {
            ResultTable.Add(HashCode, Result);
        }
        /// <summary>
        /// 通过地图挡板的哈希值检索出寻路结果
        /// </summary>
        /// <param name="HashCode">哈希值</param>
        /// <param name="Result">寻路结果</param>
        /// <returns>是否含有此哈希值</returns>
        public bool Search(HashCode_RT HashCode, ref LookRoadResult Result)
        {
            if (ResultTable.Contains(HashCode))
            {
                Result = (LookRoadResult)ResultTable[HashCode];
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据当前动作更新挡板地图哈希值
        /// </summary>
        /// <param name="HashCode_Last">上一次的哈希值</param>
        /// <param name="NA">当前动作</param>
        /// <param name="ActionLocation_Row">动作落实位置行</param>
        /// <param name="ActionLocation_Col">动作落实位置列</param>
        /// <returns></returns>
        public HashCode_RT RenewHashCode(HashCode_RT HashCode_Last, NowAction NA, int ActionLocation_Row, int ActionLocation_Col)
        {
            HashCode_RT NewHashCode = new HashCode_RT(HashCode_Last);
            int BoardIndex1 = ActionLocation_Row * 7 + ActionLocation_Col;
            int BoardIndex2 = 0;
            if (NA == NowAction.Action_PlaceHorizontalBoard)
            {
                BoardIndex2 = ActionLocation_Row * 7 + ActionLocation_Col + 1;                
                NewHashCode.HorizontalHashCode += Convert.ToInt64((Math.Pow(2.0, Convert.ToDouble(BoardIndex1))));
                NewHashCode.HorizontalHashCode += Convert.ToInt64((Math.Pow(2.0, Convert.ToDouble(BoardIndex2))));
            }
            else if (NA == NowAction.Action_PlaceVerticalBoard)
            {
                BoardIndex2 = (ActionLocation_Row + 1) * 7 + ActionLocation_Col;
                NewHashCode.VerticalHashCode += Convert.ToInt64((Math.Pow(2.0, Convert.ToDouble(BoardIndex1))));
                NewHashCode.VerticalHashCode += Convert.ToInt64((Math.Pow(2.0, Convert.ToDouble(BoardIndex2))));
            }
            return NewHashCode;
        }
    }
}
