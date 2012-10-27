using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen{
    public class Network{
        public const int NETWORK_DEPTH = 2;
        public const int NETWORK_BREADTH = Program.DATA_SIZE*2;
        private Neuron[][] _neurons = new Neuron[NETWORK_DEPTH][];

        public void Mutate(){
            
        }

        public static void Crossover(Network s1, Network s2){
            
        }

        public Network(Network n){
            for(int i=0;i<_neurons.Length;i++){
                _neurons[i] =  new Neuron[NETWORK_BREADTH];
                for(int j=0;j<_neurons[i].Length;j++){
                    _neurons[i][j] = new Neuron(n._neurons[i][j]);
                }
            }
        }

        public Network(){
            for (int i = 0; i < NETWORK_DEPTH; i++){
                _neurons[i] = new Neuron[NETWORK_BREADTH];
                for (int j = 0; j < NETWORK_BREADTH; j++){
                    _neurons[i][j] = new Neuron(j%2 == 0);
                }
            }
        }

        public float Calculate(float[] input){
            float answer = 0;
            float[] data = new float[NETWORK_BREADTH];
            data[0] = 1;
            data[1] = -1;
            for (int i = 1; i < Program.DATA_SIZE; i++){
                data[i] = input[i];
                data[2*i] = ((float) 1)/input[i];
            }
            for (int i = 0; i < NETWORK_DEPTH; i++){
                float[] tmp = new float[NETWORK_BREADTH];
                for (int j = 0; j < NETWORK_BREADTH; j++){
                    tmp[j] = _neurons[i][j].Calculate(data);
                }
                data = tmp;
            }
            for (int i = 0; i < NETWORK_BREADTH; i++){
                answer += data[i];
            }
            return answer;
        }
    }
}