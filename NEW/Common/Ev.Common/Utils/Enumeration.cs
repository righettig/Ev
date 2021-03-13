namespace Ev.Common.Utils
{
    public abstract class Enumeration
    {
        public string Name { get; }

        public int Id { get; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;
    }
}
