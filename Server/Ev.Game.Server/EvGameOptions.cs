using System;
using Ev.Common.Core.Interfaces;

namespace Ev.Game.Server
{
    public class EvGameOptions
    {
        public bool RenderEachTurn { get; init; }
        public bool WaitAfterEachMove { get; init; }
        public bool DumpWinnerHistory { get; init; }
        public string WinnerHistoryFilename { get; init; }
        public IRandom Random { get; init; }
        public int Players { get; }

        public EvGameOptions(int players)
        {
            if (players <= 0) throw new ArgumentOutOfRangeException(nameof(players));

            Players = players;
        }
    }
}
