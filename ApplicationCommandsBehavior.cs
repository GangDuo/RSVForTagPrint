using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RSVForTagPrint
{
    class ApplicationCommandsBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var CloseCommandBinding = new CommandBinding(
                ApplicationCommands.Close,
                CloseCommandExecuted,
                CloseCommandCanExecute);
            AssociatedObject.CommandBindings.Add(CloseCommandBinding);
        }

        private void CloseCommandExecuted(object target, ExecutedRoutedEventArgs e)
        {
            AssociatedObject.Close();
            e.Handled = true;
        }

        private void CloseCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
    }
}
