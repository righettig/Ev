using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;
using Ev.Infrastructure;
using Ev.Infrastructure.Core;

namespace Ev.Domain.Server
{
    public class Game : IGame
    {
        private readonly IPlatform _platform;

        public Game(string type, IWorld world)
        {
            // return LocalPlatform or RemotePlatform depending on type
            _platform = new LocalPlatform(this); // use a factory?
        }

        public IPlatform GetPlatform()
        {
            return _platform;
        }

        // onRegisterAgent
        public void RegisterAgent(string agentName, string agentColor)
        {
            // adds server-side tribe to the world
            // if #agents is OK -> start game loop
        }

        public void Start()
        {
        }

        private void GameLoop()
        {
            _platform.OnGameStart();

            // loop through tribes {
                _platform.OnTurnStart();

                IWorldState worldState = null;
                ITribe tribe = null;

                var action = _platform.Update(worldState, tribe);

                // update world

                _platform.OnTurnEnd();
            //}

            _platform.OnGameEnd();
        }
    }
}
