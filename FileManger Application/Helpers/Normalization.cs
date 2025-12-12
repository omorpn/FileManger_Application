namespace FileManger_Application.Helpers
{
    public static class Normalization
    {
        public static string NormalizeName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            name = name.Trim().ToLower();
            if (!name.Contains(' '))
            {
                return char.ToUpper(name[0]) + name.Substring(1);
            }
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);

            }
            return string.Join(" ", parts);
        }
        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return email;
            return email.Trim().ToLower();

        }
    }
}
