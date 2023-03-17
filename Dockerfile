FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app
ADD . /app/

RUN dotnet build src/