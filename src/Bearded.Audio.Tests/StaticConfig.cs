using VerifyTests;

namespace Bearded.Audio.Tests;

public static class StaticConfig
{
    public static VerifySettings DefaultVerifySettings
    {
        get
        {
            var settings = new VerifySettings();
            settings.UseDirectory("goldens");
            return settings;
        }
    }
}
