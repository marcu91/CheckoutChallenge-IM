FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
### Create image used for building source code and run a Publish
#FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS publish

WORKDIR /app
# copy csproj and restore as distinct layers
COPY Checkout.API/*.csproj ./Checkout.API/
WORKDIR /app/Checkout.API
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY  Checkout.API/. ./ Checkout.API/
WORKDIR /app/Checkout.API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/Checkout.API/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
