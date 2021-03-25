using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModChart
{
    public class Transformation
    {
        public static Transformation fromMatrix(Matrix4x4 Matrix)
        {
            Vector3 pos;
            Quaternion rot;
            Vector3 sca;
            Matrix4x4.Decompose(Matrix, out sca, out rot, out pos);
            return new Transformation() { Position = pos, RotationEul = rot.ToEuler(), Scale = sca, RotationQuat = rot };
        }
        public Transformation Clone()
        {
            return new Transformation() { Position = Position, RotationEul = RotationEul, Scale = Scale, RotationQuat = RotationQuat };
        }
        public override string ToString()
        {
            return $@"
Position: {{ X:{Position.X} Y:{Position.Y} Z:{Position.Z} }}
Rotation (Euler): {{ X:{RotationEul.X} Y:{RotationEul.Y} Z:{RotationEul.Z} }}
Rotation (Quaternion): {{ X:{RotationQuat.X} Y:{RotationQuat.Y} Z:{RotationQuat.Z} W:{RotationQuat.W}}}
Scale: {{ X:{Scale.X} Y:{Scale.Y} Z:{Scale.Z} }}";
        }
        
        public Vector3 Position { get; set; }
        public Quaternion RotationQuat { get; set; }
        public Vector3 RotationEul { get; set; }
        public Vector3 Scale { get; set; }
    }
    public struct ValuePair<F, S>
    {
        public F Main { get; set; }
        public S Extra { get; set; }
    }
    public struct Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }
    public struct IntVector2
    {
        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
    public class DoubleInt
    {
        public DoubleInt(int first, int second)
        {
            Val1 = first;
            Val2 = second;
        }
        public int Val1 { get; set; }
        public int Val2 { get; set; }
        public override string ToString()
        {
            return $"{Val1},{Val2}";
        }
        public DoubleInt Clone()
        {
            return new DoubleInt(Val1, Val2);
        }
    }
    static class Maths
    {
        public static Vector3 ToVector3(this float[] array)
        {
            return new Vector3(array[0], array[1], array[2]);
        }
        public static float[] Mult(this float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length) throw new ArgumentOutOfRangeException("Mult() array1 length does not equal array2 length");

            for (int index = 0; index < array1.Length; index++)
            {
                array1[index] = array1[index] * array2[index];
            }
            return array1;
        }
        public static string ToBetterString(this Matrix4x4 m, int spaces = 15)
        {


            return $@"
{m.M11}{space(m.M11)} {m.M12}{space(m.M12)} {m.M13}{space(m.M13)} {m.M14}{space(m.M14)}

{m.M21}{space(m.M21)} {m.M22}{space(m.M22)} {m.M23}{space(m.M23)} {m.M24}{space(m.M24)}

{m.M31}{space(m.M31)} {m.M32}{space(m.M32)} {m.M33}{space(m.M33)} {m.M34}{space(m.M34)}

{m.M41}{space(m.M41)} {m.M42}{space(m.M42)} {m.M43}{space(m.M43)} {m.M44}{space(m.M44)}
";

            string space(float matval)
            {
                return new string(' ', spaces - matval.ToString().Length);
            }
        }
        public static Matrix4x4 TransformLoc(this Matrix4x4 matrix, Vector3 trans)
        {
            var dec = Transformation.fromMatrix(matrix);

            var difference = Matrix4x4.CreateTranslation(trans * dec.Scale);
            var compensation = Matrix4x4.Transform(difference, dec.RotationQuat);
            matrix.Translation = matrix.Translation + compensation.Translation;
            return matrix;
        }
        /// <summary>
        /// Returns a scale vector with dimensions equal to the bounding box of the matrix
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static ValuePair<Transformation, Vector3[]> GetBoundingBox(this Matrix4x4 m)
        {
            List<Vector3> corners = new List<Vector3>();
            corners.Add(m.TransformLoc(new Vector3(-1, -1, -1)).Translation);
            corners.Add(m.TransformLoc(new Vector3(1, -1, -1)).Translation);
            corners.Add(m.TransformLoc(new Vector3(-1, 1, -1)).Translation);
            corners.Add(m.TransformLoc(new Vector3(1, 1, -1)).Translation);

            corners.Add(m.TransformLoc(new Vector3(-1, -1, 1)).Translation);
            corners.Add(m.TransformLoc(new Vector3(1, -1, 1)).Translation);
            corners.Add(m.TransformLoc(new Vector3(-1, 1, 1)).Translation);
            corners.Add(m.TransformLoc(new Vector3(1, 1, 1)).Translation);

            var orderedbyX = corners.OrderBy(p => p.X);
            var orderedbyY = corners.OrderBy(p => p.Y);
            var orderedbyZ = corners.OrderBy(p => p.Z);

            return new ValuePair<Transformation, Vector3[]>()
            {
                Main = new Transformation()
                {
                    Scale = new Vector3(
                    orderedbyX.Last().X - orderedbyX.First().X,
                    orderedbyY.Last().Y - orderedbyY.First().Y,
                    orderedbyZ.Last().Z - orderedbyZ.First().Z),
                    Position = new Vector3(
                        (orderedbyX.Last().X + orderedbyX.First().X) / 2f,
                        (orderedbyY.Last().Y + orderedbyY.First().Y) / 2f,
                        (orderedbyZ.Last().Z + orderedbyZ.First().Z) / 2f),
                    RotationEul = new Vector3()
                },
                Extra = corners.ToArray()
            };
        }
        public static ValuePair<Transformation, Vector3[]> GetBoundingBox(this Matrix4x4[] ms, Vector3 PosOffset)
        {
            List<Vector3> corners = new List<Vector3>();
            foreach (var matrix in ms) corners.AddRange(matrix.GetBoundingBox().Extra);

            var orderedbyX = corners.OrderBy(p => p.X);
            var orderedbyY = corners.OrderBy(p => p.Y);
            var orderedbyZ = corners.OrderBy(p => p.Z);

            return new ValuePair<Transformation, Vector3[]>()
            {
                Main = new Transformation()
                {
                    Scale = new Vector3(
                    orderedbyX.Last().X - orderedbyX.First().X,
                    orderedbyY.Last().Y - orderedbyY.First().Y,
                    orderedbyZ.Last().Z - orderedbyZ.First().Z),
                    Position = new Vector3(
                        (orderedbyX.Last().X + orderedbyX.First().X) / 2f + PosOffset.X,
                        (orderedbyY.Last().Y + orderedbyY.First().Y) / 2f + PosOffset.Y,
                        (orderedbyZ.Last().Z + orderedbyZ.First().Z) / 2f + PosOffset.Z),
                    RotationEul = new Vector3()
                },
                Extra = corners.ToArray()
            };
        }
        public static float toFloat(this object v)
        {
            //try { return Convert.ToSingle(v); } catch { }
            return Convert.ToSingle(v.ToString());
        }
        public static double toDouble(this object v)
        {
            return Convert.ToDouble(v.ToString());
        }
        public static Vector2 ToVector2(this object[] array)
        {
            return new Vector2(array[0].toFloat(), array[1].toFloat());
        }
        public static object[] FromVector2(this Vector2 vector)
        {
            return new object[] { vector.X, vector.Y };
        }
        public static Vector3 ToVector3(this object[] array)
        {
            return new Vector3(array[0].toFloat(), array[1].toFloat(), array[2].toFloat());
        }
        public static object[] FromVector3(this Vector3 vector)
        {
            return new object[] { vector.X, vector.Y, vector.Z };
        }

        public static Vector2 PolarToCartesian(float angle, float radius)
        {
            float angleRad = (Math.PI.toFloat() / 180f) * (angle - 90f);
            float x = radius * Math.Cos(angleRad).toFloat();
            float y = radius * Math.Sin(angleRad).toFloat();
            return new Vector2(x, y);
        }

        public static float Coterminal(this float angle)
        {
            while (angle > 360)
            {
                angle -= 360;
            }
            return angle;
        }

        public static Vector3 ToEuler(this Quaternion q)
        {

            Vector3 euler;

            // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
            float unit = (q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z) + (q.W * q.W);

            // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
            float test = q.X * q.W - q.Y * q.Z;

            if (test > 0.4995f * unit) // singularity at north pole
            {
                euler.X = MathF.PI / 2;
                euler.Y = 2f * MathF.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                euler.X = -MathF.PI / 2;
                euler.Y = -2f * MathF.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else // no singularity - this is the majority of cases
            {
                euler.X = MathF.Asin(2f * (q.W * q.X - q.Y * q.Z));
                euler.Y = MathF.Atan2(2f * q.W * q.Y + 2f * q.Z * q.X, 1 - 2f * (q.X * q.X + q.Y * q.Y));
                euler.Z = MathF.Atan2(2f * q.W * q.Z + 2f * q.X * q.Y, 1 - 2f * (q.Z * q.Z + q.X * q.X));
            }

            // all the math so far has been done in radians. Before returning, we convert to degrees...
            euler.X *= (180 / (float)Math.PI);
            euler.Y *= (180 / (float)Math.PI);
            euler.Z *= (180 / (float)Math.PI);

            //...and then ensure the degree values are between 0 and 360
            euler.X %= 360;
            euler.Y %= 360;
            euler.Z %= 360;

            return euler;
        }

        public static Quaternion ToQuaternion(this Vector3 euler)
        {
            float xOver2 = euler.X * ((float)Math.PI / 180) * 0.5f;
            float yOver2 = euler.Y * ((float)Math.PI / 180) * 0.5f;
            float zOver2 = euler.Z * ((float)Math.PI / 180) * 0.5f;

            float sinXOver2 = MathF.Sin(xOver2);
            float cosXOver2 = MathF.Cos(xOver2);
            float sinYOver2 = MathF.Sin(yOver2);
            float cosYOver2 = MathF.Cos(yOver2);
            float sinZOver2 = MathF.Sin(zOver2);
            float cosZOver2 = MathF.Cos(zOver2);

            Quaternion result;
            result.X = cosYOver2 * sinXOver2 * cosZOver2 + sinYOver2 * cosXOver2 * sinZOver2;
            result.Y = sinYOver2 * cosXOver2 * cosZOver2 - cosYOver2 * sinXOver2 * sinZOver2;
            result.Z = cosYOver2 * cosXOver2 * sinZOver2 - sinYOver2 * sinXOver2 * cosZOver2;
            result.W = cosYOver2 * cosXOver2 * cosZOver2 + sinYOver2 * sinXOver2 * sinZOver2;

            return result;
        }
    }



}
