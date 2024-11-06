// Generation password for eset account
public static class PassGen
{
    public static string GeneratePassword(int length)
    {
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string digitChars = "0123456789";
        const string allChars = uppercaseChars + lowercaseChars + digitChars;

        var random = new Random();
        var passwordChars = new char[length];
        passwordChars[0] = uppercaseChars[random.Next(uppercaseChars.Length)];
        passwordChars[1] = lowercaseChars[random.Next(lowercaseChars.Length)];
        passwordChars[2] = digitChars[random.Next(digitChars.Length)];

        for (int i = 3; i < length; i++)
        {
            passwordChars[i] = allChars[random.Next(allChars.Length)];
        }

        return new string(passwordChars.OrderBy(_ => random.Next()).ToArray());
    }
}
