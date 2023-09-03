﻿namespace BetfairDotNet.Models.Streaming;


/// <summary>
/// An immutable, atomic snapshot of orders for a given market.
/// </summary>
public record OrderMarketSnapshot {

    /// <summary>
    /// The id of this market.
    /// </summary>
    public string MarketId { get; init; } = string.Empty;

    /// <summary>
    /// The clock value of the initial sub message. Use on reconnect.
    /// </summary>
    public string InitialClk { get; init; } = string.Empty;

    /// <summary>
    /// The clock value of this update. Use on reconnect.
    /// </summary>
    public string Clk { get; init; } = string.Empty;

    /// <summary>
    /// The order snapshots for each runner.
    /// </summary>
    public Dictionary<long, OrderRunnerSnapshot> OrderRunnerSnapshots { get; init; } = new();
}
