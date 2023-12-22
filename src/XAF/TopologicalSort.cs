namespace XAF.Utilities;

/// <summary>
/// A Simple implementation of the Topological sort
/// </summary>
public static class TopologicalSort
{
    /// <summary>
    /// Sort the source in Topological order
    /// </summary>
    /// <typeparam name="T">Type of the item</typeparam>
    /// <param name="source">Items that should be sorted</param>
    /// <param name="getDependencies">function to get the dependencies of the item</param>
    /// <returns></returns>
    public static IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        where T : notnull
    {
        var sorted = new List<T>();
        var visited = new Dictionary<T, bool>();

        foreach (var element in source)
        {
            Visit(element, getDependencies, sorted, visited);
        }

        return sorted;
    }

    private static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        where T : notnull
    {
        bool inProcess;
        var alreadyVisited = visited.TryGetValue(item, out inProcess);

        if (alreadyVisited)
        {
            if (inProcess)
            {
                throw new ArgumentException($"Cyclic dependency found for '{item}' ");
            }
        }
        else
        {
            visited[item] = true;

            var dependencies = getDependencies(item);
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    Visit(dependency, getDependencies, sorted, visited);
                }
            }

            visited[item] = false;
            sorted.Add(item);
        }
    }
}
