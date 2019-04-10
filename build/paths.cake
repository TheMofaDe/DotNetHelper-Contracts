public class BuildPaths
{
    public BuildFiles Files { get; private set; }
    public BuildDirectories Directories { get; private set; }

    public static BuildPaths GetPaths(
        ICakeContext context,
        BuildParameters parameters,
        string configuration,
        BuildVersion version
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        if (string.IsNullOrEmpty(configuration))
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        if (version == null)
        {
            throw new ArgumentNullException(nameof(version));
        }

        var semVersion = version.SemVersion;

        var artifactsDir                  = (DirectoryPath)(context.Directory("./artifacts") + context.Directory("v" + semVersion));
        var artifactsBinDir               = artifactsDir.Combine("bin");
        var artifactsBinFullFxDir         = artifactsBinDir.Combine("net452");  // TODO :: Configurable per project. enter the framework your targeting
        var artifactsBinFullFxILMergeDir  = artifactsBinFullFxDir.Combine("il-merge");
        var artifactsBinFullFxPortableDir = artifactsBinFullFxDir.Combine("portable");
        var artifactsBinFullFxCmdlineDir  = artifactsBinFullFxDir.Combine("cmdline");
        var artifactsBinCoreFxDir         = artifactsBinDir.Combine("netcoreapp2.0");  // TODO :: Configurable per project. enter the framework your targeting
        var artifactsBinStandardFxDir     = artifactsBinDir.Combine("netstandard2.0");  // TODO :: Configurable per project. enter the framework your targeting
        var nugetRootDir                  = artifactsDir.Combine("nuget");
        var buildArtifactDir              = artifactsDir.Combine("build-artifact");
        var testCoverageOutputDir         = artifactsDir.Combine("code-coverage");

        var zipArtifactPathCoreClr = artifactsDir.CombineWithFilePath("ApplicationName-bin-corefx-v" + semVersion + ".zip"); // TODO :: Configurable per project
        var zipArtifactPathDesktop = artifactsDir.CombineWithFilePath("ApplicationName-bin-fullfx-v" + semVersion + ".zip"); // TODO :: Configurable per project
        var zipArtifactPathStandard = artifactsDir.CombineWithFilePath("ApplicationName-bin-standardfx-v" + semVersion + ".zip"); // TODO :: Configurable per project

        var testCoverageOutputFilePath = testCoverageOutputDir.CombineWithFilePath("TestResult.xml");

        var releaseNotesOutputFilePath = buildArtifactDir.CombineWithFilePath("releasenotes.md");
        var gemOutputFilePath  = buildArtifactDir.CombineWithFilePath("-" + version.GemVersion + ".gem");

        var tfsSuffix = parameters.IsStableRelease() ? "" : "preview-";
        var vsixOutputFilePath = buildArtifactDir.CombineWithFilePath("-" + tfsSuffix + version.TfxVersion + ".vsix");
        var vsixCoreFxOutputFilePath = buildArtifactDir.CombineWithFilePath("-netcore-" + tfsSuffix + version.TfxVersion + ".vsix");

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            buildArtifactDir,
            testCoverageOutputDir,
            nugetRootDir,
            artifactsBinDir,
            artifactsBinFullFxDir,
            artifactsBinCoreFxDir,
            artifactsBinStandardFxDir);

        // Files
        var buildFiles = new BuildFiles(
            context,
            zipArtifactPathCoreClr,
            zipArtifactPathDesktop,
            zipArtifactPathStandard,
            testCoverageOutputFilePath,
            releaseNotesOutputFilePath,
            vsixOutputFilePath,
            vsixCoreFxOutputFilePath,
            gemOutputFilePath);

        return new BuildPaths
        {
            Files = buildFiles,
            Directories = buildDirectories
        };
    }
}

public class BuildFiles
{
    public FilePath ZipArtifactPathCoreClr { get; private set; }
    public FilePath ZipArtifactPathDesktop { get; private set; }
    public FilePath ZipArtifactPathStandard { get; private set; }
    public FilePath TestCoverageOutputFilePath { get; private set; }
    public FilePath ReleaseNotesOutputFilePath { get; private set; }
    public FilePath VsixOutputFilePath { get; private set; }
    public FilePath VsixCoreFxOutputFilePath { get; private set; }
    public FilePath GemOutputFilePath { get; private set; }

    public BuildFiles(
        ICakeContext context,
        FilePath zipArtifactPathCoreClr,
        FilePath zipArtifactPathDesktop,
        FilePath zipArtifactPathStandard,
        FilePath testCoverageOutputFilePath,
        FilePath releaseNotesOutputFilePath,
        FilePath vsixOutputFilePath,
        FilePath vsixCoreFxOutputFilePath,
        FilePath gemOutputFilePath
        )
    {
        ZipArtifactPathCoreClr = zipArtifactPathCoreClr;
        ZipArtifactPathDesktop = zipArtifactPathDesktop;
        ZipArtifactPathStandard = zipArtifactPathStandard;
        TestCoverageOutputFilePath = testCoverageOutputFilePath;
        ReleaseNotesOutputFilePath = releaseNotesOutputFilePath;
        VsixOutputFilePath = vsixOutputFilePath;
        VsixCoreFxOutputFilePath = vsixCoreFxOutputFilePath;
        GemOutputFilePath = gemOutputFilePath;
    }
}

public class BuildDirectories
{
    public DirectoryPath Artifacts { get; private set; }
    public DirectoryPath NugetRoot { get; private set; }
    public DirectoryPath BuildArtifact { get; private set; }
    public DirectoryPath TestCoverageOutput { get; private set; }
    public DirectoryPath ArtifactsBin { get; private set; }
    public DirectoryPath ArtifactsBinFullFx { get; private set; }
    public DirectoryPath ArtifactsBinCoreFx { get; private set; }
    public DirectoryPath ArtifactsBinStandardFx { get; private set; }
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(
        DirectoryPath artifactsDir,
        DirectoryPath buildArtifactDir,
        DirectoryPath testCoverageOutputDir,
        DirectoryPath nugetRootDir,
        DirectoryPath artifactsBinDir,
        DirectoryPath artifactsBinFullFxDir,
        DirectoryPath artifactsBinCoreFxDir,
        DirectoryPath artifactsBinStandardFxDir
        )
    {
        Artifacts = artifactsDir;
        BuildArtifact = buildArtifactDir;
        TestCoverageOutput = testCoverageOutputDir;
        NugetRoot = nugetRootDir;
        ArtifactsBin = artifactsBinDir;
        ArtifactsBinFullFx = artifactsBinFullFxDir;
        ArtifactsBinCoreFx = artifactsBinCoreFxDir;
        ArtifactsBinStandardFx = artifactsBinStandardFxDir;
        ToClean = new[] {
            Artifacts,
            BuildArtifact,
            TestCoverageOutput,
            NugetRoot,
            ArtifactsBin,
            ArtifactsBinFullFx,
             ArtifactsBinCoreFx,
            ArtifactsBinStandardFx,
        };
    }
}
