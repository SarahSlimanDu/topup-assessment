#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TopUpBeneficiary.Api/TopUpBeneficiary.Api.csproj", "TopUpBeneficiary.Api/"]
COPY ["TopUpBeneficiary.Application/TopUpBeneficiary.Application.csproj", "TopUpBeneficiary.Application/"]
COPY ["TopUpBeneficiary.Domain/TopUpBeneficiary.Domain.csproj", "TopUpBeneficiary.Domain/"]
COPY ["Commons/Commons.csproj", "Commons/"]
COPY ["TopUpBeneficiary.Infrastructure/TopUpBeneficiary.Infrastructure.csproj", "TopUpBeneficiary.Infrastructure/"]
RUN dotnet restore "./TopUpBeneficiary.Api/./TopUpBeneficiary.Api.csproj"
COPY . .
WORKDIR "/src/TopUpBeneficiary.Api"
RUN dotnet build "./TopUpBeneficiary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TopUpBeneficiary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TopUpBeneficiary.Api.dll"]