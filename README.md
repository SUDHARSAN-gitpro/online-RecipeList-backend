# 🍽️ Recipe List Backend

ASP.NET Core Web API backend for the Recipe Management System.

The API performs CRUD operations and stores recipe data in a MySQL database.

## 🚀 Technologies Used

- ASP.NET Core Web API
- C#
- Entity Framework Core
- MySQL

## 📋 Prerequisites

- .NET SDK 8.0 (or the version used in the project)
- MySQL Server

## ⚙️ Installation

### Clone the repository

```bash
git clone https://github.com/SUDHARSAN-gitpro/online-RecipeList-backend.git
```

### Navigate to the project

```bash
cd online-RecipeList-backend
```

### Restore packages

```bash
dotnet restore
```

### Configure Database

Update the MySQL connection string inside:

```
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=RecipeDB;user=root;password=yourpassword;"
}
```

### Run the project

```bash
dotnet run
```

The API will start on:

```
https://localhost:5001
```

or

```
http://localhost:5000
```

## ✨ Features

- RESTful API
- Create Recipe
- Read Recipes
- Update Recipe
- Delete Recipe
- MySQL Database Integration

## 📌 API Methods

- GET
- POST
- PUT
- DELETE
