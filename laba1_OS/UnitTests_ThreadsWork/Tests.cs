using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ThreadsWork;

namespace UnitTests_ThreadsWork
{
    public class Tests
    {
        [Test]
        public void calculate_AllStringsAreTheSame_DictionarySizeEqualsOne()
        {
            //arrange
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 7;
            for (int i = 0; i < 7; i++)
                ThreadsFunction.Strings.Add("Nice smile");

            int begin = 0;

            //act
            ThreadsFunction.Calculate(begin);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.Count, 1);
        }

        [Test]
        public void calculate_AllNStringsAreDifferent_DictionarySizeEqualsN()
        {
            //arrange
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 7;
            for (int i = 0; i < 7; i++)
                ThreadsFunction.Strings.Add(i.ToString());

            int begin = 0;

            //act
            ThreadsFunction.Calculate(begin);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.Count, 7);
        }

        [Test]
        public void calculate_BeginBorderIsBiggerThanDictionarySize_DictionarySizeEqualsZero()
        {
            //arrange
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 7;
            for (int i = 0; i < 7; i++)
                ThreadsFunction.Strings.Add(i.ToString());

            int begin = 10;

            //act
            ThreadsFunction.Calculate(begin);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.Count, 0);
        }

        [Test]
        public void calculate_EndBorderIsBiggerThanDictionarySize_DictionarySizeEqualsN()
        {
            //arrange
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 18;
            for (int i = 0; i < 7; i++)
                ThreadsFunction.Strings.Add(i.ToString());

            int begin = 0;

            //act
            ThreadsFunction.Calculate(begin);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.Count, 7);
        }

        [Test]
        public void calculate_StringSunRepeatsThreeTimes_DictionaryValueOfElementCatEquals3()
        {
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 3;
            for (int i = 0; i < 3; i++)
                ThreadsFunction.Strings.Add("Sun");

            int begin = 0;

            //act
            ThreadsFunction.Calculate(begin);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.GetValueOrDefault("Sun"), 3);
        }

        [Test]
        public void calculate_StringSunRepeatsThreeTimes_DictionaryValueOfElementSunEquals3()
        {
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 3;

            for (int i = 0; i < 3; i++)
                ThreadsFunction.Strings.Add("Sun");

            int begin = 0;

            //act
            ThreadsFunction.Calculate(begin);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.GetValueOrDefault("Sun"), 3);
        }

        [Test]
        public void threadsWork_InputFiveStringsWithThreeSame_DictionarySizeEquals2()
        {
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 2;

            ThreadsFunction.Strings.Add("Moon");
            ThreadsFunction.Strings.Add("Sun");
            ThreadsFunction.Strings.Add("Sun");
            ThreadsFunction.Strings.Add("Moon");
            ThreadsFunction.Strings.Add("Moon");

            //act
            ThreadsFunction.ThreadsWork(3);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.Count, 2);
        }

        [Test]
        public void ThreadsWork_CompareTimeOfDifferentAmountOfThreadsWorking_SecondTimeIsGreater()
        {
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();

            ThreadsFunction.GenerateStringsList(200);

            ThreadsFunction.StrPortion = ThreadsFunction.Strings.Count / 5 + 1;

            //act
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            ThreadsFunction.ThreadsWork(5);
            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;

            ThreadsFunction.SortedStrings.Clear();
            ThreadsFunction.StrPortion = ThreadsFunction.Strings.Count / 200 + 1;

            watch.Start();
            ThreadsFunction.ThreadsWork(200);
            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;

            //assert
            Assert.Less(elapsedMs1, elapsedMs2);
        }

        [Test]
        public void PLinqWork_InputFiveStringsWithThreeSame_DictionarySizeEquals2()
        {
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();
            ThreadsFunction.StrPortion = 2;

            ThreadsFunction.Strings.Add("Moon");
            ThreadsFunction.Strings.Add("Sun");
            ThreadsFunction.Strings.Add("Sun");
            ThreadsFunction.Strings.Add("Moon");
            ThreadsFunction.Strings.Add("Moon");

            //act
            ThreadsFunction.PLinqWork(3);

            //assert
            Assert.AreEqual(ThreadsFunction.SortedStrings.Count, 2);
        }

        [Test]
        public void PLinqWork_CompareTimeOfDifferentAmountOfThreadsWorking_SecondTimeIsGreater()
        {
            ThreadsFunction.Strings = new List<String>();
            ThreadsFunction.SortedStrings = new ConcurrentDictionary<string, int>();

            ThreadsFunction.GenerateStringsList(200);

            ThreadsFunction.StrPortion = ThreadsFunction.Strings.Count / 5 + 1;

            //act
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            ThreadsFunction.PLinqWork(5);
            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;

            ThreadsFunction.SortedStrings.Clear();
            ThreadsFunction.StrPortion = ThreadsFunction.Strings.Count / 200 + 1;

            watch.Start();
            ThreadsFunction.PLinqWork(200);
            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;

            //assert
            Assert.Less(elapsedMs1, elapsedMs2);
        }
    }
}