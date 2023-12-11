#!/bin/sh

echo "############################"
echo "# RUN dotnet build FIRST!! #"
echo "############################"
dotnet.exe build

tempdir=$(mktemp -d)

cp -f /mnt/c/Users/evan/LethalFetch/bin/Debug/net46/LethalFetch.dll "$tempdir"
cp manifest.json "$tempdir"
cp CHANGELOG.md "$tempdir"
cp README.md "$tempdir"
cp icon.png "$tempdir"

back=$(pwd)
cd "$tempdir" || exit
zip release.zip ./*
mv ./release.zip "$back/release.zip"
cd "$back" || exit
rm -rf "$tempdir"
mv release.zip ~/Libraries/Desktop/AAA.zip
