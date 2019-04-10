using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Queen
{
    /// <summary>
    /// 八皇后求解类
    /// </summary>
    public class QueenSolve
    {
        public List<Point> ChessLocationList = new List<Point>();//棋子位置列表
        public List<Point> QueenLocationList = new List<Point>();//八皇后结果列表

        public enum DistanceCalMethod
        {
            ManhattanDistance,
            EuclideanDistance,
            ManhattanDistance_Diagonal
        }
        public DistanceCalMethod UsedCalMethod = DistanceCalMethod.ManhattanDistance;
        double DiagonalWalkTimeRate = Math.Sqrt(2);//斜走一个距离放大系数
        double StraightWalkTimeRate = 1;//直走一个距离放大系数

        /// <summary>
        /// 计算棋子和皇后位置之间的距离
        /// </summary>
        /// <param name="Chess">棋子位置点</param>
        /// <param name="Queen">皇后位置点</param>
        /// <param name="CalMethod">距离计算方式（曼哈顿、欧式、带斜走的曼哈顿）</param>
        /// <returns>两点间的距离</returns>
        public double CalDistance_QueenToChess(Point Chess, Point Queen, DistanceCalMethod CalMethod)
        {
            int x1 = Chess.X, y1 = Chess.Y;
            int x2 = Queen.X, y2 = Queen.Y;
            switch (CalMethod)
            {
                case DistanceCalMethod.ManhattanDistance:
                    return Convert.ToDouble(Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
                case DistanceCalMethod.EuclideanDistance:
                    double sum_pingfang = x1 * x1 + y1 * y1;
                    return Math.Sqrt(Convert.ToDouble(sum_pingfang));
                case DistanceCalMethod.ManhattanDistance_Diagonal:
                    int x0 = Math.Abs(x1 - x2);
                    int y0 = Math.Abs(y1 - y2);
                    double distancebuff = Convert.ToDouble(x0 + y0);
                    if (x0 == y0)
                    {
                        return DiagonalWalkTimeRate * Convert.ToDouble(x0);
                    }
                    else if (x0 < y0)
                    {
                        double DiagonalDisbuff = x0 * DiagonalWalkTimeRate;
                        return StraightWalkTimeRate * (distancebuff - 2 * x0) + DiagonalDisbuff;
                    }
                    else
                    {
                        double DiagonalDisbuff = y0 * DiagonalWalkTimeRate;
                        return StraightWalkTimeRate * (distancebuff - 2 * y0) + DiagonalDisbuff; 
                    }
                default:
                    return 999;
            }
        }

        List<int> InitResult = new List<int>();//其中10~17代表第0~7个棋子，20~27代表第0~7个皇后

        public enum InitResultMethod///初始解计算方法
        {
            Dijkstra,//迪杰斯特拉算法
            Other
        }
        public InitResultMethod UsedInitResultMethod = InitResultMethod.Dijkstra;
        /// <summary>
        /// 创建初始解
        /// </summary>
        /// <param name="ChessList">棋子位置列表</param>
        /// <param name="QueenList">皇后位置列表</param>
        /// <param name="Method">计算初始解的方法</param>
        /// <param name="Distance_All">总距离</param>
        /// <returns>路径移动序列,其中10~17代表第0~7个棋子，20~27代表第0~7个皇后</returns>
        public List<int> CreateInitResult(List<Point> ChessList, List<Point> QueenList, ref double Distance_All)
        {
            List<int> MoveSequence = new List<int>();

            if (UsedInitResultMethod == InitResultMethod.Dijkstra)
            {
                for (int i = 0; i < ChessList.Count; i++)
                {
                    double disbuff = 0;
                    double dismin = 999;
                    int minindex = 0;
                    for (int j = 0; j < QueenList.Count; j++)
                    {
                        disbuff = CalDistance_QueenToChess(ChessList[i], QueenList[j], UsedCalMethod);
                        if (disbuff < dismin && !MoveSequence.Contains(20 + j))
                        {
                            dismin = disbuff;
                            minindex = j;
                        }
                    }
                    MoveSequence.Add(10 + i);
                    MoveSequence.Add(20 + minindex);
                }
            }

            Distance_All = CalMoveSequenceDistance(MoveSequence, ChessList, QueenList);

            return MoveSequence;
        }
        /// <summary>
        /// 计算一个移动序列的总距离
        /// </summary>
        /// <param name="MoveSequence">移动序列列表</param>
        /// <param name="ChessList">棋子位置列表</param>
        /// <param name="QueenList">皇后位置列表</param>
        /// <returns>移动序列的总距离</returns>
        public double CalMoveSequenceDistance(List<int> MoveSequence, List<Point> ChessList, List<Point> QueenList)
        {
            double dis_all = 0;

            for (int i = 0; i < MoveSequence.Count - 1; i++)
            {
                if (i % 2 == 0)
                {
                    dis_all += CalDistance_QueenToChess(
                                ChessList[MoveSequence[i] % 10]
                                , QueenList[MoveSequence[i + 1] % 10]
                                , UsedCalMethod);
                }
                else
                {
                    dis_all += CalDistance_QueenToChess(
                                QueenList[MoveSequence[i] % 10]
                                , ChessList[MoveSequence[i + 1] % 10]
                                , UsedCalMethod); 
                }
            }

            return dis_all;
        }
        public QueenSolve(DistanceCalMethod SetCalMethod, InitResultMethod InitMethod)
        {
            UsedCalMethod = SetCalMethod;
            UsedInitResultMethod = InitMethod;
        }
        public void PrintMoveSequence(List<int> MoveSequence, List<Point> ChessList, List<Point> QueenList)
        {
            for (int i = 0; i < MoveSequence.Count; i++)
            {
                Console.Write("地点" + (i+1).ToString() + ": ");
                if (i % 2 == 0)
                {
                    Console.Write("Chess----(" + ChessList[MoveSequence[i] % 10].X.ToString()
                        + "," + ChessList[MoveSequence[i] % 10].Y.ToString() + ")");
                }
                else
                {
                    Console.Write("Queen----(" + QueenList[MoveSequence[i] % 10].X.ToString()
                        + "," + QueenList[MoveSequence[i] % 10].Y.ToString() + ")");
                }
                Console.WriteLine();
            }
        }
    }
}
