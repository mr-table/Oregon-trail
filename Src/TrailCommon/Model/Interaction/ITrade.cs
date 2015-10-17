﻿using System.Collections.ObjectModel;

namespace TrailCommon
{
    public interface ITrade : IGameMode
    {
        ReadOnlyCollection<IItem> PossibleTrades { get; }
        void TradeAttempt(IItem item);
    }
}