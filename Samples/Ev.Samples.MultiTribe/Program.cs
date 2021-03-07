using Ev.Agents.Core;
using Ev.Agents.Tribe;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Utils;
using Ev.Samples.Behaviours;
using System;
using Random = Ev.Domain.Utils.Random;

namespace Ev.Samples.MultiTribe
{
    class Program
    {
        static IAgent<TribeAgent> CreateTribeAgent(string name, Color color, ITribeBehaviour behaviour, int? port = null)
        {
            return new TribeAgent(name, color, behaviour, port).Start().Connect("127.0.0.1:30051");
        }

        static void Main()
        {
            var tribe1 = CreateTribeAgent("tribe1", Color.Cyan,    new RandomWalkerTribeBehaviour(new Random(1)));
            var tribe2 = CreateTribeAgent("tribe2", Color.Magenta, new FleeTribeBehaviour(new Random(1)), 30053);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            tribe1.Shutdown();
            tribe2.Shutdown();
        }
    }
}