# TaskTracker
.Net Core Web API with CRUD functionalities.

## Table of Contents

- Technical Overview
  - Description
  - Technology stack
  - Patterns and Principles
- Documentation
  - Controllers and methods
  - Start

# Technical Overview
## Description
TaskTracker provides:

Ability to create / view / edit / delete information about projects
Ability to create / view / edit / delete information about tasks
Ability to add and remove tasks from a project (one project can contain several tasks)
Ability to view all tasks in the project
Ability to filter and sort projects with methods (start at, end at, range, exact value) and by all project fields

TaskTracker has a three-level project architecture.
Data access level is stored in a class library TaskTracker.DAL.
It also provides with Unit Testing in TaskTracker.Tests.


## Technology stack
Technology stack used:
- .NET Core Web API
- Entity Framework Core
- MS SQL Server

## Patterns and Principles
Patterns and Principles implemented:
- Repository pattern
- Unit Of Work pattern
- SOLID principles
- OOP principles

# Documentation
##Start
To start using TaskTracker you need to clone this repository and run it with Microsoft Visual Studio.
