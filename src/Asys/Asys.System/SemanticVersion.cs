using SystemVersion = System.Version;

namespace Asys.System;

/// <summary>
/// A semantic version identifies a package build.
/// </summary>
/// <remarks>
/// More info on versions here: <see href="http://semver.org/"/>.
/// </remarks>
public class SemanticVersion : IComparable<SemanticVersion>
{
    private readonly SystemVersion _version;

    /// <summary>
    /// The numeric version in the form "Major.Minor.Patch" excluding pre-release tag and build metadata (if any).
    /// </summary>
    public string VersionNumber => _version.ToString();

    /// <summary>
    /// First number of the version (1 for "1.2.3")
    /// </summary>
    public int Major => _version.Major;

    /// <summary>
    /// Second number of the version (2 for "1.2.3")
    /// </summary>
    public int Minor => _version.Minor;

    /// <summary>
    /// Third number of the version (3 for "1.2.3")
    /// </summary>
    public int Patch => _version.Build;

    /// <summary>
    /// The pre-release tag. For example: "beta2.3" in "1.2.3-beta2.3"
    /// </summary>
    public string? PreReleaseTag { get; }

    /// <summary>
    /// Meta information on build. For example: "sha.71e65a" in "1.2.3+sha.71e65a"
    /// </summary>
    /// <remarks>
    /// These information are additionnal and are NOT taken in account for sorting versions.
    /// For instance 1.0.0 and 1.0.0+build.4 are considered equal.
    /// </remarks>
    public string? BuildMetaData { get; }

    /// <summary>
    /// A pre-release version has a pre-release tag. For example: 1.1-alpha
    /// </summary>
    public bool IsPreRelease => PreReleaseTag != null;

    /// <summary>
    /// The version contains build information. For example: 1.1+build.100 (for a stable release) or 1.1-beta+build.100 (for a pre-release)
    /// </summary>
    public bool HasBuildMetaData => BuildMetaData != null;

    /// <summary>
    /// Initalizes an instane of <see cref="SemanticVersion"/>.
    /// </summary>
    /// <param name="major">The major.</param>
    /// <param name="minor">The minor.</param>
    /// <param name="patch">The patch.</param>
    /// <param name="preReleaseTag">The pre release tag.</param>
    public SemanticVersion(int major, int minor, int patch, string? preReleaseTag)
        : this(new SystemVersion(Math.Max(major, 0), Math.Max(minor, 0), Math.Max(patch, 0)), CleanInputPreReleaseTag(preReleaseTag), null)
    {
    }

    /// <summary>
    /// Initalizes an instane of <see cref="SemanticVersion"/>.
    /// </summary>
    /// <param name="versionNumber">The full version using <see cref="SystemVersion"/> syntax.</param>
    /// <param name="preReleaseTag">The pre release tag.</param>
    /// <param name="buildMetaData">The pre release tag.</param>
    public SemanticVersion(string versionNumber, string? preReleaseTag, string? buildMetaData)
        : this(GetVersion(versionNumber), CleanInputPreReleaseTag(preReleaseTag), CleanInputBuildMetadata(buildMetaData))
    {
    }

    /// <summary>
    /// Initalizes an instane of <see cref="SemanticVersion"/>.
    /// </summary>
    /// <param name="versionNumber">The full version using <see cref="SystemVersion"/> syntax.</param>
    /// <param name="preReleaseTag">The pre release tag.</param>
    public SemanticVersion(string versionNumber, string? preReleaseTag)
        : this(GetVersion(versionNumber), CleanInputPreReleaseTag(preReleaseTag), null)
    {
    }

    private static string? CleanInputPreReleaseTag(string? preReleaseTag)
    {
        if (string.IsNullOrEmpty(preReleaseTag))
        {
            return null;
        }

        return preReleaseTag.TrimStart('-');
    }

    private static string? CleanInputBuildMetadata(string? buildMetaData)
    {
        if (string.IsNullOrEmpty(buildMetaData))
        {
            return null;
        }

        return buildMetaData.TrimStart('+');
    }

    private static SystemVersion GetVersion(string? versionNumber)
    {
        if (versionNumber is null)
        {
            throw new ArgumentNullException(nameof(versionNumber));
        }

        versionNumber = CleanVersion(versionNumber);

        if (!IsValidVersionNumber(versionNumber))
        {
            throw new ArgumentException($"Invalid version number '{versionNumber}'. Expected a version in the form 2.3.4", nameof(versionNumber));
        }

        return new SystemVersion(versionNumber);
    }

