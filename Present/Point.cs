using System;
using System.Drawing;
using System.Numerics;

namespace CHBuilder {
    internal readonly struct Point {
        private readonly long _x, _y;
        public Point(long x, long y) { _x = x; _y = y; }
        public long X { get { return _x; } }
        public long Y { get { return _y; } }

        static public bool operator ==(Point p, Point q) {
            if (p.X == q.X && p.Y == q.Y) {
                return true;
            }
            return false;
        }
        static public bool operator !=(Point p, Point q) {
            if (p == q) {
                return false;
            }
            return true;
        }

        public override string ToString() {
            return String.Format("({0}, {1})", _x, _y);
        }
    }
}