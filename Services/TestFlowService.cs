using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateContext.Services
{
    public class TestFlowService : ServiceBase
    {
        public void GenerateTestFlow()
        {
            Console.WriteLine("Digite o caminho base do projeto de testes:");
            string basePath = Console.ReadLine();

            string useCasesDirectory = FindDirectory(basePath, "UseCases");
            string buildersDirectory = FindDirectory(basePath, "Builders");
            Console.WriteLine("Digite o contexto:");
            string contexto = Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("Digite o nome do caso de uso:");
            string casoDeUso = Console.ReadLine();

            bool generateRequestBuilder = true;
            bool generateResponseBuilder = true;

            // Verificar se deve gerar o builder do request
            if (casoDeUso.Contains("-t"))
            {
                generateRequestBuilder = false;
                casoDeUso = casoDeUso.Replace("-t", "").Trim();
            }

            // Verificar se deve gerar o builder do response
            if (casoDeUso.Contains("-s"))
            {
                generateResponseBuilder = false;
                casoDeUso = casoDeUso.Replace("-s", "").Trim();
            }

            GenerateBuilderFile(buildersDirectory, contexto, casoDeUso, generateRequestBuilder, generateResponseBuilder);
            GenerateUseCaseTestFile(useCasesDirectory, contexto, casoDeUso);
        }

        private static void GenerateBuilderFile(string buildersDirectory, string contexto, string casoDeUso, bool generateRequestBuilder, bool generateResponseBuilder)
        {
            string nameSpace = $"namespace {GetNamespace(buildersDirectory, contexto, "", "test")}\n";
            string requestBuilderClassName = $"{casoDeUso.TrimEnd()}RequestBuilder";
            string responseBuilderClassName = $"{casoDeUso.TrimEnd()}ResponseBuilder";

            // Verificar se deve criar o builder do request
            if (generateRequestBuilder)
            {
                string requestBuilderContent = $"{nameSpace}{{\n    public class {requestBuilderClassName}\n    {{\n        private {casoDeUso}Request _instance;\n        private static readonly Faker _faker = FakerPtBr.CreateFaker();\n\n        public {requestBuilderClassName}()\n        {{\n        }}\n\n        public {casoDeUso}Request Build()\n        {{\n            return _instance;\n        }}\n    }}\n}}";

                string requestDirectoryPath = Path.Combine(buildersDirectory, contexto);
                EnsureDirectoryExists(requestDirectoryPath);

                string requestOutputPath = Path.Combine(requestDirectoryPath, $"{requestBuilderClassName}.cs");
                File.WriteAllText(requestOutputPath, requestBuilderContent);

                Console.WriteLine($"Arquivo {requestBuilderClassName}.cs criado com sucesso em {requestDirectoryPath}");
                Console.WriteLine("----------------------------------------------------------------------------------");
            }

            // Verificar se deve criar o builder do response
            if (generateResponseBuilder)
            {
                string responseBuilderContent = $"{nameSpace}{{\n    public class {responseBuilderClassName}\n    {{\n        private {casoDeUso}Response _instance;\n        private static readonly Faker _faker = FakerPtBr.CreateFaker();\n\n        public {responseBuilderClassName}()\n        {{\n        }}\n\n        public {casoDeUso}Response Build()\n        {{\n            return _instance;\n        }}\n    }}\n}}";

                string responseDirectoryPath = Path.Combine(buildersDirectory, contexto);
                EnsureDirectoryExists(responseDirectoryPath);

                string responseOutputPath = Path.Combine(responseDirectoryPath, $"{responseBuilderClassName}.cs");
                File.WriteAllText(responseOutputPath, responseBuilderContent);

                Console.WriteLine($"Arquivo {responseBuilderClassName}.cs criado com sucesso em {responseDirectoryPath}");
                Console.WriteLine("----------------------------------------------------------------------------------");
            }
        }

        private static void GenerateUseCaseTestFile(string useCasesDirectory, string contexto, string casoDeUso)
        {
            string nameSpace = $"namespace {GetNamespace(useCasesDirectory, contexto, "", "test")}";
            string useCaseTestClassName = $"{casoDeUso}UseCaseTest";

            string useCaseTestContent = $"{nameSpace}\n{{\n    public class {useCaseTestClassName}\n    {{\n        public {useCaseTestClassName}()\n        {{\n        }}\n    }}\n}}";

            string testDirectoryPath = Path.Combine(useCasesDirectory, contexto);
            EnsureDirectoryExists(testDirectoryPath);

            string testOutputPath = Path.Combine(testDirectoryPath, $"{useCaseTestClassName}.cs");
            File.WriteAllText(testOutputPath, useCaseTestContent);

            Console.WriteLine($"Arquivo {useCaseTestClassName}.cs criado com sucesso em {testDirectoryPath}");
            Console.WriteLine("----------------------------------------------------------------------------------");
        }
    }
}