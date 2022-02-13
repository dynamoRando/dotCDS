# What is dotCDS?
CDS stands for "cooperative data store." dotCDS is a thin C# written database wrapper that enables you to give data that belongs to your users back to them. It's an iteration of the implementation of a "[Cooperative Database System](https://github.com/dynamoRando/CooperativeDatabaseSystems)." It's also an evolution of the idea behind [DrummerDB](https://github.com/dynamoRando/DrummerDB), which itself is an iteration of a former codebase FrostDB that tried to implement these ideas into a database system.

dotCDS is NOT -
* An ORM (Object Relational Mapper). dotCDS returns data in the shape it was requested. Currently dotCDS only supports SQL statements, so you interact with dotCDS with SQL and get SQL results back.

* A full database system. All the data passed through dotCDS ultimately is stored in the backing data stores of dotCDS, and this is a database of your choosing.

# Architecture
dotCDS is a service that sits alongside your chosen database system. Your application makes calls to dotCDS which then either passes those calls thru to the database or if needed routes the request from users who may have the information you need. 

Your database is known as the "host" and your users that have data that your system has references to are known as "participants."

To interact with the dotCDS service you leverage a dotCDS client. This client is written in C#; though underneath the client (and the service) both leverage gRPC for calls, so nothing prevents clients in other languages from being written. 

## Planned Supported Data Stores
The planned backing database systems are:

* MS SQL Server
* Postgres
* Sqlite

# Planned Implementation
dotCDS must configured with a primary data store, which is usually a database system of your choosing. You will need in your database system to give dotCDS a reference to a user who has access to your databases that you intend to leverage for cooperation of data. To understand the data in your database that is cooperative, dotCDS will leverage its own schema (or a separate database entirely if configured/needed) in your database system (by default usually "coop") with its own tables that map references to your participants and to tables that are being shared.

As you make calls to dotCDS for data (currently only via SQL statements, though in the future this is planned to be expanded), dotCDS examines the context of the SQL statement and either passes the statement through back to the local database or makes external requests to participants for the data. The returned result is always a result set.

# Planned Components
| Library         | Use                                                                              |
| --------------- | -------------------------------------------------------------------------------- |
| DotCDS          | The core dotCDS functions. This library is hosted by a service.                  |
| DotCDS.Service  | A service that sits alongside your database system.                              |
| DotCDS.Console  | A console application that hosts dotCDS. Used for proof-of-concept.              |
| DotCDS.Client   | A library that your application uses to interact with dotCDS.                    |
| DotCDS.Terminal | A console application that can be used to interact with dotCDS.                  |
| DotCDS.Common   | A library of gRPC objects and other structures common to DotCDS and it's client. |
                                                                                  
                                                                                  