namespace PayarcSDK.Exceptions {
    public class InvalidResponseFormatException : Exception {
        public InvalidResponseFormatException(string message, Exception innerException) : base(message, innerException) { }
    }
}