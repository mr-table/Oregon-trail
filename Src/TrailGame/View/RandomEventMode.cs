﻿using TrailCommon;
using TrailEntities;

namespace TrailGame
{
    public class RandomEventMode : GameMode
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailEntities.GameWindow" /> class.
        /// </summary>
        public RandomEventMode(Vehicle vehicle) : base(vehicle)
        {
        }

        public override SimulationMode Mode
        {
            get { return SimulationMode.RandomEvent; }
        }
    }
}