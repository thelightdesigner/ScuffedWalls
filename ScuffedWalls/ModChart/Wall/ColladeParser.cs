using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml;
using System.Xml.Serialization;

namespace ModChart.Wall
{
    class Model
    {
        public Cube[] cubes;
        public Model(string path, bool useAnimation)
        {
            ColladaXML colladaFile = (ColladaXML)XMLDeserialize<ColladaXML>(path);
            cubes = getAllCubes(colladaFile, useAnimation);
        }

        object XMLDeserialize<t>(string path)
        {
            object obj = null;
            XmlSerializer serializer = new XmlSerializer(typeof(t));
            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                obj = serializer.Deserialize(reader);
            }
            return obj;
        }
        Cube[] getAllCubes(ColladaXML model, bool useAnimation)
        {
            List<Cube> cubes = new List<Cube>();

            if (model.library_visual_scenes.visual_scene.node != null)
            {
                for (int i = 0; i < model.library_visual_scenes.visual_scene.node.Length; i++)
                {
                    Color color = null;
                    Transformation node;
                    string thisCubeName = model.library_visual_scenes.visual_scene.node[i].name;

                    //node transformation
                    node = Transformation.createFromNode(model.library_visual_scenes.visual_scene.node[i]);

                    //color 
                    if (model.library_effects != null)
                    {
                        foreach (var effect in model.library_effects.effect)
                        {
                            if (model.library_visual_scenes.visual_scene.node[i].instance_geometry.bind_material != null)
                            {
                                if (effect.id.Split('-')[0] == model.library_visual_scenes.visual_scene.node[i].instance_geometry.bind_material.technique_common.instance_material.symbol.Split('-')[0])
                                {
                                    float[] colorArray = effect.profile.technique.lambert.diffuse.color.toFloatArray();
                                    color = new Color();
                                    color.R = colorArray[0];
                                    color.G = colorArray[1];
                                    color.B = colorArray[2];
                                    color.A = colorArray[3];
                                    continue;
                                }
                            }
                        }
                    }

                    if (useAnimation)
                    {
                        AnimationList animationList = null;
                        int count = 1;
                        //animation points
                        if (model.library_animations != null)
                        {
                            //find this cubes corrisponding animation
                            foreach (var element in model.library_animations.animation)
                            {
                                if (element.name == thisCubeName && element.animation != null)
                                {
                                    animationList = AnimationList.decompose(element.animation[0].sources[1].float_array.values.toFloatArray());
                                    count = Convert.ToInt32(element.animation[0].sources[1].float_array.count);
                                }
                                continue;
                            }
                        }
                        animationList ??= new AnimationList(); animationList.addTransformation(node);

                        //add finished cube
                        cubes.Add(new Cube()
                        {
                            Position = animationList.Position.ToArray(),
                            Rotation = animationList.Rotation.ToArray(),
                            Scale = animationList.Scale.ToArray(),
                            Color = color
                        });
                    }
                    else
                    {
                        cubes.Add(new Cube()
                        {
                            Position = new Vector3[] { node.Position },
                            Rotation = new Vector3[] { node.Rotation },
                            Scale = new Vector3[] { node.Scale },
                            Color = color
                        });
                    }

                }
            }
            return cubes.ToArray();
        }

    }
    public class MeshPoint
    {
        public Vector3 Pos;
    }
    public class Cube
    {
        public Vector3[] Position;
        public Vector3[] Rotation;
        public Vector3[] Scale;
        public Color Color;
    }
    public class Color
    {
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
        public bool isBlackOrEmpty(float tolerance)
        {
            if (R + B + G < tolerance) return true;
            if (A == 0) return true;
            return false;
        }
    }
    public class Transformation
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public static Transformation createFromNode(ColladaXML.Visual_scenes.Visual_scene.Node node)
        {
            return DecomposeColladaMatrix(node.matrix.toFloatArray());
        }
        public static Transformation DecomposeColladaMatrix(float[] floatArray)
        {
            Transformation transformation = new Transformation();
            object transformationMatrix = new Matrix4x4()
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
            Quaternion quaternion;
            Matrix4x4.Decompose((Matrix4x4)transformationMatrix, out transformation.Scale, out quaternion, out transformation.Position);
            transformation.Rotation = quaternion.QuaternionToEuler();
            return transformation;
        }
    }
    public class AnimationList
    {
        public List<Vector3> Position = new List<Vector3>();
        public List<Vector3> Rotation = new List<Vector3>();
        public List<Vector3> Scale = new List<Vector3>();
        public void addTransformation(Transformation transformation)
        {
            Position.Add(transformation.Position);
            Rotation.Add(transformation.Rotation);
            Scale.Add(transformation.Scale);
        }
        public static AnimationList decompose(float[] floatArray)
        {
            AnimationList animationList = new AnimationList();
            for (int i = 0; i < floatArray.Length; i += 16)
            {
                animationList.addTransformation(Transformation.DecomposeColladaMatrix(new float[] { floatArray[i + 0], floatArray[i + 1], floatArray[i + 2], floatArray[i + 3], floatArray[i + 4], floatArray[i + 5], floatArray[i + 6], floatArray[i + 7], floatArray[i + 8], floatArray[i + 9], floatArray[i + 10], floatArray[i + 11], floatArray[i + 12], floatArray[i + 13], floatArray[i + 14], floatArray[i + 15] }));
            }
            return animationList;
        }
    }
    public static class Converters
    {
        public static float[] toFloatArray(this string values)
        {
            return values.Split(' ').Select(item => { return Convert.ToSingle(item); }).ToArray();
        }
        public static List<float> toListWithSize(this float floatA, int count)
        {
            List<float> newArray = new List<float>();
            for (int i = 0; i < count; i++)
            {
                newArray.Add(floatA);
            }
            return newArray;
        }
    }












    //basic collada schema structure
    [Serializable, XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", ElementName = "COLLADA")]
    public class ColladaXML
    {
        //base transformation
        [XmlElement(ElementName = "library_visual_scenes")]
        public Visual_scenes library_visual_scenes { get; set; }


        //color
        [XmlElement(ElementName = "library_effects")]
        public Library_Effects library_effects { get; set; }


        //animation
        [XmlElement(ElementName = "library_animations")]
        public Library_Animations library_animations { get; set; }


        public class Library_Animations
        {
            [XmlElement(ElementName = "animation")]

            //outer most animation array is for the different objects in the model
            public Animations[] animation { get; set; }
            public class Animations
            {
                [XmlAttribute(AttributeName = "name")]
                public string name { get; set; }

                [XmlElement(ElementName = "animation")]

                //inner most animation array is for the different split
                public Animation[] animation { get; set; }
                public class Animation
                {
                    [XmlAttribute(AttributeName = "name")]
                    public string name { get; set; }

                    [XmlAttribute(AttributeName = "id")]
                    public string id { get; set; }

                    [XmlElement(ElementName = "source")]
                    public Source[] sources { get; set; }
                    public class Source
                    {
                        [XmlAttribute(AttributeName = "id")]
                        public string id { get; set; }

                        [XmlElement(ElementName = "float_array")]
                        public Float_Array float_array { get; set; }
                        public class Float_Array
                        {
                            [XmlText]
                            public string values { get; set; }

                            [XmlAttribute(AttributeName = "count")]
                            public string count { get; set; }

                            [XmlAttribute(AttributeName = "id")]
                            public string id { get; set; }
                        }
                    }
                }
            }
        }

        public class Library_Effects
        {
            [XmlElement(ElementName = "effect")]
            public Effect[] effect { get; set; }
            public class Effect
            {
                [XmlAttribute(AttributeName = "id")]
                public string id { get; set; }

                [XmlElement(ElementName = "profile_COMMON")]
                public Profile_Common profile { get; set; }
                public class Profile_Common
                {
                    [XmlElement(ElementName = "technique")]
                    public Technique technique { get; set; }
                    public class Technique
                    {
                        [XmlElement(ElementName = "lambert")]
                        public Lambert lambert { get; set; }
                        public class Lambert
                        {
                            [XmlElement(ElementName = "diffuse")]
                            public Diffuse diffuse { get; set; }
                            public class Diffuse
                            {
                                [XmlElement(ElementName = "color")]
                                public string color { get; set; }
                            }
                        }
                    }
                }
            }
        }
        public class Visual_scenes
        {
            [XmlElement(ElementName = "visual_scene")]
            public Visual_scene visual_scene { get; set; }
            public class Visual_scene
            {
                [XmlElement(ElementName = "node")]
                public Node[] node { get; set; }
                public class Node
                {
                    [XmlAttribute(AttributeName = "id")]
                    public string id { get; set; }

                    [XmlAttribute(AttributeName = "name")]
                    public string name { get; set; }

                    //matrix transformation
                    [XmlElement(ElementName = "matrix")]
                    public string matrix { get; set; }

                    [XmlElement(ElementName = "instance_geometry")]
                    public Instance_Geometry instance_geometry { get; set; }
                    public class Instance_Geometry
                    {
                        [XmlElement(ElementName = "bind_material")]
                        public Bind_Material bind_material { get; set; }
                        public class Bind_Material
                        {
                            [XmlElement(ElementName = "technique_common")]
                            public Technique_Common technique_common { get; set; }
                            public class Technique_Common
                            {
                                [XmlElement(ElementName = "instance_material")]
                                public Instance_Material instance_material { get; set; }
                                public class Instance_Material
                                {
                                    [XmlAttribute(AttributeName = "symbol")]
                                    public string symbol { get; set; }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
