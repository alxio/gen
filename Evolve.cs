using System;
using System.Collections.Generic;

namespace gen{
    internal class Evolve{
        public const int INF = 99999999;
        private readonly int _height;
        private readonly int _mutationChance;
        private readonly int _popSize;
        private readonly SortedList<float, Network> _rated = new SortedList<float, Network>();
        private readonly int _survivedSize;
        private float _best = INF;

        private readonly List<float[]> _tests = new List<float[]>();
        private Network[] _population;

        public Evolve(){
            _popSize = 200;
            _height = 4;
            _survivedSize = 20;
            _mutationChance = 5;
            InitPopulation();
        }

        public void AddTest(float[] test){
            _tests.Add(test);
        }

        public void Iteration(){
            Rate();
            //System.Console.Out.WriteLine(_best);
            for (int i = 0; i < _popSize; i++){
                //System.Console.Out.Write(_population[i].Height());
                //System.Console.Out.Write(" ");
            }
            //System.Console.Out.WriteLine();
            //System.Console.In.ReadLine();
            Select();
            //System.Console.In.ReadLine();
            Breed();
        }

        private void InitPopulation(){
            _population = new Network[_popSize];
            for (int i = 0; i < _popSize; i++){
                _population[i] = new Network();
            }
        }

        private void Rate(){
            _rated.Clear();
            for (int i = 0; i < _popSize; i++){
                float rating = RateNetwork(_population[i]);
                if (rating < _best) _best = rating;
                _rated.Add(rating, _population[i]);
            }
        }

        private float RateNetwork(Network network){
            float rating = 0;
            foreach (var test in _tests) {
                float eval = network.Calculate(test) - test[0];
                rating += Math.Abs(eval);
            }
            return rating;
        }

        private void Select(){
            IEnumerator<KeyValuePair<float, Network>> it = _rated.GetEnumerator();
            int i = 0;
            while (it.MoveNext() && i++ <= _survivedSize){
                _population[i] = it.Current.Value;
            }
        }

        private int RandomParentId(int secondParent){
            double parent = Program.RandomGenerator.NextDouble();
            parent *= _survivedSize;
            parent = parent*parent;
            parent /= 100;
            int result = (int) parent;
            if (result == secondParent){
                result = (result + 1)%_survivedSize;
            }
            return result;
        }

        private void Breed(){
            for (int i = _survivedSize; i + 1 < _popSize; i += 2){
                int p1 = RandomParentId(INF);
                int p2 = RandomParentId(p1);
                _population[i] = new Network(_population[p1]);
                _population[i + 1] = new Network(_population[p2]);
                Network.Crossover(_population[i], _population[i + 1]);
            }
            for (int i = _survivedSize/2; i < _popSize; i++){
                if (Program.RandomGenerator.Next()%100 < _mutationChance){
                    //System.Console.Out.WriteLine("MUTATING " + i + _population[i]);
                    _population[i].Mutate();
                }
            }
        }
    }
}