set -e

script_dir=$(dirname $(readlink -f $0))
mod_dir=$(dirname $script_dir)
pushd $script_dir

# build dll
rm -f ../1.4/Assemblies/*
dotnet build mod.csproj

# generate About.xml
rm -f ../About/About.xml
xsltproc -o ../About/About.xml ./about.xml.xslt ./mod.csproj 

popd