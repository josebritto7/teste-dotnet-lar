# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Lar.TesteDotNet.sln ./
COPY src/Lar.TesteDotNet.Domain/Lar.TesteDotNet.Domain.csproj src/Lar.TesteDotNet.Domain/
COPY src/Lar.TesteDotNet.Shared/Lar.TesteDotNet.Shared.csproj src/Lar.TesteDotNet.Shared/
COPY src/Lar.TesteDotNet.Application/Lar.TesteDotNet.Application.csproj src/Lar.TesteDotNet.Application/
COPY src/Lar.TesteDotNet.Infrastructure/Lar.TesteDotNet.Infrastructure.csproj src/Lar.TesteDotNet.Infrastructure/
COPY src/Lar.TesteDotNet.Presentation/Lar.TesteDotNet.Presentation.csproj src/Lar.TesteDotNet.Presentation/

RUN dotnet restore src/Lar.TesteDotNet.Presentation/Lar.TesteDotNet.Presentation.csproj

COPY . .
RUN dotnet publish src/Lar.TesteDotNet.Presentation/Lar.TesteDotNet.Presentation.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ConnectionStrings__DefaultConnection="Data Source=/app/data/lar.testedotnet.db"

RUN mkdir -p /app/data

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Lar.TesteDotNet.Presentation.dll"]
