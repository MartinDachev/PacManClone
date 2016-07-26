using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    class GameTimer
    {
        public GameTimer(double interval)
        {
            this.Interval = interval;
            this.Time = 0.0;
        }

        public GameTimer()
            : this(0.0)
        {

        }

        public void Start()
        {
            this.Enabled = true;
        }

        public void Pause()
        {
            this.Enabled = false;
        }

        public void Reset()
        {
            this.Time = 0.0;
        }

        public bool Enabled { get; set; }

        public void Tick(double time)
        {
            if (this.Enabled)
            {
                this.Time += time;
            }
        }

        public double Interval { get; set; }

        public double Time { get; private set; }

    }
}
