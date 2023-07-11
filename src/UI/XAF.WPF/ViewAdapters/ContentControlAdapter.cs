﻿using System.Windows;
using System.Windows.Controls;
using XAF.UI.WPF.ExtensionMethodes;

namespace XAF.UI.WPF.ViewAdapters;
public sealed class ContentControlAdapter : ViewAdapterBase<ContentControl>
{
    public override void Clear(ContentControl container)
    {
        container.Content = null;
    }

    public override bool Contains(ContentControl container, FrameworkElement view)
    {
        return container.Content == view;
    }

    public override FrameworkElement? GetActiveView(ContentControl container)
    {
        return container.Content as FrameworkElement;
    }

    public override IEnumerable<FrameworkElement> GetElements(ContentControl container)
    {
        if(container.Content is FrameworkElement element)
        {
            yield return element;
        }
        yield break;
    }

    public override void Insert(ContentControl container, FrameworkElement view)
    {
        container.Content = view;
    }

    public override void Remove(ContentControl container, FrameworkElement view)
    {
        container.Content = null;
    }

    public override void Select(ContentControl container, FrameworkElement view)
    {
        container.Content = view;
    }
}
