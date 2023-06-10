FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app

COPY ./AspNetCore.Data/*.csproj ./AspNetCore.Data/
COPY ./AspNetCoreMVC/*.csproj ./AspNetCoreMVC/
COPY *.sln .
RUN dotnet restore

COPY . .
RUN dotnet publish ./AspNetCoreMVC/*.csproj -c Release -o /publish/

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /publish .
ENV ASPNETCORE_URLS="http://*:5000"
ENTRYPOINT ["dotnet","AspNetCoreMVC.dll"]
