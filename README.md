# QuicksilverB2B
Installation
------------

1.  Configure Visual Studio to add this package source: http://nuget.episerver.com/feed/packages.svc/. This allows missing packages to be downloaded, when the solution is built.
2.  Open solution and build to download nuget package dependencies.
3.  Search the solution for "ChangeThis" and review/update as described.
4.  This uses Local DB so no need to run database setup  
5.  Start the site (Debug-Start from Visual studio) and browse to http://localhost:50244 to finish installation. Login with admin@episerver.com / store.

Note: SQL scripts are executed using Windows authentication so make sure your user has sufficient permissions.

Logging in
-------------
To log in to the site, use u: admin@episerver.com  p: store.
