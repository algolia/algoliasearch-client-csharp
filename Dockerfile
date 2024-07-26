FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app
ADD . /app/

RUN dotnet build src/