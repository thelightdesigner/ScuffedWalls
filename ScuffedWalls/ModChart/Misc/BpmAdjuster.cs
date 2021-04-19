using System;


namespace ModChart
{
    class BpmAdjuster
    {
        public float Bpm { get; private set; }
        public float Njs { get; private set; }
        public float StartBeatOffset { get; private set; }
        /// <summary>
        /// how long a beat is in seconds
        /// </summary>
        public float BeatLength { get; private set; }
        /// <summary>
        /// how long a second is in beats
        /// </summary>
        public float SecondLength { get; private set; }
        /// <summary>
        /// how many beats long the half jump duration is
        /// </summary>
        public float HalfJumpBeats { get; private set; }
        /// <summary>
        /// how many seconds long the half jump duration is
        /// </summary>
        public float HalfJumpSeconds { get; private set; }

        public BpmAdjuster(float Bpm, float Njs, float NjsOffset)
        {
            this.Bpm = Bpm;
            this.Njs = Njs;
            this.StartBeatOffset = NjsOffset;
            this.HalfJumpBeats = GetJumps(StartBeatOffset,Njs,Bpm);
            SetBeatLength();
        }
        public float GetPlaceTimeBeats(float beat)
        {
            return beat + HalfJumpBeats;
        }
        public float GetPlaceTimeBeats(float beat, float NjsOffset)
        {
            return beat + GetJumps(NjsOffset,Njs,Bpm);
        }
        /// <summary>
        /// gets a duration that makes the wall stay around for this exactly long
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public float GetDefiniteDurationBeats(float duration)
        {
            return duration - (2f * HalfJumpBeats);
        }
        public float GetDefiniteDurationBeats(float duration, float NjsOffset)
        {
           // Console.WriteLine(duration - (2f * GetJumps(NjsOffset, Njs, Bpm)));
            return duration - (2f * GetJumps(NjsOffset,Njs,Bpm));
        }
        /// <summary>
        /// Gets a njs offset that will make the note/bomb stay around for exactly this long in beats
        /// </summary>
        /// <returns></returns>
        public float GetDefiniteNjsOffsetBeats(float duration)
        {
            return (duration/2f) - GetJumps(0,Njs,Bpm);
        }
        /// <summary>
        /// gets the amount of beats that the map object will stay around for
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="NjsOffset"></param>
        /// <returns></returns>
        public float GetRealDuration(float duration, float NjsOffset)
        {
            return duration + (2 * GetJumps(NjsOffset,Njs,Bpm));
        }
        public float GetRealTime(float beat, float NjsOffset)
        {
            return beat - GetJumps(NjsOffset, Njs, Bpm);
        }
        public float GetRealDuration(float duration)
        {
            return duration + (2 * HalfJumpBeats);
        }
        public float GetRealTime(float beat)
        {
            return beat - HalfJumpBeats;
        }


        public float ToBeat(float seconds)
        {
            return seconds * SecondLength;
        }
        void SetBeatLength()
        {
            SecondLength = Bpm / 60f;
            BeatLength = 60f / Bpm;
            HalfJumpSeconds = BeatLength * HalfJumpBeats;
        }
        public static float GetJumps(float NjsOffset, float NJS, float BPM)
        {
            float _startHalfJumpDurationInBeats = 4;
            float _maxHalfJumpDistance = 18;
            float _startNoteJumpMovementSpeed = NJS;
            float _noteJumpStartBeatOffset = NjsOffset;

            float _noteJumpMovementSpeed = (_startNoteJumpMovementSpeed * BPM) / BPM;
            float num = 60f / BPM;
            float num2 = _startHalfJumpDurationInBeats;
            while (_noteJumpMovementSpeed * num * num2 > _maxHalfJumpDistance)
            {
                num2 /= 2;
            }
            //at this point num2 is equal to the hjd at njsoffset 0
            //num2 concat what equals 1
            num2 += _noteJumpStartBeatOffset;
            if (num2 < 1)
            {
                num2 = 1;
            }
            return num2;
        }
        public float GetJumps(float NjsOffset)
        {
            float _startHalfJumpDurationInBeats = 4;
            float _maxHalfJumpDistance = 18;
            float _startNoteJumpMovementSpeed = Njs;
            float _noteJumpStartBeatOffset = NjsOffset;

            float _noteJumpMovementSpeed = (_startNoteJumpMovementSpeed * Bpm) / Bpm;
            float num = 60f / Bpm;
            float num2 = _startHalfJumpDurationInBeats;
            while (_noteJumpMovementSpeed * num * num2 > _maxHalfJumpDistance)
            {
                num2 /= 2;
            }
            //at this point num2 is equal to the hjd at njsoffset 0
            //num2 concat what equals 1
            num2 += _noteJumpStartBeatOffset;
            if (num2 < 1)
            {
                num2 = 1;
            }
            return num2;
        }
    }
}
