using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
{
    internal class Broadcast : IModule
    {
        public Signal GetPulse(Signal signal, string src)
        {
            return signal;
        }
    }
}
