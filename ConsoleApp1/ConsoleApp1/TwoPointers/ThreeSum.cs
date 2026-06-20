
using ConsoleApp1.Abstract;
using System.Collections.Generic;

namespace ConsoleApp1.TwoPointers;

public class ThreeSum:Solution
{
    public void Main(int[] nums)
    {
        int k = nums.Length -1;
        List<List<int>> output = [];
        for (int i = 0 ; i < nums.Length; i++) 
        {
            for(int j = 0 ; j < nums.Length; j++){
                int a = nums[i];
                int b = nums[j];
                int c = nums[k];
                if(a != b && a != c && b != c)
                {
                   if( (a + b + c) == 0)
                   {
                     output.Add([ a, b, c]);
                   }
                }
                k--;
            }
            k = nums.Length -1;
        }
        Output(output);
    }

    public void Run(int[] nums)
    {
        
    }
}