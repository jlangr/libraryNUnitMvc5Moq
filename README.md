The following instructions are based on Visual Studio 2015.

After cloning this repo:

- In Visual Studio, open .\Library.sln.
- Build the solution. It should automatically load packages using NuGet. If not:
  - Open the `Tools->Options` dialog.
  - Type NuGet in the search entry field.
  - Open the `NuGet Package Manager->General` settings page.
  - Ensure that `Allow NuGet to download missing packages` is checkced.
  
- Ensure that the `NUnit Test Adapter` is installed in Visual Studio. It is available from the Visual Studio Marketplace.
- In Test Explorer:
  - Using the `Group By` dropdown toolbar button in the upper left, select Traits.
  - Click Run All.

You should see at least 100 tests passing and no tests failing. Report back how many tests you have passing overall.

You should also be able to start the application.

If the "slow" category tests are failing:
- Try running again.
- Start the application at least once and add at least one branch. It is possible the database was not created.
- If there appear to be database problems:
  - Open the NuGet Package Manager Console (`Tools->NuGet Package Manager->Package Manager Console`).
  - Enter the command:
      Update-Database
