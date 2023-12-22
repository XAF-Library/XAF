using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Hosting.Abstraction;
using XAF.Utilities;
using XAF.Utilities.ExtensionMethods;

namespace XAF.Hosting.Internal;
internal class OrderedStartupActionCollection
{
    private readonly IEnumerable<IHostStartupAction> _startupActions;

    public OrderedStartupActionCollection(IEnumerable<IHostStartupAction> startupActions)
    {
        _startupActions = startupActions;
    }

    public IEnumerable<IHostStartupAction> GetOrderedStartupActions()
    {
        var dependencies = CreateDependencyGraph(_startupActions);
        return TopologicalSort.Sort(_startupActions, a => dependencies[a]);
    }

    Dictionary<IHostStartupAction, HashSet<IHostStartupAction>> CreateDependencyGraph(IEnumerable<IHostStartupAction> startupActions)
    {
        var actions = new Dictionary<Type, IHostStartupAction>();
        var dic = new Dictionary<IHostStartupAction, HashSet<IHostStartupAction>>();

        foreach (var startupAction in startupActions)
        {
            actions[startupAction.GetType()] = startupAction;
            dic[startupAction] = new();
        }

        foreach (var startupAction in startupActions)
        {
            var orderRule = startupAction.ConfigureExecutionTime();
            foreach (var dependency in orderRule.After)
            {
                dic.Add(startupAction, actions[dependency]);
            }

            foreach (var dependent in orderRule.Before)
            {
                dic.Add(actions[dependent], startupAction);
            }
        }

        return dic;
    }
}
