using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Utilities;
public static class TopologicalSort
{
    public static IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
    {
        var sorted = new List<T>();
        var visited = new Dictionary<T, bool>();

        foreach (var element in source)
        {
            Visit(element, getDependencies, sorted, visited);
        }

        return sorted;
    }

    public static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies,
                   List<T> sorted, Dictionary<T, bool> visited)
    {
        bool inProcess;
        var alreadyVisited = visited.TryGetValue(item, out inProcess);

        if (alreadyVisited)
        {
            if (inProcess)
            {
                throw new ArgumentException("Cyclic dependency found.");
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
