FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HotelMVCPrototype/*.csproj HotelMVCPrototype/
RUN dotnet restore HotelMVCPrototype/HotelMVCPrototype.csproj

COPY . .
RUN dotnet publish HotelMVCPrototype/HotelMVCPrototype.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "HotelMVCPrototype.dll"]
