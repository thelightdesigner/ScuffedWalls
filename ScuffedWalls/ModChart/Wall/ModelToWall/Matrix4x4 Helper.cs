using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModChart
{
    static class Matrix4x4_Helper
    {
        public static bool DeepEquals(this Matrix4x4 m, Matrix4x4 n)
        {
            if (n.M11 != m.M11) return false;
            if (n.M12 != m.M12) return false;
            if (n.M13 != m.M13) return false;
            if (n.M14 != m.M14) return false;
            if (n.M21 != m.M21) return false;
            if (n.M22 != m.M22) return false;
            if (n.M23 != m.M23) return false;
            if (n.M24 != m.M24) return false;
            if (n.M31 != m.M31) return false;
            if (n.M32 != m.M32) return false;
            if (n.M33 != m.M33) return false;
            if (n.M34 != m.M34) return false;
            if (n.M41 != m.M41) return false;
            if (n.M42 != m.M42) return false;
            if (n.M43 != m.M43) return false;
            if (n.M44 != m.M44) return false;
            return true;
        }
        public static Matrix4x4 DeepClone(this Matrix4x4 m)
        {
            return new Matrix4x4()
            {
                M11 = m.M11,
                M12 = m.M12,
                M13 = m.M13,
                M14 = m.M14,
                M21 = m.M21,
                M22 = m.M22,
                M23 = m.M23,
                M24 = m.M24,
                M31 = m.M31,
                M32 = m.M32,
                M33 = m.M33,
                M34 = m.M34,
                M41 = m.M41,
                M42 = m.M42,
                M43 = m.M43,
                M44 = m.M44
            };
        }
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
        public static ValuePair<Transformation, IEnumerable<Vector3>> GetBoundingBox(this IEnumerable<Matrix4x4> ms)
        {
            List<Vector3> corners = new List<Vector3>();
            foreach (var matrix in ms) corners.AddRange(matrix.GetBoundingBox().Extra);

            var orderedbyX = corners.OrderBy(p => p.X);
            var orderedbyY = corners.OrderBy(p => p.Y);
            var orderedbyZ = corners.OrderBy(p => p.Z);

            return new ValuePair<Transformation, IEnumerable<Vector3>>()
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
                Extra = corners
            };
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
    }
}