    private static string CleanVersion(string versionNumber)
    {
        if (versionNumber.StartsWith("v."))
        {
            versionNumber = versionNumber.Substring(2);
        }
        else if (versionNumber.StartsWith("v"))
        {
            versionNumber = versionNumber.Substring(1);
        }

        if (versionNumber.EndsWith("."))
        {
            versionNumber = versionNumber.Substring(0, versionNumber.Length - 1);
        }

        var dotCount = versionNumber.Count(x => x == '.');
        if (dotCount == 0)
        {
            versionNumber += ".0.0";
        }
        else if (dotCount == 1)
        {
            versionNumber += ".0";
        }

        return versionNumber;
    }

    internal SemanticVersion(SystemVersion version, string? preReleaseTag, string? buildMetaData, bool checkInputs = true)
    {
        _version = version ?? throw new ArgumentNullException(nameof(version));

        if (preReleaseTag is not null)
        {
            if (checkInputs && !IsValidVersionIdentifiers(preReleaseTag))
            {
                throw new ArgumentException($"{preReleaseTag} is not a valid prerelease tag. It can't be empty and must contain identifiers separated by . (eg beta.10)");
            }

            PreReleaseTag = preReleaseTag;
        }

        if (buildMetaData is not null)
        {
            if (checkInputs && !IsValidBuildIdentifiers(buildMetaData))
            {
                throw new ArgumentException($"{buildMetaData} is not a valid build metadata. It can't be empty and must contain identifiers separated by . (eg build.100)");
            }

            BuildMetaData = buildMetaData;
        }
    }

