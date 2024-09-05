# Use a imagem oficial do .NET SDK para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie os arquivos de projeto e restaure as dependências
COPY *.csproj ./
RUN dotnet restore

# Copie o restante dos arquivos e compile a aplicação
COPY . ./
RUN dotnet publish -c Release -o /out

# Use a imagem do runtime do .NET para executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Exponha a porta que a aplicação usará
EXPOSE 8080

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "BikeRentalApp.dll"]