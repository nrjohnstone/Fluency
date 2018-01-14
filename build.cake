#addin "Newtonsoft.Json"
#addin "Cake.Powershell&version=0.3.5"
#tool "nuget:?package=xunit.runner.console&version=2.2.0"
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
var solutionDir = Directory("./src");
var solutionFile = solutionDir + File("Fluency.NET.sln");
var buildDir = solutionDir + Directory("bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Clean")
    .Does(() =>
{
    var path = solutionDir.Path;
    Information("Cleaning build output", path);
    CleanDirectories(path + "/**/bin/" + configuration);
    CleanDirectories(path + "/**/obj/" + configuration);
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
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});


Task("Pack-Nuget")
    .Does(() => 
{
    EnsureDirectoryExists("./artifacts");
    string version = GitVersion().NuGetVersion;
    
    var nugetPackageDir = Directory("./artifacts");

    var nuGetPackSettings = new NuGetPackSettings
    {   
        Version                 = version,
        OutputDirectory         = nugetPackageDir,
        BasePath                = "src/Fluency/bin/" + configuration,
        ArgumentCustomization   = args => args.Append("-Prop Configuration=" + configuration)
    };

    NuGetPack("src/Fluency/Fluency.nuspec", nuGetPackSettings);
});


Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Update-Version")
    .Does(() =>
{
    MSBuild(solutionFile, settings => settings
        .SetConfiguration(configuration)
        .SetVerbosity(Verbosity.Quiet));
});


Task("Run-Unit-Tests")
    .Does(() =>
{
    var testAssemblies = GetFiles("./test/**/bin/" + configuration + "/*.Tests.dll");
    XUnit2(testAssemblies, new XUnit2Settings {
        Parallelism = ParallelismOption.None,
        HtmlReport = false,
        NoAppDomain = true,
        UseX86 = true
    });
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
