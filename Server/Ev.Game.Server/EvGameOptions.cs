namespace Ev.Game.Server
{
    public class EvGameOptions 
    {
        public bool RenderEachTurn { get; set; }
        public bool WaitAfterEachMove { get; set; }
        public bool DumpWinnerHistory { get; set; }
        public string WinnerHistoryFilename { get; set; }
    }
}
