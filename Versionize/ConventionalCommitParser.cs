using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace Versionize
{
    public class ConventionalCommitParser
    {
        private static readonly string[] s_noteKeywords = new string[] { "BREAKING CHANGE" };

        private static readonly Regex s_headerPattern = new Regex("^(?<type>\\w*)(?:\\((?<scope>.*)\\))?: (?<subject>.*)$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public ConventionalCommitParser()
        {
        }

        public List<ConventionalCommit> Parse(List<Commit> commits)
        {
            return commits.Select(Parse).ToList();
        }

        public ConventionalCommit Parse(Commit commit)
        {
            var conventionalCommit = new ConventionalCommit();

            var commitMessageLines = commit.Message.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                )
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();

            var header = commitMessageLines.FirstOrDefault();

            if (header == null)
            {
                return conventionalCommit;
            }

            var match = s_headerPattern.Match(header);

            if (match.Success)
            {
                conventionalCommit.Scope = match.Groups["scope"].Value;
                conventionalCommit.Type = match.Groups["type"].Value;
                conventionalCommit.Subject = match.Groups["subject"].Value;
            }
            else
            {
                conventionalCommit.Subject = header;
            }

            for (var i = 1; i < commitMessageLines.Count; i++)
            {
                foreach (var noteKeyword in s_noteKeywords)
                {
                    var line = commitMessageLines[i];
                    if (line.StartsWith($"{noteKeyword}:", StringComparison.InvariantCulture))
                    {
                        conventionalCommit.Notes.Add(new ConventionalCommitNote
                        {
                            Title = noteKeyword,
                            Text = line.Substring($"{noteKeyword}:".Length).TrimStart()
                        });
                    }
                }
            }

            return conventionalCommit;
        }
    }
}
