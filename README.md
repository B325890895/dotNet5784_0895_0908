Application including UI with a layered architecture
created by Batya Blau and Yael Glick
Technologies: C#, .NET

Description: 


The project is an application with a user interface for the project manager,
which enables the management of project tasks. 
In addition, there will also be a user interface for the engineer
that will allow him to update his details and report/update the status of the tasks assigned to him.

Project stages:

1. Building the data layer (DAL), defining its contracts (DO/DalApi), and building an interface to test the data layer.

2. Updating the data layer by creating an API with a generic ICrud interface.

3. Updating the data layer (DAL) by defining an additional database that will save the data in XML files, and implementing the interface methods for each type of entity, so that the data will be saved in XML files.

4. Building the logical layer (BL), defining and implementing its contracts (BO/BlApi).

5. Add a basic presentation layer (PL) with WPF.

6. Completion of the PL layer and the graphical interface completion  - in progress, not over yet.

