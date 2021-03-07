using Ev.Agents.Tribe;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Utils;
using System;
using Random = Ev.Domain.Utils.Random;

namespace Ev.Samples.SingleTribe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Wrong command line params. Format: name color behaviour behaviourSeed localPort gameMasterPort");
                return;
            }

            var name = args[0];
            var color = args[1];
            var behaviour = args[2];
            var behaviourSeed = int.Parse(args[3]);
            var localPort = int.Parse(args[4]);
            var gameMasterPort = int.Parse(args[5]);

            var assemblyQualifiedName =
                $"Ev.Samples.Behaviours.{behaviour}TribeBehaviour, Ev.Behaviours, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null";

            var type = Type.GetType(assemblyQualifiedName, true, true);

            var behaviourInstance =
                Activator.CreateInstance(
                    type,
                    new[] { new Random(behaviourSeed) }) as ITribeBehaviour;

            var tribe = new TribeAgent(name, Enum.Parse<Color>(color), behaviourInstance, localPort)
                .Start()
                .Connect($"127.0.0.1:{gameMasterPort}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            tribe.Shutdown();
        }
    }
}