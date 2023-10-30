{ config, pkgs, lib, ... }:
let
  dotnet-sdk = pkgs.dotnet-sdk_8;
  dotnet-runtime = pkgs.dotnet-aspnetcore_8;
in
{
  config.packages.backend = pkgs.buildDotnetModule {
    pname = "tf-viewer";
    version = "0.1";
    meta.mainProgram = "Server";

    src = ./src;
    nugetDeps = ./deps.nix;

    projectFile = "Server/Server.csproj";

    inherit dotnet-sdk dotnet-runtime;

    executables = [ "Server" ];
    runtimeDeps = [ pkgs.libspatialite pkgs.sqlite ];
  };

  config.packages.backend-image =
    let
      backend = config.packages.backend;
    in
    pkgs.dockerTools.buildLayeredImage {
      name = backend.name;
      tag = "latest";

      extraCommands = "mkdir -m 0777 tmp";

      config = {
        WorkingDir = "/data";
        Cmd = [ (lib.getExe backend) ];
        Env = [
          "ASPNETCORE_URLS=http://+:8080"
          "DOTNET_RUNNING_IN_CONTAINER=true"
          "ASPNETCORE_ENVIRONMENT=Production"
        ];
      };
    };

  config.devShells.backend = pkgs.mkShell {
    packages = [ dotnet-sdk dotnet-runtime ];

    LD_LIBRARY_PATH = lib.makeLibraryPath config.packages.backend.runtimeDeps;
  };
}
