using GenerateContext.Extensions;

namespace GenerateContext.Services
{
    public class ServiceBase
    {
        public static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Diretório {directoryPath} criado com sucesso.\n");
            }
        }

        public static string FindDirectory(string basePath, string directoryName)
        {
            string[] projectDirectories = Directory.GetDirectories(basePath, "*", SearchOption.AllDirectories);
            foreach (string directory in projectDirectories)
            {
                Console.WriteLine(directory);
                Console.WriteLine("----------------------------------------------------------------------------------");
                if (directory.EndsWith(directoryName))
                {
                    return directory;
                }
            }
            throw new DirectoryNotFoundException($"Não foi possível encontrar o diretório {directoryName}.\n");
        }

        public static string FindDtoDirectory(string bordersDirectory)
        {
            string[] possibleDtoPaths = { "DTO", "Dtos" };
            foreach (string possiblePath in possibleDtoPaths)
            {
                string dtoDirectory = Path.Combine(bordersDirectory, possiblePath);
                if (Directory.Exists(dtoDirectory))
                {
                    return dtoDirectory;
                }
            }
            throw new DirectoryNotFoundException("Diretório DTO não encontrado dentro de Borders.\n");
        }

        public static void GenerateClass(string outputDirectory, string className, string basePathInformed)
        {
            string classContent = $"namespace {GetNamespace(outputDirectory, "", basePathInformed)}\n{{\n    public class {className}\n    {{\n        // Conteúdo da classe\n    }}\n}}";

            string directoryPath = outputDirectory;

            string outputPath = Path.Combine(directoryPath, $"{className}.cs");
            File.WriteAllText(outputPath, classContent);
        }

        public static string GetNamespace(string directory, string contexto = "", string basePathInformed = "", string search = "")
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory.RemoveSrc(search));
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string namespacePath = directoryInfo.FullName.Replace(basePath, "").Trim('\\');

            var response = string.Empty;
            response = string.IsNullOrEmpty(contexto) ? $"{namespacePath}".Replace("\\", ".") : $"{namespacePath}.{contexto}".Replace("\\", ".");
            return response;
        }
    }
}