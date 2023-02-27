namespace MessageHandler.Quickstart.Contract
{
    public class Context
    {
        public string Id { get; set; }

        public string What { get; set; }

        public DateTime When { get; set; }

        public string Who { get; set; }

        public Context Why { get; set; }
    }
}