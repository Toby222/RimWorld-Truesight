{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    flake-parts.url = "github:hercules-ci/flake-parts";
    systems.url = "github:nix-systems/default";
    # Dev tools
    treefmt-nix.url = "github:numtide/treefmt-nix";
  };

  outputs = inputs:
    inputs.flake-parts.lib.mkFlake { inherit inputs; } {
      systems = import inputs.systems;
      imports = [ inputs.treefmt-nix.flakeModule ];
      perSystem = { config, self', pkgs, lib, system, ... }: {
        # Rust dev environment
        devShells.default = pkgs.mkShell {
          inputsFrom = [ config.treefmt.build.devShell ];
          nativeBuildInputs = with pkgs; [ dotnet-sdk mono libxslt ];
          shellHook = ''
            DOTNET_ROOT="${pkgs.dotnet-sdk}";
          '';
        };

        # Add your auto-formatters here.
        # cf. https://numtide.github.io/treefmt/
        treefmt.config = {
          projectRootFile = "flake.nix";
          programs = {
            nixpkgs-fmt.enable = true;
            csharpier.enable = true;
            prettier.enable = true;
            nixfmt.enable = true;
          };
        };
      };
    };
}
