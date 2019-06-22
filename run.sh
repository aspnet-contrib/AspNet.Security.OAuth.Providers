#!/usr/bin/env bash

root=$(cd "$(dirname "$0")"; pwd -P)
artifacts=$root/artifacts/build
configuration=Release
skipTests=0

while :; do
    if [ $# -le 0 ]; then
        break
    fi

    lowerI="$(echo $1 | awk '{print tolower($0)}')"
    case $lowerI in
        -\?|-h|--help)
            echo "./build.sh [--skip-tests] [--output <OUTPUT_DIR>]"
            exit 1
            ;;

        --output)
            artifacts="$2"
            shift
            ;;

        --skip-tests)
            skipTests=1
            ;;

        *)
            __UnprocessedBuildArgs="$__UnprocessedBuildArgs $1"
            ;;
    esac

    shift
done

export CLI_VERSION=`cat ./global.json | grep -E '[0-9]\.[0-9]\.[a-zA-Z0-9\-]*' -o`
export DOTNET_INSTALL_DIR="$root/.dotnetcli"
export PATH="$DOTNET_INSTALL_DIR:$PATH"

dotnet_version=$(dotnet --version)

if [ "$dotnet_version" != "$CLI_VERSION" ]; then
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version "$CLI_VERSION" --install-dir "$DOTNET_INSTALL_DIR"
fi

dotnet build ./AspNet.Security.OAuth.Providers.sln --output $artifacts --configuration $configuration || exit 1
dotnet pack ./AspNet.Security.OAuth.Providers.sln --output $artifacts --configuration $configuration || exit 1

if [ $skipTests == 0 ]; then
    if [ "$TF_BUILD" != "" ]; then
        dotnet test ./test/AspNet.Security.OAuth.Providers.Tests/AspNet.Security.OAuth.Providers.Tests.csproj --output $artifacts --configuration $configuration --logger trx || exit 1
    else
        dotnet test ./test/AspNet.Security.OAuth.Providers.Tests/AspNet.Security.OAuth.Providers.Tests.csproj --output $artifacts --configuration $configuration || exit 1
    fi
fi
