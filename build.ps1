$ErrorActionPreference = "Stop"

Set-Location $PSScriptRoot
$buildFile = Join-Path "$PSScriptRoot" "Run.ps1"

&"$buildFile" @args
