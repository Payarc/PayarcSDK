using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class TokenResponse : BaseResponse {
		[JsonProperty("object")]
		public override string Object { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("used")]
		public bool Used { get; set; }

		[JsonProperty("ip")]
		public string? Ip { get; set; }

		[JsonProperty("tokenization_method")]
		public string? TokenizationMethod { get; set; }

		[JsonProperty("created_at")]
		public long CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public long UpdatedAt { get; set; }

		[JsonProperty("card")]
		public CardWrapper Card { get; set; }
	}

	public class CardWrapper {
		[JsonProperty("data")]
		public Card Data { get; set; }
	}
}
