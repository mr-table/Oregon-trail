﻿namespace TrailEntities
{
    /// <summary>
    ///     Defines all of the management options for the game like removing tombstones, resetting high scores, and lets you
    ///     also view the original high scores.
    /// </summary>
    public enum OptionCommands
    {
        /// <summary>
        ///     Shows the current top ten list as it is known from flat-file JSON list, players high scores can go into this list.
        /// </summary>
        SeeCurrentTopTen = 1,

        /// <summary>
        ///     Shows the original top ten list as it is known internally as a hard-coded list.
        /// </summary>
        SeeOriginalTopTen = 2,

        /// <summary>
        ///     Resets the current top ten list back to hard-coded defaults.
        /// </summary>
        EraseCurrentTopTen = 3,

        /// <summary>
        ///     Removes any custom tombstone messages from the trail, there are two opportunities in the game where if party leader
        ///     dies they can leave a tombstone on the trail for future travelers to find.
        /// </summary>
        EraseTomstoneMessages = 4,

        /// <summary>
        ///     Delete any save games from the slots they are holding. Any saved progress will be lost.
        /// </summary>
        EraseSavedGames = 5,

        /// <summary>
        ///     Removes the management options game mode and returns to main menu which should be below it.
        /// </summary>
        ReturnToMainMenu = 6
    }
}