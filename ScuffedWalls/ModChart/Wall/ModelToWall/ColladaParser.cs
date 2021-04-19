using ModChart;
using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml;
using System.Xml.Serialization;

using static ScuffedWalls.ScuffedLogger;

namespace ModChart.Wall
{
    public class Model
    {
        public Cube[] Cubes { get; set; }

        Collada model;

        public Model(string path)
        {
            model = (Collada)Converters.DeserializeXML<Collada>(path);
            SetCubes();
            foreach (var cube in Cubes) cube.SetOffset();
            foreach (var cube in Cubes) cube.Decompose();
        }
        public Model(Cube[] Cubes)
        {
            this.Cubes = Cubes;
            //lmao
        }


        
        void SetCubes()
        {
            List<Cube> cubes = new List<Cube>();

            if (model.library_visual_scenes.visual_scene.node != null)
            {
                foreach (var node in model.library_visual_scenes.visual_scene.node)
                {
                    //ignore the not cubes
                    if (!(node.instance_geometry != null || node.instance_cam != null)) continue;

                    Cube cube = new Cube();

                    void doframes(int count)
                    {
                        if (cube.Frames != null && cube.Frames.Length != count) throw new Exception("please tell light about this 01");
                        if (cube.Frames == null) cube.Frames = new Cube.Frame[count];

                        if (cube.Count.HasValue && cube.Count != count) throw new Exception("please tell light about this 02");
                        if (!cube.Count.HasValue) cube.Count = count;

                        if (cube.FrameSpan == null) cube.FrameSpan = new DoubleInt(0,count);
                    }

                    //base matrix
                    cube.Matrix = node.matrix.ParseToFloatArray().toMatrix();

                    //name
                    cube.Name = node.name;
                    
                    //library_animations
                    if (model.library_animations != null && model.library_animations.animation.Any(a => a.name == node.name))
                    {
                        Collada.Library_Animations.Animations cubeAnimationsContainer = model.library_animations.animation.Where(a => a.name == node.name).First();

                        if (cubeAnimationsContainer.animation != null && cubeAnimationsContainer.animation.Any(a => a.id.Contains("transform")))
                        {
                            var source = cubeAnimationsContainer.animation
                            .Where(a => a.id.Contains("transform"))
                            .First().sources.Where(a => a.id
                            .Contains("-output"))
                            .First();

                            Matrix4x4[] matrices = source.float_array.values.ParseToFloatArray().to4x4Matricies();

                            doframes(matrices.Length);

                            for (int frame = 0; frame < cube.Count; frame++)
                            {
                                if (cube.Frames[frame] == null) cube.Frames[frame] = new Cube.Frame();
                                cube.Frames[frame].Matrix = matrices[frame];
                                cube.Frames[frame].Number = frame;
                            }
                        }
                        if (cubeAnimationsContainer.animation != null && cubeAnimationsContainer.animation.Any(a => a.id.Contains("color")))
                        {

                            float?[] Red = null;
                            float?[] Green = null;
                            float?[] Blu = null;
                            float?[] Alpa = null;

                            var R = cubeAnimationsContainer.animation.Where(a => a.id.Contains("_R"));
                            if (R != null && R.Any())
                            {
                                var source = R.First().sources.Where(s => s.id.Contains("-output")).First();
                                doframes(int.Parse(source.float_array.count));
                                Red = source.float_array.values.ParseToNullFloatArray();
                            }

                            var G = cubeAnimationsContainer.animation.Where(a => a.id.Contains("_G"));
                            if (G != null && G.Any())
                            {
                                var source = G.First().sources.Where(s => s.id.Contains("-output")).First();
                                doframes(int.Parse(source.float_array.count));
                                Green = source.float_array.values.ParseToNullFloatArray();
                            }

                            var B = cubeAnimationsContainer.animation.Where(a => a.id.Contains("_B"));
                            if (B != null && B.Any())
                            {
                                var source = B.First().sources.Where(s => s.id.Contains("-output")).First();
                                doframes(int.Parse(source.float_array.count));
                                Blu = source.float_array.values.ParseToNullFloatArray();
                            }

                            var A = cubeAnimationsContainer.animation.Where(a => a.id.Contains("_color") && !new string[] { "_R","_G","_B"}.Any(sigh => a.id.Contains(sigh)));
                            if (A != null && A.Any())
                            {
                                var source = A.First().sources.Where(s => s.id.Contains("-output")).First();
                                doframes(int.Parse(source.float_array.count));
                                Alpa = source.float_array.values.ParseToNullFloatArray();
                            }

                            for (int frame = 0; frame < cube.Count; frame++)
                            {
                                var c = new Color();
                                if (Red != null && Red.Length >= cube.Frames.Length) c.R = Red[frame].Value;
                                if (Green != null && Green.Length >= cube.Frames.Length) c.G = Green[frame].Value;
                                if (Blu != null && Blu.Length >= cube.Frames.Length) c.B = Blu[frame].Value;
                                if (Alpa != null && Alpa.Length >= cube.Frames.Length) c.A = Alpa[frame].Value;

                                if (cube.Frames[frame] == null) cube.Frames[frame] = new Cube.Frame();
                                cube.Frames[frame].Color = c;
                                cube.Frames[frame].Number = frame;
                            }

                        }
                        if (cubeAnimationsContainer.animation != null && cubeAnimationsContainer.animation.Any(a => a.id.Contains("hide_viewport")))
                        {
                            var source = cubeAnimationsContainer.animation
                            .Where(a => a.id.Contains("hide_viewport"))
                            .First().sources.Where(s => s.id
                            .Contains("-output"))
                            .First();

                            int[] visible = source.float_array.values.ParseToIntArray();
                            doframes(int.Parse(source.float_array.count));

                            //Console.WriteLine(source.float_array.values);
                            for (int frame = 0; frame < cube.Count; frame++)
                            {
                                if (cube.Frames[frame] == null) cube.Frames[frame] = new Cube.Frame();
                                cube.Frames[frame].Active = !Convert.ToBoolean(visible[frame]);
                                cube.Frames[frame].Number = frame;
                            }

                        }
                    }

                    if (node.instance_cam != null)
                    {
                        cube.isCamera = true;
                        cubes.Add(cube);
                        continue;
                    }

                    if(node.instance_geometry != null && node.instance_geometry.url.ToLower().Contains("sphere"))
                    {
                        cube.isBomb = true;
                    }

                    //materials
                    if (node.instance_geometry != null && node.instance_geometry.bind_material != null && node.instance_geometry.bind_material.technique_common.instance_material != null)
                        cube.Material = node.instance_geometry.bind_material.technique_common.instance_material.Select(m => m.symbol.Split("-material")[0]).ToArray();

                    //library_effects
                    if (model.library_effects != null)
                    {

                        if(cube.Material != null && cube.Material.Any())
                        {
                            var effectcontainer/* = model.library_effects.effect.Where(e => cube.Material.Any(m => e.id.Contains(m)));*/ = model.library_effects.effect;
                            var correcteffect = effectcontainer.Where(e => e.id.Split("-effect").First() == cube.Material[0]).First();


                            if (correcteffect.profile.technique.lambert.diffuse == null) Warning.Log($"{cube.Name} diffuse is nulled! skipping");

                            float[] colorArray = correcteffect.profile.technique.lambert.diffuse.color.ParseToFloatArray();
                            cube.Color = new Color() { R = colorArray[0], G = colorArray[1], B = colorArray[2], A = colorArray[3] };
                        }

                    }

                    if (cube.Material != null && cube.Material.Any(m => m.ToLower().Contains("note"))) cube.isNote = true;
                    if (cube.Material != null && cube.Material.Where(m => !m.ToLower().Contains("note")).Count() > 1) cube.Track = cube.Material.Where(m => !m.ToLower().Contains("note")).Last();
                    
                    cubes.Add(cube);
                }
            }

            List<Cube> instancecubes = new List<Cube>();
            foreach (var cube in cubes) instancecubes.AddRange(cube.InstantiateMultiples());
            Cubes = instancecubes.ToArray();
        }

    }


    

