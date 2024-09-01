# Temel imaj olarak .NET SDK kullan
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
# Uygulama dosyalarını kopyala
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out
# Çalışma zamanı imajı olarak .NET Runtime kullan
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_ENVIRONMENT=Development

# Uygulamayı çalıştırmak için komutu ayarla
ENTRYPOINT ["dotnet", "myapp.dll"]