    /// <summary>
    /// Parse the version string
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the input is not of valid form.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the input is null.</exception>
    public static SemanticVersion Parse(string? input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input)); 
        }

        ExtractVersionTagAndBuild(input, out var versionNumber, out var preReleaseTag, out var buildMetadata);

        return new SemanticVersion(GetVersion(versionNumber), preReleaseTag, buildMetadata);
    }

    private static void ExtractVersionTagAndBuild(string input, out string versionNumber, out string? preReleaseTag, out string? buildMetadata)
    {
        input = input.Trim(' ');

        // Extract version from tag and build
        buildMetadata = null;
        preReleaseTag = null;
        int index;
        for (index = 0; index < input.Length; index++)
        {
            var c = input[index];
            if (index == 0 && c == 'v')
                continue;
            if (!char.IsDigit(c) && c != '.')
                break;
        }

        versionNumber = input.Substring(0, index);

        if (index <= input.Length - 1)
        {
            ExtractTagAndBuild(input.Substring(index), ref preReleaseTag, ref buildMetadata);
        }
    }

    private static void ExtractTagAndBuild(string input, ref string? preReleaseTag, ref string? buildMetadata)
    {
        if (input.StartsWith("+"))
        {
            buildMetadata = input.Substring(1);
            return;
        }

        if (input[0] == '.' || input[0] == '-' || input[0] == '_')
        {
            input = input.Substring(1);
        }

        var parts = input.Split('+');
        preReleaseTag = parts[0];
        if (parts.Length > 1)
        {
            buildMetadata = parts[1];
        }
    }

    /// <summary>
    /// Try to parse the input as a semantic version, returns false and null when it fails.
    /// </summary>
    public static bool TryParse(string? input, out SemanticVersion? result)
    {
        if (string.IsNullOrEmpty(input))
        {
            result = null;
            return false;
        }

        ExtractVersionTagAndBuild(input, out var versionNumber, out var preReleaseTag, out var buildMetadata);

        versionNumber = CleanVersion(versionNumber);
        if (!IsValidVersionNumber(versionNumber))
        {
            result = null;
            return false;
        }

        if (!IsValidVersionIdentifiers(preReleaseTag))
        {
            result = null;
            return false;
        }

        if (!IsValidBuildIdentifiers(buildMetadata))
        {
            result = null;
            return false;
        }

        result = new SemanticVersion(new SystemVersion(versionNumber), preReleaseTag, buildMetadata, false);
        return true;
    }

    private static bool IsValidVersionNumber(string versionNumber)
    {
        return SystemVersion.TryParse(versionNumber, out var _);
    }

    private static bool IsValidVersionIdentifiers(string? identifiers)
    {
        if (identifiers is null)
        {
            return true; 
        }

        if (identifiers == string.Empty)
        {
            return false; 
        }

        return identifiers.Split('.').All(IsValidVersionIdentifier);
    }

    private static bool IsValidBuildIdentifiers(string? identifiers)
    {
        // Build Metadata Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]
        // they can have leading zeros, see https://semver.org/#spec-item-10
        if (identifiers is null)
        {
            return true;
        }

        if (identifiers == string.Empty)
        {
            return false;
        }

        return identifiers.Split('.').All(IsValidBuildIdentifier);
    }

    private static bool IsValidVersionIdentifier(string identifier)
    {
        if (identifier == string.Empty)
        {
            // Identifiers MUST NOT be empty
            return false;
        }

        if (identifier[0] == '0')
        {
            if (identifier.Length == 1)
            {
                return true;
            }

#if STRICT_V2_PARSING
                if (char.IsDigit(identifier[1]))
                {
                    // Numeric identifiers MUST NOT include leading zeroes
                    return false;
                }
#endif
        }

        // . Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]
        return identifier.All(c => char.IsLetterOrDigit(c) || c == '-');
    }

    private static bool IsValidBuildIdentifier(string identifier)
    {
        if (identifier is null)
        {
            return true;
        }

        if (identifier == string.Empty)
        {
            // Identifiers MUST NOT be empty
            return false;
        }

        return identifier.All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '-');
    }

    /// <summary>
    /// Returns the Normalized Version string, without any Build Metadata (if any)
    /// </summary>
    /// <returns>The Normalized Version string</returns>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/en-us/nuget/concepts/package-versioning#normalized-version-numbers"/>
    /// </remarks>
    public string ToNormalizedVersionString()
    {
        var version = _version.ToString();
        if (version.EndsWith(".0", StringComparison.Ordinal) && version.Count(c => c == '.') == 3)
        {
            version = version.Substring(0, version.Length - 2);
        }

        return IsPreRelease ?
            $"{version}-{PreReleaseTag}" :
            version;
    }

    public override string ToString()
    {
        return HasBuildMetaData ?
            $"{ToNormalizedVersionString()}+{BuildMetaData}" :
            ToNormalizedVersionString();
    }

    #region Operator overloading

    /// <summary>
    /// Gets the hash code for this unique key.
    /// </summary>
    public override int GetHashCode()
    {
        return (VersionNumber ?? "").GetHashCode() ^ (PreReleaseTag?.ToLower() ?? "").GetHashCode();
    }

    public int CompareTo(SemanticVersion? other) => CompareTo(this, other);

    public bool Equals(SemanticVersion x)
    {
        return CompareTo(this, x) == 0;
    }

    public static bool operator <(SemanticVersion x, SemanticVersion y)
    {
        return CompareTo(x, y) < 0;
    }

    public static bool operator >(SemanticVersion x, SemanticVersion y)
    {
        return CompareTo(x, y) > 0;
    }

    public static bool operator <=(SemanticVersion x, SemanticVersion y)
    {
        return CompareTo(x, y) <= 0;
    }

    public static bool operator >=(SemanticVersion x, SemanticVersion y)
    {
        return CompareTo(x, y) >= 0;
    }

    public static bool operator ==(SemanticVersion x, SemanticVersion y)
    {
        return CompareTo(x, y) == 0;
    }

    public static bool operator !=(SemanticVersion x, SemanticVersion y)
    {
        return CompareTo(x, y) != 0;
    }

    public override bool Equals(object? obj)
    {
        return (obj is SemanticVersion semver) && (CompareTo(this, semver) == 0);
    }

    private static int CompareTo(SemanticVersion? a, SemanticVersion? b)
    {
        // A null version is unspecified and should be considered larger than a fixed one.
        if (a is null)
        {
            if (b is null)
            {
                return 0;
            }

            return 1;
        }

        if (b is null)
        {
            return -1;
        }

        var versionComparison = a._version.CompareTo(b._version);
        if (versionComparison != 0)
        {
            return versionComparison;
        }

        return ComparePreReleaseIdentifiers(a.PreReleaseTag, b.PreReleaseTag);
    }

    private static int ComparePreReleaseIdentifiers(string? preReleaseTag1, string? preReleaseTag2)
    {
        if (preReleaseTag1 is null)
        {
            if (preReleaseTag2 is null)
            {
                return 0;
            }

            return 1;
        }

        if (preReleaseTag2 is null)
        {
            return -1;
        }

        var identifiers1 = preReleaseTag1.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        var identifiers2 = preReleaseTag2.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < identifiers1.Length && i < identifiers2.Length; i++)
        {
            var tag1 = identifiers1[i];
            var tag2 = identifiers2[i];

            bool isNum1 = long.TryParse(tag1, out long num1);
            bool isNum2 = long.TryParse(tag2, out long num2);

            if (isNum1)
            {
                if (isNum2)
                {
                    int numComparison = num1.CompareTo(num2);
                    if (numComparison != 0)
                    {
                        return numComparison;
                    }

                    continue;
                }

                return -1;
            }

            if (isNum2)
            {
                return 1;
            }

            int stringComparison = string.Compare(tag1, tag2, StringComparison.OrdinalIgnoreCase);
            if (stringComparison != 0)
            {
                return stringComparison;
            }
        }

        return identifiers1.Length.CompareTo(identifiers2.Length);
    }

    #endregion
}