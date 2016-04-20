// ######################################################
// # Include FAKE libraries
// ######################################################
#r "packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.AssemblyInfoFile

// ######################################################
// # Get all external references via NuGet
// ######################################################
RestorePackages()

// ######################################################
// # Properties
// ######################################################
let outputDir = "./output/"
let buildDir = outputDir + "build/"
let testDir = outputDir + "test/"
let deployDir = outputDir + "deploy/"
let packagingDir = outputDir + "nuget-packaging/"
let testResults = testDir + "TestResults.xml"

// ######################################################
// # Targets
// ######################################################
let CleanOutputTask = "CleanOutput"
let BuildSourceTask = "BuildSource"
let DeployAppTask = "Deploy"
let BuildTestTask = "BuildTest"
let RunTestTask = "RunTest"
let FxCopCheckTask = "FxCopCheck"

// ######################################################
// # Literals
// ######################################################
let SourceProjectFilesPattern = "src/**/*.csproj"
let TestProjectFilesPattern = "test/**/*.csproj"
let DllFilePattern = @"\**\*.dll"
let ExeFilePattern = @"\**\*.exe"
let FxCopResultsFileName = "FXCopResults.xml";
let FxCopPath = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Team Tools\Static Analysis Tools\FxCop\FxCopCmd.exe"
let TestDllNamePattern = "/NtErp.*.Tests.dll"





// ######################################################
// # Target 'Clean'
// ######################################################
Description "  > Cleaning output directories"
Target CleanOutputTask (fun _ ->
    CleanDirs [outputDir; testDir; deployDir]
)

// ######################################################
// # Target 'Build'
// ######################################################
Description "  > Building all projects inside src folder"
Target BuildSourceTask (fun _ ->
    !! SourceProjectFilesPattern
      |> MSBuildRelease buildDir "Build"
      |> Log "    > AppBuild-Output: "
)

// ######################################################
// # Target 'FxCop'
// ######################################################
Description "  > Running FxCop"
Target FxCopCheckTask (fun _ ->  
    !! (buildDir + DllFilePattern) 
    ++ (buildDir + ExeFilePattern) 
    |> FxCop 
        (fun p -> 
            {p with
              ReportFileName = testDir + FxCopResultsFileName
              //FailOnError = FxCopErrorLevel.CriticalWarning
              ToolPath = FxCopPath})
)

// ######################################################
// # Target 'BuildTest'
// ######################################################
Description "  > Building all projects inside test folder"
Target BuildTestTask (fun _ ->
    !! TestProjectFilesPattern
      |> MSBuildDebug testDir "Build"
      |> Log "    > TestBuild-Output: "
)

// ######################################################
// # Target 'RunTest'
// ######################################################
Description "  > Running all tests within output/tests/NtErp.*.Tests.dll assemblies"
Target RunTestTask (fun _ ->
    !! (testDir + TestDllNamePattern)
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testResults; })
)

// ######################################################
// # Target dependencies
// ######################################################
CleanOutputTask
   ==> BuildSourceTask
   ==> FxCopCheckTask
   ==> BuildTestTask
   ==> RunTestTask

// ######################################################
// Start the build process with a given target or the default target DeployZip
// ######################################################
RunTargetOrDefault RunTestTask