namespace SafeVault.Utils;

public static class InputValidator
{
    // Simple integer ID validator
    public static bool IsValidId(int id) => id > 0 && id < 1_000_000;

    // Add other validators as needed (length checks, allowed characters, etc.)
    public static bool IsSafeForDisplay(string input, int maxLen = 200)
    {
        if (string.IsNullOrEmpty(input)) return true;
        return input.Length <= maxLen;
    }
}
