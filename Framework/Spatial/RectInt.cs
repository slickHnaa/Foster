﻿using System;
using System.Runtime.CompilerServices;

namespace Foster.Framework
{
    /// <summary>
    /// A 2D Integer Rectangle
    /// </summary>
    public struct RectInt
    {

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Point2 Position
        {
            get => new Point2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Point2 Size
        {
            get => new Point2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public int Area => Width * Height;

        #region Edges

        public int Left
        {
            get => X;
            set => X = value;
        }

        public int Right
        {
            get => X + Width;
            set => X = value - Width;
        }

        public int CenterX
        {
            get => X + Width / 2;
            set => X = value - Width / 2;
        }

        public int Top
        {
            get => Y;
            set => Y = value;
        }

        public int Bottom
        {
            get => Y + Height;
            set => Y = value - Height;
        }

        public int CenterY
        {
            get => Y + Height / 2;
            set => Y = value - Height / 2;
        }

        #endregion

        #region Points

        public Point2 TopLeft
        {
            get => new Point2(Left, Top);
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public Point2 TopCenter
        {
            get => new Point2(CenterX, Top);
            set
            {
                CenterX = value.X;
                Top = value.Y;
            }
        }

        public Point2 TopRight
        {
            get => new Point2(Right, Top);
            set
            {
                Right = value.X;
                Top = value.Y;
            }
        }

        public Point2 CenterLeft
        {
            get => new Point2(Left, CenterY);
            set
            {
                Left = value.X;
                CenterY = value.Y;
            }
        }

        public Point2 Center
        {
            get => new Point2(CenterX, CenterY);
            set
            {
                CenterX = value.X;
                CenterY = value.Y;
            }
        }

        public Point2 CenterRight
        {
            get => new Point2(Right, CenterY);
            set
            {
                Right = value.X;
                CenterY = value.Y;
            }
        }

        public Point2 BottomLeft
        {
            get => new Point2(Left, Bottom);
            set
            {
                Left = value.X;
                Bottom = value.Y;
            }
        }

        public Point2 BottomCenter
        {
            get => new Point2(CenterX, Bottom);
            set
            {
                CenterX = value.X;
                Bottom = value.Y;
            }
        }

        public Point2 BottomRight
        {
            get => new Point2(Right, Bottom);
            set
            {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        #endregion

        public RectInt(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public RectInt(Point2 position, Point2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(in Point2 point)
        {
            return (point.X >= X && point.Y >= Y && point.X < X + Width && point.Y < Y + Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(in RectInt rect)
        {
            return (Left < rect.Left && Top < rect.Top && Bottom > rect.Bottom && Right > rect.Right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Overlaps(in RectInt against)
        {
            return X + Width > against.X && Y + Height > against.Y && X < against.X + against.Width && Y < against.Y + against.Height;
        }

        public RectInt Inflate(int by)
        {
            return new RectInt(X - by, Y - by, Width + by * 2, Height + by * 2);
        }

        public RectInt Inflate(int byX, int byY)
        {
            return new RectInt(X - byX, Y - byY, Width + byX * 2, Height + byY * 2);
        }

        public RectInt Scale(float scale)
        {
            return new RectInt((int)(X * scale), (int)(Y * scale), (int)(Width * scale), (int)(Height * scale));
        }

        public RectInt MultiplyX(int scale)
        {
            var r = new RectInt(X * scale, Y, Width * scale, Height);

            if (r.Width < 0)
            {
                r.X += r.Width;
                r.Width *= -1;
            }

            return r;
        }

        public RectInt MultiplyY(int scale)
        {
            var r = new RectInt(X, Y * scale, Width, Height * scale);

            if (r.Height < 0)
            {
                r.Y += r.Height;
                r.Height *= -1;
            }

            return r;
        }

        public RectInt OverlapRect(in RectInt against)
        {
            var overlapX = X + Width > against.X && X < against.X + against.Width;
            var overlapY = Y + Height > against.Y && Y < against.Y + against.Height;

            RectInt r = new RectInt();

            if (overlapX)
            {
                r.Left = Math.Max(Left, against.Left);
                r.Width = Math.Min(Right, against.Right) - r.Left;
            }

            if (overlapY)
            {
                r.Top = Math.Max(Top, against.Top);
                r.Height = Math.Min(Bottom, against.Bottom) - r.Top;
            }

            return r;
        }

        public override bool Equals(object? obj) => (obj is RectInt other) && (this == other);

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + X;
            hash = hash * 23 + Y;
            hash = hash * 23 + Width;
            hash = hash * 23 + Height;
            return hash;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}, {Width}, {Height}]";
        }

        public static RectInt Box(Point2 center, Point2 size)
        {
            return new RectInt(center.X - size.X / 2, center.Y - size.Y / 2, size.X, size.Y);
        }

        public static RectInt Between(Point2 a, Point2 b)
        {
            RectInt rect;

            rect.X = a.X < b.X ? a.X : b.X;
            rect.Y = a.Y < b.Y ? a.Y : b.Y;
            rect.Width = (a.X > b.X ? a.X : b.X) - rect.X;
            rect.Height = (a.Y > b.Y ? a.Y : b.Y) - rect.Y;

            return rect;
        }

        public static bool operator ==(RectInt a, RectInt b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(RectInt a, RectInt b)
        {
            return !(a == b);
        }

        public static RectInt operator *(RectInt rect, Facing flipX)
        {
            if (flipX == Facing.Right)
                return rect;
            else
                return rect.MultiplyX(-1);
        }

        public static RectInt operator *(RectInt rect, int scaler)
        {
            return new RectInt(rect.X * scaler, rect.Y * scaler, rect.Width * scaler, rect.Height * scaler).Validate();
        }

        public static RectInt operator *(RectInt rect, Point2 scaler)
        {
            return new RectInt(rect.X * scaler.X, rect.Y * scaler.Y, rect.Width * scaler.X, rect.Height * scaler.Y).Validate();
        }

        public static RectInt operator /(RectInt rect, int scaler)
        {
            return new RectInt(rect.X / scaler, rect.Y / scaler, rect.Width / scaler, rect.Height / scaler).Validate();
        }

        public static RectInt operator /(RectInt rect, Point2 scaler)
        {
            return new RectInt(rect.X / scaler.X, rect.Y / scaler.Y, rect.Width / scaler.X, rect.Height / scaler.Y).Validate();
        }

        public static explicit operator RectInt(Rect rect)
        {
            return new RectInt((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RectInt Validate()
        {
            if (Width < 0)
            {
                X += Width;
                Width *= -1;
            }

            if (Height < 0)
            {
                Y += Height;
                Height *= -1;
            }

            return this;
        }
    }
}
