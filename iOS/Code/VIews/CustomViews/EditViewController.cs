using System;
using Foundation;
using UIKit;

namespace actchargers.iOS
{
    public class EditViewController : BackViewController
    {
        public EditViewController() : base("EditViewController", null)
        {
        }

        protected EditViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            AddEditButton();
        }

        void AddEditButton()
        {
            UIBarButtonItem editButton = new UIBarButtonItem(UIBarButtonSystemItem.Edit, EditEventHandler);
            NavigationItem.RightBarButtonItem = editButton;
        }

        public virtual void EditEventHandler(object sender, EventArgs e)
        {
            UIBarButtonItem saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveEventHandler);
            NavigationItem.RightBarButtonItem = saveButton;

            UIBarButtonItem cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelEventHandler);
            NavigationItem.LeftBarButtonItem = cancelButton;
        }

        public virtual void SaveEventHandler(object sender, EventArgs e)
        {
            View.EndEditing(true);
        }

        public virtual void CancelEventHandler(object sender, EventArgs e)
        {
            View.EndEditing(true);
        }

        internal void CreateEditAndBackButton()
        {
            UIBarButtonItem editButton = new UIBarButtonItem(UIBarButtonSystemItem.Edit, EditEventHandler);
            NavigationItem.RightBarButtonItem = editButton;
            NavigationItem.LeftBarButtonItem = null;

            CreateBackButton();
        }
    }
}

