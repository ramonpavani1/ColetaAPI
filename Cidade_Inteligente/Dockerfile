# Etapa 1: Usando a imagem base do .NET para produção
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa 2: Usando a imagem do SDK para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia o arquivo .csproj e restaura as dependências
COPY ["Cidade_Inteligente/Cidade_Inteligente.csproj", "Cidade_Inteligente/"]
RUN dotnet restore "Cidade_Inteligente/Cidade_Inteligente.csproj"

# Copia o restante dos arquivos e compila
COPY . .
WORKDIR "/src/Cidade_Inteligente"
RUN dotnet build "Cidade_Inteligente.csproj" -c Release -o /app/build

# Etapa 3: Publicando a aplicação
FROM build AS publish
RUN dotnet publish "Cidade_Inteligente.csproj" -c Release -o /app/publish

# Etapa 4: Criando a imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define o comando para rodar a aplicação
ENTRYPOINT ["dotnet", "Cidade_Inteligente.dll"]
