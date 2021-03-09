using Ev.Domain.Entities;
using Ev.Domain.Entities.Blocking;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ev.Domain.World
{
    public class MapWorld : BaseWorld
    {
        private readonly List<(int x, int y)> _spawnPoints = new();

        public static MapWorld LoadFromFile(string filename, IRandom rnd)
        {
            var header = File.ReadLines(filename).First().Replace(" ", "");

            var map = File.ReadAllText(filename);
            
            return new MapWorld(header.Length, map, rnd);
        }

        // TODO: unit test
        public MapWorld(int size, string map, IRandom rnd) : base(size, rnd)
        {
            var x = 0;
            var y = 0;

            foreach (var item in string.Concat(map.Where(c => !char.IsWhiteSpace(c))))
            {
                switch (item)
                {
                    // TODO: Collectables.Iron(rnd) or Blocking.Wall
                    case 'S': _spawnPoints.Add((x, y)); break;
                    case '_': break; // ignore
                    case 'I': _state[x, y] = new CollectableWorldEntity(rnd, 9) { Type = CollectableWorldEntityType.Iron }; break;
                    case 'F': _state[x, y] = new CollectableWorldEntity(rnd, 9) { Type = CollectableWorldEntityType.Food }; break;
                    case 'W': _state[x, y] = new CollectableWorldEntity(rnd, 9) { Type = CollectableWorldEntityType.Wood }; break;
                    case 'X': _state[x, y] = new BlockingWorldEntity { Type = BlockingWorldEntityType.Wall }; break;
                    case '~': _state[x, y] = new BlockingWorldEntity { Type = BlockingWorldEntityType.Water }; break;
                    default: throw new ArgumentException("Unsupported map token", nameof(map));
                }

                if (x == _size - 1)
                {
                    x = 0;
                    y++;
                }
                else 
                {
                    x++;
                }
            }
        }

        // TODO: unit test
        public override IWorld AddTribe(string tribeName, Color color)
        {
            var coord = NextAvailableSpawnPoint();

            var tribe = new Tribe(tribeName, coord, color);
            State[coord.x, coord.y] = tribe;

            _tribes.Add(tribe);
            _spawnPoints.Remove(coord);

            return this;
        }

        private (int x, int y) NextAvailableSpawnPoint()
        {
            if (_spawnPoints.Count == 0) {
                throw new NoMoreSpawnPointsAvailableException();
            }

            return _spawnPoints[0];
        }
    }

    public class NoMoreSpawnPointsAvailableException : Exception 
    { 
    }
}
