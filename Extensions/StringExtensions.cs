namespace GenerateContext.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSrc(this string text, string search)
        {
            // Encontra a posição da string 'src' no texto
            if (string.IsNullOrEmpty(search))
                search = "src";
            int posSrc = text.IndexOf(search);

            // Se 'src' não for encontrado, retorna o texto original
            if (posSrc == -1)
            {
                return text;
            }

            if (search == "src")
            {
                return text.Substring(posSrc + 4).TrimEnd('.');
            }
            else
            {
                return text.Substring(posSrc + 5).TrimEnd('.');
            }
        }
    }
}