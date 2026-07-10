using System.ComponentModel.DataAnnotations;

namespace WebApp2.Models;


public class PostSocketModel
{
    public int Id { get; set; }

    public string Status {get; set;} = "";

    public int Votes {get; set;} = 0;
}