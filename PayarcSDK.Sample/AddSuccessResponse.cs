using Newtonsoft.Json;
using System.Text.Json;

namespace PayarcSDK.Sample {
	public class AddSuccessResponse {
		public ResponseContent response_content { get; set; }
		public List<Permission> permissions { get; set; }
		public List<Menu> menu { get; set; }
		public User user { get; set; }
		public string current_account_id { get; set; }
		public string user_id { get; set; }
		public bool login_as_merchant { get; set; }
	}

	public class Menu {
		public string name { get; set; }
		public string routes { get; set; }
		public string routerLink { get; set; }
		public List<SubMenu> subMenu { get; set; }
	}

	public class Permission {
		public string permission_name { get; set; }
		public string type { get; set; }
		public string id { get; set; }
	}

	public class Pivot {
		public int model_id { get; set; }
		public int role_id { get; set; }
		public string model_type { get; set; }
	}

	public class ResponseContent {
		public string token_type { get; set; }
		public int expires_in { get; set; }
		public string access_token { get; set; }
	}

	public class Role {
		public int id { get; set; }
		public object team_id { get; set; }
		public string name { get; set; }
		public string guard_name { get; set; }
		public string display_name { get; set; }
		public string description { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
		public int level { get; set; }
		public string role_type { get; set; }
		public object account_id { get; set; }
		public object merchant_id { get; set; }
		public int allow_sensitive_data { get; set; }
		public string name_export { get; set; }
		public string display_name_export { get; set; }
		public string created_at_export { get; set; }
		public string role_id_export { get; set; }
		public string description_export { get; set; }
		public Pivot pivot { get; set; }
	}

	public class SubMenu {
		public string name { get; set; }
		public string routes { get; set; }
		public string routerLink { get; set; }
	}

	public class User {
		public int id { get; set; }
		public string name { get; set; }
		public string email { get; set; }
		public bool confirmed { get; set; }
		public object gender { get; set; }
		public object birth { get; set; }
		public object device { get; set; }
		public object platform { get; set; }
		public bool is_client { get; set; }
		public object timezone { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
		public object deleted_at { get; set; }
		public bool is_owner { get; set; }
		public object last_login_at { get; set; }
		public object ip_address { get; set; }
		public object mobile_number { get; set; }
		public string username { get; set; }
		public int is_agent { get; set; }
		public int is_sub_agent { get; set; }
		public object agent_id { get; set; }
		public object notification_phone_number { get; set; }
		public int is_locked { get; set; }
		public object lock_reason { get; set; }
		public string is_2fa_phone_verified { get; set; }
		public string is_2fa_email_verified { get; set; }
		public string preferred_2fa_method { get; set; }
		public int is_manager { get; set; }
		public object first_name { get; set; }
		public object middle_name { get; set; }
		public object last_name { get; set; }
		public int team_id { get; set; }

		[JsonProperty("2fa_secret")]
		public string _2fa_secret { get; set; }
		public bool is_payee { get; set; }
		public object storagePit { get; set; }
		public object agent_dba_name { get; set; }
		public List<Role> roles { get; set; }
	}
}
