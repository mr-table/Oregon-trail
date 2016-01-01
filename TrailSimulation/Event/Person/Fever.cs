﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/01/2016@3:27 AM

namespace TrailSimulation.Event.Person
{
    using Entity.Person;
    using Module.Director;
    using Prefab;

    /// <summary>
    ///     Fever, also known as pyrexia and febrile response, is defined as having a temperature above the normal range due to
    ///     an increase in the body's temperature.
    /// </summary>
    [DirectorEvent(EventCategory.Person)]
    public sealed class Fever : PersonInfect
    {
        /// <summary>Fired after the event has executed and the infection flag set on the person.</summary>
        /// <param name="person">Person whom is now infected by whatever you say they are here.</param>
        /// <returns>Name or type of infection the person is currently affected with.</returns>
        protected override string OnPostInfection(Person person)
        {
            return $"{person.Name} has dysentery.";
        }
    }
}