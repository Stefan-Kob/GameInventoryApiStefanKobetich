namespace GameInventoryApiStefanKobetich.Models
{
    // Model representing a transfer request between two wallets
    public class TransferReq
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public decimal Amount { get; set; }
    }
}