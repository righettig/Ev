using Ev.Domain.Server.World.Core;
using Ev.Game.Server;
using System;
using Random = Ev.Common.Core.Random;

namespace Ev.Infrastructure
{
    public static class GameFactory
    {
        public static GameChannel Local(EvGameOptions options, IWorld world)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (world == null) throw new ArgumentNullException(nameof(world));

            var platform = PlatformFactory.Local;

            return new GameChannel(
                new Game.Server.Game(options, platform, world, options.Random ?? new Random()),
                platform);
        }
    }
}