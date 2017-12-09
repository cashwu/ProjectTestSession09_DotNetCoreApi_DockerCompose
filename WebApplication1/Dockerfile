FROM microsoft/aspnetcore:2.0
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
ARG source=.
WORKDIR /app
EXPOSE 80
COPY $source .
ENV ASPNETCORE_ENVIRONMENT=Docker