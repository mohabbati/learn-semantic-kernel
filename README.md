# LearnSemanticKernel

This repository hosts a small console application that demonstrates how to build an agent with the [Semantic Kernel](https://github.com/microsoft/semantic-kernel) SDK. The sample agent can answer chat prompts and manage files via the included `FilePlugin`.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- Azure OpenAI endpoint and API key

## Configuration

Store your Azure OpenAI settings using [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
dotnet user-secrets set "Endpoint" "<your-endpoint>"
dotnet user-secrets set "ApiKey" "<your-api-key>"
```

## Running the sample

From the repository root run:

```bash
dotnet run --project LearnSemanticKernel
```

The agent will prompt for input and can create or list files under the `c:\sk-temp` directory.
