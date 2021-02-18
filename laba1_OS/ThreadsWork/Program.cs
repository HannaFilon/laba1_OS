using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadsWork
{
    public class MaxHeap
    {
        private static readonly List<KeyValuePair<string, int>> Heap = new List<KeyValuePair<string, int>>();
        private static int _len = 0;
        public static void ExstractMax()
        {
            int i = 0, j;
            Heap[0] = Heap[_len - 1];
            Heap.RemoveAt(_len - 1);
            _len--;
            while (2 * i + 1 < _len)
            {
                if (2 * i + 2 == _len || Heap[2 * i + 1].Value > Heap[2 * i + 2].Value)
                    j = 2 * i + 1;
                else
                    j = 2 * i + 2;
                if (Heap[i].Value >= Heap[j].Value)
                    break;
                KeyValuePair<string, int> buffer = Heap[i];
                Heap[i] = Heap[j];
                Heap[j] = buffer;
                i = j;
            }
        }
        public static void Insert(KeyValuePair<string, int> k)
        {
            Heap.Add(k);
            int i = _len, j;
            _len++;
            while (i > 0)
            {
                j = (i - 1) / 2;
                if (Heap[j].Value >= Heap[i].Value)
                    break;
                KeyValuePair<string, int> buffer = Heap[i];
                Heap[i] = Heap[j];
                Heap[j] = buffer;
                i = j;
            }
        }
        public static KeyValuePair<string, int> GetMax()
        {
            return Heap[0];
        }
    }
    public class ThreadsFunction
    {
        public static List<String> Strings = new List<String>();
        public static ConcurrentDictionary<string, int> SortedStrings = new ConcurrentDictionary<string, int>();
        public static int StrPortion = 0;
        public static object lo = new object();

        public static void GenerateStringsList(int lowerBound)
        {
            Random rnd = new Random();
            int stringsNum = rnd.Next(lowerBound, 1000);
            for (int i = 0; i < stringsNum; i++)
            {
                int stringLen = rnd.Next(1, 100);
                string curString = "";
                for (int j = 0; j < stringLen; j++)
                {
                    int symbolCode = rnd.Next(48, 126);
                    curString += Convert.ToChar(symbolCode);
                }
                Strings.Add(curString);
            }
        }

        public static void InitializeStringsList()
        {
            Console.WriteLine("Input the number of strings for the list.");
            int num = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            Console.WriteLine("Input the strings one by one.");
            for (int i = 0; i < num; i++)
                Strings.Add(Console.ReadLine());
        }

        public static int GenerateThreadsNum()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        public static int GetThreadsNum()
        {
            Console.WriteLine("Input the number of threads.");
            int threadsNum = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            return threadsNum;
        }

        public static void Calculate(object obj)
        {
            int begin = (int)obj;
            int end = begin + StrPortion;

            if (begin >= Strings.Count)
                return;
            if (end > Strings.Count)
                end = Strings.Count;

            for (int i = begin; i < end; i++)
            {
                lock (lo)
                {
                    if (!SortedStrings.TryAdd(Strings[i], 1))
                    {
                        SortedStrings.TryGetValue(Strings[i], out int value);
                        SortedStrings.TryUpdate(Strings[i], value + 1, value);
                    }
                }
            }
        }

        public static void ThreadsWork(int threadsNum)
        {
            for (int i = 0; i < threadsNum; i++)
            {
                ParameterizedThreadStart calculateFunc = new ParameterizedThreadStart(Calculate);
                Thread t = new Thread(calculateFunc);
                
                t.Start(i * StrPortion);
            }
        }

        public static void PLinqWork(int threadsNum)
        {
            var begins = new List<int>();
            for (var i = 0; i < threadsNum; i++)
                begins.Add(i * StrPortion);
            begins.AsParallel().ForAll(begin => Calculate(begin));
        }

    }

    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Input the number of strings to be written in the top-N.");
            int popularStrNum = 0, stringsNum = 0, threadsNum = 1;

            try
            {
                popularStrNum = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());


                ThreadsFunction.InitializeStringsList();
                //ThreadsFunction.GenerateStringsList(1);

                stringsNum = ThreadsFunction.Strings.Count;

                threadsNum = ThreadsFunction.GetThreadsNum();
                //int threadsNum = ThreadsFunction.GenerateThreadsNum();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }

            ThreadsFunction.StrPortion = stringsNum / threadsNum + 1;

            int finalThreadsNum;
            if (threadsNum > stringsNum)
                finalThreadsNum = stringsNum;
            else
                finalThreadsNum = threadsNum;

            ThreadsFunction.ThreadsWork(finalThreadsNum);
            //ThreadsFunction.PLinqWork(finalThreadsNum);

            Array data = ThreadsFunction.SortedStrings.ToArray();
            foreach (var n in data)
                MaxHeap.Insert((KeyValuePair<string, int>)n);


            int finalNum;
            if (popularStrNum > data.Length)
                finalNum = data.Length;
            else
                finalNum = popularStrNum;

            Console.WriteLine("\nTop-" + finalNum + " most popular strings in the list:");
            for (int i = 0; i < finalNum; i++)
            {
                Console.WriteLine((i + 1) + ".\tString: " + MaxHeap.GetMax().Key + ", frequency: " + MaxHeap.GetMax().Value);
                MaxHeap.ExstractMax();
            }
        }
    }
}