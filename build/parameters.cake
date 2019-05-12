#load "./paths.cake"
#load "./artifacts.cake"
#load "./credentials.cake"
#load "./version.cake"


public class BuildParameters
{
    public string Target { get; private set; }
    public string Configuration { get; private set; }

    public bool EnabledUnitTests { get; private set; }
    public bool EnabledPublishGem { get; private set; }
    public bool EnabledPublishTfs { get; private set; }
    public bool EnabledPublishNuget { get; private set; }
    public bool EnabledPublishChocolatey { get; private set; }
    public bool EnabledPublishDocker { get; private set; }

    public bool IsRunningOnUnix { get; private set; }
    public bool IsRunningOnWindows { get; private set; }
    public bool IsRunningOnLinux { get; private set; }
    public bool IsRunningOnMacOS { get; private set; }

    public bool IsLocalBuild { get; private set; }
    public bool IsRunningOnAppVeyor { get; private set; }
    public bool IsRunningOnTravis { get; private set; }
    public bool IsRunningOnAzurePipeline { get; private set; }

    public bool IsMainRepo { get; private set; }
    public bool IsMainBranch { get; private set; }
    public bool IsTagged { get; private set; }
    public bool IsPullRequest { get; private set; }
    
	public bool IsReleaseBranch { get; private set; }
	public bool IsFeatureBranch { get; private set; }
	public bool IsMasterBranch { get; private set; }
	public bool IsPullRequestBranch { get; private set; }
	public bool IsHotFixBranch { get; private set; }
	public bool IsSupportBranch { get; private set; }
	public bool IsDevelopBranch { get; private set; }

    public BuildParameters(){

    }


    public DotNetCoreMSBuildSettings MSBuildSettings { get; private set; }

    public BuildCredentials Credentials { get; private set; }
    public BuildVersion Version { get; private set; }
    public BuildPaths Paths { get; private set; }
    public BuildPackages Packages { get; private set; }
    public BuildArtifacts Artifacts { get; private set; }
    public DockerImages Docker { get; private set; }
    public Dictionary<string, DirectoryPath> PackagesBuildMap { get; private set; }

    public bool IsStableRelease() => !IsLocalBuild && IsMainRepo && IsMainBranch && !IsPullRequest && IsTagged;
    public bool IsPreRelease()    => !IsLocalBuild && IsMainRepo && IsMainBranch && !IsPullRequest && !IsTagged;

    public bool CanPostToGitter => !string.IsNullOrWhiteSpace(Credentials.Gitter.Token) && !string.IsNullOrWhiteSpace(Credentials.Gitter.RoomId);

    public static BuildParameters GetParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var target = context.Argument("target", "Default");
        var buildSystem = context.BuildSystem();

		var branchName = GetActiveBranchName(context);
		var regexOptions = System.Text.RegularExpressions.RegexOptions.IgnoreCase;

        var parmeters = new BuildParameters {
            Target        = target,
            Configuration = context.Argument("configuration", "Release"),

            EnabledUnitTests          = IsEnabled(context, "ENABLED_UNIT_TESTS"),
            EnabledPublishGem         = IsEnabled(context, "ENABLED_PUBLISH_GEM"),
            EnabledPublishTfs         = IsEnabled(context, "ENABLED_PUBLISH_TFS"),
            EnabledPublishNuget       = IsEnabled(context, "ENABLED_PUBLISH_NUGET"),
            EnabledPublishChocolatey  = IsEnabled(context, "ENABLED_PUBLISH_CHOCOLATEY"),
            EnabledPublishDocker      = IsEnabled(context, "ENABLED_PUBLISH_DOCKER"),

            IsRunningOnUnix    = context.IsRunningOnUnix(),
            IsRunningOnWindows = context.IsRunningOnWindows(),
            IsRunningOnLinux   = context.Environment.Platform.Family == PlatformFamily.Linux,
            IsRunningOnMacOS   = context.Environment.Platform.Family == PlatformFamily.OSX,

            IsLocalBuild             = buildSystem.IsLocalBuild,
            IsRunningOnAppVeyor      = buildSystem.IsRunningOnAppVeyor,
            IsRunningOnTravis        = buildSystem.IsRunningOnTravisCI,
            IsRunningOnAzurePipeline = buildSystem.IsRunningOnVSTS,

            IsMainRepo    = IsOnMainRepo(context),
            IsMainBranch  = IsOnMainBranch(context),
            IsPullRequest = IsPullRequestBuild(context),
            IsTagged      = IsBuildTagged(context),

			IsReleaseBranch = System.Text.RegularExpressions.Regex.Match(branchName, "releases?[/-]", regexOptions).Success,
			IsFeatureBranch = System.Text.RegularExpressions.Regex.Match(branchName, "features?[/-]", regexOptions).Success,
			IsMasterBranch  = System.Text.RegularExpressions.Regex.Match(branchName, "(master)", regexOptions).Success,
			IsPullRequestBranch  = System.Text.RegularExpressions.Regex.Match(branchName, @"(pull|pull\-requests|pr)[/-]", regexOptions).Success, 
			IsHotFixBranch  = System.Text.RegularExpressions.Regex.Match(branchName, "hotfix(es)?[/-]", regexOptions).Success,
			IsSupportBranch = System.Text.RegularExpressions.Regex.Match(branchName, "support[/-]", regexOptions).Success,
			IsDevelopBranch = System.Text.RegularExpressions.Regex.Match(branchName, "dev(elop)?(ment)?$", regexOptions).Success, 
        };

