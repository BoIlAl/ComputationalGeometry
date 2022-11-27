using System;
using System.Drawing;
using System.Numerics;

namespace MinDisk {
    internal readonly struct UPoint {
        private readonly long _x, _y;
        public UPoint(long x, long y) { _x = x; _y = y; }
        public long X { get { return _x; } }
        public long Y { get { return _y; } }

        static public bool operator == (UPoint p, UPoint q) {
            if (p.X == q.X && p.Y == q.Y) {
                return true;
            }
            return false;
        }
        static public bool operator != (UPoint p, UPoint q) {
            if (p == q) {
                return false;
            }
            return true;
        }

        public override string ToString() {
            return String.Format("({0}, {1})", _x, _y);
        }
    }

    internal class UCircle {
        private readonly UPoint _x1, _x2, _x3;
        private long _A, _B, _C, _D;
        private readonly bool _defByOne, _defByTwo;

        public static UCircle Create(UPoint x1, UPoint x2, UPoint x3) {
            if (x3 == x1 || x3 == x2) {
                if (x1 == x2) {
                    return new UCircle(x1);
                }
                else {
                    return new UCircle(x1, x2);
                }
            } 
            else if (x1 == x2) {
                return new UCircle(x1, x3);
            } 
            else if (Determ(x1, x2, x3) == 0){
                var s12 = (x1.X - x2.X) * (x1.X - x2.X) + (x1.Y - x2.Y) * (x1.Y - x2.Y);
                var s13 = (x1.X - x3.X) * (x1.X - x3.X) + (x1.Y - x3.Y) * (x1.Y - x3.Y);
                var s23 = (x2.X - x3.X) * (x2.X - x3.X) + (x2.Y - x3.Y) * (x2.Y - x3.Y);
                if (s12 > s13 && s12 > s23) {
                    return new UCircle(x1, x2);
                }
                else if (s13 > s23) {
                    return new UCircle(x1, x3);
                }
                else {
                    return new UCircle(x2, x3);
                }
            } 
            else {
                return new UCircle(x1, x2, x3);
            }
        }
        public static UCircle Create(UPoint x1, UPoint x2) {
            if (x1 == x2) {
                return new UCircle(x1);
            }
            else {
                return new UCircle(x1, x2);
            }
        }

        public static UCircle Create(UPoint x1) {
            return new UCircle(x1);
        }

        private UCircle(UPoint x1, UPoint x2, UPoint x3) {
            _defByOne = false;
            _defByTwo = false;
            _x1 = x1;
            if (Determ(x1, x2, x3) < 0) {
                _x2 = x3;
                _x3 = x2;
            }
            else {
                _x2 = x2;
                _x3 = x3;
            }
            CoeffCount();
        }
        private UCircle(UPoint x1, UPoint x2) {
            _defByTwo = true;
            _defByOne = false;
            _x1 = x1; 
            _x2 = x2;
            CoeffCount();
        }
        private UCircle(UPoint x1) {
            _defByTwo = false;
            _defByOne = true;
            _x1 = x1;
        }
        public UPoint[] GetPoints() {
            if (_defByTwo) {
                return new UPoint[] { _x1, _x2 };
            }
            else {
                return new UPoint[] { _x1, _x2, _x3 };
            }
        }

        private void CoeffCount() {
            if (_defByOne) {
                return;
            }
            if (_defByTwo) {

                _A = - _x1.X - _x2.X;
                 _B = - _x1.Y - _x2.Y;
                _C = 1;
                _D = _x1.X * _x2.X + _x2.Y * _x1.Y;
            }
            else {
                /*_A = _x1.Y * (_x2.X * _x2.X + _x2.Y * _x2.Y) +
                     _x3.Y * (_x1.X * _x1.X + _x1.Y * _x1.Y) +
                     _x2.Y * (_x3.X * _x3.X + _x3.Y * _x3.Y) -
                     _x3.Y * (_x2.X * _x2.X + _x2.Y * _x2.Y) -
                     _x1.Y * (_x3.X * _x3.X + _x3.Y * _x3.Y) -
                     _x2.Y * (_x1.X * _x1.X + _x1.Y * _x1.Y);*/

                _A = (_x1.X * _x1.X + _x1.Y * _x1.Y) * (_x3.Y - _x2.Y) +
                     (_x2.X * _x2.X + _x2.Y * _x2.Y) * (_x1.Y - _x3.Y) +
                     (_x3.X * _x3.X + _x3.Y * _x3.Y) * (_x2.Y - _x1.Y);

                /*_B = _x1.X * (_x2.X * _x2.X + _x2.Y * _x2.Y) +
                     _x3.X * (_x1.X * _x1.X + _x1.Y * _x1.Y) +
                     _x2.X * (_x3.X * _x3.X + _x3.Y * _x3.Y) -
                     _x3.X * (_x2.X * _x2.X + _x2.Y * _x2.Y) -
                     _x1.X * (_x3.X * _x3.X + _x3.Y * _x3.Y) -
                     _x2.X * (_x1.X * _x1.X + _x1.Y * _x1.Y);*/

                _B = (_x1.X * _x1.X + _x1.Y * _x1.Y) * (_x2.X - _x3.X) +
                     (_x2.X * _x2.X + _x2.Y * _x2.Y) * (_x3.X - _x1.X) +
                     (_x3.X * _x3.X + _x3.Y * _x3.Y) * (_x1.X - _x2.X);

                _C = Determ(_x1, _x2, _x3);

                /*_D = -(_x1.X * _x2.Y * (_x3.X * _x3.X + _x3.Y * _x3.Y) +
                       _x2.X * _x3.Y * (_x1.X * _x1.X + _x1.Y * _x1.Y) +
                       _x3.X * _x1.Y * (_x2.X * _x2.X + _x2.Y * _x2.Y) -
                       _x3.X * _x2.Y * (_x1.X * _x1.X + _x1.Y * _x1.Y) -
                       _x2.X * _x1.Y * (_x3.X * _x3.X + _x3.Y * _x3.Y) -
                       _x1.X * _x3.Y * (_x2.X * _x2.X + _x2.Y * _x2.Y));*/

                _D = (_x1.X * _x1.X + _x1.Y * _x1.Y) * (_x3.X * _x2.Y - _x2.X * _x3.Y) +
                     (_x2.X * _x2.X + _x2.Y * _x2.Y) * (_x1.X * _x3.Y - _x3.X * _x1.Y) +
                     (_x3.X * _x3.X + _x3.Y * _x3.Y) * (_x1.Y * _x2.X - _x1.X * _x2.Y);
            }
        }

        static public long Determ(UPoint x1, UPoint x2, UPoint x3) {
            return x1.X * (x2.Y - x3.Y) +
                   x2.X * (x3.Y - x1.Y) +
                   x3.X * (x1.Y - x2.Y);
        }

        public bool IsInside(UPoint point) {
            if (_defByOne) {
                return _x1 == point;
            } else {
                return (_A * point.X + _B * point.Y + _C * (point.X * point.X + point.Y * point.Y) + _D) <= 0;
            }
        }

        public override string ToString() {
            if (_defByTwo) {
                return String.Format("({0}, {1})", _x1, _x2);
            }
            else {
                return String.Format("({0}, {1}, {2})", _x1, _x2, _x3);
            }
        }
    }
}
