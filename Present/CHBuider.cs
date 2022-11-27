using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHBuilder {
    internal static class CHBuider {
        public static List<int> ConvexHull(Point[] points, bool isMin) {
            List<int> answer = new List<int>();

            if (points.Length == 1) {
                answer.Add(0);
                return answer;
            }
            if (points.Length == 2) {
                answer.Add(0);
                if (points[0] != points[1]) {
                    answer.Add(1);
                }
                return answer;
            }

            int ind = FindExtreme(points);
            answer.Add(ind);

            do {
                int minAngle;
                if (ind == 0) {
                    minAngle = 1;
                } 
                else {
                    minAngle = 0;
                }

                for (int i = 0; i < points.Length; ++i) {
                    if (i == ind || i == minAngle) {
                        continue;
                    }
                    var ca = CompareAngles(points[ind], points[minAngle], points[i]);
                    if (ca < 0) {
                        minAngle = i;
                    } 
                    else if (ca == 0 && IsSecondFurther(points[ind], points[minAngle], points[i]) && isMin) {
                        minAngle = i;
                    }
                }
                ind = minAngle;
                if (ind == answer[0]) {
                    break;
                }
                answer.Add(ind);
            } while (true);

            if (answer.Count == 2 && points[answer[0]] == points[answer[1]]) {
                answer.Remove(1);
            }

            return answer;
        }

        private static int FindExtreme(Point[] points) {
            int ind = 0;
            for (int i = 1; i < points.Length; i++) {
                if (points[ind].Y > points[i].Y || (points[ind].Y == points[i].Y && points[ind].X > points[i].X)) {
                    ind = i;
                }
            }
            return ind;
        }

        private static long CompareAngles(Point q, Point p1, Point p2) {
            return q.X * (p1.Y - p2.Y) +
                   p1.X * (p2.Y - q.Y) +
                   p2.X * (q.Y - p1.Y);
        }

        private static bool IsSecondFurther(Point q, Point p1, Point p2) {
            return (q.X - p1.X) * (q.X - p1.X) + (q.Y - p1.Y) * (q.Y - p1.Y) <
                   (q.X - p2.X) * (q.X - p2.X) + (q.Y - p2.Y) * (q.Y - p2.Y);
        }
    }
}
