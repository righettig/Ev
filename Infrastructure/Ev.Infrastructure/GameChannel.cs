using Ev.Domain.Client.Core;
using Ev.Domain.Server.Core;
using Ev.Infrastructure.Core;
using System;

namespace Ev.Infrastructure
{
    public class GameChannel
    {
        private readonly IPlatform _platform;
        private readonly IGame _game;

        public GameChannel(IGame game, IPlatform platform)
        {
            _game     = game     ?? throw new ArgumentNullException(nameof(game));
            _platform = platform ?? throw new ArgumentNullException(nameof(platform));
        }

        public void RegisterAgent(params ITribeAgent[] agents)
        {
            if (agents == null) throw new ArgumentNullException(nameof(agents));
            if (agents.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(agents));

            _platform.RegisterAgent(_game, agents);
        }
    }
}