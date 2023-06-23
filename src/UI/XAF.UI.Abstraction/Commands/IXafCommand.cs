using System.Windows.Input;

namespace XAF.UI.Abstraction.Commands;
public interface IXafCommand<in TParam, out TResult> : ICommand
{
    bool CanExecute(TParam param);

    TResult Execute(TParam param);
}

public interface IXafCommand<in TParam> : ICommand
{
    bool CanExecute(TParam param);

    void Execute(TParam param);
}

public interface IXafResultCommand<out TResult> : ICommand
{
    bool CanExecute();

    TResult Execute();
}
