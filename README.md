# ThAmCo Staff Backend WebAPI (ThAmCo.Staff_Portal_BFF.WebAPI)

This Web API serves as the backbone for the ThAmCo Staff Frontend React App, managing all backend operations seamlessly. Acting as a bridge between the Order and Profiles microservices, this API facilitates the flow of data to and from the frontend application.

## Feature in current system

- **Bridge Application:**
  - Connects to both Order and Profiles microservices via HTTPS to streamline communication.
  - Transfers data between microservices and the frontend app, ensuring smooth and efficient operations.

- **Token Authorization:**
  - Obtains its own token from Auth0 for authorization when making calls to other microservices.
  - Enables secure and authorized data transfer between the API, Auth0, and connected microservices.

## Upcoming Features

- **Stock Management Endpoint:** Introduce a stock management endpoints allowing staff to order new stock and manage inventory.
- **Payment Processing:** Implement payment endpoints to interact with payment microservice to facilitate seamless transactions within the application.
- **User Review Endpoint:** Create an endpoint for staff member to see submitted user reviews, providing valuable feedback on products or services.
- **Manage User Reviews:** Introduce functionality for staff members to view and manage user reviews, ensuring effective customer feedback management.
- **Enhanced Authentication:** Strengthen authentication mechanisms for enhanced security, possibly incorporating additional features such as multi-factor authentication.
  **so on..**

## Usage
- The ThAmCo Staff Frontend React App exclusively interacts with this API for all backend operations.
- This API manages communication with the Order and Profiles microservices to provide comprehensive functionality to the frontend app.

To view example how Frontend app inteacts with this BFF WebAPI: 
- Go to thamco_staff_frontend : Link:[here](https://github.com/JatinAneja1812/thamco_staff_frontendapp).
- Navigate to src -> Containers -> open any folder( e.g. Customers) -> Customer Container.
- look at fetch method which show how in react you can make call to backend webapi.

Make sure your webapi is running.

- Now open ThAmCo.Staff_Protal_BFF.WebAPI -> ThAmCo.Staff_Protal_BFF.WebAPI -> Controllers -> UserControllers
  -The above demonstrates where the requests from frontend application gets recieved and processed and then response is send back to the frontend application.

## Getting Started
Follow these steps to set up and run the ThAmCo.Staff_Protal_BFF.WebAPI on your local machine.

### Prerequisites
Before you can run the ThAmCo.Staff_Protal_BFF.WebAP project in Visual Studio 2022, ensure that you have the following prerequisites installed on your development environment:

1. **Visual Studio 2022:** 
   - You can download the latest version of Visual Studio 2022 from the [official Visual Studio website](https://visualstudio.microsoft.com/downloads/).

2. **.NET 6 SDK:**
   - The project is built using .NET 6, and you'll need the .NET 6 SDK. You can download it from [here](https://dotnet.microsoft.com/download/dotnet/6.0).

3. **Database:**
   - To connect to you database locally ensure you have SQL Server 2019 and Microsoft SQL Server Managment Studio installed:
   - Link:[Microsoft SQL Server Managment Studio](https://sqlserverbuilds.blogspot.com/2018/01/sql-server-management-studio-ssms.html) 
   - Link:[SQL Server 2019](https://www.microsoft.com/en-us/evalcenter/download-sql-server-2019)

### Installation

1. Fork the project.
2. Open Visual Studio 2022 Community/Professional/Enterprise Edition in Admin Mode.
3. Clone the project by coping the link from GitHub repository.
4. Make a new branch from master.
5. Right click on ThAmCo.Staff_Protal_BFF.WebAP and make it as default startup project ( Click on set as startup project button in the menu).
6. Clean the project. Click build(On top of IDE) -> Clean Solution.
7. Click on ThAmCo.Staff_Protal_BFF.WebAP on top navigation bar or either click on "Play" button to run the project.

If there is error connecting to your local database make sure you have installed correct version of SQL Server and SQL Server Managment Studio is connected. 
Also in SQL Server Managment Studio : Database Engine name is : Localhost\\SQLExpress.

## Connected Projects 
- ThAmCo.Order_Management.WebAPI :  Link:[here](https://github.com/JatinAneja1812/ThAmCo.Order_Management.WebAPI).
- ThAmCo.User_Profiles :      Link:[here](https://github.com/JatinAneja1812/ThAmCo.User_Profiles).
- thamco_staff_frontend :           Link:[here](https://github.com/JatinAneja1812/thamco_staff_frontendapp).
