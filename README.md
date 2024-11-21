**User CRUD POC
This is a User CRUD POC where the developer could perform CRUD operations on the User entity.

Overview
The table contains information about the user as follows:

ID: Primary Key
FirstName: Mandatory
LastName: Mandatory
Email: Mandatory, unique, and validated
Mobile: Mandatory and unique
ProfileImage: Storing the image locally
UserType: This contains a reference to another table called UserTypes
The table contains the following structure:

ID: Primary Key
UserType: A varchar(10) type for storing user type like Member or Guest.
Features
This POC shows

Adding a new user
Update user information
Delete a user
Associate the UserType field with the data in the UserTypes table
Processing Profile Picture
The profile picture is uploaded to the server locally, and the file path of the image is kept in the database.

Getting Started
Create a database with the User and UserTypes tables.
Configure the application to connect to your database.
Run the application to see how CRUD operations can be performed through the user interface or APIs.
