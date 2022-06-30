namespace actchargers.Code.Helpers.Login
{
    public static class LoginDataValidator
    {
        public static bool ShowValidationErrorIfExist(string emailId, string password)
        {
            string validationMessage = null;

            if (password.Length == 0 && emailId.Length == 0)
            {
                validationMessage = "Please Enter correct Email ID and Password";
            }
            else if (emailId.Length == 0 || !ValidationUtility.IsValidEmail(emailId.Trim()))
            {
                validationMessage = "Please Enter Valid Email ID";
            }
            else if (password.Length == 0)
            {
                validationMessage = "Please Enter Correct Password";
            }


            // Show user alert message for validation
            if (validationMessage != null)
            {
                ACUserDialogs.ShowAlert(validationMessage);

                return true;
            }

            return false;
        }
    }
}