		//   context.Information("Building on branch={1} IsMasterBranch={0}", parmeters.IsMasterBranch,branchName);
        return parmeters;
    }

    public void Initialize(ICakeContext context, GitVersion gitVersion)
    {
        Version = BuildVersion.Calculate(context, this, gitVersion);

        Paths = BuildPaths.GetPaths(context, this, Configuration, Version);

        var dockerFiles = context.GetFiles("./src/**/Dockerfile").ToArray();
        Docker = DockerImages.GetDockerImages(context, dockerFiles);

        Packages = BuildPackages.GetPackages(
            Paths.Directories.NugetRoot,
            Version.SemVersion,
            new [] { $"{MyProject.RepositoryName}.DotNetCore",   $"{MyProject.RepositoryName}.DotNetStandard", $"{MyProject.RepositoryName}"},
            new [] { "" });

        var files = Paths.Files;
        Artifacts = BuildArtifacts.GetArtifacts(new[] {
            files.TestCoverageOutputFilePath,
            files.ReleaseNotesOutputFilePath,
            files.VsixOutputFilePath,
            files.VsixCoreFxOutputFilePath,
            files.GemOutputFilePath
        });

        PackagesBuildMap = new Dictionary<string, DirectoryPath>();
    foreach(var targetFramework in MyProject.TargetFrameworks){    
            PackagesBuildMap.Add($"{MyProject.RepositoryName}.{targetFramework}", Paths.Directories.ArtifactsBin.Combine(targetFramework));
    }        


        Credentials = BuildCredentials.GetCredentials(context);

        MSBuildSettings = GetMsBuildSettings(context, Version);
    }

    private DotNetCoreMSBuildSettings GetMsBuildSettings(ICakeContext context, BuildVersion version)
    {
        var msBuildSettings = new DotNetCoreMSBuildSettings()
                                .WithProperty("Version", version.SemVersion)
                                .WithProperty("AssemblyVersion", version.Version)
                                .WithProperty("PackageVersion", version.SemVersion)
                                .WithProperty("FileVersion", version.Version);

        if(!IsRunningOnWindows)
        {
            var frameworkPathOverride = context.Environment.Runtime.IsCoreClr
                                        ?   new []{
                                                new DirectoryPath("/Library/Frameworks/Mono.framework/Versions/Current/lib/mono"),
                                                new DirectoryPath("/usr/lib/mono"),
                                                new DirectoryPath("/usr/local/lib/mono")
                                            }
                                            .Select(directory =>directory.Combine("4.5"))
                                            .FirstOrDefault(directory => context.DirectoryExists(directory))
                                            ?.FullPath + "/"
                                        : new FilePath(typeof(object).Assembly.Location).GetDirectory().FullPath + "/";

            // Use FrameworkPathOverride when not running on Windows.
            context.Information("Build will use FrameworkPathOverride={0} since not building on Windows.", frameworkPathOverride);
            msBuildSettings.WithProperty("FrameworkPathOverride", frameworkPathOverride);
        }

        return msBuildSettings;
    }

    private static bool IsOnMainRepo(ICakeContext context)
    {
        var buildSystem = context.BuildSystem();
		var repositoryName = "";
        if (buildSystem.IsRunningOnAppVeyor)
        {
            repositoryName = buildSystem.AppVeyor.Environment.Repository.Name;
        }
        else if (buildSystem.IsRunningOnTravisCI)
        {
            repositoryName = buildSystem.TravisCI.Environment.Repository.Slug;
        }
        else if (buildSystem.IsRunningOnVSTS)
        {
            repositoryName = buildSystem.TFBuild.Environment.Repository.RepoName;
        }
        else if (buildSystem.IsLocalBuild)
        {
           repositoryName = "Local Build";
        }

        context.Information("Repository Name: {0} vs {1}" , MyProject.RepositoryName,repositoryName);

        return !string.IsNullOrWhiteSpace(MyProject.RepositoryName) && StringComparer.OrdinalIgnoreCase.Equals($"{MyProject.RepositoryOwner}/{MyProject.RepositoryName}", repositoryName); 
    }

    private static bool IsOnMainBranch(ICakeContext context)
    {
        var buildSystem = context.BuildSystem();
        string repositoryBranch = null;
        if (buildSystem.IsRunningOnAppVeyor)
        {
            repositoryBranch = buildSystem.AppVeyor.Environment.Repository.Branch;
        }
        else if (buildSystem.IsRunningOnTravisCI)
        {
            repositoryBranch = buildSystem.TravisCI.Environment.Build.Branch;
        }
        else if (buildSystem.IsRunningOnVSTS)
        {
            repositoryBranch = buildSystem.TFBuild.Environment.Repository.Branch;
        }
        else if (buildSystem.IsLocalBuild)
        {
          repositoryBranch = GetActiveBranchName(context); 
        }
        context.Information("Repository Branch: {0}" , repositoryBranch);

        return !string.IsNullOrWhiteSpace(repositoryBranch) && StringComparer.OrdinalIgnoreCase.Equals("master", repositoryBranch);
    }

    private static bool IsPullRequestBuild(ICakeContext context)
    {
        var buildSystem = context.BuildSystem();
        if (buildSystem.IsRunningOnAppVeyor)
        {
            return buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        }
        if (buildSystem.IsRunningOnTravisCI)
        {
            var value = buildSystem.TravisCI.Environment.Repository.PullRequest;
            return !string.IsNullOrWhiteSpace(value) && !string.Equals(value, false.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
        else if (buildSystem.IsRunningOnVSTS)
        {
            var value = context.EnvironmentVariable("SYSTEM_PULLREQUEST_ISFORK");
            return !string.IsNullOrWhiteSpace(value) && !string.Equals(value, false.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
        return false;
    }

    private static bool IsBuildTagged(ICakeContext context)
    {
        var gitPath = context.Tools.Resolve(context.IsRunningOnWindows() ? "git.exe" : "git");
        context.StartProcess(gitPath, new ProcessSettings { Arguments = "rev-parse --verify HEAD", RedirectStandardOutput = true }, out var sha);
        context.StartProcess(gitPath, new ProcessSettings { Arguments = "tag --points-at " + sha.Single(), RedirectStandardOutput = true }, out var redirectedOutput);

        return redirectedOutput.Any();
    }

    private static string GetActiveBranchName(ICakeContext context)
    {
        var gitPath = context.Tools.Resolve(context.IsRunningOnWindows() ? "git.exe" : "git");
        context.StartProcess(gitPath, new ProcessSettings {
		Arguments = "rev-parse --abbrev-ref HEAD" //"branch"
		, RedirectStandardOutput = true }, out var redirectedOutput);
        return redirectedOutput.FirstOrDefault().Replace("* ","");
    }
    private static bool IsEnabled(ICakeContext context, string envVar, bool nullOrEmptyAsEnabled = true)
    {
        var value = context.EnvironmentVariable(envVar);
        try{
        return string.IsNullOrWhiteSpace(value) ? nullOrEmptyAsEnabled : bool.Parse(value);
        }catch(Exception){
        return false;   
        }
    }
}
