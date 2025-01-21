using PayarcSDK.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PayarcSDK.Services {
	public class ChargeService {
		private readonly ApiClient _apiClient;

		public ChargeService(ApiClient apiClient) {
			_apiClient = apiClient;
		}
	}
}
