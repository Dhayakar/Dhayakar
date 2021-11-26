#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build
WORKDIR /src
COPY ["WYNK.Services/WYNK.Services.csproj", "WYNK.Services/"]
COPY ["WYNK.Data/WYNK.Data.csproj", "WYNK.Data/"]
COPY ["SMSand EMAILService.cs/CommonMailService.cs.csproj", "SMSand EMAILService.cs/"]
COPY ["WYNK.Helpers/WYNK.Helpers.csproj", "WYNK.Helpers/"]
RUN dotnet restore "WYNK.Services/WYNK.Services.csproj"
COPY . .
WORKDIR "/src/WYNK.Services"
RUN dotnet build "WYNK.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WYNK.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WYNK.Services.dll"]