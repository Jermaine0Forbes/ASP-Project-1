using ConsoleApp1.Abstract;

namespace ConsoleApp1.TwoPointers;
public class FourSum:Solution
{
    
    public void Main(int[] nums, int target)
    {
               IList<IList<int>> output = [];
       if(nums == null || nums.Length < 4) Output(output);

       nums.Sort();

       int  n = nums.Length;

       for(int a = 0; a < n-3; a++)
       {
            if(a > 0 && nums[a] == nums[a-1]) continue;

            for(int b = a+1; b < n-2; b++)
            {
                if(b > a+1 && nums[b] == nums[b-1]) continue;
                    int c= b+1;
                    int d = n-1;
                 while(c < d){

                    long sum = (long)nums[a] + nums[b] + nums[c] + nums[d];
                    if(sum == target)
                    {
                        output.Add([nums[a] , nums[b] , nums[c] , nums[d]]);

                        while( c < d && nums[c] == nums[c+1]) c++;
                        while(c < d && nums[d] == nums[d-1]) d--;

                        c++;
                        d--;

                    }else{

                        if( sum < target){
                            c++;
                        }
                        else if(sum > target)
                        {
                            d--;
                        }
                    }

                 }


            } 

        }
        Output(output);
    }

}