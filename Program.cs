using System;

namespace gen{
    internal class Program{
        public const int DATA_SIZE = 20;
        public static Random RandomGenerator = new Random(12);

        private static float[] CreateTest(){
            float[] test = new float[DATA_SIZE];
            for (int i = 0; i < DATA_SIZE; i++){
                test[i] = (float) RandomGenerator.NextDouble();
            }
            test[0] = test[3]*test[2] + (10*test[10] - test[13])/test[4];
            return test;
        }

        private static void Main(string[] args){
            var e = new Evolve();

            for (int i = 0; i < 20; i++)
                e.AddTest(CreateTest());

            while (true){
                e.Iteration();
                //System.Console.ReadKey();
            }
        }
    }
}