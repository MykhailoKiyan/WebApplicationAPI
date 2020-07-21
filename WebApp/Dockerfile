#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0
COPY ["WebApplicationAPI.csproj", ""]
RUN dotnet restore "./WebApplicationAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApplicationAPI.csproj" -c $BUILDCONFIG -o /app/build /p:Version=$VERSION

FROM build AS publish
RUN dotnet publish "WebApplicationAPI.csproj" -c $BUILDCONFIG -o /app/publish /p:Version=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplicationAPI.dll"]
