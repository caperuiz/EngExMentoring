#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EngExWeek2/EngExWeek2.csproj", "EngExWeek2/"]
RUN dotnet restore "EngExWeek2/EngExWeek2.csproj"
COPY . .
WORKDIR "/src/EngExWeek2"
RUN dotnet build "EngExWeek2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EngExWeek2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EngExWeek2.dll"]