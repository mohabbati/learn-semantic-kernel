# LearnSemanticKernelEmbedding

A minimal example showing how to generate embeddings with Semantic Kernel.

The application reads an endpoint and API key from user secrets:

```
{
  "Endpoint": "https://your-endpoint.openai.azure.com/",
  "ApiKey": "<your-key>"
}
```

Run the program and enter two texts to see their cosine similarity.
