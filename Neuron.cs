using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen{
    public class Neuron{
        public const int INIT_FILLED = 3;
        private bool _sum = true;
        private float[] _values = new float[Network.NETWORK_BREADTH];
        private bool _first = true;

        public Neuron(Neuron n){
            _sum = n._sum;
            _first = n._first;
            if (_first) _values = new float[Network.FIRST_BREADTH];
            for (int i = 0; i < n._values.Length; i++){
                _values[i] = n._values[i];
            }
        }

        public Neuron(bool sum, bool first){
            _sum = sum;
            _first = first;

            if (_first) _values = new float[Network.FIRST_BREADTH];
            for (int i = 0; i < INIT_FILLED; i++){
                int index = Program.RandomGenerator.Next(Network.NETWORK_BREADTH);
                if (index > 1 && _first){
                    index = Program.RandomGenerator.Next(Network.FIRST_BREADTH - 1);
                }
                _values[index] = (float) (Program.RandomGenerator.NextDouble())*20;
            }
        }

        public Neuron(){}

        public void Mutate(){
            var los = Program.RandomGenerator.Next(5);
            switch (los){
                case 0:
                    _sum = !_sum;
                    break;
                case 1:
                    if (_first) _values = new float[Network.FIRST_BREADTH];
                    else _values = new float[Network.NETWORK_BREADTH];
                    for (int i = 0; i < INIT_FILLED; i++){
                        int index = Program.RandomGenerator.Next(Network.NETWORK_BREADTH);
                        if (index > 1 && _first){
                            index = Program.RandomGenerator.Next(Network.FIRST_BREADTH - 1);
                        }
                        _values[index] = (float) (Program.RandomGenerator.NextDouble())*10;
                    }
                    break;
                default:
                    _values[Program.RandomGenerator.Next(Network.NETWORK_BREADTH)] =
                        (float) (Program.RandomGenerator.NextDouble())*100;
                    break;
            }
        }

        public static void Crossover(Neuron n1, Neuron n2){
            if (n1._sum != n2._sum){
                n1._sum = !n1._sum;
                n2._sum = !n2._sum;
            }
            else if (n1._first){
                int border = Program.RandomGenerator.Next(Network.NETWORK_BREADTH);
                for (int i = 0; i < border; i++){
                    float tmp = n1._values[i];
                    n1._values[i] = n2._values[i];
                    n2._values[i] = tmp;
                }
            }
            else
                for (int i = 0; i < n1._values.Length; i++){
                    n1._values[i] = (n1._values[i] + n2._values[i])/2;
                    n2._values[i] = n1._values[i];
                }
        }

        public
            float Calculate
            (float[]
                 prev){
            if (_sum) return Sum(prev);
            else return Mult(prev);
        }

        private
            float Mult
            (float[]
                 prev){
            double answer = 1;
            for (int i = 0; i < Network.NETWORK_BREADTH; i++){
                answer *= Math.Pow(prev[i], _values[i]);
            }
            return (float) answer;
        }

        private
            float Sum
            (float[]
                 prev){
            float answer = 0;
            for (int i = 0; i < Network.NETWORK_BREADTH; i++){
                answer += _values[i]*prev[i];
            }
            return answer;
        }
    }
}