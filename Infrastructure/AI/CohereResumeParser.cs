//  Infrastructure/AI/CohereResumeParser.cs
using Core.Interfaces;
using Core.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


namespace Infrastructure.AI
{
    public class CohereResumeParser : IResumeParser
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CohereResumeParser(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.cohere.ai/")
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Cohere-Version", "2022-12-06");
        }

        public async Task<Resume> ParseAsync(string plainTextCv)
            {
                var prompt = $@"
                Extract the following details from the resume below and respond in JSON format:
                - Full name
                - Email
                - Skills (as a list)
                - Experience (as a list of objects with: position, company, duration, responsibilities)

                Resume:
                {plainTextCv}

                Respond in this JSON format:
                {{
                ""name"": ""..."",
                ""email"": ""..."",
                ""skills"": [""..."", ""...""],
                ""experience"": [
                    {{
                    ""position"": ""..."",
                    ""company"": ""..."",
                    ""duration"": ""..."",
                    ""responsibilities"": [""..."", ""...""]
                    }}
                ]
                }}";

                    var requestBody = new
                    {
                        model = "command-r-plus",
                        prompt = prompt,
                        max_tokens = 1000,
                        temperature = 0.3
                    };

                    var response = await _httpClient.PostAsJsonAsync("v1/generate", requestBody);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadFromJsonAsync<CohereResponse>();

                    if (result?.generations != null && result.generations.Count > 0)
                    {
                        var rawOutput = result.generations[0].text.Trim();

                        // 🧼 Strip backticks and any markdown fencing like ```json
                        var cleanedJson = rawOutput
                            .Replace("```json", "")
                            .Replace("```", "")
                            .Trim();

                        try
                        {
                            var resume = JsonSerializer.Deserialize<Resume>(cleanedJson, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            return resume ?? new Resume();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("⚠️ Failed to parse cleaned JSON response:");
                            Console.WriteLine(cleanedJson);
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }

                    return new Resume(); // fallback
            }


        private class CohereResponse
        {
            public List<Generation> generations { get; set; }

            public class Generation
            {
                public string text { get; set; }
            }
        }
    }
}
