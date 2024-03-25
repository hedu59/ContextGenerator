namespace GenerateContext.Services
{
    public class ContextFlowService : ServiceBase
    {
        public void GenerateContextFlow()
        {
            Console.WriteLine("Digite o caminho base do projeto (O caminho até a pasta src). Ex: C:\\TEMP\\src");
            string basePath = Console.ReadLine();

            string bordersDirectory = FindDirectory(basePath, "Borders");
            string useCasesDirectory = FindDirectory(basePath, "UseCases");

            Console.WriteLine("Digite o contexto:");
            string contexto = Console.ReadLine();

            Console.WriteLine("Digite o nome do caso de uso (Não usar a terminologia UseCase). Ex: GetPersonById");
            string casoDeUso = Console.ReadLine();

            bool generateRequest = true;
            bool generateResponse = true;
            bool generateValidator = false;

            if (casoDeUso.Contains("-r", StringComparison.CurrentCultureIgnoreCase))
            {
                generateRequest = false;
                casoDeUso = casoDeUso.Replace("-r", "").Trim();
            }

            if (casoDeUso.Contains("-s", StringComparison.CurrentCultureIgnoreCase))
            {
                generateResponse = false;
                casoDeUso = casoDeUso.Replace("-s", "").Trim();
            }

            if (casoDeUso.Contains("--v", StringComparison.CurrentCultureIgnoreCase))
            {
                generateValidator = true;
                casoDeUso = casoDeUso.Replace("--v", "").Trim();
            }

            string dtoDirectory = FindDtoDirectory(bordersDirectory);
            dtoDirectory = Path.Combine(dtoDirectory, contexto);
            EnsureDirectoryExists(dtoDirectory); // Verifica se o diretório existe, senão cria

            if (generateRequest)
            {
                GenerateClass(dtoDirectory, $"{casoDeUso}Request", basePath);
            }

            if (generateResponse)
            {
                GenerateClass(dtoDirectory, $"{casoDeUso}Response", basePath);
            }

            ///GenerateUseCaseClass(bordersDirectory, useCasesDirectory, casoDeUso, contexto, basePath);
            GenerateUseCaseClass(bordersDirectory, useCasesDirectory, casoDeUso, contexto, basePath, generateRequest, generateResponse, generateValidator);

            Console.WriteLine($"Scaffolding para o Caso de Uso {casoDeUso} no Contexto {contexto} gerado com sucesso.\n");
            Console.ReadLine();
        }

        private static void GenerateUseCaseClass(string bordersDirectory, string useCaseDirectory, string casoDeUso, string contexto, string basePathInformed, bool generateRequest, bool generateResponse, bool generateValidator)
        {
            string useCaseClassName = $"{casoDeUso}UseCase";
            string useCaseContent;

            useCaseContent = GenerateUseCaseContent(useCaseDirectory, casoDeUso, contexto, basePathInformed, generateRequest, generateResponse, useCaseClassName);

            string directoryPath = Path.Combine(useCaseDirectory, contexto);
            EnsureDirectoryExists(directoryPath);

            string outputPath = Path.Combine(directoryPath, $"{useCaseClassName}.cs");
            File.WriteAllText(outputPath, useCaseContent);

            Console.WriteLine($"Classe {useCaseClassName} gerada com sucesso em {directoryPath}.\n");

            // Criação da interface
            GenerateInterfaceUseCase(bordersDirectory, useCaseDirectory, casoDeUso, contexto, basePathInformed, generateRequest, generateResponse);

            // Criação do Validator
            if (generateValidator)
            {
                GenerateValidator(bordersDirectory, casoDeUso, contexto, generateRequest, useCaseClassName);
            }
        }

        private static void GenerateValidator(string useCaseDirectory, string casoDeUso, string contexto, bool generateRequest, string useCaseClassName)
        {
            string validatorClassName = $"{casoDeUso}RequestValidator";
            string validatorContent = GenerateValidatorContent(useCaseDirectory, casoDeUso, contexto, generateRequest, useCaseClassName);

            string validatorDirectory = Path.Combine(useCaseDirectory, "Validators", contexto);
            EnsureDirectoryExists(validatorDirectory);

            string validatorOutputPath = Path.Combine(validatorDirectory, $"{validatorClassName}.cs");
            File.WriteAllText(validatorOutputPath, validatorContent);

            Console.WriteLine($"Validator {validatorClassName} gerado com sucesso em {validatorDirectory}\n");
        }

        private static string GenerateValidatorContent(string bordersDirectory, string casoDeUso, string contexto, bool generateRequest, string useCaseClassName)
        {
            string requestTypeName = generateRequest ? $"{casoDeUso}Request" : "Guid";
            string validatorClassName = $"{casoDeUso}RequestValidator";

            return $"namespace {GetNamespace(bordersDirectory, $"Validators.{contexto}", "")}\n{{\n    public class {validatorClassName} : AbstractValidator<{requestTypeName}>\n    {{\n        public {validatorClassName}()\n        {{\n            // Implementação do Validator\n        }}\n    }}\n}}";
        }

        private static void GenerateInterfaceUseCase(string bordersDirectory, string useCaseDirectory, string casoDeUso, string context, string basePathInformed, bool generateRequest, bool generateResponse)
        {
            string interfaceName = $"I{casoDeUso}UseCase";
            string interfaceContent;

            interfaceContent = GenerateInterfaceUseCaseContent(useCaseDirectory, casoDeUso, context, basePathInformed, generateRequest, generateResponse, interfaceName);

            string interfaceDirectory = Path.Combine($"{bordersDirectory}\\UseCases", context);
            EnsureDirectoryExists(interfaceDirectory);

            string interfaceOutputPath = Path.Combine(interfaceDirectory, $"{interfaceName}.cs");
            File.WriteAllText(interfaceOutputPath, interfaceContent);

            Console.WriteLine($"Interface {interfaceName} gerada com sucesso em {interfaceDirectory}.\n");
        }

        private static string GenerateInterfaceUseCaseContent(string useCaseDirectory, string casoDeUso, string context, string basePathInformed, bool generateRequest, bool generateResponse, string interfaceName)
        {
            string interfaceContent;
            if (generateRequest && generateResponse)
            {
                interfaceContent = $"namespace {GetNamespace(useCaseDirectory, context, basePathInformed)}\n{{\n    public interface {interfaceName} : IUseCase<{casoDeUso}Request, {casoDeUso}Response>\n    {{\n    }}\n}}";
            }
            else if (generateRequest)
            {
                interfaceContent = $"namespace {GetNamespace(useCaseDirectory, context, basePathInformed)}\n{{\n    public interface {interfaceName} : IUseCase<{casoDeUso}Request, Guid>\n    {{\n    }}\n}}";
            }
            else if (generateResponse)
            {
                interfaceContent = $"namespace {GetNamespace(useCaseDirectory, context, basePathInformed)}\n{{\n    public interface {interfaceName} : IUseCase<Guid, {casoDeUso}Response>\n    {{\n    }}\n}}";
            }
            else
            {
                interfaceContent = $"namespace {GetNamespace(useCaseDirectory, context, basePathInformed)}\n{{\n    public interface {interfaceName} : IUseCase<Guid, Guid>\n    {{\n    }}\n}}";
            }

            return interfaceContent;
        }

        private static string GenerateUseCaseContent(string useCaseDirectory, string casoDeUso, string contexto, string basePathInformed, bool generateRequest, bool generateResponse, string useCaseClassName)
        {
            string useCaseContent;
            if (generateRequest && generateResponse)
            {
                useCaseContent = $"namespace {GetNamespace(useCaseDirectory, contexto, basePathInformed)}\n{{\n    public class {useCaseClassName} : UseCase<{casoDeUso}Request, {casoDeUso}Response>, I{casoDeUso}UseCase\n    {{\n        // Implementação do Use Case\n    }}\n}}";
            }
            else if (generateRequest)
            {
                useCaseContent = $"namespace {GetNamespace(useCaseDirectory, contexto, basePathInformed)}\n{{\n    public class {useCaseClassName} : UseCase<{casoDeUso}Request, Guid>, I{casoDeUso}UseCase\n    {{\n        // Implementação do Use Case\n    }}\n}}";
            }
            else if (generateResponse)
            {
                useCaseContent = $"namespace {GetNamespace(useCaseDirectory, contexto, basePathInformed)}\n{{\n    public class {useCaseClassName} : UseCase<Guid, {casoDeUso}Response>, I{casoDeUso}UseCase\n    {{\n        // Implementação do Use Case\n    }}\n}}";
            }
            else
            {
                useCaseContent = $"namespace {GetNamespace(useCaseDirectory, contexto, basePathInformed)}\n{{\n    public class {useCaseClassName} : UseCase<Guid, Guid>, I{casoDeUso}UseCase\n    {{\n        // Implementação do Use Case\n    }}\n}}";
            }

            return useCaseContent;
        }
    }
}