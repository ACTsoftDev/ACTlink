using System;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class MenuViewItem : MvxViewModel
    {
        /// <summary>
        /// Gets or sets the type of the view model.
        /// </summary>
        /// <value>The type of the view model.</value>
        public Type ViewModelType { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        public string DefaultImage { get; set; }
        public string SelectedImage { get; set; }
        private bool _showAsHighlated;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:actchargers.MenuViewItem"/> show as highlated.
        /// </summary>
        /// <value><c>true</c> if show as highlated; otherwise, <c>false</c>.</value>
        public bool ShowAsHighlated
        {
            get { return _showAsHighlated; }
            set
            {
                _showAsHighlated = value;
                RaisePropertyChanged(() => ShowAsHighlated);
            }
        }

        private string _image;

        public string Image
        {
            get { return _image = IsSelected ? SelectedImage : DefaultImage; }
            set
            {
                _image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        private bool _isSelected;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:actchargers.MenuViewItem"/> is selected.
        /// </summary>
        /// <value><c>true</c> if is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                UpdateImage();
                RaisePropertyChanged(() => IsSelected);
            }
        }
        /// <summary>
        /// Updates the image.
        /// </summary>
        private void UpdateImage()
        {
            Image = IsSelected ? SelectedImage : DefaultImage;
        }

    }
}
