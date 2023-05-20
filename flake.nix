{

  outputs = { self, nixpkgs }:
  let
    pkgs = nixpkgs.legacyPackages.x86_64-linux;
    dotnet-sdk = pkgs.dotnet-sdk_7;
    dotnet-runtime = pkgs.dotnet-aspnetcore_7;
  in {
    devShells.x86_64-linux = {
      default = pkgs.mkShell {
        packages = with pkgs; [
          dotnet-sdk
          dotnet-runtime
          libspatialite
        ];
      };
    };
  };
}
