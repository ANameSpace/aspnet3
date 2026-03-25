namespace PSite3.Models.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Total { get; set; }
        public int TotalItems => Items.Sum(i => i.Quantity);
    }
}
