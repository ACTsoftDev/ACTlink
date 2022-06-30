using System.Collections.Generic;
using System.Drawing;

namespace actchargers
{
    public class VerifyControl
    {
        int countOfErrors;
        readonly List<string> errorList;

        public VerifyControl()
        {
            countOfErrors = 0;
            errorList = new List<string>();
        }

        public string GetErrorString()
        {
            if (countOfErrors > 0)
            {
                if (countOfErrors > 3)
                {
                    return AppResources.data;
                }
                else
                {
                    return string.Join(",", errorList);
                }
            }
            else
            {
                return AppResources.data;
            }
        }

        public bool HasErrors()
        {
            return (countOfErrors > 0);
        }

        public void VerifyTextBox
        (ListViewItem textBox, ListViewItem label, int min = 0, int max = 32000)
        {
            if (!textBox.IsEditable)
                return;

            textBox.Text = textBox.Text.Trim();
            if (textBox.Text.Length > max || textBox.Text.Length < min)
            {
                countOfErrors++;
                errorList.Add(textBox.Title);
                label.ForeColor = Color.Red;
            }
            else
            {
                label.ForeColor = Color.Black;
            }
        }

        public void VerifyFloatNumber
        (ListViewItem textBox, ListViewItem label, float min, float max)
        {
            if (!textBox.IsEditable)
                return;

            textBox.Text = textBox.Text.Trim();
            if (float.TryParse(textBox.Text, out float x) && x >= min && x <= max)
            {
                label.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(textBox.Title);
                countOfErrors++;
                label.ForeColor = Color.Red;
            }

        }

        public void InsertRemoveFault
        (bool insertFault, ListViewItem label, TableHeaderItem section)
        {
            if (insertFault)
            {
                errorList.Add(section.SectionHeader + " " + label.Title);
                countOfErrors++;
                label.ForeColor = Color.Red;
            }
            else
            {
                label.ForeColor = Color.Black;
            }
        }

        public void VerifyComboBox(ListViewItem combo, TableHeaderItem section)
        {
            if (!combo.IsEditable)
                return;

            if (!string.IsNullOrEmpty(combo.Text))
            {
                combo.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(section.SectionHeader + " " + combo.Title);
                countOfErrors++;
                combo.ForeColor = Color.Red;
            }
        }

        public void InsertRemoveFault(bool insertFault, ListViewItem label)
        {
            if (insertFault)
            {
                errorList.Add(label.Title);
                countOfErrors++;
                label.ForeColor = Color.Red;
            }
            else
            {
                label.ForeColor = Color.Black;
            }
        }

        public void VerifyInteger(ListViewItem textBox, ListViewItem label, int min, int max)
        {
            if (!textBox.IsEditable)
                return;

            textBox.Text = textBox.Text.Trim();
            if (int.TryParse(textBox.Text, out int x) && x >= min && x <= max)
            {
                label.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(textBox.Title);
                countOfErrors++;
                label.ForeColor = Color.Red;
            }
        }

        public void VerifyUInteger(ListViewItem textBox, ListViewItem label, uint min, uint max)
        {
            if (!textBox.IsEditable)
                return;

            textBox.Text = textBox.Text.Trim();
            if (uint.TryParse(textBox.Text, out uint x) && x >= min && x <= max)
            {
                label.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(textBox.Title);
                countOfErrors++;
                label.ForeColor = Color.Red;
            }
        }

        public void VerifyComboBox(ListViewItem combo)
        {
            if (!combo.IsEditable)
                return;

            if (!string.IsNullOrEmpty(combo.Text))
            {
                combo.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(combo.Title);
                countOfErrors++;
                combo.ForeColor = Color.Red;
            }
        }

        internal void VerifyComboBox
        (ListViewItem mCB_chargerTypeTOSAVE1, ListViewItem mCB_chargerTypeTOSAVE2)
        {
            if (!string.IsNullOrEmpty(mCB_chargerTypeTOSAVE1.Text))
            {
                mCB_chargerTypeTOSAVE1.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(mCB_chargerTypeTOSAVE1.Title);
                countOfErrors++;
                mCB_chargerTypeTOSAVE1.ForeColor = Color.Red;
            }
            if (!string.IsNullOrEmpty(mCB_chargerTypeTOSAVE2.Text))
            {
                mCB_chargerTypeTOSAVE1.ForeColor = Color.Black;
            }
            else
            {
                errorList.Add(mCB_chargerTypeTOSAVE2.Title);
                countOfErrors++;
                mCB_chargerTypeTOSAVE1.ForeColor = Color.Red;
            }
        }
    }
}
