using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string DeepSeekApiUrl = "https://api.deepseek.com/v1/chat/completions";
    private const string ApiKey = "sk-3b8842c4b8de41b48ad350662886e849"; // Replace with your actual DeepSeek API key

    static async Task Main(string[] args)
    {
        Console.WriteLine("DeepSeek English to Persian Translator");
        Console.WriteLine("--------------------------------------");

        while (true)
        {
            Console.Write("\nEnter English text to translate (or 'exit' to quit): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    string translatedText = await TranslateEnglishToPersian(input);
                    Console.WriteLine($"\nTranslation (Persian): {translatedText}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
            }
        }

        Console.WriteLine("\nTranslation session ended. Press any key to exit...");
        Console.ReadKey();
    }

    private static async Task<string> TranslateEnglishToPersian(string englishText)
    {
        // Prepare the request payload
        var requestData = new
        {
            model = "deepseek-chat",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are a professional translator. Translate the following English text to Persian (Farsi). " +
                              "Provide only the translation, no additional explanations or notes."
                },
                new
                {
                    role = "user",
                    content = englishText
                }
            },
            temperature = 0.3
        };

        string jsonPayload = JsonSerializer.Serialize(requestData);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Add authorization header
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

        // Send the request
        HttpResponseMessage response = await client.PostAsync(DeepSeekApiUrl, content);
        response.EnsureSuccessStatusCode();

        // Parse the response
        string responseBody = await response.Content.ReadAsStringAsync();
        using JsonDocument document = JsonDocument.Parse(responseBody);
        JsonElement root = document.RootElement;
        string translatedText = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

        return translatedText.Trim();
    }
}