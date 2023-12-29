using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
{
    internal class FlipFlop : IModule
    {
        public bool HighState { get; set; }

        public Signal GetPulse(Signal signal, string src)
        {
            if (signal != Signal.Low)
                return Signal.None;

            HighState = !HighState;
            return HighState ? Signal.High : Signal.Low;
        }
    }
}
