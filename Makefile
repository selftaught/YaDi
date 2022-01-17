PROJECT=YaDi
CSPROJECT=$(PROJECT).csproj

remove-packages:
	rm -rf packages

install-packages: remove-packages
	nuget install -OutputDirectory ./packages

build-debug: install-packages
	msbuild -p:Configuration=Debug $(CSPROJECT)

build-release: install-packages
	msbuild -p:Configuration=Release $(CSPROJECT)