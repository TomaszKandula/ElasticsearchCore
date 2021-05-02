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

        public IActionResult Index() => View();

        /// <summary>
        /// Return book that contains searched text in Title field; or first ten books.
        /// </summary>
        /// <param name="AInputQuery"></param>
        /// <returns></returns>
        public IActionResult TitleSearch(string AInputQuery)
        {
            if (!string.IsNullOrWhiteSpace(AInputQuery)) 
            {
                var LShowResult = FClient
                    .Search<BookModel>(ASearch => ASearch
                    .Query(AQuery => AQuery
                    .Match(AMatch => AMatch
                    .Field(ABookModel => ABookModel.Title)
                    .Query(AInputQuery))));

                FLogger.LogInformation("{AInputQuery}",$"Searched phrase: {AInputQuery}");
                return View(LShowResult);
            }

            FLogger.LogInformation("Display first ten results");
            var LShowAll = FClient
                .Search<BookModel>(ASearch => ASearch
                .Query(AQuery => AQuery
                .MatchAll()));

            return View(LShowAll);
        }

        /// <summary>
        /// Aggregation of "pageCounts" field.
        /// </summary>
        /// <returns></returns>
        public IActionResult PageCount() 
        {
            FLogger.LogInformation("Return page counts");
            return View(FClient.Search<BookModel>(ASearch => ASearch
                .Query(AQuery => AQuery
                .MatchAll())
                .Aggregations(AAggregate => AAggregate
                .Range("pageCounts", ARange => ARange
                .Field(ABookModel => ABookModel.PageCount)
                .Ranges(ARanges => ARanges
                    .From(0), ARanges => ARanges
                    .From(200).To(400), ARanges => ARanges
                    .From(400).To(600), ARanges => ARanges
                    .From(600))))));
        }

        /// <summary>
        /// Aggregation of filtered "categories" term.
        /// </summary>
        /// <returns></returns>
        public IActionResult Categories() 
        {
            FLogger.LogInformation("Return aggregation of filtered terms");
            return View(FClient.Search<BookModel>(ASearch => ASearch
                .Query(AQuery => AQuery
                .MatchAll())
                .Aggregations(AAggregate => AAggregate
                .Terms("categories", ATerms => ATerms
                .Field("categories.keyword")))));
        }

        public IActionResult Privacy() => View();

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
