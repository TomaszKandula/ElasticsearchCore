﻿using System.Diagnostics;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AQuery"></param>
        /// <returns></returns>
        public IActionResult Index(string AQuery)
        {
            if (!string.IsNullOrWhiteSpace(AQuery)) 
            {
                var LShowResult = FClient
                    .Search<BookViewModel>(Search => Search
                    .Query(Query => Query
                    .Match(Match => Match
                    .Field(Field => Field.Title)
                    .Query(AQuery))));

                FLogger.LogInformation($"Searched phrase: {AQuery}.");
                return View(LShowResult);
            }

            FLogger.LogInformation($"Display first 10 results.");
            var LShowAll = FClient
                .Search<BookViewModel>(Search => Search
                .Query(Query => Query
                .MatchAll()));

            return View(LShowAll);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult PageCount() 
        {
            FLogger.LogInformation("Return page counts.");
            return View(FClient.Search<BookViewModel>(Search => Search
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Categories() 
        {
            FLogger.LogInformation("Return kewords count.");
            return View(FClient.Search<BookViewModel>(Search => Search
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
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}