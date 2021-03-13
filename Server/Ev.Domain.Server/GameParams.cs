using System;
using Ev.Domain.Server.World;

namespace Ev.Domain.Server
{
    // https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/
    public sealed class GameParams
    {
        private GameParams()
        {
            // default values
            
            UpgradeActionsCost   = new WorldResources { WoodCount = 10, IronCount = 5 };
            UpgradeActionsLength = 2;
            UpgradeBonus         = .1f;

            WinGain     = 20;
            DefeatLoss  = 20;
            
            IdleLoss    = 1;
            MoveLoss    = 3;
        }

        private static readonly Lazy<GameParams> Lazy = new(() => new GameParams());
        
        public static GameParams Instance => Lazy.Value;

        public WorldResources UpgradeActionsCost { get; }
        public int UpgradeActionsLength { get; }
        public float UpgradeBonus { get; }
        
        public int WinGain { get; }
        public int DefeatLoss { get; }

        public int IdleLoss { get; }
        public int MoveLoss { get; }

        // TODO
        //WORLD_STATE_SIZE (traverseList -> calc based on worldState size)
        //Food/Iron/Wood Max value
    }
}