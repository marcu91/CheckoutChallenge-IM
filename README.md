# Dev Readme

The deliverables are the two API projects: 
  1. Gateway.API - the API that processes merchant transactions
  2. Gateway.MockBank - the mocked acquiring bank


The following extra features have been added:
  •  Application logging - done using Serilog for .Net Core
    - serilog is added as middleware to the project, configured in Program.cs, 
    using the settings from appsettings.json in Gateway.API project
    - the logs are stored in a table (called Logs), in the GatewayDataDb database.  
    The table is created automatically if it doesn't exist.
    - all exceptions are logged there, as well as some warnings in some cases, during the execution of the code.
	
  •  Application metrics - done using Prometheus for metrics collection and Grafana for data presentation.
    - for this, I have also used docker-compose, in order to create the 3 needed images that run on the same network.
	
  •  Containerization 
    - done using Docker. To build the docker image, right click the Dockerfile from Gateway.API folder, and click Build Docker Image.
    - the Gateway.API image will be called "gatewayapi"

  •  Authentication - done using the .Net Core built in mechanism
	
  •  API client - done using an Angular client app, to simulate the merchant, and allow for user registration/login
    - also added a dashboard, inside the Angular app, where the merchant can see all of his transactions	
	
  •  Performance testing - done using JMeter

  •  Unit Testing - done using MSTest and Moq. Given the time constraints, test coverage is not 100%.
	
  •  Data storage- SQL Server database, relational DB

  •  Encryption 
    - not done due to time constraints. 
    - It can be done using the Always Encrypted feature of SQL Server, and the keys could be stored in a safe vault, such as Azure Key Vault.
    - .Net core provides a package that integrates with Entity Framework Core and Azure Key Vault, enabling us to configure EF Core 
    to retrive a key safely from an Azure Key Vault, in order to perform encryption/decryption of data going in and out of SQL server.
	
  •  Build script / CI 

  In the main folder of the solution, a postman collection of requests has been added, for easier testing of the API and also the JMeter tests.

  Tools used for development:
    1. SQL Server 2016 
    2. Sql Management Studio v 18.1
    3. Visual Studio 2019 
    4. VS Code
    5. Entity Framework Core tools
    6. Docker Desktop
    7. Git
    8. Postman
    9. JMeter



# Running the API's and the client app'
	
	1. Set the solution to have multiple startup projects, and select both the Gateway.API 
	(runs on http://localhost:54176) and Gateway.MockBank (runs on http://localhost:32771) as startup projects.
	 *** The bank, has also been deployed to an Azure free App service here: http://gatewayapimockbank.azurewebsites.net/.
		- to make calls to the azure instance, when making transaction requests, set the Bank Name to "MockBank" instead of 
	"LocalTestBank" when making transactions.
	2. Run the solution. 
	3. Open VS Code, and open folder: GatewayFrontEnd.
	4. In a terminal window, run: ng serve -o (when the build is done, the client app will run on http://localhost:4200)


# Gateway.API Containerization, Prometheus and Grafana (for metrics):
To build and run the project as a container, you need to have Docker and Docker Compose installed. 
(Tip: Install Docker Desktop, it contains both.)

First, build the Docker image for the Gateway.API (right click the Dockerfile in Visual Studio, and Build Docker File).
Second, in the Gateway.API folder run the "docker-compose up" command. This will create the necessary networks and mount the necessary images 
to the following local URL's:
	http://localhost:54176  - Gateway.API's image will run
	http://localhost:9090/ - Prometheus's will run
	http://localhost:3000/ - Grafana's will run
	- login credentials for Grafana: admin/P@ssw0rd (also configured in the docker-compose.yml)

	For the first time when running it, for Grafana, a datasource needs to be created, 
	that points to Prometheus (http://localhost:9090/) 
	
	and the dashboard needs to be configured. A preset dashboard can be imported and used 
	from Graphana Dashboards Online, ex: https://grafana.com/grafana/dashboards/10427

	When you are done testing, remove the images and stop them from executing by running "docker-compose down"


 # SQL Server Database prerequisites for running the project;

	SQL Server should be configured to allow TCP/IP connection.
	
	The firewall should allow connections through the port SQL Server uses. (default is 1433)

	Make sure the connection strings (all of them) in appsettings.json in Gateway.API, 
	are updated with corresponding IP/Port number for your instance of SQL Server,
	on the environment you are running the app on, before executing the following steps: 

	1. Create the application user in SQL server, and grant permissions to be able to create any database:
	In SQL Server run the following T-SQL statement (you can use Sql Server Management Studio for this):

		IF NOT EXISTS(SELECT * FROM sys.server_principals WHERE name = 'gatewayapi_DbUser')
		BEGIN
			 CREATE LOGIN gatewayapi_DbUser WITH PASSWORD = 'P@ssw0rd'
		END
		use master; 
		GRANT CREATE ANY DATABASE to gatewayapi_DbUser

	2. In Visual Studio, open Package Manager Console and run the EF core database 
	creation/update commands to create the databases (GatewayAuthDb and GatewayDataDb):
	 (if you don't already have Entity Framework Core tools, .Net CLI installed, 
	 run "dotnet tool install --global dotnet-ef" first, to install them globally. Otherwise the following commands won't work.)
		
		Create/Update the AuthDb:
		dotnet ef database update --project Gateway.API --context ApplicationDbContext

		Create/Update the GatewayDb:
		dotnet ef database update --project Gateway.Data

	3. Run the following commands In SQL Server, to grant the application user permissions on the two newly created DB's:

		Add application user to GatewayAuthDB, and grant db_owner permissions:

		use GatewayDataDb;
		CREATE USER gatewayapi_DbUser FOR LOGIN gatewayapi_DbUser
		EXEC sp_addrolemember N'db_owner', N'gatewayapi_DbUser'
