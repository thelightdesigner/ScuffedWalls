using ModChart;
using ModChart.Wall;
using System.Collections;
using System.Linq;

namespace ScuffedWalls
{
    //scuffed function parser request data layout
    //------------------------------------------//
    public class S_Request
    {
        public S_Request(string[] args)
        {
            this.args = args;
        }
        public string[] args { get; set; }
        public W_Request[] Workspace { get; set; }

        public class W_Request
        {
            public string Name { get; set; }
            public F_Request[] Function { get; set; }

            public class F_Request
            {
                public string Name { get; set; }
                public F_Parameter Parameter { get; set; }
                public F_Parameter RepeatAddition { get; set; }
                public int RepeatCount { get; set; }
                public class F_Parameter
                {
                    public float Time { get; set; }
                    public string Path { get; set; }
                    public string FullPath { get; set; }
                    public AppendTechnique appendTechnique { get; set; }
                    public float toBeat { get; set; }
                    public bool convertToProps { get; set; }
                    public float rainbowFactor { get; set; }
                    public float propFactor { get; set; }
                    public string Type { get; set; }
                    public string Name { get; set; }
                    public float Duration { get; set; }
                    public int MaxLineLength { get; set; }
                    public bool IsBlackEmpty { get; set; }
                    public float Size { get; set; }
                    public float SpreadSpawnTime { get; set; }
                    public float Alpha { get; set; }
                    public float Thicc { get; set; }
                    public float Shift { get; set; }
                    public bool Centered { get; set; }
                    public float Compression { get; set; }
                    public ModelTechnique modelTechnique { get; set; }
                    public bool HasAnimation { get; set; }
                    public BeatMap.CustomData CustomData { get; set; }
                    public BeatMap.CustomData.CustomEvents.Data CustomEventData { get; set; }
                }
            }

        }



        //scuffed wall file parser//
        void Parse()
        {
            var FileIterator = args.GetEnumerator();

            do
            {

            }
            while (FileIterator.MoveNext());
        }
    }
}
