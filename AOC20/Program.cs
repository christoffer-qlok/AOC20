namespace AOC20
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
            long low = 0;
            long high = 0;
            for (int i = 0; i < 10000000; i++)
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
                    if(signalEvent.Dst == "rx" && signalEvent.Signal == Signal.Low)
                    {
                        Console.WriteLine($"rx low found at {i+1}");
                    }
                    if (signalEvent.Signal == Signal.Low) low++;
                    if (signalEvent.Signal == Signal.High) high++;
                    //Console.WriteLine(signalEvent.ToString());
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
                //Console.WriteLine();
            }
            Console.WriteLine($"low: {low}, high: {high}, product: {low * high}");
        }
    }
}
