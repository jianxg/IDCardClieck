﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("{0:MMdd}", Convert.ToDateTime("2013-6-8"));
            Console.ReadKey();
        }
    }
}