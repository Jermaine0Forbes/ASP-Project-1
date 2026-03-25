// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Console.WriteLine("The current time is "+ DateTime.Now);
using ConsoleApp1.Abstract;
using ConsoleApp1.TwoPointers;




class Program
{
    

    static void Main(string[] args)
    {
        var s = new MinSubArray();
        int target = 7;
        int[] nums = [2,3,1,2,4,3];
        s.MinSubArrayLen(target, nums);
    }
}










