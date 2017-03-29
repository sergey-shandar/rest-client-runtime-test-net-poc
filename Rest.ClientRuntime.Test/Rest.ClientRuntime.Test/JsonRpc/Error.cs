namespace Rest.ClientRuntime.Test.JsonRpc
{
    sealed class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; } 
    }
}
