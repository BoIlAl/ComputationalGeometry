using System.Diagnostics.CodeAnalysis;

namespace MinDisk {
    internal static class MDSolver {
        public static int[] MinDisk(UPoint[] points, out double accuracy) {
            UPoint[] tmpPoints = (UPoint[])points.Clone();
            perturb(tmpPoints);
            UCircle circle = UCircle.Create(tmpPoints[0], tmpPoints[1]);
            for (int i = 2; i < tmpPoints.Length; i++) {
                if (!circle.IsInside(tmpPoints[i])) {
                    circle = MinDisk(tmpPoints.Take(i).ToArray(), tmpPoints[i]);
                }
            }
            #if DEBUG
                double sum = 0;
                for (int i = 0; i < points.Length; i++) {
                    if (circle.IsInside(points[i])) {
                        sum += 1;
                    }
                }
                accuracy = sum / points.Length;
                //Console.WriteLine(circle.ToString());
            #else
                accuracy = 1;
            #endif
            int i_p1 = -1, i_p2 = -1, i_p3 = -1;
            var answer = circle.GetPoints();
            for (int j = 0; j < points.Length; ++j) {
                foreach(var point in answer) {
                    if (points[j] == point) {
                        if (i_p1 < 0) {
                            i_p1 = j;
                        }
                        else if (i_p2 < 0) {
                            if (points[j] == points[i_p1]) {
                                continue;
                            }
                            i_p2 = j;
                        }
                        else {
                            if (points[j] == points[i_p1] || points[j] == points[i_p2]) {
                                continue;
                            }
                            i_p3 = j;
                        }
                    }
                }
            }
            if (i_p3 >= 0) {
                return new[] { i_p1, i_p2, i_p3 };
            } 
            else if (i_p2 >= 0) {
                return new[] { i_p1, i_p2 };
            }
            return new[] { i_p1 };
        }

        private static UCircle MinDisk(UPoint[] points, UPoint p) {
            perturb(points);
            UCircle circle = UCircle.Create(points[0], p);
            for (int i = 1; i < points.Length; i++) {
                if (!circle.IsInside(points[i])) {
                    circle = MinDisk(points.Take(i).ToArray(), p, points[i]); 
                }
            }
            return circle;
        }

        private static UCircle MinDisk(UPoint[] points, UPoint p, UPoint q) {
            perturb(points);
            UCircle circle = UCircle.Create(p, q);
            for (int i = 0; i < points.Length; i++) {
                if (!circle.IsInside(points[i])) {
                    circle = UCircle.Create(points[i], p, q);
                }
            }
            return circle;
        }

        private static void perturb(UPoint[] points) {
            if (points.Length == 0) {
                return;
            }

            var rand = new Random();

            for (int i = 0; i < points.Length; i++) {
                int k = (int)(rand.NextDouble() * points.Length);
                UPoint tmp = points[i];
                points[i] = points[k];
                points[k] = tmp;
            }
        }
    }
}
