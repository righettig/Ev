using Ev_NEW;

namespace Ev_Test
{
    // TODO: implement missing methods and add unit test

    class Program
    {
        static void Main(string[] args)
        {
            var t = new Tribe("t1", 2, 2);
            var enemy = new Tribe("t2", 0, 0);

            var world = new IWorldEntity[5, 5];
            world[0, 0] = enemy;
            world[2, 2] = t;
            world[1, 1] = new Collectable(CollectableType.Food, 10);

            var state = new WorldState(world);

            var foo = state.GetEntity<ITribe>((2, 2));

            var blockings = state.GetBlockings();
            var enemies = state.GetTribes();
            var collectables = state.GetCollectables();

            var allIron = state.GetCollectables(CollectableType.Iron);

            // cannot re-assign elements
            //state.Traverse((e, x, y) =>
            //{
            //    e = t;
            //});

            //var a = state.Closest<ICollectable>();

            //var generic            = state.Closest(t, enemies);      // return the closest tribe as a generic WorldEntity
            //var closestEnemy       = state.Closest<ITribe>(t);       // return the closest tribe
            //var closestCollectable = state.Closest<ICollectable>(t); // return the closest collectable
            //var closestBlocking    = state.Closest<IBlocking>(t);    // return the closest blocking

            //var closest            = state.Closest(t, enemy, world[1,1]); // return who is the closest entity between enemy and world[1,1]

            //var foo2 = state.Closest(t); // return the generic closest entity


            var b = new TestTribeBehaviour(new Random(1));

            var action = b.DoMove(state, t);
            //var action = b.DoMove(state, t);
        }
    }
}
