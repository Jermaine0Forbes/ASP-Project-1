using System.Text.Json;

namespace ConsoleApp1.Abstract
{
    public class Solution
    {
        
        public Solution()
            {
                
            }

        public void Output<T>(T value)
        {
            Console.WriteLine(JsonSerializer.Serialize(value));
        }



    }
    
}
