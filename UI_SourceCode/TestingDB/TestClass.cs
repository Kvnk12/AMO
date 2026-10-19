using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestingDB
{
    public class TestClass
    {
        private string testClassVariable="";
        public TestClass()
        {
            testClassVariable = "initiated value";
        }

        public void generateJobDetail()
        {
            Thread.Sleep(30000);
            testClassVariable = "Generated value" + Thread.CurrentThread.ManagedThreadId;
            
        }

        public void processJobDetail()
        {
            testClassVariable = "Processed value" + Thread.CurrentThread.ManagedThreadId;
        }

        public string classvariable { get { return testClassVariable; }
            set { testClassVariable = value; }
             }

    }
}
