using System;

namespace Ev.Agents.Spectator
{
    class Program
    {
        static void Main()
        {
            var spectator = new SpectatorAgent("spectator1").Start().Connect("127.0.0.1:30051");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            spectator.Shutdown();
        }
    }
}