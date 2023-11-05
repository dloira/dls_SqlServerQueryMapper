# Proxy pattern for handling database access over ADO.NET

Here, you could find an approach to use the Proxy pattern for handling the database access over ADO.NET. Currently, the market is plenty of really nice tools to work with databases; nevertheless, once the code is running in production environment, the performance tunning tasks could become tough when an ORM is involved. Hands up who has never dreamed of owning a magic wand, when your database administrator colleague phones you under water because one of your queries is collapsing the server and you can not find it in your code because the ORM is making it for you behind its wonderfull abstract language !!

To make that easier, Sql Server setup is provided using docker containers technology.

The code was built in .NET Core 6.0 using the official SqlClient driver provided by RabbitMQ and distributed via nuget. By the way, as you could check reading the code, it was implemented a Factory method pattern to make easy further connection with any other database provider like Oracle or MySql.

## Getting Started

The idea behind is to get a Proxy class with a reference variable that points to the repository object in charge of database accessing. Everytime the repository will be executed, the proxy object will intercept the calling in order to run itself before to pass the request to the repository object; the proxy logic retrieves the attribute with which the repository was anotated by reflexion and invokes it.

There were implemented two attributes, one for manage the database connection and another for query execution. Both share the same foundations in their logic, building the concrete Data Access Object Factory and executing the Sql Command with the query configured within the repository object.

Take a look for a while in this GoF observer pattern link for begginers https://refactoring.guru/design-patterns/proxy

Out of the box .NET gives a proxy factory solution based on DispatchProxy abstract class. Drill down the DBProxyFactory class in Create method. 

Please, find below the concept diagram for overview better understanding.

![Concept diagram](https://github.com/dloira/dls_SqlServerQueryMapper/blob/master/concept_diagram.jpg)

### SqlServer with Docker Compose

Docker Compose tool was used to provide an easy way to get up a local SqlServer server where to run the code for dispatch and consume messages.

The docker-compose.yml file, within docker folder, build the image running the DockerFile; it was made in this way to run the container from an image with the required database already created.

```docker
version: "3.6"

networks:
  mssql_net:
    name: mssql_net
    driver: bridge
      
services:
  mssql:
    build:
      context: .
      dockerfile: sqlserver/Dockerfile
    networks:
      - mssql_net
    environment:
      - SA_PASSWORD=P@55w0rd
      - ACCEPT_EULA=Y
      - MSSQL_PID=Express
    ports:
      - ${SQL_PORT:-1433}:1433
```

```docker
FROM mcr.microsoft.com/mssql/server:2022-latest
USER root
EXPOSE 1433
COPY sqlserver/docker-entrypoint.sh /usr/local/bin/
COPY sqlserver/docker-entrypoint-initdb.sh /usr/local/bin/
RUN chmod 777 /usr/local/bin/docker-entrypoint.sh \
    && ln -s /usr/local/bin/docker-entrypoint.sh /
RUN chmod 777 /usr/local/bin/docker-entrypoint-initdb.sh \
    && ln -s /usr/local/bin/docker-entrypoint-initdb.sh /
RUN apt-get update
RUN apt-get install dos2unix
RUN dos2unix -F /docker-entrypoint*
ENTRYPOINT ["/docker-entrypoint.sh"]
```

```bash
#!/bin/bash
echo "$0: Starting SQL Server"
docker-entrypoint-initdb.sh & /opt/mssql/bin/sqlservr
```

```bash
#!/bin/bash

# wait for database to start...
for i in {30..0}; do
  if /opt/mssql-tools/bin/sqlcmd -U SA -P ${SA_PASSWORD} -Q 'SELECT 1;' &> /dev/null; then
    echo "$0: SQL Server started"
    break
  fi
  echo "$0: SQL Server startup in progress..."
  sleep 1
done

echo "$0: Initializing database by parameter 2"
/opt/mssql-tools/bin/sqlcmd \
   -U SA -P ${SA_PASSWORD} \
   -Q 'CREATE DATABASE [Weather]'
echo "$0: SQL Server Database ready"
```

Once you get installed Docker Desktop locally https://www.docker.com/products/docker-desktop/, to run the docker compose file it is only needed to run the terminal prompt, place the path where the file is and execute the following command.

```bash
docker-compose -f docker-compose.yaml up -d
```

## Running the tests

The code could be downloaded and executed with Visual Studio 2022; just recompile the solution for downloading dependencies and enable the dls_SqlQueryMapper_Migration project as the starting project (only for the first time). Please, run the project for creating the database schema and populate the dummy data inside; as you could check, Fluent Migrator was used to code the migrations.

Next, set dls_SqlQueryMapper_Test project as the starting project. For better and easier undertanding a RESTFul web service facade was built; you could follow the code and check how the mapper was implemented to deal with transactionality and how the queries are retrieved from a config file. 

The config parameters are setup in appsettings.json file; it is not needed to change anything inside to run the test; by the way, feel free to configure wathever you like there. 

Once the project is up and running, a Swager Open API browser window arises and you could execute and debug the endopints logic.


## Built With

* [.Net-6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) - The .Net toolkit framework
* [Microsoft.Data.SqlClient-5.0.1](https://learn.microsoft.com/en-us/sql/connect/ado-net/introduction-microsoft-data-sqlclient-namespace?view=sql-server-ver16) - SQL Server client
* [FluentMigrator-3.3.2](https://fluentmigrator.github.io/) - Fluent Migrator
* [VisualStudio-22](https://visualstudio.microsoft.com/es/vs/community/) - IDE
* [Docker-20.10.23](https://www.docker.com/) - Containers
* [Microsoft.Extensions.Configuration-7.0.0](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration?view=dotnet-plat-ext-7.0) - Nuget package for setting up key/value application configuration properties 
* [Microsoft.Extensions.Configuration.Json-7.0.0](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.json?view=dotnet-plat-ext-7.0) - Nuget package for getting up configuration data from JSON file

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the tags on this repository. 
