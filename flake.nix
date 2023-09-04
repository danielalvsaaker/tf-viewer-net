{
  inputs.nixpkgs.url = "github:nixos/nixpkgs/nixpkgs-unstable";

  outputs = { self, nixpkgs }:
  let
    pkgs = nixpkgs.legacyPackages.x86_64-linux;
    dotnet-sdk = pkgs.dotnet-sdk_7;
    dotnet-runtime = pkgs.dotnet-aspnetcore_7;
  in rec {
    packages.x86_64-linux.default = pkgs.buildDotnetModule {
      pname = "tf-viewer";
      version = "0.1";
      meta.mainProgram = "Server";

      src = ./src;
      nugetDeps = ./deps.nix;
      projectFile = "tf-viewer-net.sln";
      dotnet-sdk = pkgs.dotnet-sdk_7;
      dotnet-runtime = pkgs.dotnet-aspnetcore_7;

      executables = [ "Server" ];
      runtimeDeps = [ pkgs.libspatialite pkgs.sqlite ];
    };

    devShells.x86_64-linux.default = pkgs.mkShell {
      inputsFrom = [ packages.x86_64-linux.default ];

      LD_LIBRARY_PATH = nixpkgs.lib.makeLibraryPath [ pkgs.libspatialite pkgs.sqlite ];
    };
  };
}
