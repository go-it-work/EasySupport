using System;

partial class ElasticSearchAPI
{
    public class Testing
    {
        public string Description;
        public DateTime Timestamp;

        public Testing(string Description, DateTime Timestamp)
        {
            this.Description = Description;
            this.Timestamp = Timestamp;
        }
    }
}

