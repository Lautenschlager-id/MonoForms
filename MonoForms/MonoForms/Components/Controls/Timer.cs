using System;

namespace MonoForms
{
    public class Timer // Not an actual control
    {
        #region Property
        public bool Enabled { get; set; }

        public object Tag { get; set; }
        #endregion

        #region ControlProperty
        public int Interval
        {
            get => interval;
            set => interval = Math.Max(1, value);
        }
        #endregion

        #region PrivateControlProperty
        private int interval;

        private double time;
        #endregion

        #region Event
        public event EventHandler Tick;
        #endregion

        public Timer()
        {
            #region Properties
            Enabled = false;
            Interval = 100;
            #endregion
        }

        public void Update()
        {
            if (Enabled)
            {
                time += Game.Instance.GameTime.ElapsedGameTime.Milliseconds;
                if (time >= Interval)
                {
                    Tick?.Invoke(this, null);
                    time = 0;
                }
            }
        }

        #region Method
        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }
        #endregion
    }
}
