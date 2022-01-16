mkfile_path := $(abspath $(lastword $(MAKEFILE_LIST)))
current_dir := $(notdir $(patsubst %/,%,$(dir $(mkfile_path))))

msbuild:
	nuget install -OutputDirectory $(current_dir)/packages
	msbuild YaDi.csproj
