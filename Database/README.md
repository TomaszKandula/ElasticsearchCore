# Elasticsearch

## Database content

Assuming that Elasticsearch engine runs on Docker (using port 9200), to upload sample json file:

1. Open terminal.
1. Execute CURL command:

`curl -XPOST localhost:9200/books/book/_bulk --data-binary @dataset.json -H "Content-Type: application/json"`

Once the data has been uploaded, its content will be printed on the screen. From that moment, we can start using the demo application.

## End note

If you decide to make your own json dataset file, make sure that your file will have empty newline and the end of the file. Otherwise, it will not be possible to uploaded this way. Also please make sure to use backtick before `@` character if `curl` is executed in PowerShell.
