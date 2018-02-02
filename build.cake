#addin "Newtonsoft.Json"
#addin "Cake.Powershell"
#addin "Cake.Incubator"
#tool "nuget:?package=xunit.runner.console&version=2.3.0"
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.8.0"
#tool "nuget:?package=GitVersion.CommandLine"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Debug");
var name = Argument<string>("name", "Fluency");
var verbosity = Argument<string>("verbosity", "Minimal");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var solutionDir = Directory("./");
var solutionFile = solutionDir + File("Fluency.NET.sln");
var buildDir = solutionDir + Directory("bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreCleanSettings
     {         
         Configuration = configuration      
     };

    DotNetCoreClean(solutionFile, settings);
});


Task("Update-Version")
    .Description("Calculate the version number and update assembly version files.")
    .Does(() => 
{
    try 
    {    
        var assemblyInfoFile = Directory("./src/Fluency") + File("Properties/AssemblyVersionInfo.cs");
        if (!FileExists(assemblyInfoFile))
        {
            Information("Assembly version file does not exist : " + assemblyInfoFile.Path);
            CopyFile("./src/AssemblyVersionInfo.template.cs", assemblyInfoFile);
        }
        
        GitVersion(new GitVersionSettings { 
            NoFetch = false,
            OutputType = GitVersionOutput.BuildServer,
            UpdateAssemblyInfo = true,
            UpdateAssemblyInfoFilePath = assemblyInfoFile });                    
    }
    catch (Exception ex) {
        Information(ex.ToString());
        // Assume that we might be in a pull request build which cannot have the version calculated
    }   
});


Task("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(solutionFile);    
});


Task("Pack-Nuget")
    .Does(() => 
{
    var nugetPackageDir = Directory("./artifacts");
    EnsureDirectoryExists(nugetPackageDir);
    
    var version = GitVersion();
    var settings = new DotNetCorePackSettings
    {
        ArgumentCustomization = args=>args.Append("/p:PackageVersion=" + version.NuGetVersionV2),
        Configuration = configuration,
        OutputDirectory = nugetPackageDir,
        NoRestore = true,
        IncludeSymbols = true
    };

    DotNetCorePack("src/Fluency/Fluency.csproj", settings);
});


Task("Build")
    .IsDependentOn("Update-Version")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration      
    };

    DotNetCoreBuild("./Fluency.NET.sln", settings);
});


Task("Run-Unit-Tests")
    .Does(() =>
{
    var testAssemblies = GetFiles("./test/**/*.csproj");

    // Disable parallel test runs due to static configuration in Fluency
    var netCoreTestSettings = new DotNetCoreTestSettings() {
        ArgumentCustomization = args => args.Append("-p:ParallelizeTestCollections=false"),
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true
    };

    var netCoreXunitTestSettings = new XUnit2Settings {
            Parallelism = ParallelismOption.None,
            HtmlReport = false,
            UseX86 = true
    };

    DotNetCoreTest(netCoreTestSettings, "./test/Fluency.Net.Standard.Tests/Fluency.Net.Standard.Tests.csproj", netCoreXunitTestSettings);

    var netStandardXunitSettings = new XUnit2Settings {
        Parallelism = ParallelismOption.None,
        HtmlReport = false,
        NoAppDomain = true,
        UseX86 = true
    };

    NUnit3("./test/Fluency.Net.Framework.40.Tests/bin/" + configuration + "/Fluency.Net.Framework.40.Tests.dll",
        new NUnit3Settings { NoResults = true });
    XUnit2("./test/Fluency.Net.Framework.461.Tests/bin/" + configuration + "/Fluency.Net.Framework.461.Tests.dll", netStandardXunitSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests");
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
