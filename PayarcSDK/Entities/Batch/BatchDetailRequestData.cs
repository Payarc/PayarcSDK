using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Batch {
	public class BatchDetailRequestData {
		public string? Merchant_Account_Number { get; init; }
		public string? Reference_Number { get; init; }
		public string? Date { get; init; }
	}
}
