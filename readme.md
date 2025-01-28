# LogEvent

LogEvent is a simple API intended to log an event to an SQLite database.

# Download

Compiled downloads are not available.


# Compiling

To clone and run this application, you'll need [Git](https://git-scm.com) and [.NET](https://dotnet.microsoft.com/) installed on your computer. From your command line:

```
# Clone this repository
$ git clone https://github.com/btigi/LogEvent

# Go into the repository
$ cd src

# Build  the app
$ dotnet build
```


# Configuration

The appsettings.json file contains three settings:
 - DbPath - the full path to the SQLite database. The file will be created if it does not exist.
 - SecurityKey - the security key. Events are only logged to the database if the security key matches the value in the HTTP request.


## Usage
``` 
POST https://your-url.ext?eventname=run&security=0abdd994-bbb2-4874-affc-2eb73bee05d4&category=test
```

- `eventname` - the name of the event

- `security` - the security key

- `category` - an optional category

The event is logged to the database only if the security key matches the value defined in config.

Successful requests return Created, failed security requests return Unauthorized.

## License

 LogEvent is licensed under [Mozilla Public Licence](https://www.mozilla.org/en-US/MPL/)