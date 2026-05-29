namespace ConsoleApp1.Examples;

public class Car
{
   protected  string Name = "Car";

    public string GetName()
    {
        return Name;
    }
}

public class Volvo : Car
{
    public new string Name = "Volvo";

   public Volvo()
    {
        base.Name = Name;
    }

}