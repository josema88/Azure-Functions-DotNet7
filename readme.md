# Azure Functions with .Net 7 - Isolated Worker Process

This repo contains a sample project using Azure Functions with C# and .NET 7 Isolated Worker Process.

This project contains a CRUD using HTTP Trigger Functions and a Queue Trigger Functions that inserts data to a DB.

## Requirements

- [Install Azure CLI version 2.4 or later](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Install .Net Core 7](https://dotnet.microsoft.com/es-es/download)
- [Install Azure Functions Core Tools version 4.x](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local#v2)

Option 1: Visual Studio

- [Install Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

Option 2: Visual Studio Code

- [Install Visual Studio Code](https://code.visualstudio.com/)
- [Install Azure Functions VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- [Install C# VS Code Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)

## Functions Project

This project contains a CRUD for SQL Server DB using functions for each operation. In order to run locally the project you should configure a DB with a simple table that will contains 4 columns.

If you want to test the Queue Trigger Function, you should setup an Azure Storage Queue.

### Database setup

At your SQL Server instance, create a new DB and create a simple table that will contains 4 columns: id, name, description and enabled. You can use the following SQL Server Script:

```SQL
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [name] [varchar](255) NOT NULL,
    [description] [varchar](255) NOT NULL,
    [enabled] [bit] NOT NULL DEFAULT 1
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Departments] ADD PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
```

### Local Storage for Queue

You can setup an Azure Storage Emulator as Azurite, to download and configure check [here.](https://learn.microsoft.com/es-mx/azure/storage/common/storage-use-azurite?tabs=npm)

To handle your storage account (local or cloud) you can download and install [Azure Storage Explorer.](https://azure.microsoft.com/en-us/products/storage/storage-explorer)

Once you have your Queue instance, you should create a Queue called `queue-departments`. Then you can send messages to this queue in order to trigger your function.

### Connect to your DB and Storage Services in your local environment

Be sure that in your local environment you have the fille local.settings.json. Within that file you should add the connection strings for DB and Storage Account, like this sample:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "MyDbConnection": "<DB_CONNECTION_STRING>",
    "MyStorageConnection": "<STORAGE_CONNECTION_STRING>"
  }
}
```

### Run the project

This project can be executed using one of these three alternatives:

- Visual Studio: Open the solution with Visual Studio and run the project.
- Azure Functions Core Tools: Once you have installed the toolkit, in your CLI go to the path that contains the project and execute `func start`
- Visual Studio Code: Open the folder with VS Code, go to options panel at the top and click in "Run" then you can select "Start debugging" or "Run without debugging".

Note: If you want to debug consider use Visual Studio or Visual Studio Code (Plugins Configured).

## Create Function App and Deploy your Azure function

In order to run your Function in the cloud, you should create an Azure Function App instance and then deploy your project to it. 

You can create the instance using different alternatives (official documentation):

- [Create Functions with Visual Studio](https://learn.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio?tabs=isolated-process#prerequisites)
- [Create Functions with Azure Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cisolated-process)
- [Create Functions with Visual Studio Code](https://learn.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cisolated-process)
- [Create Functions with the Azure Portal](https://learn.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cisolated-process)

### Connect to DB and Storage Account in your Azure Cloud environment

Go to your Azure App Function in Azure and add two environment variables at the instance's Settings -> Configuration -> Application settings.

For the DB connection:
- Name: MyDbConnection
- Value: < Your Connection String >

For the Storage Account (Queue) connection:
- Name: MyStorageConnection
- Value: < Your Connection String >

After that, save changes.

That's it, you have your Azure Functions Up and Running!


