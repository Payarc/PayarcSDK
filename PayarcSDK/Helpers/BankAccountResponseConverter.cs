using PayarcSDK.Entities;

namespace PayarcSDK.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
public class BankAccountResponseConverter : JsonConverter<BankAccountResponse?>
{
    public override BankAccountResponse? ReadJson(JsonReader reader, Type objectType, BankAccountResponse? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Parse the JSON token
        var token = JToken.Load(reader);
        
        // Check if the token is an array, and if it's empty, treat it as an empty object
        if (token.Type == JTokenType.Array && !token.HasValues)
        {
            return null; // or you can return a default BankAccountResponse() if needed
        }

        // Otherwise, deserialize the token as a normal object
        return token.ToObject<BankAccountResponse?>();
    }

    public override void WriteJson(JsonWriter writer, BankAccountResponse? value, JsonSerializer serializer)
    {
        // This is where you handle the serialization logic (we can leave this as the default)
        serializer.Serialize(writer, value);
    }
}