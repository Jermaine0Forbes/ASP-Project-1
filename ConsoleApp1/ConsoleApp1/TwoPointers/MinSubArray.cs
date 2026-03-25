using System.Collections;
using ConsoleApp1.Abstract;

namespace ConsoleApp1.TwoPointers
{
    


    public class MinSubArray:Solution
    {

        public MinSubArray(){}
        public void MinSubArrayLen(int target, int[] nums)
        {
            ArrayList sub = new ArrayList();
            int result = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                int window = i + 1;

                for (int y = 0; y < nums.Length; y++)
                {

                    sub.Add(nums[y]);

                    if (sub.Count == window)
                    {

                        foreach (int x in sub)
                        {
                            result += x;
                        }

                        if (result >= target)
                        {
                            // return sub.Count;
                            Output(sub.Count);
                        }
                        result = 0;
                        if (sub.Count >= 1)
                        {
                            sub.RemoveAt(0);

                        }
                    }

                }
                sub.Clear();



            }

            Output(result);
        }

        public  void MinSub2(int target, int[] nums)
        {
            int result = int.MaxValue, runningSum = 0, left = 0;
            for (var right = 0; right < nums.Length; right++)
            {
                runningSum += nums[right];
                while (runningSum >= target)
                {
                    result = Math.Min(result, right - left + 1);
                    runningSum -= nums[left];
                    left++;
                }
            }
             Output(result == int.MaxValue ? 0 : result);
        }
    }

}