# CDRWebAPI - Call Detail Records Web API

## Overview
CDRWebAPI is a web API application that provides functionality for managing Call Detail Records (CDRs). It allows users to perform various operations on CDRs such as uploading CDR data from CSV files, retrieving statistical information about the calls, and generating reports.
You can visit the API at https://cdr-api.azurewebsites.net/swagger/index.html
## Technology Choices

### Programming Language
The CDRWebAPI is developed using **C# .NET6**, a widely-used programming language known for its performance, versatility, and strong type-safety. C# is supported by the .NET ecosystem, which provides robust frameworks and libraries for building web applications.

### Frameworks and Libraries
- **ASP.NET Core**: The web API is built on ASP.NET Core, a cross-platform framework for building modern web applications. ASP.NET Core provides excellent performance, scalability, and developer productivity.
- **Entity Framework Core**: The application uses Entity Framework Core as the ORM (Object-Relational Mapping) framework to interact with the underlying database. Entity Framework Core simplifies data access and provides a high-level abstraction for database operations.
- **CsvHelper**: CsvHelper is a powerful library for reading and writing CSV files in .NET. It is used in the application to parse CSV files containing CDR data and map them to C# objects.
- **Serilog**: Serilog is a popular logging library for .NET. It is used to log application events, exceptions, and other useful information for monitoring and troubleshooting.

### Database
The application uses a **relational database** to store and manage CDR data. The specific database provider can be chosen based on the requirements and preferences of the deployment environment. The application is designed to work with various databases supported by Entity Framework Core, such as SQL Server, MySQL, and PostgreSQL.

## Assumptions
The following assumptions were made during the development of CDRWebAPI:
- CDR data is provided in CSV format with specific column headers and data formats. The application expects the CSV files to contain columns such as CallerId, Recipient, Duration, Cost, Reference, Currency, etc.
- The CSV files adhere to a specific structure and formatting, including the date and time formats.
- The CSV is uploaded just once with new references. So we always insert new data and there is no update or checking if the data exisit already.
- Security considerations such as authentication and authorization are not the primary focus of this application. The API endpoints are assumed to be accessed by authorized users or within a secured environment.

## Future Enhancements
Given more time, the following considerations and enhancements could be made to further improve the CDRWebAPI:

- **Authentication and Authorization**: Implement a secure authentication mechanism, such as JWT (JSON Web Tokens), to protect the API endpoints and allow only authorized users to access them.
- **Input Validation**: Implement comprehensive input validation and data sanitization to ensure the integrity and validity of the data being processed.
- **Caching**: Introduce caching mechanisms to optimize the performance of frequently accessed data and reduce the load on the database.
- **Pagination and Filtering**: Enhance the API endpoints to support pagination and filtering options, allowing clients to retrieve CDRs and statistical data based on specific criteria.
- **Unit Testing**: Increase the test coverage by writing MORE unit tests to validate the functionality of the application and ensure code quality. We can also using CSV files to have better testing.
- **Error Handling and Logging**: Improve the error handling mechanism by implementing custom exception handling and enriching the logging capabilities to provide more detailed error information.
- **Monitoring and Metrics**: Set up monitoring and metrics collection to gain insights into the performance, availability, and usage patterns of the API. This could involve integrating with monitoring tools like Application Insights or ELK stack.
- **Containerization and Deployment**: Containerize the application using Docker and define a CI/CD pipeline to automate the deployment process. This would make the application more portable, scalable, and easier to manage in different environments.
- **Making some classes into a package**: : We could create packages and move some code to be shared, such as the CDRService, mapping class, and time converters. This would improve the code's organization and make it easier to maintain.
- **Improving the performance**: : We could improve the performance for example using SqlBulkCopy and more..
- **Use Storage**: : We could let the user upload the file to a azure storage then having azure function to insert data into the database to have more flexibility. 
- **Use Keyvaults**: : We could use Keyvaults to store the secrets like connection strings. 
- **Hardcoded values**: : Appsetting can have all hardcoded values in the code.
- **Add CDR table if missing**: : Adding code for migration to create the database table if not exisit. 
- **Testing swagger endpoints**: : Manual testing all endpoints to validate them or using Postman. 


## Conclusion
CDRWebAPI is a versatile web API application that provides essential functionality for managing Call Detail Records. It leverages the power of C#, ASP.NET Core, and Entity Framework Core to deliver a reliable and efficient solution. With the mentioned enhancements, it can become a robust and scalable system for handling CDR data and generating meaningful insights.
