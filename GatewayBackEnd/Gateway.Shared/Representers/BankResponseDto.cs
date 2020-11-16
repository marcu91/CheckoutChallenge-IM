using System;

namespace Gateway.Shared.Representers
{
    public class BankResponseDto
    {
        public Guid BankResponseID { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
    }
}
