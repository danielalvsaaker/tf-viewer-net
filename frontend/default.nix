{ pkgs, config, lib, ... }:
let
  nodejs = pkgs.nodejs_21;
in
{
  config.packages.frontend = pkgs.buildNpmPackage {
    pname = "tf-viewer-web";
    version = "0.1.0";
    meta.mainProgram = "tf-viewer-web";

    src = ./.;
    npmDepsHash = "sha256-RgYbZMcliJURp3wGXS3kzaDf5mx9FcutvpxPOULCrgY=";

    inherit nodejs;

    nativeBuildInputs = [
      pkgs.pkg-config
      pkgs.python3
      pkgs.makeWrapper
    ];
    buildInputs = [ pkgs.vips ];

    installPhase = ''
      runHook preInstall

      npm config delete cache
      npm prune --omit=dev --no-save
      find node_modules -maxdepth 1 -type d -empty -delete

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
        Env = [
          "NODE_EXTRA_CA_CERTS=${pkgs.cacert}/etc/ssl/certs/ca-bundle.crt"
        ];
      };
    };

  config.devShells.frontend = pkgs.mkShell {
    inputsFrom = [ config.packages.frontend ];
  };
}
