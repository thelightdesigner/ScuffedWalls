using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace ModChart.Wall
{
    class ImageToWall
    {
        ImageSettings _Settings;
        Bitmap _Bitmap;
        BeatMap.Obstacle[] Walls;

        public ImageToWall(string path, ImageSettings settings)
        {
            _Settings = settings;
            _Bitmap = new Bitmap(path);
            Run();
        }

        public BeatMap.Obstacle[] GetWalls()
        {
            return Walls;
        }

        public void Run()
        {
            Pixel[] pixels = AnalyzeImage();
            pixels.ToList().ForEach(p => p.FlipVertical(_Bitmap));
            //pixels.ToList().ForEach(p => p.Inverse());
            FloatingPixel[] floatingPixels = pixels.Select(p => { return p.ToFloating(); }).ToArray();
            //floatingPixels.ToList().ForEach(p => p.Resize(0.2f));

            Walls = floatingPixels.Select(p =>
            {
                return new BeatMap.Obstacle()
                {
                    _time = 5,
                    _duration = 1,
                    _lineIndex = 0,
                    _type = 0,
                    _width = 0,
                    _customData = new BeatMap.CustomData()
                    {
                        _position = new object[] { p.Position.X, p.Position.Y },
                        _scale = new object[] { p.Scale.X, p.Scale.Y, 0.2 },
                        _color = p.Color.ToObjArray()
                    }
                };
            }).ToArray();
        }

        Pixel[] AnalyzeImage()
        {
            return CompressPixels(GetAllPixels());


            Pixel[] GetAllPixels()
            {
                List<Pixel> pixels = new List<Pixel>();
                IntVector2 Pos = new IntVector2();
                for (Pos.Y = 0; Pos.Y < _Bitmap.Height; Pos.Y++)
                {
                    for (Pos.X = 0; Pos.X < _Bitmap.Width; Pos.X++)
                    {
                        pixels.Add(_Bitmap.ToPixel(Pos));
                    }
                }
                return pixels.ToArray();
            }

            Pixel[] CompressPixels(Pixel[] pixels)
            {
                IntVector2 Pos = new IntVector2();
                IntVector2 Dimensions = pixels.GetDimensions();
                List<Pixel> CompressedPixels = new List<Pixel>();

                Pixel CurrentPixel = null;
                for (Pos.X = 0; Pos.X < Dimensions.X; Pos.X++)
                {
                    for (Pos.Y = 0; Pos.Y < Dimensions.Y; Pos.Y++)
                    {
                        bool CountPixel =
                            pixels.GetCurrent(Pos) != null && //this pixel has to exist
                            pixels.GetCurrent(Pos).Equals(pixels.GetCurrent(Pos.Transform(new IntVector2() { X = 0, Y = -1 })), 0f); // the last one has to exist and be the same

                        if (CountPixel) //this vs last, color, width, existance
                        {
                            CurrentPixel.AddHeight();
                        }
                        else
                        {
                            //add and reset
                            if (CurrentPixel != null) CompressedPixels.Add(CurrentPixel);
                            CurrentPixel = pixels.GetCurrent(Pos);
                        }
                    }
                }
                if (CurrentPixel != null) CompressedPixels.Add(CurrentPixel);
                return CompressedPixels.ToArray();
            }

        }






    }

    public class ImageSettings
    {
        public ImageSettings(bool isBlackEmpty, float scale, float thicc, bool track, bool centered, float spread, float alfa, BeatMap.Obstacle baseWall)
        {
            this.isBlackEmpty = isBlackEmpty;
            this.scale = scale;
            this.thicc = thicc;
            this.centered = centered;
            this.spread = spread;
            this.alfa = alfa;
            this.Wall = baseWall;
        }
        public ImageSettings() { }

        public bool isBlackEmpty { get; set; }
        public float scale { get; set; }
        public float thicc { get; set; }
        public bool track { get; set; }
        public bool centered { get; set; }
        public float spread { get; set; }
        public float alfa { get; set; }
        public float tolerance { get; set; }
        public BeatMap.Obstacle Wall { get; set; }
    } //settings

    class Pixel
    {
        //data type
        public IntVector2 Position { get; set; }
        public IntVector2 Scale { get; set; }
        public Color Color { get; set; }
        public void FlipVertical(Bitmap bitmap)
        {
            IntVector2 NewPosition = new IntVector2() { X = Position.X, Y = bitmap.Height - Position.Y };
            Position = NewPosition;
        }
        public void Inverse()
        {
            IntVector2 NewPosition = new IntVector2() { X = Position.Y, Y = Position.X };
            IntVector2 NewScale = new IntVector2() { X = Scale.Y, Y = Scale.X };
            Position = NewPosition;
            Scale = NewScale;
        }
        public void AddHeight()
        {
            Scale = Scale.Transform(new IntVector2() { X = 0, Y = 1 });
        }
        public bool Equals(Pixel pixel, float tolerance)
        {
            if (pixel == null) return false;
            if (!Color.Equals(pixel.Color, tolerance)) return false;
            if (Scale.X != pixel.Scale.X) return false;
            return true;
        }
        public FloatingPixel ToFloating()
        {
            return new FloatingPixel()
            {
                Position = new Vector2() { X = Position.X.toFloat(), Y = Position.Y.toFloat() },
                Scale = new Vector2() { X = Scale.X.toFloat(), Y = Scale.Y.toFloat() },
                Color = Color
            };
        }

    }

    class FloatingPixel
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; }

        //0,0 origin
        public void Resize(float Factor)
        {
            Vector2 NewPosition = new Vector2()
            {
                X = Position.X * Factor,
                Y = Position.Y * Factor,
            };
            Vector2 NewScale = new Vector2()
            {
                X = Scale.X * Factor,
                Y = Scale.Y * Factor,
            };
            Position = NewPosition;
            Scale = NewScale;
        }
        public void Transform(Vector2 tranformation)
        {
            Vector2 NewPosition = new Vector2()
            {
                X = Position.X + tranformation.X,
                Y = Position.Y + tranformation.Y,
            };
            Position = NewPosition;
        }
    }

    static class BitmapHelper
    {
        public static Pixel GetCurrent(this Pixel[] pixels, IntVector2 pos)
        {
            if (!pixels.Any(p => p.Position.X == pos.X && p.Position.Y == pos.Y)) return null; //if it dont exist
            return pixels.Where(p => p.Position.Y == pos.Y && p.Position.X == pos.X).First();
        }
        public static Color GetCurrent(this Bitmap bitmap, IntVector2 pos)
        {
            return new Color()
            {
                R = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).R) / 255f,
                G = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).G) / 255f,
                B = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).B) / 255f,
                A = Convert.ToSingle(bitmap.GetPixel(pos.X, pos.Y).A) / 255f
            };
        }
        public static Pixel ToPixel(this Bitmap bitmap, IntVector2 pos)
        {
            return new Pixel()
            {
                Position = pos,
                Scale = new IntVector2() { X = 1, Y = 1 },
                Color = bitmap.GetCurrent(pos)
            };
        }
        public static IntVector2 GetDimensions(this Pixel[] pixels)
        {
            if (pixels.Length == 0) return new IntVector2() { X = 0, Y = 0 };
            Pixel[] Sorted = pixels.OrderBy(p => p.Position.Y).ThenBy(p => p.Position.X).ToArray();
            return new IntVector2()
            {
                Y = Sorted.Last().Position.Y + Sorted.Last().Scale.Y,
                X = Sorted.Last().Position.X + Sorted.Last().Scale.X
            };

        }
        public static IntVector2 Transform(this IntVector2 t1, IntVector2 t2)
        {
            return new IntVector2()
            {
                X = t1.X + t2.X,
                Y = t1.Y + t2.Y
            };
        }
    }

}
