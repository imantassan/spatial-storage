param(
    [Parameter()]
    [ValidateSet('Build', 'Test', 'Publish', 'Run')]
    [string[]]
    $Target = 'Run',

    [Parameter()]
    [ValidateSet('Debug', 'Release')]
    [string]
    $Configuration = 'Debug'
)

Function Check-Result {
    Param([string]$Operation)

    if ($LastExitCode -ne 0) {
        Write-Host "$($Operation) failed." -ForegroundColor Red
        Exit 1
    }
}

Write-Verbose "Running target $($Target) with configuration $($Configuration)"

$solution = "$($PSScriptRoot)\SpatialStorage.sln"
$testProject = "$($PSScriptRoot)\SpatialStorage.Services.Test\SpatialStorage.Services.Test.csproj"
$publishProject = "$($PSScriptRoot)\SpatialStorage\SpatialStorage.csproj"
$publishDirectory = ".\publish"
$runnable = Join-Path -Path $publishDirectory -ChildPath "SpatialStorage.exe"


if (@('Test', 'Publish', 'Run') -Contains $Target) {
    $targetProject = $solution
}
else {
    $targetProject = $publishProject
}
Write-Verbose 'Cleaning previous builds'
dotnet clean -c $Configuration $targetProject
Check-Result -Operation "Clean"

Write-Verbose 'Restoring NuGet packages'
dotnet restore $targetProject
Check-Result -Operation "Restore"

Write-Verbose 'Building project(s)'
dotnet build -c $Configuration --no-restore $targetProject
Check-Result -Operation "Build"

if (@('Test', 'Publish', 'Run') -Contains $Target) {
    Write-Verbose 'Running tests'
    dotnet test --no-build -c $Configuration $testProject
    Check-Result -Operation "Test"
}

if (@('Publish', 'Run') -Contains $Target) {
    Write-Verbose 'Publishing project'
    dotnet publish -c $Configuration --output $publishDirectory $publishProject --no-build
    Check-Result -Operation "Pack"
}

Write-Host "Build succeeded" -ForegroundColor Green

Start-Process -FilePath $runnable -WorkingDirectory $publishDirectory