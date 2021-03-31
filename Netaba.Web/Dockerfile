#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Netaba.Web/Netaba.Web.csproj", "Netaba.Web/"]
COPY ["Netaba.Data/Netaba.Data.csproj", "Netaba.Data/"]
COPY ["Netaba.Tests/Netaba.Tests.csproj", "Netaba.Tests/"]
COPY ["Netaba.Services/Netaba.Services.csproj", "Netaba.Services/"]
RUN dotnet restore "Netaba.Web/Netaba.Web.csproj"
COPY . .
WORKDIR "/src/Netaba.Web"
RUN dotnet build "Netaba.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Netaba.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Netaba.Web.dll"]
