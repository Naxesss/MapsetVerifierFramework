﻿using MapsetParser.objects;
using MapsetVerifier.objects.metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapsetVerifier.objects
{
    public abstract class BeatmapSetCheck : Check
    {
        public abstract IEnumerable<Issue> GetIssues(BeatmapSet aBeatmapSet);
    }
}
