# ZBlogAPI

Running the ZBlog

1. Obtain the GitRepo: https://github.com/griverahn/ZBlog.git
2. Generate the data base throug migration.
3. Run the App to see the APIs.
4. Configure the users
    a. There is a method in the APIs, named /api/add-user under the [Authentication] Section.
       a.1 You will need to create the next Users with the same named roles:
         - Public
         - Writer
         - Editor
       
         sugested password: [username] + "123*"

    b. Now that you have the user, you must login the user to test the [Post] APIs, you will find 
       /api/login under [Authentication] for that purpouse. You must save the "token" value to pass
       it when you were calling the API in order to pass throug the authorization security.

       Should look similar to this:
       eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUHVibGljIiwianRpIjoiOTdhZTJiMDItOTVjZC00MWRkLWE1NDgtYTZmYmY2OWEyMjBjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUHVibGljIiwiZXhwIjoxNjc2NDAwMTU2LCJpc3MiOiJVUkxEZWxQcm95ZWN0b1VJIiwiYXVkIjoiVVJMRGVsUHJveWVjdG9VSSJ9.K4z9zW9_3MlkYppdNZTaG50WwWwfd8IKvNpkR3wLruI

    c. For some methods, you will need to specify the userId, since the correct way to define the APIs is the userId, this method was created to obtain it:
        /api/get-user-id under the [Authentication] options. It is a helper method for the sole purpose of testing the APIs. For any other information needed I would recommend that
        look in the database tables.

        The userId must be something similar to: aeb4199d-d100-4d91-8cbb-47dd2945d147


5. Test the APIs against the acceptance criteria.
    a. Open postman inyour computer.
    b. Select the HTTP verb that math the API call you want to test.
    c. Define parameters (if any).
    d. Paste the API url, for example: https://localhost:7241/api/get-all-posts
    c. In the call tabs, select [Headers], then add the Key [Authorization], in the [Value] column, write: Bearer + [space] + the token you got in the login.
       It should look similar to this:
       Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUHVibGljIiwianRpIjoiOTdhZTJiMDItOTVjZC00MWRkLWE1NDgtYTZmYmY2OWEyMjBjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUHVibGljIiwiZXhwIjoxNjc2NDAwMTU2LCJpc3MiOiJVUkxEZWxQcm95ZWN0b1VJIiwiYXVkIjoiVVJMRGVsUHJveWVjdG9VSSJ9.K4z9zW9_3MlkYppdNZTaG50WwWwfd8IKvNpkR3wLruI
    e. Send the request using Body>Raw>JSON to fullfil the needed parameters and look for the response.
6. Total Time: 8 hours 40 min.

