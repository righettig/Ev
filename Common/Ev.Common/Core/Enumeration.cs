using Ev.Common.Core.Interfaces;

namespace Ev.Common.Core
{
    public abstract class Enumeration : IEnumeration
    {
        public string Name { get; }

        public int Id { get; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;
    }
}
