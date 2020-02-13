using System;

namespace PublishHelper
{

    static class Strings
    {

        public static string Root => App.OverridenRoot ?? Environment.CurrentDirectory;

        public static readonly string project = Root + @"\PaletteGenerator\PaletteGenerator.csproj";
        public static readonly string frameworkDependentProfile = Root + @"\PaletteGenerator\Properties\PublishProfiles\Trimmed.pubxml";
        public static readonly string selfContainedProfile = Root + @"\PaletteGenerator\Properties\PublishProfiles\Self-contained.pubxml";

        public const string Queued = "Queued";
        public const string InProgress = "Publishing...";
        public const string Ready = "Ready";

    }

}
