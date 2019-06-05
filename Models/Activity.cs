using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DojoActivityCenter.Models{
    public class Activity 
    {
        //Main fields for DB
        [Key]
    public int ActivityId {get;set;}
    
    [Required]
    [MinLength(2, ErrorMessage="Title must be 2 or more characters")]
    public string Title {get;set;}

    [Required]
    [NotInPast]
    [DisplayFormat(DataFormatString = "{0:MM dd}")]
    [DataType(DataType.Date)]
    public DateTime Date{get;set;}

    [Required]
    [DataType(DataType.Time)]
    
    public TimeSpan Time{get;set;}

    [Required]
    [Range(0,Int32.MaxValue)]
    public int Duration{get;set;}

    [Required]
    public string Duration_Type{get;set;}


    [Required]
    [MinLength(10, ErrorMessage="Description must be 10 or more characters")]
    public string Description {get;set;}


    public DateTime Created_at {get;set;} = DateTime.Now;
    public DateTime Updated_at {get;set;} = DateTime.Now;


    [NotMapped]
    public bool Status {get;set;} = false;

    //Many To Many
    public List<Roster> Attendees {get;set;}
    //One To Many
    public int UserId {get;set;}
    public User Creator {get;set;}
        
    }
}