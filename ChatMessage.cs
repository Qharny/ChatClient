using System;
using Newtonsoft.Json;
using System.Collections.Generic; // Added for List<string>

namespace ChatClient
{
    /// <summary>
    /// Represents a chat message that can be sent between client and server
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Type of message (e.g., "chat", "private", "system", "userlist")
        /// </summary>
        [JsonProperty("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Username of the sender
        /// </summary>
        [JsonProperty("From")]
        public string From { get; set; }

        /// <summary>
        /// Username of the recipient (empty for broadcast messages)
        /// </summary>
        [JsonProperty("To")]
        public string To { get; set; }

        /// <summary>
        /// The actual message content
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }

        /// <summary>
        /// Timestamp when the message was sent
        /// </summary>
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatMessage()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public ChatMessage(string type, string from, string to, string message)
        {
            Type = type;
            From = from;
            To = to;
            Message = message;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Serialize the message to JSON
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Deserialize JSON to ChatMessage
        /// </summary>
        public static ChatMessage FromJson(string json)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<ChatMessage>(json);
                
                // Ensure Type is never null
                if (string.IsNullOrEmpty(message.Type))
                {
                    message.Type = "system";
                }
                
                return message;
            }
            catch (JsonException ex)
            {
                // If JSON parsing fails completely, create a system message with the raw text
                return new ChatMessage("system", "", "", $"Raw message: {json}");
            }
        }

        /// <summary>
        /// Get a formatted display string for the message
        /// </summary>
        public string GetDisplayText()
        {
            string type = Type ?? "system";
            
            if (type == "system")
            {
                return $"[{Timestamp:HH:mm:ss}] {Message}";
            }
            else if (type == "private")
            {
                return $"[{Timestamp:HH:mm:ss}] {From} -> {To}: {Message}";
            }
            else if (type == "userlist")
            {
                return $"[{Timestamp:HH:mm:ss}] User list updated";
            }
            else
            {
                return $"[{Timestamp:HH:mm:ss}] {From}: {Message}";
            }
        }

        /// <summary>
        /// Creates a user list message containing the list of online users
        /// </summary>
        public static ChatMessage CreateUserListMessage(List<string> usernames)
        {
            var userListJson = JsonConvert.SerializeObject(usernames);
            return new ChatMessage("UserList", "Server", "", userListJson);
        }
    }
} 