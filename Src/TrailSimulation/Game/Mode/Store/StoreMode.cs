﻿using System;
using System.Text;
using TrailSimulation.Core;
using TrailSimulation.Entity;

namespace TrailSimulation.Game
{
    /// <summary>
    ///     Manages a general store where the player can buy food, clothes, bullets, and parts for their vehicle.
    /// </summary>
    public sealed class StoreMode : ModeProduct<StoreCommands, StoreInfo>
    {
        /// <summary>
        ///     Defines the text prefix which will go above the menu, used to show any useful information the game mode might need
        ///     to at the top of menu selections.
        /// </summary>
        protected override string MenuHeader { get; set; }

        /// <summary>
        ///     Defines the current game mode the inheriting class is going to take responsibility for when attached to the
        ///     simulation.
        /// </summary>
        public override Mode Mode
        {
            get { return Mode.Store; }
        }

        /// <summary>
        ///     Offers chance to purchase a special vehicle part that is also an animal that eats grass and can die if it starves.
        /// </summary>
        private void BuyOxen()
        {
            UserData.SelectedItem = Parts.Oxen;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Offers the chance to buy some food for the players to eat everyday.
        /// </summary>
        private void BuyFood()
        {
            UserData.SelectedItem = Resources.Food;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Offers chance to buy some clothing to protect the players party in harsh climates.
        /// </summary>
        private void BuyClothing()
        {
            UserData.SelectedItem = Resources.Clothing;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Offers chance to buy bullets for hunting animals and killing them for food.
        /// </summary>
        private void BuyAmmunition()
        {
            UserData.SelectedItem = Resources.Bullets;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Offers a chance to purchase some spare wheels for the vehicle.
        /// </summary>
        private void BuySpareWheels()
        {
            UserData.SelectedItem = Parts.Wheel;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Offers a chance to purchase some spare axles for the vehicle.
        /// </summary>
        private void BuySpareAxles()
        {
            UserData.SelectedItem = Parts.Axle;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Offers a chance to purchase some spare vehicle tongues.
        /// </summary>
        private void BuySpareTongues()
        {
            UserData.SelectedItem = Parts.Tongue;
            SetState(typeof (BuyItemState));
        }

        /// <summary>
        ///     Attaches a game mode state what will show the player some basic information about what the various items mean and
        ///     what their purpose is in the simulation.
        /// </summary>
        private void StoreAdvice()
        {
            SetState(typeof (StoreAdviceState));
        }

        /// <summary>
        ///     Detaches the store mode from the simulation and returns to the one previous.
        /// </summary>
        private void LeaveStore()
        {
            // Complain if the player does not have any oxen to pull their vehicle.
            if (GameSimulationApp.Instance.Trail.IsFirstLocation &&
                UserData.Transactions[SimEntity.Animal].Quantity <= 0)
            {
                UserData.SelectedItem = Parts.Oxen;
                SetState(typeof (MissingItemState));
                return;
            }

            // Check if player can afford the items they have selected.
            if (GameSimulationApp.Instance.Vehicle.Balance < UserData.GetTransactionTotalCost())
            {
                SetState(typeof (StoreDebtState));
                return;
            }

            // Remove the store if we make this far!
            RemoveModeNextTick();
        }

        /// <summary>
        ///     Fired when the game mode changes it's internal state. Allows the attached mode to do special behaviors when it
        ///     realizes a state is set or removed and act on it.
        /// </summary>
        protected override void OnStateChange()
        {
            base.OnStateChange();

            // Print out store items and their prices for user selection.
            UpdateDebts();
        }

        /// <summary>
        ///     Called after the mode has been added to list of modes and made active.
        /// </summary>
        public override void OnModePostCreate()
        {
            // Print out store items and their prices for user selection.
            UpdateDebts();

            // Trigger the store advice automatically on the first location, deeper check is making sure we are in new game mode also (travel mode always there).
            if (GameSimulationApp.Instance.Trail.IsFirstLocation &&
                GameSimulationApp.Instance.ModeManager.ModeCount > 1)
            {
                StoreAdvice();
            }
        }

        /// <summary>
        ///     Called when the mode manager in simulation makes this mode the currently active game mode. Depending on order of
        ///     modes this might not get called until the mode is actually ticked by the simulation.
        /// </summary>
        public override void OnModeActivate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Fired when the simulation adds a game mode that is not this mode. Used to execute code in other modes that are not
        ///     the active mode anymore one last time.
        /// </summary>
        public override void OnModeAdded()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Fired when this game mode is removed from the list of available and ticked modes in the simulation.
        /// </summary>
        protected override void OnModeRemoved(Mode mode)
        {
            base.OnModeRemoved(mode);

            // Store is only going to process transactions on removal when it is the one up for removal.
            if (mode != Mode)
                return;

            // TODO: Store needs to keep existing items if not first turn and add to them.

            // Modify the vehicles cash from purchases they made.
            var totalBill = UserData.GetTransactionTotalCost();
            var amountPlayerHas = GameSimulationApp.Instance.Vehicle.Balance - totalBill;
            UserData.Transactions[SimEntity.Cash] =
                new SimItem(UserData.Transactions[SimEntity.Cash],
                    (int) amountPlayerHas);

            // Process all of the pending transactions in the store receipt info object.
            foreach (var transaction in UserData.Transactions)
            {
                GameSimulationApp.Instance.Vehicle.BuyItem(transaction.Value);
            }

            // Remove all the transactions now that we have processed them.
            UserData.ClearTransactions();

            // Travel mode waits until it is by itself on first location and first turn.
            if (GameSimulationApp.Instance.Trail.IsFirstLocation &&
                GameSimulationApp.Instance.ModeManager.ModeCount >= 3)
            {
                // Establishes configured vehicle onto running simulation, sets first point on trail as visited.
                // NOTE: Also calculates initial distance to next point!
                GameSimulationApp.Instance.Trail.ArriveAtNextLocation();
            }
        }

        /// <summary>
        ///     Build up representation of store inventory as text.
        /// </summary>
        private void UpdateDebts()
        {
            // Header text for above menu.
            var headerText = new StringBuilder();
            headerText.AppendLine($"--------------------------------");
            headerText.AppendLine($"{GameSimulationApp.Instance.Trail.CurrentLocation?.Name} General Store");
            headerText.AppendLine($"{GameSimulationApp.Instance.Time.Date}");
            headerText.Append("--------------------------------");
            MenuHeader = headerText.ToString();

            // Keep track if this is the first point of interest, it will alter how the store shows values.
            var isFirstPoint = GameSimulationApp.Instance.Trail.IsFirstLocation;

            // Clear all the commands store had, then re-populate the list with them again so we can change the titles dynamically.
            ClearCommands();

            // Animals
            AddCommand(BuyOxen, StoreCommands.BuyOxen,
                $"Oxen              {UserData.Transactions[SimEntity.Animal].ToString(isFirstPoint)}");

            // Food
            AddCommand(BuyFood, StoreCommands.BuyFood,
                $"Food              {UserData.Transactions[SimEntity.Food].ToString(isFirstPoint)}");

            // Clothes
            AddCommand(BuyClothing, StoreCommands.BuyClothing,
                $"Clothing          {UserData.Transactions[SimEntity.Clothes].ToString(isFirstPoint)}");

            // Bullets
            AddCommand(BuyAmmunition, StoreCommands.BuyAmmunition,
                $"Ammunition        {UserData.Transactions[SimEntity.Ammo].ToString(isFirstPoint)}");

            // Wheel
            AddCommand(BuySpareWheels, StoreCommands.BuySpareWheel,
                $"Vehicle wheels    {UserData.Transactions[SimEntity.Wheel].ToString(isFirstPoint)}");

            // Axle
            AddCommand(BuySpareAxles, StoreCommands.BuySpareAxles,
                $"Vehicle axles     {UserData.Transactions[SimEntity.Axle].ToString(isFirstPoint)}");

            // Tongue
            AddCommand(BuySpareTongues, StoreCommands.BuySpareTongues,
                $"Vehicle tongues   {UserData.Transactions[SimEntity.Tongue].ToString(isFirstPoint)}");

            // Exit store
            AddCommand(LeaveStore, StoreCommands.LeaveStore, "Leave store");

            // Footer text for below menu.
            var footerText = new StringBuilder();
            footerText.Append($"{Environment.NewLine}--------------------------------{Environment.NewLine}");

            // Calculate how much monies the player has and the total amount of monies owed to store for pending transaction receipt.
            var totalBill = UserData.GetTransactionTotalCost();
            var amountPlayerHas = GameSimulationApp.Instance.Vehicle.Balance - totalBill;

            // If at first location we show the total cost of the bill so far the player has racked up.
            footerText.Append(isFirstPoint
                ? $"Total bill:            {totalBill.ToString("C2")}" +
                  $"{Environment.NewLine}Amount you have:       {amountPlayerHas.ToString("C2")}"
                : $"You have {GameSimulationApp.Instance.Vehicle.Balance.ToString("C2")} to spend.");
            MenuFooter = footerText.ToString();
        }
    }
}