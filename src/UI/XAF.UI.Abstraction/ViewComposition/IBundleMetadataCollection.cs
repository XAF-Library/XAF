using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IBundleMetadataCollection
{
    void AddFromViewType(Type viewType);

    IEnumerable<IBundleMetadata> GetMetadataForDecorator<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    IBundleMetadata GetMetadataForViewModel<TViewModel>()
        where TViewModel : IXafViewModel;
}
