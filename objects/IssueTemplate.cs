using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapsetVerifierFramework.objects
{
    public class IssueTemplate
    {
        public Issue.Level Level { get; }

        private readonly string format;
        private readonly object[] defaultArguments;

        private string cause;

        /// <summary>
        /// Constructs a new issue template with the given issue level, format and default arguments.
        /// </summary>
        /// <param name="aLevel">The type and priority of the issue (e.g. minor/warning/unrankable).</param>
        /// <param name="aFormat">The formatting string, use {0}, {1}, etc to insert arguments.</param>
        /// <param name="aDefaultArguments">The default arguments for the format string, supply as many of these as you have {0}, {1}, etc.</param>
        public IssueTemplate(Issue.Level aLevel, string aFormat, params object[] aDefaultArguments)
        {
            Level = aLevel;

            format = aFormat;
            defaultArguments = aDefaultArguments;

            for (int i = 0; i < defaultArguments.Length; ++i)
            {
                if (!format.Contains("{" + i + "}"))
                    throw new ArgumentException(
                        "\"" + aFormat + "\" There are " + defaultArguments.Length + " default arguments given, but the format string " +
                        "does not contain any \"{" + i + "}\", which makes the latter one(s) useless. Ensure there " + 
                        "are an equal amount of {0}, {1}, etc as there are default arguments.");
            }

            if (format.Contains("{" + defaultArguments.Length + "}"))
                throw new ArgumentException(
                    "\"" + aFormat + "\" There are " + defaultArguments.Length + " default arguments given, but the format string " +
                    "contains an unused argument place, \"{" + defaultArguments.Length + "}\". Ensure there " +
                    "are an equal amount of {0}, {1}, etc as there are default arguments.");

            cause = null;
        }

        /// <summary> Returns the template with a given cause, which will be shown below the issue template in the documentation. </summary>
        public IssueTemplate WithCause(string aCause)
        {
            cause = aCause;

            return this;
        }

        /// <summary> Returns the cause for the issue, which is shown below the template in the documentation, or null if none is supplied. </summary>
        public string GetCause()
        {
            return cause;
        }

        /// <summary> Returns the format with {0}, {1}, etc. replaced with the respective given arguments. </summary>
        public string Format(object[] anArguments)
        {
            if (anArguments.Length != defaultArguments.Length)
                throw new ArgumentException(
                    "The format for a template is \"" + format + "\", which takes " + defaultArguments.Length + " arguments " +
                    "(according to the default argument amount), but was given the unexpected argument amount " + anArguments.Length + ". " +
                    "Make sure that, when creating a new issue, you supply it with the correct amount of arguments for its template.");

            // Trimming format and args separately allows for "timestamp - " in "{0} /.../" without double spacing.
            // This way we still maintain any double space causing things like incorrect filenames within arguments.
            return
                string.Format(
                    format.Trim(),
                    anArguments.Select(anArg => anArg.ToString().Trim()).ToArray());
        }

        /// <summary> Returns the default arguments for this template. </summary>
        public object[] GetDefaultArguments()
        {
            return defaultArguments;
        }

        public override string ToString()
        {
            return Format(defaultArguments);
        }
    }
}
