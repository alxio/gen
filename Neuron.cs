using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen{
    public class Neuron{
        public const int INIT_FILLED = 3;
        private bool _sum = true;
        private float[] _values = new float[Network.NETWORK_BREADTH];

        public Neuron(Neuron n) {
            _sum = n._sum;
            for (int i = 0; i < n._values.Length; i++) {
                _values[i] = n._values[i];
            }
        }

        public Neuron(bool sum) {
            _sum = sum;
            for (int i = 0; i < INIT_FILLED; i++) {
                _values[Program.RandomGenerator.Next(Network.NETWORK_BREADTH - 1)] =
                    (float)Program.RandomGenerator.NextDouble();
            }
        }

        public void Mutate(){
            
        }

        public static void Crossover(Neuron n1, Neuron n2){
            if (n1._sum != n2._sum){
                n1._sum = !n1._sum;
                n2._sum = !n2._sum;
            }
            else{
                int border = Program.RandomGenerator.Next(Network.NETWORK_BREADTH);
                for (int i = 0; i < border; i++){
                    float tmp = n1._values[i];
                    n1._values[i] = n2._values[i];
                    n2._values[i] = tmp;
                }
            }
        }

        public float Calculate(float[] prev){
            if (_sum) return Sum(prev);
            else return Mult(prev);
        }

        private float Mult(float[] prev){
            double answer = 1;
            for (int i = 0; i < Network.NETWORK_BREADTH; i++){
                answer *= Math.Pow(prev[i], _values[i]);
            }
            return (float) answer;
        }

        private float Sum(float[] prev){
            float answer = 0;
            for (int i = 0; i < Network.NETWORK_BREADTH; i++){
                answer += _values[i]*prev[i];
            }
            return answer;
        }
    }
}