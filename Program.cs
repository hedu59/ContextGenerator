using GenerateContext.Services;

namespace GenerateContext
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("Digite o número do fluxo que deseja realizar:");
                Console.WriteLine("1 - Fluxo de Contexto");
                Console.WriteLine("2 - Fluxo de Testes");
                Console.WriteLine("3 - Help");
                Console.WriteLine("4 - Sair");
                Console.WriteLine("-----------------------------------------------------------------------");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        GenerateContextFlow();
                        break;

                    case "2":
                        GenerateTestFlow();
                        break;

                    case "3":
                        ShowHelp();
                        break;

                    case "4":
                        continuar = false;
                        break;

                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }

        private static void GenerateContextFlow()
        {
            var flowService = new ContextFlowService();
            flowService.GenerateContextFlow();
        }

        private static void GenerateTestFlow()
        {
            var testFlowService = new TestFlowService();
            testFlowService.GenerateTestFlow();
        }

        private static void ShowHelp()
        {
            Console.WriteLine("----------------------------------------------------------------------------------------");
            Console.WriteLine("1 - O Contexto representa a entidade ou context ao qual o UseCase pertence. Ex: Person, Proposal, Quote. \n");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            Console.WriteLine("2 - Ao digitar o UseCase, você tem as seguintes opções: \n");
            Console.WriteLine("GetPersonById -r (Criar sem o arquivo de Request, incluirá um Guid no local)");
            Console.WriteLine("GetPersonById -s (Criar sem o arquivo de Response, incluirá um Guid no local)");
            Console.WriteLine("GetPersonById --v (Criar COM o arquivo de Validator)\n");
            Console.WriteLine("As tags podem ser usadas em conjunto, por exemplo: GetPersonById -r -s --v");
            Console.WriteLine("----------------------------------------------------------------------------------------");
        }
    }
}