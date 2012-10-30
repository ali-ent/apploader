using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApp
{
    public class Entrance : Taobao.Infrastructure.Toolkit.AppDomains.Entrance
    {
        public override void Main()
        {
            Console.WriteLine("app init...");
        }

        public override void Unload()
        {
            Console.WriteLine("app unload...");
        }
    }
}
