﻿using BetfairDotNet.Converters;
using BetfairDotNet.Enums.Betting;
using BetfairDotNet.Models.Betting;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace BetfairDotNet.Tests.ModelsTests.BettingModelTests;

public class PlaceInstructionTests {

    [Fact]
    public void PlaceInstruction_ShouldSerializeCorrectly() {
        // Arrange
        var placeInstruction = new PlaceInstruction {
            OrderType = OrderTypeEnum.LIMIT,
            SelectionId = 123456,
            Handicap = 1.5,
            Side = SideEnum.BACK,
            LimitOrder = new LimitOrder {
                Price = 1.23,
                Size = 10.0,
                PersistenceType = PersistenceTypeEnum.LAPSE,
            },
            LimitOnCloseOrder = new LimitOnCloseOrder {
                Price = 1.23,
                Liability = 10.0,
            },
            MarketOnCloseOrder = new MarketOnCloseOrder {
                Liability = 10.0,
            },
            CustomerOrderRef = "MyRef"
        };

        var expectedJObject = new JObject {
            { "orderType", "LIMIT" },
            { "selectionId", 123456 },
            { "handicap", 1.5 },
            { "side", "BACK" },
            { "limitOrder", new JObject() },
            { "limitOnCloseOrder", new JObject() },
            { "marketOnCloseOrder", new JObject() },
            { "customerOrderRef", "MyRef" }
        };

        // Act
        var serializedJson = JsonConvert.Serialize(placeInstruction);
        var serializedJObject = JObject.Parse(serializedJson);

        // Assert
        serializedJObject.Should().BeEquivalentTo(expectedJObject);
    }
}
