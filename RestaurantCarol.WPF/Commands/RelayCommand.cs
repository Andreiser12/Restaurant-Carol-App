using System.Windows.Input;

namespace RestaurantCarol.ViewModels
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> commandTask;
        private readonly Predicate<T> canExecuteTask;

        public RelayCommand(Action<T> workToDo)
            : this(workToDo, _ => true) { }

        public RelayCommand(Action<T> workToDo, Predicate<T> canExecute)
        {
            commandTask = workToDo;
            canExecuteTask = canExecute;
        }

        public bool CanExecute(object? parameter) => canExecuteTask((T)parameter!);

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object? parameter) => commandTask((T)parameter!);
    }
}