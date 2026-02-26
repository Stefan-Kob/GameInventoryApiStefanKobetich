namespace GameInventoryApiStefanKobetich.Models
{
    public class TransferReq
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public decimal Amount { get; set; }
    }
}