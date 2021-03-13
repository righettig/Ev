using Ev.Domain.Client;
using Ev.Domain.Server;

namespace Ev.Game.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Domain.Server.Game("local", new World());
            game.Start(); // server starts waiting for join requests

            var platform = game.GetPlatform();

            var agent1 = new TribeAgent("t1", "red", new TribeBehaviour());

            platform.RegisterAgent(agent1);
        }
    }
}
