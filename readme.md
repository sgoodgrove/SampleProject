# Overview

I've written this API using .Net Core, not for any special reason other than it is the latest whiz-bang technology. I could have just as well written this using ASP.Net MVC / WebAPI, the differences aren't worlds apart.

I've only taken this project to a certain degree as there is seemingly endless opportunity to go full-on Enterprise-FizzBuzz on it, so for time purposes I have drawn a line at the point where the project is.

I've used [LiteDB](http://www.litedb.org/) for persistence, mainly because it's NoSQL, self-contained and easy to use/redistribute. 

The solution should just be loadable into Visual Studio 2017 and should run from there.

## API

I have implemented the following methods

* `GET /api/Users` retrieves the full list of users
* `GET /api/Users/{Id}` retrieves a single user
* `PUT /api/Users/{Id}` updates an existing user (only updates supplied fields that are NOT NULL, so if you only supply a `Name` field, only the name is updated
* `POST /api/Users` creates a new user
* `DELETE /api/Users/{Id}` deletes a user

I've implemented `GET /api/Users` as an asynchronous method only to demonstrate some primitive use of `async` / `await`: LiteDB itself does not directly support asynchronous operations.

## Testing

I have implemented a few unit tests and a few integration tests to demonstrate some test writing. I have used XUnit as I get the impression that is what is used at OneIdentity, though in actual fact I didn't like the assertion methods, so I decided to experiment and give [FluentAssertions](https://fluentassertions.com/) and [NFluent](http://n-fluent.net/) a whirl. Coming from an NUnit background I still prefer the NUnit fluent assertions interface (but I don't like the fact that NUnit assertions are not strongly typed). Of the two here I preferred the style of NFluent, but found it to be a tiny bit immature in comparison to FluentAssertions.

In the unit tests of the merge record functionality I included numerous assertions; typically I try to stick to the notion of having only one assertion per test, but again, due to time constraints I chose to go with this for brevity. In a more real-world situation I may choose to go with a reflection based solution to ensure that all readable properties match or I may choose to isolate each assert in its own test.

I've not written tests on the controller actions themselves, but I'm aware that I could have done. I could have written tests that create a new instance of the `UsersController` class supplying a mock `IUserRepository` to the constructor and testing the outcomes... again though putting a reasonable time restriction on my time on nthis means that I didn't go into this much depth.

## Code Structure

### Controllers/UsersController

Exactly what you'd expect, not much of interest in here except to say that in a more real-world situation I would be considering **logging**, **authentication**, **authorisation** and **error** handling and I would most likely be handling them with ASP.Net filters.

### Models/*

Contains a set of POCO classes that map to the JSON structure provided.

### Pages/*
I'm not actually using these, initially when I started to write the project I was intending on writing a simple UI using something such as Mithril.js, however I realised that I was making work for myself here and decided instead to just demonstrate the API with the use of the [Insomnia](https://insomnia.rest/) REST client.

### Services

I've done interfaces for these as I would typically.

#### Services/UserRepository

CRUD operations for persisting user details (backed by [LiteDB](http://www.litedb.org/))

#### Services/UserServices

Only contains a single method `MergeRecords()` which takes two records and creates a new record which updates a "target" record with values from a "source" record **where values are present** (i.e. not null). The naming here could possibly do with a little work.

I've written this method as I thought it would be handy if that is how the UPDATE API call should work and it demonstrates a little extra C# code that the basic controller/repository methods don't.

### Startup

I've modified `Startup.cs` so that [Autofac](https://autofac.org/) dependency injection is used. I've also placed a little routine in there (`ImportSampleData()`) that reads in the supplied `sampleUsers.json` data file and pre-populates the LiteDB database with.

The place I'm calling `ImportSampleData()` from might not be entirely suitable... I'm more comfortable with regular ASP.NET MVC than .Net Core.

