using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
{
    internal class SignalEvent
    {
        public string Src { get; set; }
        public string Dst { get; set; }
        public Signal Signal { get; set; }

        public override string ToString()
        {
            string signal;

            switch (Signal)
            {
                case Signal.None:
                    signal = "None";
                    break;
                case Signal.Low:
                    signal = "Low";
                    break;
                case Signal.High:
                    signal = "High";
                    break;
                default:
                    throw new Exception("Bad signal value");
                    break;
            }

            return $"{Src} -- {signal} --> {Dst}";
        }
    }
}
