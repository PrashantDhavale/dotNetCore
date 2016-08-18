(A) Project folder structure. Using suggested convention
   ___
    |Services
    |____/src
    |________/OBDSim
    |______________/project.json
    |______________/Controllers
    |______________/Models
    |______________/Providers
    |____/tests
    |________/OBDSim.Tests
    |______________/project.json
    |______________/IntegrationTests
    |______________/UnitTests
    |____/global.json
    |____/Readme.md
    |____/Services.sln

(B) To start the OBDSim service on Development environment

    -In a console, browse to the Services folder and run the following in sequence

    dotnet restore
    cd src\OBDSim
    dotnet watch run

(C) To execute the xUnit tests

    -In a console, browse to the Services folder and run the following in sequence

    dotnet restore
    cd tests\OBDSim.Tests
    dotnet watch test

    Note:   The service application is referencing the Microsoft.DotNet.Watcher.Tools
            'dotnet watch' is a development time tool that runs a dotnet command when source files change.
            It can be used to compile, run tests, or publish when code changes.

(D) API's in this service are described using Swashbuckle Swagger.
    To list, view and try the API's open the swagger UI

    http://<server host name>:<port>/api/swagger/ui
    e.g. http://localhost:5000/swagger/ui

(E) Logging and Instrumentation

    >>  The service is configured to write log messages on Elasticsearch as a Serilog sink.
        The sink dependencies are specified in project.json file

        "Serilog.Extensions.Logging": "1.0.0-rc2-*"
        "Serilog.Settings.Configuration": "2.1.0"
        "Serilog.Sinks.Literate": "2.0.0"
        "Serilog.Sinks.Elasticsearch": "4.1.0"

        The Serilog logger is initialized in Startup.cs as follows;
       
        Log.Logger = new LoggerConfiguration()
                    .WriteTo.LiterateConsole()
                    .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                    {
                        IndexFormat = "logstash-index-{0:yyyy.MM}",
                        AutoRegisterTemplate = true
                    })
                    .MinimumLevel.Verbose()
                    .CreateLogger();

    >>  You can download Elasticsearch from https://www.elastic.co/downloads/elasticsearch
        Follow the setup instructions and start Elasticsearch service locally.

        Elasticsearch service will be available on http://localhost:9200/
        
    >>  You can now execute the OBDSim service API's using swagger or postman or just the browser
        http://localhost:5000/api/OBDSim/GetOBDSimsByEmployeeID/1234

    >>  Use a new browser tab to view elasticsearch log entries using 
          http://localhost:9200/logstash-index-2016.08/_search
          
        **Note that you will need to use the correct yyyy.MM value in the index
        
        All the logs entries related to OBDSim service are now available on elasticsearch.

    >>  Analytics using Kibana

        >   Download and install Kibana from https://www.elastic.co/downloads/kibana
        >   Configure kibana to reference elastic search at http://localhost:9200
        >   Follow the setup instructions and start the kibana service.
        >   Open a new browser window and open kibana visualizer using http://localhost:5601/
        >   Kibana visualizer will initialize and open in default state
        
