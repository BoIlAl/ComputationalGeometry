using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CHBuilder;

enum GenType {
    GT_square,
    GT_circle,
    GT_rhombus
}

internal sealed class Program {
    static readonly uint s_nPoints = 10;
    static readonly long s_minValue = -1073741823;
    static readonly long s_maxValue = 1073741823;
    static string _inputPath = "";
    public static void Main(string[] args) {
        Point[] points = new Point[0];
        _inputPath = Directory.GetCurrentDirectory() + @"\input.txt";

        switch (args.Length) {
            case 0:
                GenerateInput(GenType.GT_square);
                points = ReadInput(_inputPath);
                break;
            case 1:
                points = ReadInput(args[0]);
                break;
            case 2:
                points = ReadInput(args[0]);
                break;
            default:
                Console.WriteLine("Incorrect program args");
                Console.WriteLine("Expected: inputPath.txt outputPath.txt or inputPath.txt or nothing");
                return;
        }
        if (points == null || points.Length == 0) {
            Console.WriteLine("Empty Input");
            return;
        }

#if DEBUG
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
#endif
        var answer = CHBuider.ConvexHull(points, true);

#if DEBUG
        stopWatch.Stop();
#endif

        if (args.Length == 2) {
            try {
                using (FileStream fs = File.Create(args[1])) {
                    StringBuilder sb = new StringBuilder();
                    foreach (var num in answer) {
                        sb.Append(num.ToString() + " ");
                    }
                    byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Problem with output file");
                Console.WriteLine(ex.Message);
                return;
            }
        }
        else {
            foreach (var ind in answer) {
                Console.Write(ind.ToString() + " ");
            }
            Console.Write("\n");
        }

#if DEBUG
        TimeSpan ts = stopWatch.Elapsed;

        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine(elapsedTime);
#endif

#if DEBUG
        int nTests = 50;
        List<List<Point[]>> tests = new List<List<Point[]>>();
        for (int N = 1000; N <= 128000; N *= 2) {
            tests.Add(GenerateTests(N, nTests));
        }

        foreach (var test in tests) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<int> ans = new List<int>();
            foreach (var pnts in test) {
                ans = CHBuider.ConvexHull(pnts, true);
            }
            foreach (var ind in ans) {
                Console.Write(ind.ToString() + " ");
            }
            Console.Write("\n");
            sw.Stop();
            var time = (double)sw.ElapsedMilliseconds / nTests;
            Console.WriteLine(time.ToString());
        }

#endif
        Console.WriteLine();
    }

    private static List<Point[]> GenerateTests(int N, int nTests) {
        var tests = new List<Point[]>();
        Random rnd = new Random();
        for (int i = 0; i < nTests; i++) {
            List<Point> points = new List<Point>();
            for (int k = 0; k < N; k++) {
                var nextX = rnd.NextInt64(s_maxValue - s_minValue) + s_minValue;
                var nextY = rnd.NextInt64(s_maxValue - s_minValue) + s_minValue;

                points.Add(new Point(nextX, nextY));
            }
            tests.Add(points.ToArray());
        }
        return tests;
    }

    private static void GenerateInput(GenType gt) {
        try {
            Random rnd = new Random();
            if (File.Exists(_inputPath)) {
                File.Delete(_inputPath);
            }
            using (FileStream fs = File.Create(_inputPath)) {
                StringBuilder sb = new StringBuilder();
                switch (gt) {
                    case (GenType.GT_square):
                        for (int i = 0; i < s_nPoints; i++) {
                            var nextX = (rnd.NextInt64(s_maxValue - s_minValue) + s_minValue).ToString();
                            var nextY = (rnd.NextInt64(s_maxValue - s_minValue) + s_minValue).ToString();
                            sb.Append(nextX + " " + nextY + "\n");
                        }
                        break;
                    case (GenType.GT_circle):
                        for (int i = 0; i < s_nPoints; i++) {
                            var nextX = rnd.NextInt64(s_maxValue - s_minValue) + s_minValue;
                            var nextY = rnd.NextInt64(s_maxValue - s_minValue) + s_minValue;
                            if (nextX * nextX + nextY * nextY > s_maxValue * s_maxValue) {
                                --i;
                                continue;
                            }
                            sb.Append(nextX.ToString() + " " + nextY.ToString() + "\n");
                        }
                        break;
                    case (GenType.GT_rhombus):
                        for (int i = 0; i < s_nPoints; i++) {

                        }
                        break;
                    default:
                        throw new Exception("Invalid generation type param");
                }
                for (int i = 0; i < s_nPoints; i++) {


                    var nextX = (rnd.NextInt64(s_maxValue - s_minValue) + s_minValue).ToString();
                    var nextY = (rnd.NextInt64(s_maxValue - s_minValue) + s_minValue).ToString();
                    sb.Append(nextX + " " + nextY + "\n");
                }

                byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Can`t auto generate input file");
            Console.WriteLine(ex.Message);
        }
    }

    private static Point[] ReadInput(string input) {
        List<Point> points = new List<Point>();
        try {
            using (StreamReader sr = new StreamReader(input)) {
                string numbers;
                while ((numbers = sr.ReadLine()) != null) {
                    var splt = numbers.Split();
                    long x, y;
                    if (splt.Length != 2 || !long.TryParse(splt[0], out x) || !long.TryParse(splt[1], out y)) {
                        throw new Exception("invalid line:" + numbers);
                    }
                    points.Add(new Point(x, y));
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Problem with input file");
            Console.WriteLine(ex.Message);
        }
        return points.ToArray();
    }
}