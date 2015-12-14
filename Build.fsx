#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.AssemblyInfoHelper
open Fake.Git
open Fake.DotCover

let binDir = "./bin"
let srcDir = "./src"
let testDlls = srcDir + "/**/bin/Release/*Tests.dll"

let solutionFile = srcDir + "/CrossCutting.sln"
let packageDir = "packages"

let versionMajorMinor = "0.2"
let version = versionMajorMinor + ".0"

let commitHash = Information.getCurrentSHA1("")

let versionMacroBuild = 
    match buildServer with
    | AppVeyor -> ("0." + appVeyorBuildVersion.Replace("0.0.", ""))
    | _ -> "0"

let buildVersion = versionMajorMinor + "." + versionMacroBuild

Target "Clean" (fun _ ->
    CleanDirs []
    DeleteDir binDir
    CreateDir binDir
)

Target "Version" (fun _ ->
    CreateCSharpAssemblyInfo (srcDir + "/VersionInfo.cs")
        [Attribute.Version version
         Attribute.FileVersion buildVersion
         Attribute.Metadata("githash", commitHash)]

    match buildServer with
    | _ -> ()
)

Target "RestorePackages" (fun _ -> 
     solutionFile
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             OutputPath = packageDir
             ToolPath = "tools/Nuget/nuget.exe"
             Retries = 4 }))

Target "Build" (fun _ ->
    !! solutionFile
        |> MSBuildReleaseExt "" [("Configuration", "Release")] "Build"
        |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    !! (testDlls) 
        |> DotCoverNUnit 
            (fun dotCoverOptions ->
                { dotCoverOptions with
                    ErrorLevel = DontFailBuild
                    Filters = "+:module=CrossCutting.*;class=*;function=*;-:module=*Test*;"
                    Output = binDir + "/DotCover.snapshot" }) 
            (fun nUnitOptions ->
                { nUnitOptions with
                    ErrorLevel = DontFailBuild
                    OutputFile = binDir + "/TestResult.xml"
                    DisableShadowCopy = true })

    DotCoverReport
        (fun p ->
            { p with 
                Source = binDir + "/DotCover.snapshot"
                Output = binDir + "/DotCoverReport.html"
                ReportType = DotCoverReportType.Html }) false
)

Target "CreatePackage" (fun _ ->
    // Copy all the package files into a package folder
    for nuspec in !! "src/**/*.nuspec" do

        let projFileName = nuspec.Replace(".nuspec", ".csproj")
        
        NuGetPack (fun p -> 
            {p with
                OutputPath = binDir
                WorkingDir = binDir
                Version = buildVersion
                IncludeReferencedProjects = true
                Properties = [ ("configuration", "release") ]
                }) 
                projFileName
)

Target "Default" (fun _ ->
    trace "Build Complete"
)

"Clean"
 ==> "Version"
 ==> "RestorePackages"
 ==> "Build"
 ==> "Test"
 ==> "CreatePackage"
 ==> "Default"

RunTargetOrDefault "Default"