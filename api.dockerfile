FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ./SeeTrue.API ./SeeTrue.API
COPY ./SeeTrue.Infrastructure ./SeeTrue.Infrastructure
COPY ./SeeTrue.Models ./SeeTrue.Models

WORKDIR /app/SeeTrue.API

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/SeeTrue.API/out .
ENTRYPOINT ["dotnet", "SeeTrue.API.dll"]

