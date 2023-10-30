{ pkgs, config, lib, ... }:
let
  nodejs = pkgs.nodejs_21;
  buildNpmPackage = pkgs.buildNpmPackage.override {
    inherit nodejs;
  };
in
{
  config.packages.frontend = buildNpmPackage {
    pname = "tf-viewer-web";
    version = "0.1.0";
    meta.mainProgram = "tf-viewer-web";

    src = ./.;
    npmDepsHash = "sha256-heqBdIpOKA9icD4h8RxPlSchGcj3wtjZKiOIWnAiCcE=";

    nativeBuildInputs = [
      pkgs.pkg-config
      pkgs.python3
      pkgs.makeWrapper
    ];
    buildInputs = [ pkgs.vips ];

    installPhase = ''
      runHook preInstall

      mkdir -p $out/lib
      mv build package.json node_modules $out/lib

      makeWrapper "${nodejs}/bin/node" "$out/bin/tf-viewer-web" \
        --append-flags "$out/lib/build"

      runHook postInstall
    '';
  };

  config.packages.frontend-image =
    let
      frontend = config.packages.frontend;
    in
    pkgs.dockerTools.buildLayeredImage {
      name = frontend.name;
      tag = "latest";

      maxLayers = 125;

      config = {
        WorkingDir = "/data";
        Cmd = [ (lib.getExe frontend) ];
      };
    };

  config.devShells.frontend = pkgs.mkShell {
    inputsFrom = [ config.packages.frontend ];
  };
}
