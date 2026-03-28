using ConsoleApp1.Abstract;

namespace ConsoleApp1.TwoPointers
{

    public class ThreeSumClosest : Solution
    {
        public ThreeSumClosest() { }

        public void ThreeSum1(int[] nums, int target)
        {

            int mid = 0; int right = nums.Length - 1;
            nums.Sort();
            int closestSum = int.MaxValue;
            int minDiff = int.MaxValue;
            for (int left = 0; left < nums.Length - 2; left++)
            {
                mid = left + 1;
                right = nums.Length - 1;

                while (mid < right)
                {
                    int currentSum = nums[left] + nums[mid] + nums[right];
                    int currentDiff = Math.Abs(currentSum - target);

                    if (currentDiff < minDiff)
                    {
                        minDiff = currentDiff;
                        closestSum = currentSum;
                    }

                    if (currentSum < target)
                    {
                        right--;
                    }
                    else
                    {
                        mid++;
                    }
                }
            }
            Output(closestSum);
        }


        public void ThreeSum2(int[] nums, int target)
        {
            int mid = 0; int right = nums.Length - 1;
            nums.Sort();
            int closestSum = int.MaxValue;
            int minDiff = int.MaxValue;
            for (int left = 0; left < nums.Length - 2; left++)
            {
                mid = left + 1;
                right = nums.Length - 1;

                while (mid < right)
                {
                    int currentSum = nums[left] + nums[mid] + nums[right];
                    int currentDiff = Math.Abs(currentSum - target);

                    if (currentDiff < minDiff)
                    {
                        minDiff = currentDiff;
                        closestSum = currentSum;
                    }

                    if (currentSum < target)
                    {
                        mid++;
                    }
                    else
                    {
                        right--;
                    }
                }
            }
            Output(closestSum);
        }
    }
}