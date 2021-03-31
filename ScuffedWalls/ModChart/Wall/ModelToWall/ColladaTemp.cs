using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ModChart.Wall
{
    /// <summary>
    /// A basic schema structure of a collada file. Made specifically for blender, idk how other modeling programs will export.
    /// </summary>
    [Serializable, XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", ElementName = "COLLADA")]
    public class Collada
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

            /// <summary>
            ///outer most animation array is for the different objects in the model
            /// </summary>
            public Animations[] animation { get; set; }
            public class Animations
            {
                [XmlAttribute(AttributeName = "name")]
                public string name { get; set; }

                [XmlElement(ElementName = "animation")]

                /// <summary>
                /// inner most animation array is for the different split
                /// </summary>
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
                        [XmlAttribute("url")]
                        public string url { get; set; }

                        [XmlElement(ElementName = "bind_material")]
                        public Bind_Material bind_material { get; set; }
                        public class Bind_Material
                        {
                            [XmlElement(ElementName = "technique_common")]
                            public Technique_Common technique_common { get; set; }
                            public class Technique_Common
                            {
                                [XmlElement(ElementName = "instance_material")]
                                public Instance_Material[] instance_material { get; set; }
                                public class Instance_Material
                                {
                                    [XmlAttribute(AttributeName = "symbol")]
                                    public string symbol { get; set; }
                                }
                            }
                        }
                    }
                    [XmlElement("instance_camera")]
                    public Instance_cam instance_cam { get; set; }
                    public class Instance_cam
                    {
                        [XmlAttribute("url")]
                        public string url { get; set; }
                    }
                }
            }
        }
    }




}




