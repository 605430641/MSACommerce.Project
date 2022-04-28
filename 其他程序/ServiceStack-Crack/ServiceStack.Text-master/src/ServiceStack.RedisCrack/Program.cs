using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace ServiceStack.RedisCrack
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parallel.For(0, 10000, (i) =>
                {
                    using (RedisClient client = new RedisClient("192.168.3.254"))
                    {
                        client.Set("name" + i, i);
                        client.Incr("name" + i);
                        Console.WriteLine(i);
                    }

                });
                Console.WriteLine("ok");

                Console.WriteLine("Hello World!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
			
        }
    }
}
