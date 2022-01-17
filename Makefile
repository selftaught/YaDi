mkfile_path := $(abspath $(lastword $(MAKEFILE_LIST)))
current_dir := $(notdir $(patsubst %/,%,$(dir $(mkfile_path))))

remove-packages:
	rm -rf packages

install-packages: remove-packages
	nuget install -OutputDirectory ./packages

msbuild: install-packages
	msbuild YaDi.csproj
