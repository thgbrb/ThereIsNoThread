dotnet build
start dotnet run --project TimeApi
timeout 5
start dotnet run --project Portal
start chrome "http://localhost:5000/Home/information"
start dotnet run --project Requester
