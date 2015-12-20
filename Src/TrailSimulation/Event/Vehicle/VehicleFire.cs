﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TrailSimulation.Entity;
using TrailSimulation.Game;

namespace TrailSimulation.Event
{
    /// <summary>
    ///     Fire in the vehicle occurs, there is a chance that some of the inventory items or people were burned to death.
    /// </summary>
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class VehicleFire : EventItemDestroyer
    {
        /// <summary>
        ///     Fired by the item destroyer event prefab before items are destroyed.
        /// </summary>
        /// <param name="destroyedItems">Items that were destroyed from the players inventory.</param>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("burned")
                : "no loss of items.";
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab after items are destroyed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            var firePrompt = new StringBuilder();
            firePrompt.Clear();
            firePrompt.AppendLine("Fire in the wagon");
            firePrompt.Append("resulting in ");
            return firePrompt.ToString();
        }
    }
}