    public static class Converters
    {
        public static float[] ParseToFloatArray(this string values)
        {
            return values.Split(' ').Select(item => float.Parse(item)).ToArray();
        }
        public static float?[] ParseToNullFloatArray(this string values)
        {
            return values.Split(' ').Select(item => (float?)float.Parse(item)).ToArray();
        }
        public static int[] ParseToIntArray(this string values)
        {
            return values.Split(' ').Select(item => int.Parse(item)).ToArray();
        }
        public static int?[] ParseToNullIntArray(this string values)
        {
            return values.Split(' ').Select(item => (int?)int.Parse(item)).ToArray();
        }
        /// <summary>
        /// Returns an array starting at start with a length of end minus start
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
        public static Matrix4x4[] to4x4Matricies(this float[] floatArray)
        {
            List<Matrix4x4> Matricies = new List<Matrix4x4>();
            for (int i = 0; i < floatArray.Length; i += 16)
            {
                Matricies.Add(floatArray.Slice(i, i + 16).toMatrix());
            }
            return Matricies.ToArray();
        }
        public static Matrix4x4 toMatrix(this float[] floatArray)
        {
            return new Matrix4x4()
            {
                M11 = floatArray[0],
                M12 = floatArray[4],
                M13 = floatArray[8],
                M14 = floatArray[12],
                M21 = floatArray[1],
                M22 = floatArray[5],
                M23 = floatArray[9],
                M24 = floatArray[13],
                M31 = floatArray[2],
                M32 = floatArray[6],
                M33 = floatArray[10],
                M34 = floatArray[14],
                M41 = floatArray[3],
                M42 = floatArray[7],
                M43 = floatArray[11],
                M44 = floatArray[15]
            };

        }
        public static object DeserializeXML<t>(string path)
        {
            object obj = null;
            XmlSerializer serializer = new XmlSerializer(typeof(t));
            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                obj = serializer.Deserialize(reader);
            }
            return obj;
        }
    }

}




