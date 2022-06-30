using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace actchargers.iOS
{
    /// <summary>
    /// Rounded rect button.
    /// </summary>
    [Register("RoundedRectButton")]
    public partial class RoundedRectButton : UIButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.RoundedRectButton"/> class.
        /// </summary>
        /// <param name="h">The height.</param>
        public RoundedRectButton(IntPtr h) : base(h)
        {
            BackgroundColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            SetTitleColor(UIColorUtility.FromHex(ACColors.WHITE_COLOR), UIControlState.Normal);
            Layer.CornerRadius = (nfloat)5.0;
            TitleLabel.Font = UIFont.SystemFontOfSize(20, UIFontWeight.Regular);
            TitleLabel.AdjustsFontSizeToFitWidth = true;
            TitleLabel.Lines = 1;
            TitleLabel.LineBreakMode = UILineBreakMode.Clip;

            /// <summary>
            /// ClipsToBounds
            /// </summary>
            ClipsToBounds = true;
            Layer.MasksToBounds = false;
        }
    }

    /// <summary>
    /// Days rounded rect button.
    /// </summary>
    [Register("DaysRoundedRectButton")]
    public partial class DaysRoundedRectButton : RoundedRectButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.RoundedRectButton"/> class.
        /// </summary>
        /// <param name="h">The height.</param>
        public DaysRoundedRectButton(IntPtr h) : base(h)
        {
            TitleLabel.Font = UIFont.SystemFontOfSize(14, UIFontWeight.Regular);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if(value)
                {
                    BackgroundColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
                    SetTitleColor(UIColorUtility.FromHex(ACColors.WHITE_COLOR), UIControlState.Normal);
                    Layer.BorderWidth = 0;

                }
                else{
                    BackgroundColor = UIColorUtility.FromHex(ACColors.WHITE_COLOR);
                    SetTitleColor(UIColorUtility.FromHex(ACColors.BLACK_COLOR), UIControlState.Normal);
                    Layer.BorderWidth = 1;
                    Layer.BorderColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR).CGColor;
                }
            }
        }
    }

    [Register("CalibrationRoundedRectButton")]
    public partial class CalibrationRoundedRectButton : RoundedRectButton
    {
        public CalibrationRoundedRectButton(IntPtr h) : base(h)
        {
            BackgroundColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
        }

        private bool _isEditable;
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                _isEditable = value;
                if (value)
                {
                    this.Alpha = (float)1.0;
                }
                else
                {
                    this.Alpha = (float)0.5;
                }
                this.Enabled = value;
            }
        }
    }
}