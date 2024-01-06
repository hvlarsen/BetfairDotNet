using BetfairDotNet;
using BetfairDotNet.Models.Streaming;
using BetfairDotNet.Enums.Streaming;
using BetfairDotNet.Enums.Betting;
using BetfairDotNet.Models.Betting;

/* For at det skal virke skal tilføjes en CustomBetfairClient og CustomHttpClientAdapter */
namespace BetfairDotNetApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiKey = "vnZwZgMAb0bCtKBr";
            var sessionToken = "IIcU5zZ+15jIH1ZmhlSADSlF670QSG9MNN8HjCqwFzU=";

            var betfairClient = new CustomBetfairClient(apiKey, sessionToken);
            
            var bettingService = betfairClient.Betting;
            string marketId = "1.222116043"; // Replace with your actual market ID
            long selectionId = 55190;   // Replace with the actual selection ID of the first runner

            // Create a place instruction for a BACK bet
            var placeInstruction = new PlaceInstruction
            {
                SelectionId = selectionId,
                Side = SideEnum.BACK,
                OrderType = OrderTypeEnum.LIMIT,
                LimitOrder = new LimitOrder
                {
                    Size = 30, // Stake of 30
                    Price = 1.5, // Replace with the desired odds
                    PersistenceType = PersistenceTypeEnum.PERSIST // Or another appropriate type
                }
            };

            // Create a list of place instructions (just one in this case)
            var placeInstructions = new List<PlaceInstruction> { placeInstruction };

            // Place the bet
            var placeExecutionReport = await bettingService.PlaceOrders(
                marketId,
                placeInstructions
            );

            // Check the result
            if (placeExecutionReport.Status == ExecutionReportStatusEnum.SUCCESS)
            {
                Console.WriteLine("Bet placed successfully");
            }
            else
            {
                Console.WriteLine($"Failed to place bet: {placeExecutionReport.ErrorCode}");
            }

            
            /*
            var stream = betfairClient.Streaming
                                      .CreateStream(sessionToken)
                                      .WithAutoReconnection(3000, 120000)
                                      .ForMarketIds("1.222116043")
                                      .ReturningMarketDataFor(10, MarketDataFilterEnum.EX_BEST_OFFERS,
                                                               MarketDataFilterEnum.EX_TRADED,
                                                               MarketDataFilterEnum.EX_MARKET_DEF)
                                      .ConflateUpdatesTo(200)
                                      .OnMarketChange(ms =>
                                      {
                                          Console.WriteLine($"Market Snapshot:");
                                          PrintMarketSnapshot(ms);
                                      })
                                      .OnOrderChange(os => Console.WriteLine("Order snapshot received."))
                                      .OnException(ex => Console.WriteLine($"Exception: {ex.Message}"));

            await stream.Subscribe();

            await Task.Delay(Timeout.Infinite);
            */
        }
        
        public static void PrintMarketSnapshot(MarketSnapshot marketSnapshot)
        {
            Console.WriteLine($"Market ID: {marketSnapshot.MarketId}, Timestamp: {marketSnapshot.Timestamp}");

            foreach (var runnerEntry in marketSnapshot.RunnerSnapshots)
            {
                Console.WriteLine($"Runner ID: {runnerEntry.Key}");
                Console.WriteLine("  To Lay:");
                PrintPriceLadder(runnerEntry.Value.ToLay);
                Console.WriteLine("  To Back:");
                PrintPriceLadder(runnerEntry.Value.ToBack);
                Console.WriteLine("  Traded:");
                PrintPriceLadder(runnerEntry.Value.Traded);
            }
        }

        public static void PrintPriceLadder(PriceLadder priceLadder)
        {
            if (priceLadder != null && priceLadder.Depth > 0)
            {
                for (int i = 0; i < priceLadder.Depth; i++)
                {
                    var priceSize = priceLadder[i];
                    if (priceSize != null)
                    {
                        Console.WriteLine($"    Price: {priceSize.Price}, Size: {priceSize.Size}");
                    }
                }
            }
            else
            {
                Console.WriteLine("    No data available.");
            }
        }
    }
}

