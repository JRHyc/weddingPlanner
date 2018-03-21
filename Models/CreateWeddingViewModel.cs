using System;
using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models
{
    public class CreateWeddingViewModel : BaseEntity
    {
        [Required]
        public string WedderOne {get;set;}
        [Required]
        public string WedderTwo {get;set;}
        [Required]
        [FutureDate]
        public DateTime WeddingDate {get;set;}
        [Required]
        public string Address {get;set;}
    }
}