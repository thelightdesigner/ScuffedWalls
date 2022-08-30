using ModChart;
using ModChart.Wall;
using System;
using System.Numerics;
using static ModChart.BeatMap;

namespace ScuffedWalls.Functions
{
    [SFunction("ModelToEnvironment")]
    public class ModelToEnvironment : ScuffedFunction
    {
        protected override void Update()
        {
            string Path = GetParam("path", DefaultValue: string.Empty, p => System.IO.Path.Combine(ScuffedWallsContainer.ScuffedConfig.MapFolderPath, p.RemoveWhiteSpace()));
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            float scalerX = GetParam("scalerX", 1, CustomDataParser.FloatConverter);
            float scalerY = GetParam("scalerY", 1, CustomDataParser.FloatConverter);
            float scalerZ = GetParam("scalerZ", 1, CustomDataParser.FloatConverter);
            float YOffset = GetParam("YOffset", 0, CustomDataParser.FloatConverter);
            float ZOffset = GetParam("ZOffset", 0, CustomDataParser.FloatConverter);
            float XOffset = GetParam("XOffset", 0, CustomDataParser.FloatConverter);
            string RegexStatement = GetParam("Regex", null, p => p);

            if (RegexStatement is null)
            {
                ScuffedWalls.Print("Regex is required for ModelToEnvironment function! The function will not execute.", ScuffedWalls.LogSeverity.Error);
                return;
            }

            Model model = new Model(Path);

            foreach (var cube in model.Objects)
            {
                Vector3 Scale = cube.Transformation.Scale;
                float preModScaleY = Scale.Y;
                Scale.X *= scalerX;
                Scale.Y *= scalerY;
                Scale.Z *= scalerZ;

                Matrix4x4 Transform = cube.Matrix.Value.TransformLoc(new Vector3(0, -1f, 0));
                Transformation DecomposedTransform = Transformation.fromMatrix(Transform);
                DecomposedTransform.Position *= new Vector3(-1, 1, 1);
                DecomposedTransform.Position += new Vector3(XOffset, YOffset, ZOffset);
                DecomposedTransform.RotationEul *= new Vector3(1, -1, -1);


                InstanceWorkspace.Environment.Add(new TreeDictionary()
                {
                    [_id] = RegexStatement,
                    [_lookupMethod] = "Regex",
                    [_duplicate] = 1,
                    [_position] = DecomposedTransform.Position.ToFloatArray(),
                    [_rotation] = DecomposedTransform.RotationEul.ToFloatArray(),
                    [_scale] = Scale.ToFloatArray()
                });

            }
        }
    }
}