using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManadger.Commands
{
    public class AsyncRelayCommand : AsyncCommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Func<CancellationToken, Task> _execute;

        public AsyncRelayCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null) : this((c) => execute(), canExecute)
        {

        }

        protected override bool CanExecuteCore() => _canExecute == null || _canExecute();
        protected override Task ExecuteCoreAsync(CancellationToken cancellationToken) => _execute(cancellationToken);
    }
}
