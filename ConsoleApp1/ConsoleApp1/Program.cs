// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Console.WriteLine("The current time is "+ DateTime.Now);
// using ConsoleApp1.Abstract;
using ConsoleApp1.TwoPointers;




class Program
{


    static void Main(string[] args)
    {
        // int target = 7;
        // int[] nums = [2, 3, 1, 2, 4, 3]; 
        // var s = new MinSubArray();
        // s.MinSubArrayLen(target, nums);

        int target = 1;
        int[] nums = [10,20,30,40, 50, 60, 70, 80,90];
        var s = new ThreeSumClosest();
        s.ThreeSum1(nums, target);
    }
}










