# LearnSemanticKernelEmbedding

This sample shows how to generate embeddings with [Semantic Kernel](https://github.com/microsoft/semantic-kernel) using a local [Ollama](https://ollama.com/) endpoint.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Ollama installed and running locally

Fetch the embedding model before running the sample:

```bash
ollama pull nomic-embed-text
```

## Configuration

Configure the kernel with the Ollama endpoint and model ID. Add the following values to your `appsettings.json` or user secrets:

```json
{
  "Endpoint": "http://localhost:11434",
  "ModelId": "nomic-embed-text"
}
```

If you are using user secrets, you can set them with:

```bash
dotnet user-secrets set Endpoint http://localhost:11434
dotnet user-secrets set ModelId nomic-embed-text
```

## Running the sample

Navigate to this project folder and execute:

```bash
dotnet run
```

The sample will connect to your local Ollama instance using the settings above.
