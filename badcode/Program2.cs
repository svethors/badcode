using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace badcode
{
    public class Program2
	{
        public void Calculate(string[] args)
        {
            int a, b, c, x;
            a = 90;
            b = 15;
            c = 3;
            x = a - b / 3 + c * 2 - 1;
            Console.WriteLine(x);
            Console.ReadLine();
        }
    }
}
