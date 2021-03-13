namespace Ev.Game.Server
{
    public class EvGameOptions 
    {
        public bool RenderEachTurn { get; init; }
        public bool WaitAfterEachMove { get; init; }
        public bool DumpWinnerHistory { get; init; }
        public string WinnerHistoryFilename { get; init; }
    }
}
