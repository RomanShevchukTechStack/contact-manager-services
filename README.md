# Contact Manager Backend

This repository contains the backend for a contact management application built with C#/.NET Core. It leverages FastEndpoints for API development and Fluent Validation for input validation. The project is structured into three main components: ContactManager.API, ContactManager.Data, and tests.

## Project Structure

### ContactManager.API

ContactManager.API serves as the entry point for the API endpoints related to contact management.

### ContactManager.Data

ContactManager.Data provides the repository layer for interacting with the data storage, ensuring data access operations are encapsulated.

### Tests

The tests project contains unit tests to validate the functionality of the backend components, ensuring robustness and reliability.

## Getting Started

To run the project locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd ContactManagerBackend
2. **Restore dependencies:**
   ```bash
   dotnet restore
3. **Run the application:**
   ```bash
   dotnet run --project ContactManager.API


4. **Run tests:**
   ```bash
   dotnet test