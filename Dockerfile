FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env

COPY . ./
RUN dotnet publish ./CinderBlockGames.GitHub.Actions.Ftp/CinderBlockGames.GitHub.Actions.Ftp.csproj -c Release -o out --no-self-contained

LABEL maintainer="cinder block games <hello@cinderblockgames.com>"
LABEL repository="https://github.com/cinderblockgames/ftp-action"
LABEL homepage="https://cinderblockgames.com/"

LABEL com.github.actions.name="FTP Smart File Copy"
LABEL com.github.actions.description=".NET-based GitHub Action to copy files over FTP."
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="arrow-up-circle"
LABEL com.github.actions.color="white"

FROM mcr.microsoft.com/dotnet/runtime:5.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/CinderBlockGames.GitHub.Actions.Ftp.dll" ]