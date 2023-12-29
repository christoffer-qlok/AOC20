using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC20
{
    internal class Conjunction : IModule
    {
        public Dictionary<string, bool> InputToSignal { get; set; } = new Dictionary<string, bool>();

        public Signal GetPulse(Signal signal, string src)
        {
            InputToSignal[src] = signal == Signal.High;

            if(InputToSignal.Values.All(b => b))
            {
                return Signal.Low;
            } 
            else
            {
                return Signal.High;
            }
        }
    }
}
