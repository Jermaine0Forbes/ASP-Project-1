
using ConsoleApp1.Abstract;
namespace ConsoleApp1.TwoPointers;

public class RemoveOccurences:Solution
{
    
  public void Run(List<int> arr, int ele)
    {

        /* this is a two pointer problem, but the pointers are not 
            on opposing ends. One pointer is looping through the array 
            from beginning to end, but the second pointer(k) will be 
            used to move the matching element to the back of array with the
            other pointer(i)
        */ 
                int k = 0;
        for (int i = 0; i < arr.Count; i++)
        {
            // if the current element does not equal
            // the target element then the two pointers will swap positions of the items in the array
            if (arr[i]!= ele)
            {
                int e=arr[k];
                arr[k] = arr[i];
                arr[i] = e;
                k++;
            }
        }
        Output(k);
    }
}