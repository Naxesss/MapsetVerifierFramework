# DEPRECATED / Now part of [MapsetVerifier](https://github.com/Naxesss/MapsetVerifier)
 
# Mapset Verifier Framework
This is a framework for Mapset Verifier made in C# .NET Core, containing everything needed to load and run checks on beatmapsets, such as the `GetBeatmapSetIssues` and `LoadCheckDLLs` functions, along with the `IssueTemplate`, `IssueLevel`, `Check`, `CheckMetadata`, and `CheckRegistry` classes. There are also loading hooks for progress tracking included.

This is mainly for developing applications supporting check plugins like bots, web pages, or desktop apps, but is also used in check plugins to form a common denominator between plugin and application.

# Example
In this case a DLL for MapsetChecks is placed in the relative folder `/checks`. For this to work the dependencies of MapsetChecks need to be present in the application, i.e.
- TagLib.Portable (used for image resolution & video audio presence)
- NAudio (used for accurate bitrate analysis)
- ManagedBass (used for other audio analysis, like duration, channel imbalance, and delay detection)
```csharp
BeatmapSet beatmapSet = new BeatmapSet(@"C:\...\osu\Songs\469908 Apocalyptica - At the Gates of Manala");

Checker.LoadCheckDLLs();
IEnumerable<Issue> issues =
    Checker.GetBeatmapSetIssues(beatmapSet)
        .Where(anIssue => anIssue.level >= Issue.Level.Warning);

foreach (var beatmapIssues in issues.GroupBy(anIssue => anIssue.beatmap))
{
    // The difficulty we're in, e.g. [Normal], if null it's a general issue.
    Beatmap beatmap = beatmapIssues.Key;
    Console.WriteLine(beatmap?.ToString() ?? "[General]");

    foreach (var category in beatmapIssues.GroupBy(anIssue => anIssue.CheckOrigin))
    {
        if (beatmap == null || category.Any(anIssue => anIssue.AppliesToDifficulty(beatmap.GetDifficulty())))
        {
            // The check the issue was detected in, e.g. "Unsnapped hit objects."
            Console.WriteLine($"{category.Key.GetMetadata().Message}");

            // The issues the check detected, e.g. "00:09:437 - Circle unsnapped by -3 ms."
            foreach (Issue issue in category)
                if (beatmap == null || issue.AppliesToDifficulty(beatmap.GetDifficulty()))
                    Console.WriteLine($"\t({issue.level}) {issue.message}");
        }
    }

    Console.WriteLine();
}
```
```
[Extra]
Wrongly or inconsistently snapped hit objects.
        (Warning) 02:54:681 - 1/16 is used once or twice, ensure this makes sense.
        (Warning) 02:54:585 - 05:58:091 - 1/8 is used once or twice, ensure this makes sense.
Unused uninherited lines.
        (Warning) 02:04:189 - Changes nothing, other than the finish with the nightcore mod. Ensure it makes sense to have a finish here.

[Insane]
Wrongly or inconsistently snapped hit objects.
        (Warning) 02:54:681 - 1/16 is used once or twice, ensure this makes sense.
        (Warning) 02:54:585 - 05:58:091 - 1/8 is used once or twice, ensure this makes sense.
        (Warning) 02:29:915 - (1/4) Different snapping, 02:29:885 - (1/3), is used in [Extra].
Unused uninherited lines.
        (Warning) 02:04:189 - Changes nothing, other than the finish with the nightcore mod. Ensure it makes sense to have a finish here.

[General]
Delayed hit sounds.
        (Warning) "soft-hitnormal.wav" has a delay of ~10 ms.
```

## Notes
- When using `GetBeatmapSetIssues` a lot of issues will be returned, including would-be issues depending on how the difficulty level is interpreted as well as minor issues which are normally only visible through enabling them in Mapset Verifier. Use the `Issue.AppliesToDifficulty(Beatmap.Difficulty)` function to filter out other interpretations.

- Be aware that plugins can be malicious (and is equivalent to running an exe if placed into the respective folder), so if you plan to release your own application, do make users aware of this as well so they can be critical about which plugins to trust.
