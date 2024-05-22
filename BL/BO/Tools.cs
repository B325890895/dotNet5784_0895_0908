using System.Diagnostics.CodeAnalysis;

namespace BO;

public static class Tools
{
    private static int nextMilestoneId = 1;
    internal static int NextMilestoneId { get => nextMilestoneId++; }

    public class toCompare : IEqualityComparer<IEnumerable<int>>
    {
        public bool Equals(IEnumerable<int>? x, IEnumerable<int>? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IEnumerable<int> obj)
        {
            throw new NotImplementedException();
        }
    }
}
