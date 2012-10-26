using System;
using System.Collections.Generic;

namespace gen{
    internal class Evolve{
        public const int INF = 99999999;
        private readonly int _height;
        private readonly int _mutationChance;
        private readonly int _popSize;
        private readonly SortedList<float, Tree> _rated = new SortedList<float, Tree>();
        private readonly int _survivedSize;
        private float _best = INF;

        private readonly List<float[]> _tests = new List<float[]>();
        private Tree[] _population;

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
            System.Console.Out.WriteLine(_best);
            for (int i = 0; i < _popSize; i++){
                System.Console.Out.Write(_population[i].Height());
                System.Console.Out.Write(" ");
            }
            System.Console.Out.WriteLine();
            System.Console.In.ReadLine();
            Select();
            System.Console.In.ReadLine();
            Breed();
        }

        private void InitPopulation(){
            _population = new Tree[_popSize];
            for (int i = 0; i < _popSize; i++){
                _population[i] = Tree.NewRandom(_height);
            }
        }

        private void Rate(){
            _rated.Clear();
            for (int i = 0; i < _popSize; i++){
                float rating = RateTree(_population[i]);
                if (rating < _best) _best = rating;
                try{
                    _rated.Add(rating, _population[i]);
                }
                catch (Exception e){}
            }
        }

        private void Select(){
            IEnumerator<KeyValuePair<float, Tree>> it = _rated.GetEnumerator();
            int i = 0;
            while (it.MoveNext() && i++ <= _survivedSize){
                _population[i] = it.Current.Value;
            }
        }

        private int randomParentId(int secondParent){
            double parent = Program.RandomGenerator.NextDouble();
            parent *= _survivedSize;
            parent = parent*parent;
            parent /= 100;
            int result = (int) parent;
            if(result == secondParent){
                result = (result + 1)%_survivedSize;
            }
            return result;
        }

        private void Breed(){
            for (int i = _survivedSize; i + 1 < _popSize; i += 2){
                int p1 = randomParentId(INF);
                int p2 = randomParentId(p1);
                System.Console.Out.WriteLine("CROSSOVER " + p1 + _population[p1].ToString() + " with "+ p2 + _population[p2].ToString());
                _population[i] = new Tree(_population[p1]);
                _population[i + 1] = new Tree(_population[p2]);
                Tree.Crossover(_population[i], _population[i + 1]);
            }
            for (int i = _survivedSize/2; i < _popSize; i++){
                if (Program.RandomGenerator.Next()%100 < _mutationChance){
                    System.Console.Out.WriteLine("MUTATING "+i+_population[i]);
                    _population[i].Mutate();
                }
            }
        }

        private float RateTree(Tree tree){
            float rating = 0;
            foreach (var test in _tests){
                float eval = tree.Eval(test) - test[0];
                rating += Math.Abs(eval);
            }
            return rating;
        }
    }
}