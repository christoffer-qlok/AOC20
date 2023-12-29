namespace Part2
{
    internal class Program
    {
        static Dictionary<string, List<string>> Dst = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> Src = new Dictionary<string, List<string>>();
        static Dictionary<string, IModule> Modules = new Dictionary<string, IModule>();

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(" -> ");
                string src;
                char kind = 'b';
                if (parts[0] == "broadcaster")
                {
                    src = parts[0];
                }
                else
                {
                    src = parts[0].Substring(1);
                    kind = parts[0][0];
                }
                var dst = parts[1].Split(", ");

                if (Dst.ContainsKey(src))
                {
                    Dst[src].AddRange(dst);
                }
                else
                {
                    Dst[src] = dst.ToList();
                }

                foreach (var d in dst)
                {
                    if (Src.ContainsKey(d))
                    {
                        Src[d].Add(src);
                    }
                    else
                    {
                        Src[d] = new List<string>() { src };
                    }
                }

                switch (kind)
                {
                    case '%':
                        Modules[src] = new FlipFlop();
                        break;
                    case '&':
                        Modules[src] = new Conjunction();
                        break;
                    case 'b':
                        Modules[src] = new Broadcast();
                        break;
                    default:
                        throw new Exception($"Bad kind of module '{kind}'");
                }
            }

            foreach (var pair in Modules)
            {
                if (pair.Value is Conjunction con)
                {
                    foreach (var s in Src[pair.Key])
                    {
                        con.InputToSignal[s] = false;
                    }
                }
            }

            var cycles = new Dictionary<string, List<int>>();
            for (int i = 1; i < 10000; i++)
            {
                //Console.WriteLine($"{i + 1}:");
                var frontier = new Queue<SignalEvent>();
                frontier.Enqueue(new SignalEvent()
                {
                    Src = "button",
                    Dst = "broadcaster",
                    Signal = Signal.Low
                });

                while (frontier.Count > 0)
                {
                    var signalEvent = frontier.Dequeue();
                    if(signalEvent.Dst == Src["rx"].Single() && signalEvent.Signal == Signal.High)
                    {
                        if(cycles.ContainsKey(signalEvent.Src))
                        {
                            cycles[signalEvent.Src].Add(i);
                        } else
                        {
                            cycles[signalEvent.Src] = new List<int>() { i };
                        }
                    }
                    if (!Modules.ContainsKey(signalEvent.Dst))
                        continue;

                    var module = Modules[signalEvent.Dst];
                    var signal = module.GetPulse(signalEvent.Signal, signalEvent.Src);
                    if (signal == Signal.None)
                        continue;


                    foreach (var d in Dst[signalEvent.Dst])
                    {
                        frontier.Enqueue(new SignalEvent()
                        {
                            Src = signalEvent.Dst,
                            Dst = d,
                            Signal = signal,
                        });
                    }
                }
            }
            
            var cycleLengths = new List<long>();
            foreach (var pair in cycles)
            {
                int diff = GetCycle(pair.Value.ToArray());
                cycleLengths.Add(diff);
            }

            Console.WriteLine($"Cycle lengths: {string.Join(',', cycleLengths)}");
            Console.WriteLine($"LCM: {LCM(cycleLengths.ToArray())}");

        }

        public static int GetCycle(int[] seq)
        {
            int diff = seq[1] - seq[0];
            for (int i = 2; i < seq.Length - 1; i++)
            {
                int newDiff = seq[i + 1] - seq[i];
                if(diff != newDiff)
                {
                    Console.WriteLine($"Bad diff {diff} - {newDiff}");
                    return 0;
                }
            }
            return diff;
        }

        static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long LCM(long[] numbers)
        {
            long lcmResult = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                lcmResult = LCM(lcmResult, numbers[i]);
            }

            return lcmResult;
        }

        static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }
    }
}
