using System;
using System.Collections.Generic;
using System.Text;

namespace MapsetVerifierFramework.objects.attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CheckAttribute : Attribute
    {
        // Used to identify which classes to add to checks in plugins.
    }
}
