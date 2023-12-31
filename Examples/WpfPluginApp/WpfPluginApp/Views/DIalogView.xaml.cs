﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfPluginApp.ViewModels;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.WPF.Attributes;

namespace WpfPluginApp.Views;
/// <summary>
/// Interaktionslogik für DialogView.xaml
/// </summary>
[ViewFor<DialogViewModel>]
public partial class DialogView : UserControl
{
    public DialogView()
    {
        InitializeComponent();
    }
}
