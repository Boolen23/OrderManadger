using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManadger.Commands
{
    public abstract class AsyncCommand : ICommand, INotifyPropertyChanged
    {
        private bool _isExecuting;
        private CancellationTokenSource _cts;
        private readonly EventHandler _requerySuggestedHandler;

        public AsyncCommand()
        {
            _requerySuggestedHandler = (o, e) => RaiseCanExecuteChanged();
            CommandManager.RequerySuggested += _requerySuggestedHandler;
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (_isExecuting != value)
                {
                    _isExecuting = value;
                    RaisePropertyChanged();
                    RaiseCanExecuteChanged();
                }
            }
        }
        public TimeSpan? Timeout
        {
            get;
            set;
        }

        bool ICommand.CanExecute(object parameter) => !IsExecuting && CanExecuteCore();

        public async Task ExecuteAsync()
        {
            var command = this as ICommand;
            if (!command.CanExecute(null))
                return;

            IsExecuting = true;

            _cts = new CancellationTokenSource();
            if (Timeout != null)
                _cts.CancelAfter(Timeout.Value);

            try
            {
                await ExecuteCoreAsync(_cts.Token);
            }
            catch (OperationCanceledException)
                when (_cts.IsCancellationRequested)
            {
                ExecutionCancelled();
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
                IsExecuting = false;
            }
        }
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync();
        }
        public void Cancel() => _cts?.Cancel();
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected abstract Task ExecuteCoreAsync(CancellationToken cancellationToken);
        protected abstract bool CanExecuteCore();

        protected virtual void ExecutionCancelled() { }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null, bool allProperties = false)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        }
    }
}
