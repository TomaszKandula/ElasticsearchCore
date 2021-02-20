# ElasticSearchCore

Example of the popular open-source search and analytics engine – Elasticsearch to power a simple search system in ASP.NET Core using RazorPages.

## Tech-Stack

1. RazorPages (NET Core 3.1 / C# language).
1. NEST - Official Elasticsearch Client.

## Features

Key features follows:
1. Distributed and scalable, including the ability for sharding and replicas.
1. Documents uses JSON format.
1. All interactions are done over a REST API.
1. Many software like popular Kibana which allows interrogation and analysis of data.
1. Loads of client-side libraries for all popular languages.

## Razor Pages

Example pages:

1. Title Search - allow to search via Title, when no "Term" is given, then we return first ten books from the database. For given search text, we return items that contains searched text.
1. Page Count - This page returns range aggregation of the books pages (this is done by using "Aggregate" function).
1. Categories - This page returns total count for appearing categories (aggregation on text fields) in the whole database.

## Dataset

Single index in example dataset:

```json
{ 
    "title":"string", 
    "isbn":"string", 
    "pageCount":0, 
    "thumbnailUrl":"string", 
    "shortDescription":"string", 
    "longDescription":"string", 
    "status":"string", 
    "authors":"string", 
    "categories":"string" 
}
```

Assuming you are running Elasticsearch engine in Docker on port 9200, to load your own dataset from JSON file, execute following command:

`curl -XPOST localhost:9200/books/book/_bulk --data-binary @dataset.json -H "Content-Type: application/json"`

Ensure that your file will have empty newline and the end of the file. Additionally, if using PowerShell, make sure to use backtick before `@` character.

# End Note

A comperhensive article about Elasticsearch can be found at Knowi portal: [Elasticsearch: What It Is, How It Works, And What It’s Used For](https://www.knowi.com/blog/what-is-elastic-search/)

