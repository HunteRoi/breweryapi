namespace Business
{
    public class ClientSideError
    {
        public ErrorCodes Code { get; internal set; }
        
        public string Source { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }
    }
}
