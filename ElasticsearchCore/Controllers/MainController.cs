using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElasticsearchCore.Models;
using Nest;

namespace ElasticsearchCore.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> FLogger;
        private readonly ElasticClient FClient;

        public MainController(ILogger<MainController> ALogger, ElasticClient AClient)
        {
            FLogger = ALogger;
            FClient = AClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Return book that contains searched text in Title field; or first ten books.
        /// </summary>
        /// <param name="AQuery"></param>
        /// <returns></returns>
        public IActionResult TitleSearch(string AQuery)
        {
            if (!string.IsNullOrWhiteSpace(AQuery)) 
            {
                var LShowResult = FClient
                    .Search<BookModel>(Search => Search
                    .Query(Query => Query
                    .Match(Match => Match
                    .Field(Field => Field.Title)
                    .Query(AQuery))));

                FLogger.LogInformation($"Searched phrase: {AQuery}.");
                return View(LShowResult);
            }

            FLogger.LogInformation($"Display first ten results.");
            var LShowAll = FClient
                .Search<BookModel>(Search => Search
                .Query(Query => Query
                .MatchAll()));

            return View(LShowAll);
        }

        /// <summary>
        /// Aggregation of "pageCounts" field.
        /// </summary>
        /// <returns></returns>
        public IActionResult PageCount() 
        {
            FLogger.LogInformation("Return page counts.");
            return View(FClient.Search<BookModel>(Search => Search
                .Query(Query => Query
                .MatchAll())
                .Aggregations(Aggregate => Aggregate
                .Range("pageCounts", Range => Range
                .Field(Field => Field.PageCount)
                .Ranges(Ranges => Ranges
                    .From(0), Ranges => Ranges
                    .From(200).To(400), Ranges => Ranges
                    .From(400).To(600), Ranges => Ranges
                    .From(600))))));
        }

        /// <summary>
        /// Aggregation of filtered "categories" term.
        /// </summary>
        /// <returns></returns>
        public IActionResult Categories() 
        {
            FLogger.LogInformation("Return aggregation of filtered terms.");
            return View(FClient.Search<BookModel>(Search => Search
                .Query(Query => Query
                .MatchAll())
                .Aggregations(Aggregate => Aggregate
                .Terms("categories", Terms => Terms
                .Field("categories.keyword")))));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
