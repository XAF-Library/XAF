namespace XAF.UI.Abstraction;

public interface IResultViewModel<out TResult> : IActivatableViewModel
{
    TResult CreateResult();
}

public interface IResultViewModel<out TResult, in TParameter> : IActivatableViewModel<TParameter>
{
    TResult CreateResult();
}