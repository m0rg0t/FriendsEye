# Portable feed regression checks

Run with the .NET 10 SDK:

```sh
dotnet run --project tests/FeedRegression --configuration Release -p:Flavor=Phone
dotnet run --project tests/FeedRegression --configuration Release -p:Flavor=Windows
```

The harness links each client's actual `ViewModel/MainViewModel.cs` at compile time,
with C# 5 language compatibility. It has no package dependencies. `Stubs.cs`
provides offline doubles for Windows/Phone, MVVM notifications and Parse queries;
no production service is contacted. It exercises the actual loading methods and
collection setters rather than a reimplementation of those methods.

Assertions cover nearby/active feed isolation, repeated refresh, empty responses,
thumbnail preservation and collection/derived-preview notifications. The original
source fails the first assertion in both client flavors: loading nearby photos
clears active photos. The repaired source passes 11 Phone and 14 Windows checks.

This does **not** build the legacy applications or validate XAML, device behavior,
Parse query semantics, network/authentication behavior, or third-party binaries.
Full validation still requires the original Windows Phone 8 / Windows 8 SDKs and
dependencies. Those target frameworks and dependencies are unchanged.
