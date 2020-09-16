FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build

# # copy everything else and build app
COPY . .
# WORKDIR /app
RUN dotnet restore
RUN dotnet publish -c Release -o ./out Nerdomat.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

ENV DISCORD-TOKEN=abcdefghijklmn

COPY --from=build ./build/out .
ENTRYPOINT ["dotnet","Nerdomat.dll"]