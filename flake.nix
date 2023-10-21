{
  inputs.nixpkgs.url = "github:nixos/nixpkgs";

  outputs = { self, nixpkgs }:
  let
    pkgs = nixpkgs.legacyPackages.x86_64-linux;
    lib = pkgs.lib;
    dotnet-sdk = pkgs.dotnet-sdk_8;
    dotnet-runtime = pkgs.dotnet-aspnetcore_8;
  in rec {
    packages.x86_64-linux = rec {
      server = pkgs.buildDotnetModule {
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

      image = pkgs.dockerTools.buildLayeredImage {
        name = "tf-viewer";
        tag = "latest";

        config = {
          WorkingDir = "/data";
          Cmd = [ (lib.getExe server) ];
        };
      };
    };

    devShells.x86_64-linux.default = pkgs.mkShell {
      inputsFrom = [ packages.x86_64-linux.server ];

      LD_LIBRARY_PATH = nixpkgs.lib.makeLibraryPath [ pkgs.libspatialite pkgs.sqlite ];
    };
  };
}
