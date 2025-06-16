using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Embeddings;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAITextEmbeddingGeneration("text-embedding-ada-002", configuration["Endpoint"]!, configuration["ApiKey"]!);
var kernel = builder.Build();

string text;
if (args.Length > 0 && File.Exists(args[0]))
{
    text = LoadTextFromFile(args[0]);
}
else
{
    Console.WriteLine("Enter text:");
    text = Console.ReadLine() ?? string.Empty;
}

var service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
var embedding = await service.GenerateEmbeddingAsync(text);
Console.WriteLine($"Embedding length: {embedding.Count}");

string LoadTextFromFile(string path)
{
    var ext = Path.GetExtension(path).ToLowerInvariant();
    if (ext == ".pdf")
    {
        var sb = new StringBuilder();
        using var pdf = PdfDocument.Open(path);
        foreach (var page in pdf.GetPages())
        {
            sb.AppendLine(page.Text);
        }
        return sb.ToString();
    }
    if (ext == ".xls" || ext == ".xlsx")
    {
        return LoadExcelText(path);
    }
    return File.ReadAllText(path);
}

string LoadExcelText(string path)
{
    var sb = new StringBuilder();
    using var document = SpreadsheetDocument.Open(path, false);
    var sharedStrings = document.WorkbookPart!.SharedStringTablePart?.SharedStringTable;
    foreach (var worksheet in document.WorkbookPart!.WorksheetParts)
    {
        foreach (var row in worksheet.Worksheet.Descendants<Row>())
        {
            foreach (var cell in row.Descendants<Cell>())
            {
                var value = cell.InnerText;
                if (cell.DataType?.Value == CellValues.SharedString && int.TryParse(value, out var idx))
                {
                    value = sharedStrings!.ElementAt(idx).InnerText;
                }
                sb.Append(value).Append(' ');
            }
        }
    }
    return sb.ToString();
}
