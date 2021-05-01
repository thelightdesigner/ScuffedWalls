using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModChart.Wall
{

    //good color
    public class Color
    {
        public static Color HslToRGB(float h, float s, float l)
        {
            //Console.WriteLine($"{h} {s} {l}");

            while (h > 1f) h--;

            Color rgb;
            if (s == 0) rgb = new Color() { R = l, A = l, B = l, G = l};
            else
            {
                rgb = new Color();

                float q = 
                    l < 0.5f 
                    ? ( l * (1f + s) )
                    : ( l + s - l * s);

                float p = 2 * l - q;

                rgb.R = Hue2rgb(p, q, h + 1f / 3f);
                rgb.G = Hue2rgb(p, q, h);
                rgb.B = Hue2rgb(p, q, h - 1f / 3f);

                float Hue2rgb(float p2, float q2, float t)
                {
                    if (t < 0) t += 1f;
                    if (t > 1f) t -= 1f;
                    if (t < 1f / 6f) return p2 + (q2 - p2) * 6f * t;
                    if (t < 1f / 2f) return q2;
                    if (t < 2f / 3f) return p2 + (q2 - p2) * (2f / 3f - t) * 6f;
                    return p2;
                }

            } return rgb;
        }
        
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }
        public bool Equals(Color color, float tolerance)
        {
            if (color == null) return false;
            if (Math.Abs(color.R - R) > tolerance) return false;
            if (Math.Abs(color.G - G) > tolerance) return false;
            if (Math.Abs(color.B - B) > tolerance) return false;
            if (Math.Abs(color.A - A) > tolerance) return false;
            return true;
        }
        public object[] ToObjArray()
        {
            return new object[] { R, G, B, A };
        }
        public object[] ToObjArray(float alpha)
        {
            return new object[] { R, G, B, alpha };
        }
        public Color Clone()
        {
            return new Color()
            {
                R = R,
                G = G,
                B = B,
                A = A
            };
        }
        public static Color operator *(Color color, float val)
        {
            return new Color()
            {
                R = color.R * val,
                A = color.A * val,
                B = color.B * val,
                G = color.G * val
            };
        }
        public static Color ColorFromObjArray(object[] array)
        {
            return new Color() { R = array[0].toFloat(), G = array[1].toFloat(), B = array[2].toFloat(), A = array[3].toFloat() };
        }
        public bool isBlackOrEmpty(float tolerance)
        {
            if (R + B + G < tolerance) return true;
            // if (A == 0f) return true;
            return false;
        }
        public override string ToString()
        {
            return $"{R} {G} {B} {A}";
        }
    }
    /// <summary>
    /// This class is made specifically for a beatsaber wall, 
    /// technically it can be used generically
    /// </summary>
    public class Cube
    {
        /// <summary>
        /// the name of the blender object
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An array of all the frames of this cube,
        /// Nulled if there is no animation.
        /// Properties in this array can also be nulled if they are not animated.
        /// </summary>
        public Frame[] Frames { get; set; }

        /// <summary>
        /// The static decomposed transformation of this cube
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// The static matrix transformation of this cube
        /// </summary>
        public Matrix4x4? Matrix { get; set; }

        /// <summary>
        /// The static decomposed transformation of this cube
        /// </summary>
        public Transformation OffsetTransformation { get; set; }

        /// <summary>
        /// The static matrix transformation of this cube
        /// </summary>
        public Matrix4x4? OffsetMatrix { get; set; }

        /// <summary>
        /// who left the camera in the scene *BARF*
        /// </summary>
        public bool isCamera { get; set; }

        /// <summary>
        /// shpere
        /// </summary>
        public bool isBomb { get; set; }

        public string Track { get; set; }


        /// <summary>
        /// The span of this cubes lifetime,
        /// Set by `hide viewport` in blender.
        /// </summary>
        public DoubleInt FrameSpan { get; set; }

        /// <summary>
        /// The static color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// override alpha value
        /// from material IOR
        /// </summary>
        public float? IOR { get; set; }

        /// <summary>
        /// How many total frames there are
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// bro wtf
        /// </summary>
        public bool isNote { get; set; }

        /// <summary>
        /// the name of the materials that its on
        /// </summary>
        public string[] Material { get; set; }
        public class Frame
        {
            public int Number { get; set; }
            public Transformation Transformation { get; set; }
            public Matrix4x4? Matrix { get; set; }
            public Transformation OffsetTransformation { get; set; }
            public Matrix4x4? OffsetMatrix { get; set; }
            public float? Dissolve { get; set; }
            public Color Color { get; set; }

            /// <summary>
            /// Indicates if on this frame this cube is active in heirarchy.
            /// Used to create multiple cubes by the collada parser.
            /// </summary>
            public bool? Active { get; set; }
            public Frame Clone()
            {
                var newFrame = new Frame()
                {
                    Number = Number,
                    Matrix = Matrix,
                    Dissolve = Dissolve,
                    Active = Active
                };
                if (Transformation != null) newFrame.Transformation = Transformation.Clone();
                if (Color != null) newFrame.Color = Color.Clone();
                return newFrame;
            }

        }
        public void Decompose()
        {
            if ((Matrix.HasValue))
            {
                Transformation = Transformation.fromMatrix(Matrix.Value);
            }
            if ((OffsetMatrix.HasValue))
            {
                OffsetTransformation = Transformation.fromMatrix(OffsetMatrix.Value);
            }
            if (Frames != null && Frames.All(f => f.Matrix.HasValue))
            {
                Frames = Frames.Select(frame =>
                {
                    frame.Transformation = Transformation.fromMatrix(frame.Matrix.Value);
                    return frame;
                }).ToArray();
                Transformation = Frames.First().Transformation;
                Matrix = Frames.First().Matrix;
            }
            if (Frames != null && Frames.All(f => f.OffsetMatrix.HasValue))
            {
                Frames = Frames.Select(frame =>
                {
                    frame.OffsetTransformation = Transformation.fromMatrix(frame.OffsetMatrix.Value);
                    return frame;
                }).ToArray();
                OffsetTransformation = Frames.First().Transformation;
                OffsetMatrix = Frames.First().Matrix;
            }
        }
        public Cube[] InstantiateMultiples()
        {
            if (Frames != null && Frames.Any(f => f.Active != Frames.First().Active))
            {
                //Console.WriteLine(this.Name);
                List<DoubleInt> framespan = new List<DoubleInt>();
                KeyValuePair<bool, DoubleInt>? current = null;
                bool? lastactive = null;
                for (int i = 0; i < Frames.Length; i++)
                {
                    if (lastactive.HasValue && current.HasValue && lastactive.Value == Frames[i].Active.Value)
                    {
                        current.Value //nullable value
                            .Value //key value (DoubleInt)
                            .Val2++; //second number of DoubleInt
                    }
                    else
                    {
                        //Console.WriteLine($"Current Has Value?: {current.HasValue}");
                        if (current.HasValue && current.Value.Key)
                        {
                            //Console.WriteLine("Current Added");
                            framespan.Add(current.Value.Value);
                        }

                        current = new KeyValuePair<bool, DoubleInt>(Frames[i].Active.Value, new DoubleInt(i, i + 1));

                        //Console.WriteLine($"Current Set To: {current.ToString()}");
                    }
                    lastactive = Frames[i].Active.Value;

                }
                if (current.HasValue && current.Value.Key)
                {
                    //Console.WriteLine("Current Added");
                    framespan.Add(current.Value.Value);
                }

                SetOffset();

                // Console.WriteLine(string.Join(" ",
                //        Frames.Select(f => f.Active.Value)
                //        ));

                return framespan.Select(f =>
                {
                    var Newcube = this.Clone();
                    Newcube.FrameSpan = f;
                    Newcube.Frames = Newcube.Frames.Slice(f.Val1, f.Val2);
                    return Newcube;
                }).ToArray();
            }
            return new Cube[] { this };
        }
        public Cube Clone()
        {
            var newCube = new Cube()
            {
                Count = Count,
                IOR = IOR,
                isBomb = isBomb,
                isNote = isNote,
                Track = Track,
                Name = Name,
                isCamera = isCamera,
                Matrix = Matrix.Value,
                OffsetMatrix = OffsetMatrix.Value
            };
            if (Transformation != null) newCube.Transformation = Transformation.Clone();
            if (OffsetTransformation != null) newCube.OffsetTransformation = OffsetTransformation.Clone();
            if (Frames != null && Frames.Any()) newCube.Frames = Frames.Select(f => f.Clone()).ToArray();
            if (Color != null) newCube.Color = Color.Clone();
            if (FrameSpan != null) newCube.FrameSpan = FrameSpan.Clone();
            if (Material != null) newCube.Material = (string[])Material.Clone();

            return newCube;
        }
        public void SetOffset()
        {

            if (Frames != null && Frames.Any() && Frames.All(f => f.Matrix.HasValue && f.Transformation != null))
            {
                Frames = Frames.Select(frame =>
                {
                    var trans = Transformation.fromMatrix(frame.Matrix.Value);
                    var mat = frame.Matrix.Value;
                    mat = mat.TransformLoc(new Vector3(0, -1, -1)); //compensate pivot difference

                    mat.Translation = mat.Translation + new Vector3(trans.Scale.X - 2, 0, 0); //compensate animate scale vs scale difference
                    frame.OffsetMatrix = mat;

                    return frame;
                }).ToArray();
            }
            var trans = Transformation.fromMatrix(Matrix.Value);
            var mat = Matrix.Value;
            mat = mat.TransformLoc(new Vector3(0, -1, -1));//compensate pivot difference

            mat.Translation = mat.Translation + new Vector3(trans.Scale.X - 2, 0, 0); //compensate animate scale vs scale difference
            OffsetMatrix = mat;

        }

        /// <summary>
        /// Transforms a collection of cubes around the center of their bounding box.
        /// </summary>
        /// <param name="cubes"></param>
        /// <param name="Position"></param>
        /// <param name="Rotation"></param>
        /// <param name="Scale"></param>
        /// <param name="SetPos"></param>
        /// <param name="SetScale"></param>
        /// <param name="SetScaleAxis"></param>
        /// <returns>
        /// A deep copy of the collection
        /// </returns>
        public static IEnumerable<Cube> TransformCollection(DeltaTransformOptions Options)
        {
            Cube[] newCubes = Options.cubes.Select(c => c.Clone()).ToArray();

            Transformation boundingbox = newCubes.Select(n => n.Matrix.Value).ToArray().GetBoundingBox().Main;

            Vector3 Offset = boundingbox.Position;
            if (Options.TransformOrigin == DeltaTransformOptions.TransformOptions.FrontBottomLeft) Offset = boundingbox.Position - (boundingbox.Scale / 2f);
            if (Options.TransformOrigin == DeltaTransformOptions.TransformOptions.WorldOrigin) Offset = new Vector3(0);

            //center
            newCubes = newCubes.Select(c =>
            {
                var mat = c.Matrix.Value;
                mat.Translation = mat.Translation - Offset;
                c.Matrix = mat;
                return c;
            }).ToArray();


            //scale
            if (Options.SetScale)
            {
                if (Options.SetScaleAxis == Axis.X) Options.Scale /= boundingbox.Scale.X;
                else if (Options.SetScaleAxis == Axis.Y) Options.Scale /= boundingbox.Scale.Y;
                else if (Options.SetScaleAxis == Axis.Z) Options.Scale /= boundingbox.Scale.Z;
            }

            newCubes = newCubes.Select(cube =>
            {
                cube.Matrix = Matrix4x4.Multiply(cube.Matrix.Value, Matrix4x4.CreateScale(Options.Scale));
                return cube;
            }).ToArray();


            //rotate
            newCubes = newCubes.Select(c =>
            {
                c.Matrix = Matrix4x4.Transform(c.Matrix.Value, Options.Rotation.ToQuaternion());
                return c;
            }).ToArray();


            //translate
            var ReLoc = Offset;
            if (Options.SetPos) ReLoc = new Vector3(0);


            newCubes = newCubes.Select(c =>
            {
                var mat = c.Matrix.Value;
                mat.Translation = mat.Translation + ReLoc + Options.Position;
                c.Matrix = mat;
                return c;
            }).ToArray();

            //decompose
            foreach (var c in newCubes) c.SetOffset();
            foreach (var c in newCubes) c.Decompose();

            return newCubes;
        }
    }
    public class DeltaTransformOptions
    {
        public IEnumerable<Cube> cubes { get; set; }
        public Vector3 Position { get; set; } = new Vector3(0);
        public Vector3 Rotation { get; set; } = new Vector3(0);
        public float Scale { get; set; } = 1;
        public bool SetPos { get; set; } = false;
        public bool SetScale { get; set; } = false;
        public TransformOptions TransformOrigin { get; set; } = TransformOptions.Center;
        public Axis SetScaleAxis { get; set; } = Axis.X;

        public enum TransformOptions
        {
            Center,
            FrontBottomLeft,
            WorldOrigin
        }
    }
}
