using System;
using System.Collections.Specialized;
using System.Windows.Input;
using UIKit;

namespace actchargers.iOS
{
    // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
    // are preserved in the deployed app
    public class LinkerPleaseInclude
    {
        //Added For ScanBarButton Enabling
        private void IncludeEnabled(UIBarButtonItem barButton)
        {
            barButton.Enabled = !barButton.Enabled;
        }

        //Added For Textfield BeginEditing and EndEditing
        public void Include(UITextField textField)
        {
            textField.EditingDidBegin += (sender, args) =>
            {
            };

            textField.EditingDidEnd += (sender, e) =>
            {
            };
        }

        //Added For Binding the Progress
        public void Include(UIProgressView progressView)
        {
            progressView.Progress = progressView.Progress;
        }


        public void Include(UITextView textView)
        {
            textView.Editable = textView.Editable;
            textView.Ended += (sender, e) =>
            {
            };
            textView.Started += (sender, e) =>
            {
            };
        }

        public void Include(UISearchBar searchBar)
        {
            searchBar.Text = "";
            searchBar.Placeholder = "";
            searchBar.SearchButtonClicked += (sender, e) =>
            {
            };
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) =>
            {

            };
        }

        //Added For Button Commadparam binding
        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) =>
            {
                if (command.CanExecute(null))
                    command.Execute(null);
            };
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) =>
            {
                sw.On = false;
            };
        }
    }
}