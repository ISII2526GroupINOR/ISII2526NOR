﻿namespace AppForSEII2526.API.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public IList<Plan>? Plans { get; set; }
        public List<Purchase>? Purchases { get; set; }
        public ApplicationUser? User { get; set; }

    }
}
