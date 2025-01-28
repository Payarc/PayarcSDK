using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Services
{
    public class CommonServices {
		public string BuildQueryString(Dictionary<string, object> parameters) {
			var queryString = string.Join("&",
				parameters.Select(p =>
					$"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString() ?? string.Empty)}"));
			return queryString;
		}
	}
}
