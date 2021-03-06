using System;


namespace ModChart
{
    class BpmAdjuster
    {
        public float Bpm { get; private set; }
        public float Njs { get; private set; }
        public float StartBeatOffset { get; private set; }
        public float BeatLength { get; private set; }
        public float SecondLength { get; private set; }
        public float HalfJumpBeats { get; private set; }
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
        public float GetDefiniteDurationBeats(float duration)
        {
            return duration - (2f * HalfJumpBeats);
        }
        public float GetDefiniteDurationBeats(float duration, float NjsOffset)
        {
           // Console.WriteLine(duration - (2f * GetJumps(NjsOffset, Njs, Bpm)));
            return duration - (2f * GetJumps(NjsOffset,Njs,Bpm));
        }
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
            num2 += _noteJumpStartBeatOffset;
            if (num2 < 1)
            {
                num2 = 1;
            }
            return num2;
        }
    }
}
