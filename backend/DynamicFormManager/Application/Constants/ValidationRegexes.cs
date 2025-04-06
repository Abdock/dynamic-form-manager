using System.Text.RegularExpressions;

namespace Application.Constants;

public static partial class ValidationRegexes
{
    [GeneratedRegex("""^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#@'\"\-=!$%^&*\(\)\[\]\{\}`\\|\/\?><;:,])[A-Za-z\d#@'\"\-=!$%^&*\(\)\[\]\{\}`\\|\/\?><;:,]{8,}$""")]
    public static partial Regex PasswordRegex();
}