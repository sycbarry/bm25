#!/bin/bash

function clean() {

pkill dotnet

dotnet clean ./IRLib.sln --configuration Release
dotnet clean ./IRLib/IRLib.csproj --configuration Release

dotnet clean
# build
dotnet build ./IRLib

}


function pack() {
# pack the build
dotnet pack ./IRLib

if [[ -d  ~/Documents/Projects/Assist/Source/localfeed ]]
then
    rm -rf ~/Documents/Projects/Assist/Source/localfeed
fi
# add the package to a stream
nuget add ~/Documents/Projects/Assist/Source/IRLib/IRLib/bin/Debug/IRLib.1.0.3.nupkg -Source ~/Documents/Projects/Assist/Source/localfeed

}

clean
pack
