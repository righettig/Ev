﻿using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Core;
using Ev.Domain.Client.World;
using System;
using static System.Math;

namespace Ev.Domain.Client.Behaviours.Core
{
    /// <summary>
    /// The base class for all tribe behaviours.
    /// It provides utility methods to help perform basic tasks like navigation, attack etc.
    /// </summary>
    public abstract class TribeBehaviour : ITribeBehaviour
    {
        protected readonly IRandom _rnd;

        private const int WORLD_STATE_SIZE = WorldState.WORLD_STATE_SIZE;

        protected TribeBehaviour(IRandom rnd)
        {
            _rnd = rnd ?? throw new ArgumentNullException(nameof(rnd));
        }

        /// <summary>
        /// Generates the next move of the Tribe. 
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="tribe">The current tribe state.</param>
        /// <returns>The action chosen to be performed by the given Tribe during the current turn with the specified world state.</returns>
        public abstract IGameAction DoMove(IWorldState state, ITribe tribe);

        public virtual string DebugBehaviour() => "";

        // TODO: create a separate class for the factory methods for action so they can be unit-testable.
        // This will force those who define a concrete class to pass an instance of the TribeActionFactory
        // Do I really want this? I could create two ctor overloads, one that accepts an ITribeActionFactory
        // and another that uses a singleton instance of it.
        // TribeBehaviour.DefaultActionFactory

        #region Factory methods for actions

        /// <summary>
        /// Factory method that creates the Attack move.
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="targetPosition">The world state position of the attack target.</param>
        /// <remarks>
        /// Action is validated by the game master. 
        /// If invalid - for instance trying to attack a tribe which is too far away - the tribe gets disqualified.
        /// </remarks>
        /// <returns>The attack action.</returns>
        protected static IGameAction Attack(IWorldState state, (int x, int y) targetPosition)
        {
            var enemy = state.GetEntity<ITribe>(targetPosition);

            if (enemy is null)
            {
                throw new TribeNotFoundException("tribe not found.", nameof(targetPosition));
            }

            // TODO: server MUST check this action is valid
            return new AttackAction(enemy.Name);
        }

        /// <summary>
        /// Factory method that creates the Attack move.
        /// </summary>
        /// <param name="target">The attacked target.</param>
        /// <remarks>
        /// Action is validated by the game master. 
        /// If invalid - for instance trying to attack a tribe which is too far away - the tribe gets disqualified.
        /// </remarks>
        /// <returns>The attack action.</returns>
        protected static IGameAction Attack(ITribe target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            // TODO: server MUST check this action is valid
            return new AttackAction(target.Name);
        }

        /// <summary>
        /// Factory method that creates the Hold action.
        /// </summary>
        /// <returns>The hold action.</returns>
        protected static IGameAction Hold() => new HoldAction();

        /// <summary>
        /// Factory method that creates the Move action.
        /// </summary>
        /// <param name="direction">The direction you wish the tribe to take.</param>
        /// <returns>The move action.</returns>
        protected static IGameAction Move(Direction direction) => new MoveAction(direction);

        /// <summary>
        /// Factory method that creates the Upgrade attack action.
        /// </summary>
        /// <returns>The upgrade attack action.</returns>
        protected static IGameAction UpgradeAttack() => new UpgradeAttackAction();

        /// <summary>
        /// Factory method that creates the Upgrade defenses action.
        /// </summary>
        /// <returns>The upgrade defenses action.</returns>
        protected static IGameAction UpgradeDefenses() => new UpgradeDefensesAction();

        #endregion

        #region Base behaviours utility methods

        /// <summary>
        /// Moves the tribe towards the specified world entity. 
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="entity">The entity you wish to move towards.</param>
        /// <example>
        /// Say that the entity is positioned at NW with respect to the tribe, the Move(NW) action gets generated.
        /// </example>
        /// <returns>The resulting move action, if the entity is found, Move(-1, -1) otherwise.</returns>
        protected static IGameAction MoveTowards(IWorldState state, IWorldEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var position = FindPosition(state, el => el == entity);

            return MoveTowards(position);
        }

        // TODO: unit test this

