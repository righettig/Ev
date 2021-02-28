namespace Ev_NEW
{
    // READ-ONLY model (this is the client-side version of a Tribe)

    public interface ITribe : IWorldEntity
    {
        string Name { get; }
        (int x, int y) Position { get; }
    }

    public class Tribe : ITribe
    {
        public string Name { get; init; }
        public (int x, int y) Position { get; init; }

        public Tribe(string name, int x, int y)
        {
            Name = name;
            Position = (x, y);
        }
    }
}