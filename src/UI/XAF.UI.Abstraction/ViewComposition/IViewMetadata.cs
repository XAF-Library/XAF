﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewMetadata
{
    Type ViewModelType { get; }

    IViewDecoratorCollection ViewDecorators { get; }
}