        /// <summary>
        /// Moves the tribe towards the specified target position.
        /// </summary>
        /// <remarks>The position is considered to be a world state position.</remarks>
        /// <example>
        /// Say that the entity position is at NW with respect to the tribe, the Move(NW) action gets generated.
        /// </example>
        /// <param name="targetPosition">The position of the target you wish to move towards.</param>
        /// <returns>The resulting move action.</returns>
        protected static IGameAction MoveTowards((int x, int y) targetPosition) => targetPosition switch
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.N),
            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE >= 0 => Move(Direction.S),
            (int, int) t when t.x - WORLD_STATE_SIZE >= 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.E),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.W),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.NE),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.NW),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.SE),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.SW),

            _ => null
        };

        /// <summary>
        /// Moves the tribe in the opposite direction of the specified world entity.
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="entity">The entity you wish to move away from.</param>
        /// <example>
        /// Say that the entity is positioned at NW with respect to the tribe, the Move(SE) action gets generated.
        /// </example>
        /// <returns>The resulting move action, if the entity is found, Move(-1, -1) otherwise.</returns>
        protected static IGameAction MoveAwayFrom(IWorldState state, IWorldEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var position = FindPosition(state, el => el == entity);

            return MoveAwayFrom(position);
        }

        // TODO: unit test this
        /// <summary>
        /// Moves the tribe away from the specified world state position.
        /// </summary>
        /// <remarks>The position is considered to be a world state position.</remarks>
        /// <param name="targetPosition">The position of the target you wish to move away from.</param>
        /// <example>
        /// Say that the entity position is at NW with respect to the tribe, the Move(SE) action gets generated.
        /// </example>
        /// <returns>The resulting move action.</returns>
        protected static IGameAction MoveAwayFrom((int x, int y) targetPosition) => targetPosition switch
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.S),
            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE >= 0 => Move(Direction.N),
            (int, int) t when t.x - WORLD_STATE_SIZE >= 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.W),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.E),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.SW),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.SE),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.NW),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.NE),

            _ => null
        };

        /// <summary>
        /// Generates a move towards a random direction.
        /// </summary>
        /// <returns>The move action.</returns>
        protected IGameAction RandomWalk() => new MoveAction((Direction) _rnd.Next(8));

        /// <summary>
        /// Returns the world state position of the tribe found first.
        /// </summary>
        /// <returns>The world state position as a (x,y) tuple if a tribe if found, (-1, -1) otherwise.</returns>
        protected static (int x, int y) FindAnEnemy(IWorldState state) => FindPosition(state, el => el is ITribe);

        /// <summary>
        /// Returns the position of the collectable found first.
        /// </summary>
        /// <returns>The world state position as a (x,y) tuple if a collectable is found, (-1, -1) otherwise.</returns>
        protected static (int x, int y) FindACollectable(IWorldState state) => FindPosition(state, el => el is ICollectableWorldEntity);

        /// <summary>
        /// Returns the world state position of the highest found food cell.
        /// </summary>
        /// <returns>The world state position expressed as a (x,y) tuple or (-1, -1) if no collectable is found.</returns>
        protected static (int x, int y) FindHighestValueFood(IWorldState state)
        {
            var highestPos = (-1, -1);
            var highest = 0;

            state.Traverse((el, x, y) =>
            {
                if (el is ICollectableWorldEntity { Type: CollectableWorldEntityType.Food } c && c.Value > highest)
                {
                    if (c.Value > highest)
                    {
                        highest = c.Value;
                        highestPos = (x, y);
                    }
                }
            });

            return highestPos;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Checks if an entity is present at the given world state position.
        /// </summary>
        /// <param name="target">The world state position to check.</param>
        /// <returns>True if an entity was found.</returns>
        protected static bool Found((int x, int y) target) => !NotFound(target);

        /// <summary>
        /// Checks if an entity is NOT present at the given world state position.
        /// </summary>
        /// <param name="target">The world state position to check.</param>
        /// <returns>True if no entity was found.</returns>
        protected static bool NotFound((int x, int y) target)
        {
            return target.x == -1 && target.y == -1;
        }

        protected static bool IsEnemyClose(IWorldState state)
        {
            return state.Closest<ITribe>() != null;
        }

        // TODO: unit test this
        /// <summary>
        /// Checks if the given world state position can be reached in a single turn.
        /// </summary>
        /// <param name="target">The world state position to check.</param>
        /// <returns>True if the position can be reached by the tribe.</returns>
        protected static bool Close((int x, int y) target)
        {
            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 0 0 T 0 
            // 0 0 0 0 0 

            return Abs(target.x - WORLD_STATE_SIZE) <= 1 && Abs(target.y - WORLD_STATE_SIZE) <= 1;
        }

        /// <summary>
        /// Checks if a generic world entity is close enough to be reached in a single turn.
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="other">The world entity to check.</param>
        /// <returns>True if the specified world entity is in proximity of the tribe.</returns>
        protected static bool Close(IWorldState state, IWorldEntity other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var otherPosition = FindPosition(state, other);

            return Close(otherPosition);
        }

        /// <summary>
        /// Retrieves the world state position of the given entity.
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="entity">The entity to find.</param>
        /// <returns>The world state position of the specified entity, if found, (-1, -1) otherwise.</returns>
        protected static (int x, int y) FindPosition(IWorldState state, IWorldEntity entity) => FindPosition(state, el => el == entity);

        /// <summary>
        /// Retrieves the world state position of the first entity that satisfies a predicate function.
        /// </summary>
        /// <param name="state">The current world state.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <returns>The world state position of the specified entity, if found, (-1, -1) otherwise.</returns>
        protected static (int x, int y) FindPosition(IWorldState state, Predicate<IWorldEntity> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var position = (-1, -1);

            state.Traverse((el, x, y) =>
            {
                if (predicate(el))
                {
                    position = (x, y);
                }
            });

            return position;
        }

        #endregion
    }
}