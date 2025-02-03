using System.Text.Json;

namespace PayarcSDK.Sample
{
	public class SecretManager {
		public static Dictionary<string, string> LoadTokens(string filePath) {
			if (!File.Exists(filePath)) {
				throw new FileNotFoundException($"The token file '{filePath}' does not exist.");
			}

			var jsonContent = File.ReadAllText(filePath);
			var tokens = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonContent);

			return tokens?["Tokens"] ?? throw new InvalidOperationException("Tokens section is missing.");
		}
		public static Dictionary<string, string> LoadSerials(string filePath) {
			if (!File.Exists(filePath)) {
				throw new FileNotFoundException($"The token file '{filePath}' does not exist.");
			}

			var jsonContent = File.ReadAllText(filePath);
			var tokens = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonContent);

			return tokens?["SerialNumbers"] ?? throw new InvalidOperationException("SerialNumbers section is missing.");
		}
	}
}
