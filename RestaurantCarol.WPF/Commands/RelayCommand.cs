using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Exceptions;
namespace RestaurantCarol.Commands
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
        public void Execute(object? parameter)
        {
            try
            {
                commandTask((T)parameter!);
            }
            catch (RestaurantException ex)
            {
                MessageBox.Show(ex.Message, "Atentie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A aparut o eroare: {ex.Message}", "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}