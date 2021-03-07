using Ev.Agents.Tribe;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System;
using Random = Ev.Domain.Utils.Random;

namespace Ev.Samples.HelloTribe
{
    public class HelloWorldBehaviour : TribeBehaviour
    {
        public HelloWorldBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribeState tribe) => Hold();
    }

    class Program
    {
        static void Main()
        {
            var helloWorldTribe =
                new TribeAgent("tribe1", Color.Cyan, new HelloWorldBehaviour(new Random(1)))
                    .Start()
                    .Connect("127.0.0.1:30051");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            helloWorldTribe.Shutdown();
        }
    }
}