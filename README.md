# WexAssessmentApi

## Overview:
### WexAssessmentAPI
Core API as requested by the assignment. Includes the following:
- ProductModel that handles all data validation via property annotations.
- IRepository interface that includes all basic CRUD opertions.
- IProductRepsitory interface that inherets from IRepository and adds a Get that filters on Category.
- Repository class that implements the IRepository interace and simply demonstrates a potential implementation with an in-memory Dictionary as the datastore.
- ProductRepository that implements the IProductRepository interface and uses a List as the in-memory datastore.
- ProductController that contains all API endpoints and utilizes the ProductRepository for all data operations. Protected by Duende IdentityServer implementation.

### IdentityServer
Basic Duende IdentityServer implementation built based on the official documentation and QuickGuide that can be found [here](https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/).

### Demo
Demo project used to demonstrate the use of the API and IndentityServer in tandem to complete operations. Handles the requesting of an access token and making multiple expected calls to the WexAssessmentAPI, including:
- Get all items, with default paging.
- Get one item, by Id.
  - One test where the item is known to exist.
  - One test where the item is known not to exist.
- Add an item.
  - One test where the payload is valid.
  - One test where the payload is invalid.
- Update an item by id.
  - One test where the item with matching id is known to exist.
  - One test where no item with matching id exists.
- Delete an item by id.
After each data changing operation (Create, Update, Delete) the new inventory is printed to the console so the user can see the changes to the data set.

## Running the application
The Demo console project has been built so that the user can simply run all projects and visualize the various API endpoints working via the console output. To run all projects the user can go to Project -> Configure Startup Projects and select the Multiple Startup Projects option. In the Action column, select "Start" for all three projects, and move "Demo" down to the bottom of the list using the arrows to the right side of the table when "Demo" is highlighted. The configuration should look like this:
![image](https://github.com/JCTurner91/WexAssessmentApi/assets/54905220/7cf0c848-ce63-423b-858d-e0e7b7d1ed26)
Once the startup configuration is complete, the user can simply hit the Run button and all three projects will boot up and run. The Demo project ends with a `Console.ReadLine()` so the console can remain open and allow the user to review the output before quitting the application.

The WexAssessementAPI application does include a Swagger page, but it has not been configured to accept a Bearer token or retrieve one for it's own use, so any calls using it will return a 401.

WexAssessmentAPI is hosted at: https://localhost:7153

IdentityServer is hosted at: https://localhost:5001

## Assumptions
- The Update functionality could have been built to fail if the ID provided didn't exist in the dataset, but opted for a default Upsert functionality in that case as it tends to be the preferred outcome of a user.
- Having no experience with Duende, most of the learned information and code used was from the QuickGuide found [here](https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/). Assuming client credentials, token generation, key management, etc. are mostly outside the scope of this assignment.
- Operated under the assumption that the request Repository class was for review purposes only as nothing in the assignment appeared to be requested to inheret from it.
- Assumed the GetProductsByCategory method implemented in the IProductRepository and ProductRepository was for example purposes only as it was not implemented in the original API specification. If we had implemented it, we could probably add a filter parameter in the query of the GetAll method and use the GetProductsByCategory if a Category is provided in the query.

## SOLID Principles:
S - Each class and method is built for a singular and specific purpose.

O - The use of the layered interfaces allowed for the various CRUD operations to build ontop of one another. The IProductRepository adding GetProductsByCategory ontop of the existing CRUD operations in IRepository demonstrates that.

L - All functions have been written in a way that should allow abstract and subtype referencing without breaking the exisitng implementation. This is most noteable in the generic usage in the interfaces.

I - The layering or segregation of interfaces allows for the use of only the interfaces you need. This is most noteable between the Repository class, which only implements the IRepository interface, and the ProductRepostiory, which intefaces the extended IProductRepository.

D - The separation of lower level logic via abstractions and interfaces allow for a loose coupling of responsibilies between the application components.
