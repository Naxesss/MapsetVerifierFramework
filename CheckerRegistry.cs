using MapsetVerifierFramework.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MapsetVerifierFramework
{
    public static class CheckerRegistry
    {
        private static List<Check> checks = new List<Check>();

        /// <summary> Adds the given check to the list of checks to process when checking for issues. </summary>
        public static void RegisterCheck(Check aCheck)
        {
            if (aCheck == null)
                return;

            checks.Add(aCheck);
        }

        /// <summary> Unloads all checks, so that the list of checks to execute becomes empty. </summary>
        public static void ClearChecks()
        {
            checks = new List<Check>();
        }

        /// <summary> Returns all checks which are processed when checking for issues. </summary>
        public static List<Check> GetChecks()
        {
            return new List<Check>(checks);
        }

        /// <summary> Returns checks which are processed beatmap-wise when checking for issues.
        /// These are isolated from the set for optimization purposes. </summary>
        public static IEnumerable<BeatmapCheck> GetBeatmapChecks()
        {
            return checks.OfType<BeatmapCheck>();
        }

        /// <summary> Returns checks which are processed beatmapset-wise when checking for issues.
        /// These are often checks which need to compare between difficulties in a set.</summary>
        public static IEnumerable<BeatmapSetCheck> GetBeatmapSetChecks()
        {
            return checks.OfType<BeatmapSetCheck>();
        }

        /// <summary> Returns checks which are processed beatmapset-wise when checking for issues and stored in a seperate difficulty.
        /// These are general checks which are independent from any specific difficulty, for example checking files.</summary>
        public static IEnumerable<GeneralCheck> GetGeneralChecks()
        {
            return checks.OfType<GeneralCheck>();
        }
    }
}
