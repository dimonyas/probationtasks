using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneObjectExtension
{
    public class TestObjectA
    {
        public static int a;
        protected const long b = 2;

        private string c;
        public int d;

        private TestObjectB tob;

        public TestObjectA()
        {
            a = 1;
            d = 4;
            c = "aba";
            tob = new TestObjectB();
        }

        public override string ToString()
        {
            return string.Format("a={0} b={1} c={2} d={3} tob: {4}", a, b, c, d, tob);
        }
    }

    public class TestObjectB
    {
        public static int e;
        protected const long f = 6;

        private string g;
        public int h;
        private long[] mas;

        public TestObjectB()
        {
            e = 5;
            g = "caba";
            h = 8;
            mas = new[] {9L, 10};
        }

        public override string ToString()
        {
            return string.Format("e={0} f={1} g={2} h={3}", e, f, g, h);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            TestObjectA testObject = new TestObjectA();
            object clonedTestObject = testObject.Clone();
            Console.WriteLine(testObject);
            Console.WriteLine(clonedTestObject);
        }
    }
}
