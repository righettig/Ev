using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;
using System;

namespace Ev.Domain.Client
{
    public class TribeAgent : ITribeAgent
    {
        public string Name { get; }
        public Color Color { get; }
        public ITribeBehaviour Behaviour { get; }

        public TribeAgent(string name, Color color, ITribeBehaviour behaviour)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            Name      = name;
            Color     = color;
            Behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour));
        }

        public void OnGameStart()
        {
            //Console.WriteLine("CLIENT - Game started");
        }

        public void OnTurnStart()
        {
            //Console.WriteLine("CLIENT - Turn started");
        }

        public void OnBeforeMove(IWorldState worldState, ITribe tribe)
        {
            if (worldState == null) throw new ArgumentNullException(nameof(worldState));
            if (tribe == null) throw new ArgumentNullException(nameof(tribe));

            //Console.WriteLine("CLIENT - It's your turn!");
            
            // TODO: create helper for client
            //Helpers.Debug.AsAlive(tribe);
            //Helpers.Debug.DumpWorldState(worldState);
        }

        public void OnTurnEnd()
        {
            //Console.WriteLine("CLIENT - Turn ended");
        }

        public void OnGameEnd()
        {
            //Console.WriteLine("CLIENT - Game ended");
        }
    }
}
