# API-Exploration

## Goals
- Learn the benifets and differences of implementing the following API types built on .NET Core Web API
    - A Maturity Level 2 REST API
    - A Maturity Level 3 REST API (implementing HATEOS)
    - A GraphQL API
- Create an implementation of each API type with the following varying frontend / client archectures
    - Angular
    - .NET Core MVC
    - .NET Core MVC with Razor pages

## Requirements
- Use Identity Server and Entity Framework across all permutations
- Use either Postgres or SQL.
- Create the following features:
    - Login
        - Add Google, Facebook, and GitHub authentication logins
    - User account and roles management
    - Build a simple blogging feature.
        - Type:
            - User
            - Blogs
            - Comments
        - Full CRUD operations
- Add automated testing to the API and the Client (for NG apps).
- Add continious integration
- Add a hosting platform. Maybe Heroku.

## Structure
- API-Exploration.sln will have the following projects:
    - API.Authentication
        - A .NET Core Web API to handle authentication using identity framework.
        - Abstract this to its own API so that it can easially be shared across implementations
    - REST.API
        - The lvl 2 Rest implementation
    - REST.NG-REST
        - The Angular client.
        - Built with a .NET Core web app
    - REST.MVC-REST
        - Built with .NET Core MVC using API.REST
    - REST.RAZOR-REST
        - Built with .NET Core Razor pages using API.REST
    - HATEOS.API
        - The lvl 2 Rest implementation
    - HATEOS.NG-REST
        - The Angular client.
        - Built with a .NET Core web app
    - HATEOS.MVC-REST
        - Built with .NET Core MVC using API.REST
    - HATEOS.RAZOR-REST
        - Built with .NET Core Razor pages using API.REST
    - GRAPH.API
        - The lvl 2 Rest implementation
    - GRAPH.NG-REST
        - The Angular client.
        - Built with a .NET Core web app
    - GRAPH.MVC-REST
        - Built with .NET Core MVC using API.REST
    - GRAPH.RAZOR-REST
        - Built with .NET Core Razor pages using API.REST

## Path
Complete the goals in the following order:

1) Configure
    - Build API.Authentication with Identity server
    - Build Data layer
2) Rest Lvl 2
    - Build REST.API
    - Build REST.NG-REST

## ToDo's
1) Figure out how to handle the `[ValidateAntiForgaryToken]` on controller actions via PostMan