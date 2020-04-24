using CodeMash.Models;


namespace HrApp
{
    public class WishlistSummary
    {
        [Field("tpye")]
        public string Type { get; set; }
        [Field("total")]
        public double Total { get; set; }
    }
}
