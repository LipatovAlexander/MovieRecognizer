FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ./publish/ ./
ENTRYPOINT ["dotnet", "Api.dll"]
