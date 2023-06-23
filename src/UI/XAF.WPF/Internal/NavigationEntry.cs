﻿using System.Windows;

namespace XAF.WPF.Internal;
public class NavigationEntry
{
    public NavigationEntry? Before { get; set; }
    public NavigationEntry? After { get; set; }
    public FrameworkElement View { get; }

    public NavigationEntry(FrameworkElement element)
    {
        View = element;
    }
}
