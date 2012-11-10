using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen{
    public class Network{
        public const int NETWORK_DEPTH = 2;
        public const int FIRST_BREADTH = Program.DATA_SIZE * 2;
        public const int NETWORK_BREADTH = 5;
        private Neuron[][] _neurons = new Neuron[NETWORK_DEPTH][];

        public void Mutate(){
            var i = Program.RandomGenerator.Next(_neurons.Length);
            var j = Program.RandomGenerator.Next(_neurons[i].Length);
            _neurons[i][j].Mutate();
            if(Program.RandomGenerator.Next(3)==0) Mutate();
        }

        public static void Crossover(Network s1, Network s2){
            for (int i = 0; i < s1._neurons.Length; i++){
                for (int j = 0; j < s1._neurons[i].Length; j++){
                    var los = Program.RandomGenerator.Next(3);
                    switch (los){
                        case 0:
                            break;
                        case 1:
                            var temp = s1._neurons[i][j];
                            s1._neurons[i][j] = s2._neurons[i][j];
                            s2._neurons[i][j] = temp;
                            break;
                        case 2:
                            Neuron.Crossover(s1._neurons[i][j], s2._neurons[i][j]);
                            break;
                    }
                }
            }
        }

        public Network(Network n){
            for (int i = 0; i < _neurons.Length; i++){
                _neurons[i] = new Neuron[NETWORK_BREADTH];
                for (int j = 0; j < _neurons[i].Length; j++){
                    _neurons[i][j] = new Neuron(n._neurons[i][j]);
                }
            }
        }

        public Network(){
            for (int i = 0; i < NETWORK_DEPTH; i++){
                _neurons[i] = new Neuron[NETWORK_BREADTH];
                for (int j = 0; j < _neurons[i].Length; j++) {
                    _neurons[i][j] = new Neuron(j%2 == 0 , j==0);
                }
            }
        }

        public float Calculate(float[] input){
            float answer = 0;
            float[] data = new float[FIRST_BREADTH];
            data[0] = 1;
            data[1] = -1;
            for (int i = 1; i < Program.DATA_SIZE; i++){
                data[i] = input[i];
                data[2*i] = ((float) 1)/input[i];
            }
            for (int i = 0; i < NETWORK_DEPTH; i++){
                float[] tmp = new float[FIRST_BREADTH];
                for (int j = 0; j < _neurons[i].Length; j++){
                    tmp[j] = _neurons[i][j].Calculate(data);
                }
                data = tmp;
            }
            for (int i = 0; i < NETWORK_BREADTH; i++){
                answer += data[i];
            }
            return answer;
        }

        public void Print(){
            System.Console.Out.WriteLine("Depth: " + NETWORK_DEPTH);
            System.Console.Out.WriteLine("Breadth: " + NETWORK_BREADTH);
            for (int i = 0; i < _neurons.Length; i++){
                for (int j = 0; j < _neurons[i].Length; j++){}
            }
        }
    }
}