using Ev.Common.Core;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Entities.Collectables;
using Ev.Domain.Server.World.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ev.Domain.Server.World
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
                    case 'S': _spawnPoints.Add((x, y)); break;
                    case '_': break; // ignore
                    case 'I': _state[x, y] = new Iron(rnd); break;
                    case 'F': _state[x, y] = new Food(rnd); break;
                    case 'W': _state[x, y] = new Wood(rnd); break;
                    case 'X': _state[x, y] = new Wall(); break;
                    case '~': _state[x, y] = new Water(); break;
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
        public override void AddTribe(string tribeName, Color color)
        {
            var coord = NextAvailableSpawnPoint();

            var tribe = new Tribe(tribeName, coord, color);
            State[coord.x, coord.y] = tribe;

            _tribes.Add(tribe);
            _spawnPoints.Remove(coord);
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
