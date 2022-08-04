FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Petspot.csproj", "."]
RUN dotnet restore "./Petspot.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Petspot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Petspot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Petspot.dll"]