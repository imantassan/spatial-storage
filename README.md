## Structure

The solution is developed using `ASP.NET Core MVC`, `.NET Core`, `EntityFrameworkCore` and `NetTopologySuite`.
Solution contains the following projects:

* `SpatialStorage` - holds the web app wrapping the service for convenient testing. It also holds test data under `Data\locations.csv` path which is loaded automatically if database is empty for `Debug` builds.
* `SpatialStorage.Repository` - contains data access layer, including repository class, database context and entities.
* `SpatialStorage.Services` - holds two services for retrieving data using different ways:
  * `SimpleLocationService` uses simple data types like `double` for querying data.
  * `GeographyLocationService` uses geography (geometry) data types like `IPoint` for querying data.
* `SpatialStorage.Services.Test` covers the respective service with unit tests.

## Starting the application

`SpatialStorage` is the web app that should be used for querying.

Insert the connection string to `appsettings.json/connectionStrings/Locations` pointing to either an empty database (will be migrated automatically) or existing database represeting the structure defined in the `SpatialStorage.Repository` project.

For a convenient load of test data, a CSV file with locations can be copied to `Data\locations.csv` in the same project. It will be loaded into database upon application start if `Debug` configuration is used. The operation might take a while as indexes are built along with the data insert.

`build.ps1` in the root directory will build the app and launch it.

## API

#### Querying

After running the `SpatialStorage` (web app) app or publishing it using `dotnet publish`, the API will expose the following methods:

* `/api/simple?lat=[latitude]&lng=[longitude]&range=[range in meters]&maxresults=[max count]`
* `/api/geo?lat=[latitude]&lng=[longitude]&range=[range in meters]&maxresults=[max count]`

Where
* `[latitude]` and `[longitude]` is the coordinates of the location to perform the search around.
* `[range in meters]` is the max allowed distance from any of the results to the location described above.
* `[max count]` allows the app to restrict the search to exact number of results. Because the results are sorted by distance, the closest results will be returned.

#### Results

Querying the API will produce a JSON object of the following structure:

````
{
    "time": "01:23:45.678"
    "results": [
        {
            "address": "location 1 address",
            "latitude": 123.456,
            "longitude": 54.321,
            "distance": 1234.5678
        },
        {
            "address": "location 2 address",
            "latitude": 13.456,
            "longitude": 4.321,
            "distance": 234.5678
        },
        ...
    ]
}
````

Where

* `"time"` is the `TimeSpan` reflecting the time it took to query the database for results. Mainly for informational purposes.
* `"results"` array contain the search results
  * `"address"`, `"latitude"`, `"longitude"` are the respective fields in the database
  * `"distance"` is the calculated distance in meters from the location returned as a result to the location where the search was performed.