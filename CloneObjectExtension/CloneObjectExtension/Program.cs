using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneObjectExtension
{
    public class Parent
    {
        protected const int IntegerConst = 7;

        private string _stringField;
        public int IntegerField;

        public long LongProperty { get; private set; }

        private List<int> _intetgerList;
        private int[] _integerArray;

        public Child Child;

        public void Initialize()
        {
            _stringField = "testString";
            IntegerField = 4;
            LongProperty = 261;
            _intetgerList = new List<int> {26, 12, 24};
            _integerArray = new[] {12, 18, 1};
            Child = new Child {Parent = this};
        }

        public override string ToString()
        {
            return string.Format("IntegerConst={0} _stringField={1} IntegerField={2} LongProperty={3}", IntegerConst, _stringField, IntegerField, LongProperty);
        }
    }

    public class Child
    {
        public Parent Parent { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parent testObject = new Parent();
            testObject.Initialize();
            Parent clonedTestObject = testObject.Clone();
            Console.WriteLine(testObject);
            Console.WriteLine(clonedTestObject);
        }
    }
}
