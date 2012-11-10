using System;
using System.Collections.Generic;

namespace gen{
    internal class Evolve{
        public const int INF = 99999999;
        private readonly int _height;
        private readonly int _mutationChance;
        private readonly int _popSize;
        private readonly int _freshSize;
        private readonly SortedList<float, Network> _rated = new SortedList<float, Network>();
        private readonly int _survivedSize;
        private float _best = INF;

        private readonly List<float[]> _tests = new List<float[]>();
        private Network[] _population;

        public Evolve(){
            _popSize = 2000;
            _freshSize = 500;
            _survivedSize = 500;
            _mutationChance = 50;
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

        private bool AddToRated(float rating, Network n){
            try{
                _rated.Add(rating, n);
                return true;
            }
            catch (Exception e){
                return false;
            }
        }

        private void Rate(){
            _rated.Clear();
            for (int i = 0; i < _popSize; i++){
                float rating = RateNetwork(_population[i]);
                if (rating < _best){
                    if(_best - rating > 1){
                        System.Console.Out.WriteLine(rating);
                    }
                    _best = rating;
                }
                AddToRated(rating, _population[i]);
            }
        }

        private float RateNetwork(Network network){
            float rating = 0;
            foreach (var test in _tests){
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

        private int RandomParentId(int secondParent, bool best){
            double parent = Program.RandomGenerator.NextDouble();
            if (best) {
                parent *= _survivedSize;
            } else{
                parent *= (_population.Length - 1);
            }
            int result = (int) parent;
            if (result == secondParent){
                result = (result + 1)%_survivedSize;
            }
            return result;
        }

        private void Breed(){
            for (int i = _survivedSize; i + 1 < _popSize - _freshSize; i += 2){
                int p1 = RandomParentId(INF,true);
                int p2 = RandomParentId(p1,false);
                _population[i] = new Network(_population[p1]);
                _population[i + 1] = new Network(_population[p2]);
                Network.Crossover(_population[i], _population[i + 1]);
            }
            for (int i = _popSize - _freshSize; i < _popSize; i++){
                _population[i] = new Network();
            }
            for (int i = 0; i < _popSize - _freshSize; i++){
                if (Program.RandomGenerator.Next()%100 < _mutationChance){
                    _population[i].Mutate();
                }
            }
        }
    }
}