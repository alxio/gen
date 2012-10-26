using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen{
    internal class Neural{
        public const int NETWORK_DEPTH = 2;
        public const int NETWORK_BREADTH = Program.DATA_SIZE*2;
        private Layer[] _network = new Layer[NETWORK_DEPTH];

        public float Calculate(float [] input){
            float answer = 0;
            float[] data = new float[NETWORK_BREADTH];
            data[0] = 1;
            data[1] = -1;
            for(int i=1;i<Program.DATA_SIZE;i++){
                data[i] = input[i];
                data[2*i] = ((float)1)/input[i];
            }
            for(int i=0;i<NETWORK_DEPTH;i++){
                data = _network[i].Calculate(data);
            }
            for(int i=0;i<NETWORK_BREADTH;i++){
                answer += data[i];
            }
            return answer;
        }

        private class Layer{
            private Neuron[] _neurons = new Neuron[NETWORK_BREADTH];
            public float[] Calculate(float[] prev){
                float[] answer = new float[NETWORK_DEPTH];
                for (int i = 0; i < NETWORK_DEPTH; i++){
                    answer[i] = _neurons[i].Calculate(prev);
                }
                return answer;
            }
        }

        private class Neuron{


            private bool _type = true;
            private float[] _values = new float[NETWORK_BREADTH];

            public float Calculate(float[] prev){
                if (_type) return Sum(prev);
                else return Mult(prev);
            }

            private float Mult(float[] prev){
                double answer = 1;
                for (int i = 0; i < NETWORK_BREADTH; i++) {
                    answer *= Math.Pow(prev[i],_values[i]);
                    
                }
                return (float) answer;
            }

            private float Sum(float[] prev){
                float answer = 0;
                for (int i = 0; i < NETWORK_BREADTH; i++) {
                    answer += _values[i]*prev[i];
                }
                return answer;
            }
        }
    }
}