namespace gen{
    internal class Tree{
        public const int MULT = 1;
        public const int ADD = 2;
        public const int NEG = 3;
        public const int INV = 4;
        public const int CONST = 5;
        public const int INDEX = 6;

        public const int MAX_CONST = 100;

        private Tree _left;
        private Tree _right;
        private int _type;
        private int _value;

        private Tree(int type, int value, Tree left, Tree right){
            _type = type;
            _value = value;
            _left = left;
            _right = right;
        }

        public Tree(Tree t){
            Copy(t);
            if(Height() > 1)
            this._type = t._type;
        }

        public static void Crossover(Tree t1, Tree t2){
            Tree tmp1 = t1.Find(Program.RandomGenerator.Next());
            Tree tmp2 = t2.Find(Program.RandomGenerator.Next());
            Swap(tmp1, tmp2);
        }

        public void Mutate(){
            System.Console.Out.WriteLine("FIND ");
            Tree t = Find(Program.RandomGenerator.Next());
            System.Console.Out.WriteLine("MUTATE ");
            t.MutateHere();
        }

        public static Tree NewRandom(int height){
            int random = Program.RandomGenerator.Next();
            int type = 0;
            if (height < 1 || (height == 1 && random%2 == 0)){
                random /= 2;
                type = CONST + random%2;
                if (type == CONST){
                    return NewConst((random/2)%MAX_CONST);
                }
                else{
                    return NewIndex((random/2)%(Program.DATA_SIZE - 1) + 1);
                }
            }
            else{
                height--;
                random /= 10;
                type = random%4 + 1;
                switch (type){
                    case ADD:{
                        return NewAdd(NewRandom(height), NewRandom(height));
                    }
                    case MULT:{
                        return NewMult(NewRandom(height), NewRandom(height));
                    }
                    case NEG:{
                        return NewNeg(NewRandom(height));
                    }
                    case INV:{
                        return NewInv(NewRandom(height));
                    }
                }
            }
            //should never happen
            return null;
        }

        public float Eval(float[] values){
            switch (_type){
                case INDEX:{
                    return values[_value];
                }
                case CONST:{
                    return _value;
                }
                case ADD:{
                    return _left.Eval(values) + _right.Eval(values);
                }
                case MULT:{
                    return _left.Eval(values)*_right.Eval(values);
                }
                case NEG:{
                    return -_left.Eval(values);
                }
                case INV:{
                    return 1/_left.Eval(values);
                }
                default:{
                    return 0;
                }
            }
        }


        private void Copy(Tree t){
            _type = t._type;
            _value = t._value;
            if (t._left != null) {
                _left = new Tree(t._left);
            } else {
                _left = null;
            }
            if(t._right != null){
                _right = new Tree(t._right);
            } else {
                _left = null;
            }
            this._type = _type;

        }

        private static void Swap(Tree t1, Tree t2){
            var t3 = new Tree(t1);
            t1.Copy(t2);
            t2.Copy(t3);
        }

        private Tree Find(int random){
            if (random%3 == 0 || this.IsTerminal()){
                return this;
            }
            else if (random%6 < 3 && TwoChildren()){
                return _right.Find(random/6);
            }
            else{
                return _left.Find(random/6);
            }
        }


        public int Height(){
            int h = 0;
            if (_left != null){
                h = _left.Height();
            }
            if (_right != null){
                int tmp = _right.Height();
                if (tmp > h){
                    h = tmp;
                }
            }
            return h + 1;
        }

        private void MutateHere(){
            Copy(NewRandom(Height()));
        }

        private bool IsTerminal(){
            return _type >= CONST;
        }

        private bool TwoChildren(){
            return _type <= ADD;
        }

        private static Tree NewMult(Tree x, Tree y){
            return new Tree(MULT, 0, x, y);
        }

        private static Tree NewAdd(Tree x, Tree y){
            return new Tree(ADD, 0, x, y);
        }

        private static Tree NewNeg(Tree x){
            return new Tree(NEG, 0, x, null);
        }

        private static Tree NewInv(Tree x){
            return new Tree(INV, 0, x, null);
        }

        private static Tree NewConst(int x){
            return new Tree(CONST, x, null, null);
        }

        private static Tree NewIndex(int x){
            return new Tree(INDEX, x, null, null);
        }

        new public string ToString(){
            return " TYPE: " + _type + " HEIGHT " + Height();
        }
    }
}