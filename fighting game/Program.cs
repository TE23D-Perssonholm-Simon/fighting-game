
void move(){

}

class Move
{
    string namne;
    int damedge;
    string type;

    attack() {
        int crit;
        opponent.getdef();
        int damedge;
        crit = Random.Shared.Next(15);
        if (crit == 1){
            crit = 2;
        }
        else{
            crit = 1;
        }
        damedge= (42 + crit)*;
    }
}


class Pokemon
{
    Move move1,move2,move3,move4;
    string name;
    int hp;

    int def;

    int attack;
    int speed;



    public Pokemon(Move a,Move b, Move c, Move d, string n, int hp, int def, int attack, int speed){
        move1 = a;
        move2 = b;
        move3 = c;
        move4 = d;
        name = n;
        this.hp = hp;
        this.def = def;
        this.attack = attack;
        this.speed = speed;

    }

    public void battle(Pokemon opponent){


        

    }

    public int getdef(){
        return(def);
    }

    public int getattack(){
        return(attack);
    }
    
}