﻿using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using Version = System.Version;

namespace Versionize
{
    public static class RespositoryExtensions
    {
        public static Tag SelectVersionTag(this Repository repository, Version version)
        {
            return repository.Tags.SingleOrDefault(t => t.IsAnnotated && t.Annotation.Name == $"v{version}");
        }

        public static List<Commit> GteCommitsSinceLastVersion(this Repository repository, Tag versionTag)
        {
            if (versionTag == null)
            {
                return repository.Commits.ToList();
            }

            var filter = new CommitFilter()
            {
                ExcludeReachableFrom = versionTag
            };

            return repository.Commits.QueryBy(filter).ToList();
        }
    }
}
