using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EngApp.Models;

namespace EngApp
{
    public class DeleteICommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
        public async void Execute(object parameter)
        {
            string name = parameter as string;
            await Deck.Delete(name);
            ToastPrompt toast = new ToastPrompt();
            toast.Message = "deck " + name + " is deleted";
            toast.FontSize = 24;
            toast.Show();
        }
        public event EventHandler<CommandEventArgs> Notify;
    }
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
