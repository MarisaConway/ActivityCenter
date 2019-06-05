using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DojoActivityCenter.Models {
    public class Roster
    {
        [Key]
        public int RosterId {get;set;}
        
        public int UserId {get;set;}
        public User User {get;set;}

        public int ActivityId{get;set;}
        public Activity Activity {get;set;}
    }
}