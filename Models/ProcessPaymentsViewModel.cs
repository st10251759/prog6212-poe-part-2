using System.Collections.Generic;

namespace ST10251759_PROG6212_POE.Models
{
    public class ProcessPaymentsViewModel
    {
        public List<Claim> Claims { get; set; }
        public int TotalClaims { get; set; }
        public decimal TotalAmountToPay { get; set; }
        public int PendingPayments { get; set; }
    }
}
