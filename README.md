# TaskTracker
.Net Core Web API with CRUD functionalities.

## Table of Contents

- Task Description
- Technical Overview
  - Description
  - Technology stack
  - Patterns and Principles
- Documentation
  - Controllers and methods
  - Start

# Task Description
CODE STYLE REQUIREMENTS
The code of the completed task should:
Be well structured
Be easy to read
Contain the necessary comments

The program you have written must be a complete program product, i.e. should be easy to install, provide for the handling of non-standard situations, be resistant to incorrect user actions, etc. 
TECHNOLOGY REQUIREMENTS
Tasks should be completed:
On .Net or .Net Core (EntityFramework / EntityFramework Core, Web API)
With any relational DBMS (e.g. MS SQL Server, PostgreSQL)
With Swagger for automated API documentation
Uploaded to the GitHub
Using English to write comments and descriptions of classes, fields, etc.

Non-functional requirements:
Three-level project architecture (data access level, logic level, representation)
When using third-party frameworks and packages – they must be publicly available
It is recommended to cover the logic level with unit tests 
REQUIREMENTS FOR PRESENTATION FORMAT
The completed task must be uploaded to GitHub and include:
1. Mandatory: provide all source files along with the project files.
2. Mandatory: describe the system configuration, startup process in the explanatory note.
WILL BE A PLUS
README.md added
Applying design patterns
Deploy and provide public access to the application
Using Docker
Textual description of the product, technologies, and templates used, instructions for use
TIME TO PERFORM THE TEST TASK
The approximate completion time for a developer of the expected level can take up to 8-16 hours.
Task: Implement Web API for entering project data into the database (task tracker)
You need to implement task storage by the project. “Task” is an instance that contains at least 3 fields listed below:
1.     Id
2.     Task name
3.     Task description

The solution should provide an ability to easily add new fields to the Task entity.

Each task should be a part of only one project. Project is an entity that contains name, id (and also keeps Tasks entities). The program must be a Web API.

Functional requirements:
Ability to create / view / edit / delete information about projects
Ability to create / view / edit / delete task information
Ability to add and remove tasks from a project (one project can contain several tasks)
Ability to view all tasks in the project
WIll be a plus to have an ability to filter and sort projects with various methods (start at, end at, range, exact value, etc.) and by various fields (start date, priority, etc.)

Project information that should be stored:
the name of the project
project start date
project completion date
the current status of the project (enum: NotStarted, Active, Completed)
priority (int)

Task information that should be stored:
task name
task status (enum: ToDo / InProgress / Done)
description
priority (int)


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
Controllers are coupled with XML comments.


## Technology stack
Technology stack used:
- .NET Core Web API
- Entity Framework Core
- MS SQL Server
- FluentValidation
- AutoMapper
- NUnit

## Patterns and Principles
Patterns and Principles implemented:
- Repository pattern
- Unit Of Work pattern
- SOLID principles
- OOP principles

# Documentation

## Controllers and methods

| Controllers | Methods | Description |
| -------------------- | ---------  |---------  |
| ProjectsController   | CreateProject  | Create a project |
|                      | GetProject  | Get a project by ID with tasks |
|                      | GetProjects  | Get filtered and sorted projects by fields |
|                      | Update  | Update a project by ID |
|                      | Delete  | Delete a project by ID |
| TasksController      | CreateTask  | Create a task |
|                      | GetTask  | Get a task by ID |
|                      | Update  | Update a task by ID |
|                      | Delete  | Delete a task by ID |




## Start

To start using TaskTracker you need to clone this repository and run it with Microsoft Visual Studio.
You need to have SQL Server Management Studio (SSMS) installed on your computer.
