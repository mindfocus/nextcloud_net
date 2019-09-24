dotnet sonarscanner begin /k:"my:nextcloud"
#dotnet build nextcloud.sln
dotnet build model/model.csproj
dotnet sonarscanner end
