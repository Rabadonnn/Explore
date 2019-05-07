@echo off
title Publish app

dotnet publish -c release -r win10-x64
dotnet publish -c release -r win10-x86