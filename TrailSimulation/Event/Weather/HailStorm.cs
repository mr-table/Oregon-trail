﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/01/2016@3:27 AM

namespace TrailSimulation.Event.Weather
{
    using System.Collections.Generic;
    using System.Text;
    using Entity;
    using Entity.Vehicle;
    using Module.Director;
    using Prefab;
    using Window.RandomEvent;

    /// <summary>
    ///     Bad hail storm damages supplies, this uses the item destroyer prefab like the river crossings do.
    /// </summary>
    [DirectorEvent(EventCategory.Weather, EventExecution.ManualOnly)]
    public sealed class HailStorm : ItemDestroyer
    {
        /// <summary>Fired by the item destroyer event prefab before items are destroyed.</summary>
        /// <param name="destroyedItems"></param>
        /// <returns>The <see cref="string" />.</returns>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Grab an instance of the game simulation.
            var game = GameSimulationApp.Instance;

            // Check if there are enough clothes to keep people warm, need two sets of clothes for every person.
            return game.Vehicle.Inventory[Entities.Clothes].Quantity >= (game.Vehicle.PassengerLivingCount*2) &&
                   destroyedItems.Count < 0
                ? "no loss of items."
                : TryKillPassengers("frozen");
        }

        /// <summary>
        ///     Fired when the event handler associated with this enum type triggers action on target entity. Implementation is
        ///     left completely up to handler.
        /// </summary>
        /// <param name="userData">
        ///     Entities which the event is going to directly affect. This way there is no confusion about
        ///     what entity the event is for. Will require casting to correct instance type from interface instance.
        /// </param>
        public override void Execute(RandomEventInfo userData)
        {
            base.Execute(userData);

            // Cast the source entity as vehicle.
            var vehicle = userData.SourceEntity as Vehicle;

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle?.ReduceMileage(vehicle.Mileage - 5 - GameSimulationApp.Instance.Random.Next()*10);
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab after items are destroyed.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnPreDestroyItems()
        {
            var _floodPrompt = new StringBuilder();
            _floodPrompt.Clear();
            _floodPrompt.AppendLine("Severe hail storm");
            _floodPrompt.Append("results in");
            return _floodPrompt.ToString();
        }
    }
}