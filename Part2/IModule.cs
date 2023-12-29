using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
{
    internal interface IModule
    {
        Signal GetPulse(Signal signal, string src);
    }
}
