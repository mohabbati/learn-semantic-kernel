#region warning disable
//#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0110
#pragma warning disable SKEXP0050
#endregion

using LearnSemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel;
using System.Text;

Console.WriteLine("Hello, how can I assist you!");

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion("gpt-4o-mini", configuration.GetValue<string>("Endpoint")!, configuration.GetValue<string>("ApiKey")!);

builder.Services.AddSingleton<IAutoFunctionInvocationFilter, AutoInvocationFilter>();

var kernel = builder.Build();

var history = new ChatHistory();

kernel.ImportPluginFromType<TimePlugin>();

#region Plugin
var firstPlugin = new FilePlugin();
kernel.ImportPluginFromObject(firstPlugin);
#endregion

var agent = new ChatCompletionAgent
{
    Name = "MyAgent",
    Kernel = kernel,
    //Instructions = "You are a basic assistant. Response in German.",

    #region Plugin
    Instructions = $"Your name is SK Ninja. You are File Manager that can create and list files and folders. " +
                   $"When you create files and folder you need to give the full path based on this root folder: {firstPlugin.RootFolder}",

    Arguments = new KernelArguments(
        new AzureOpenAIPromptExecutionSettings
        {
            Temperature = 0.5,
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        })
    #endregion
};

Console.OutputEncoding = Encoding.UTF8;
while (true)
{
    Console.Write("You > ");
    var question = Console.ReadLine() ?? "";
    history.AddUserMessage(question);

    try
    {
        Console.Write("Assistant : ");

        await foreach (var response in agent.InvokeStreamingAsync(history))
        {
            foreach (var content in response.Content ?? "")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(content);
            }
        }
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Exception: " + e.Message);
    }
    finally
    {
        Console.ForegroundColor = ConsoleColor.White;
    }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
}