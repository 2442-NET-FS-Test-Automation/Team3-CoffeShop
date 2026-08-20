FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CoffeShop.Controllers/CoffeShop.Controllers.csproj CoffeShop.Controllers/
COPY CoffeShop.Data/CoffeShop.Data.csproj CoffeShop.Data/
RUN dotnet restore CoffeShop.Controllers/CoffeShop.Controllers.csproj

COPY CoffeShop.Controllers/ CoffeShop.Controllers/
COPY CoffeShop.Data/ CoffeShop.Data/
RUN dotnet publish CoffeShop.Controllers/CoffeShop.Controllers.csproj -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "CoffeShop.Controllers.dll"]