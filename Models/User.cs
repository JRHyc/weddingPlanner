using System;
using System.Collections.Generic;

namespace weddingPlanner.Models
{
    public class User : BaseEntity
    {
        public int UserId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Wedding> Weddings {get;set;}
        public List<RSVP> RSVPs {get;set;}
        public User()
        {
            RSVPs = new List<RSVP>();
            Weddings = new List<Wedding>();
        }
    }
}