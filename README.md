# Notes
  - For ease of testing the Api, use the included postman collection.

# Assumptions
  - Step 1:
    - The input files will be a known configured location, not supplied by the user
    - The input rows will always be in the same order, as defined
    - There is no row header in the files
    - The files as a whole are related specifically to the Console, not the api
  - Step 2:  
    - The sorting endpoints will follow the same sorting logic as the Console app sorting logic
    - The single data line is allowed to accept multiple people records
    - The Post is allowed to be json, containing the data line, not just literally a single data line
