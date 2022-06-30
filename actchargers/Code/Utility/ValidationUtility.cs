using System;
using System.Text.RegularExpressions;

namespace actchargers
{
	public class ValidationUtility
	{
		internal static bool IsValidCharacters(string text)
		{
			return Regex.IsMatch(text, @"^[a-zA-Z]+$");
		}

		internal static bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
		}
	}
}
