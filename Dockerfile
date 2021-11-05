FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
ADD . /app/

RUN dotnet build src/