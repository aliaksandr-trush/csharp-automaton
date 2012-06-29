namespace RegOnline.RegressionTest.Fixtures.Warmup
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category("Regression")]
    public class TfsTesting : FixtureBase
    {
		[Manual]
		[Category(Priority.Two)]
        public void Release1a()
        {
			/*
			step one
			step two
			step three
			*/
		}

		[Manual]
		[Category(Priority.Four)]
        public void AuthTest1()
        {
			/*
			
			*/
		}

		[Manual]
		[Category(Priority.Three)]
        public void Test6()
        {
			/*
			
			*/
		}

		[Manual]
		[Category(Priority.Three)]
        public void Test4()
        {
			/*
			
			*/
		}

		[Manual]
		[Category(Priority.Three)]
        public void Test3()
        {
			/*
			
			*/
		}

		[Manual]
		[Category(Priority.Four)]
        public void ANewTest()
        {
			/*
			
			*/
		}

		[Manual]
        public void test1()
        {
			/*
			Open login page
			Login
			Create a new event
			Open a test registration
			Verify that event information is correct
			*/
		}

		[Manual]
		[Category(Priority.Four)]
        public void atest()
        {
			/*
			Step
			*/
		}

		[Manual]
        [Category(Priority.Three)]
        public void PriorityTest2()
        {
			/*
			
			*/
		}

		[Manual]
        public void PriorityTest()
        {
			/*
			
			*/
		}

		[Manual]
        public void test()
        {
			/*
			
			*/
		}


	}
}