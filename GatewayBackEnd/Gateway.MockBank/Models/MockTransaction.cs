namespace Gateway.MockBank.Moodels
{
    public class MockTransaction
    {
        public decimal TransactionAmount { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string CardHolderName { get; set; }
        public string CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
    }
}