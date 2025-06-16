using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAITextEmbeddingGeneration(
    "embedding-model",
    configuration["Endpoint"]!,
    configuration["ApiKey"]!);

var kernel = builder.Build();

Console.WriteLine("Enter first text:");
var text1 = Console.ReadLine() ?? string.Empty;
Console.WriteLine("Enter second text:");
var text2 = Console.ReadLine() ?? string.Empty;

var service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
var embedding1 = await service.GenerateEmbeddingAsync(text1, new());
var embedding2 = await service.GenerateEmbeddingAsync(text2, new());

static double CosineSimilarity(IReadOnlyList<float> v1, IReadOnlyList<float> v2)
{
    if (v1.Count != v2.Count) throw new ArgumentException("Different vector size");
    double dot = 0, mag1 = 0, mag2 = 0;
    for (int i = 0; i < v1.Count; i++)
    {
        dot += v1[i] * v2[i];
        mag1 += v1[i] * v1[i];
        mag2 += v2[i] * v2[i];
    }
    return dot / Math.Sqrt(mag1 * mag2);
}

var score = CosineSimilarity(embedding1, embedding2);
Console.WriteLine($"Similarity: {score:F3}");
