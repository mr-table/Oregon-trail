﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VehicleFloods.cs" company="Ron 'Maxwolf' McDowell">
//   ron.mcdowell@gmail.com
// </copyright>
// <summary>
//   When crossing a river there is a chance that your wagon will flood if you choose to caulk and float across the
//   river.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TrailSimulation.Event
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Entity;
    using Game;

    /// <summary>
    ///     When crossing a river there is a chance that your wagon will flood if you choose to caulk and float across the
    ///     river.
    /// </summary>
    [DirectorEvent(EventCategory.RiverCross, EventExecution.ManualOnly)]
    public sealed class VehicleFloods : EventItemDestroyer
    {
        /// <summary>Fired by the item destroyer event prefab before items are destroyed.</summary>
        /// <param name="destroyedItems">Items that were destroyed from the players inventory.</param>
        /// <returns>The <see cref="string"/>.</returns>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            return destroyedItems.Count > 0
                ? TryKillPassengers("drowned")
                : "no loss of items.";
        }

        /// <summary>Fired when the event handler associated with this enum type triggers action on target entity. Implementation is
        ///     left completely up to handler.</summary>
        /// <param name="userData">Entities which the event is going to directly affect. This way there is no confusion about
        ///     what entity the event is for. Will require casting to correct instance type from interface instance.</param>
        public override void Execute(RandomEventInfo userData)
        {
            base.Execute(userData);

            // Cast the source entity as vehicle.
            var vehicle = userData.SourceEntity as Vehicle;
            Debug.Assert(vehicle != null, "vehicle != null");

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle.ReduceMileage(20 - 20*GameSimulationApp.Instance.Random.Next());
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
            _floodPrompt.AppendLine("Vehicle floods");
            _floodPrompt.AppendLine("while crossing the");
            _floodPrompt.Append("river results in");
            return _floodPrompt.ToString();
        }
    }
}