
using Microsoft.Win32.SafeHandles;

void move(){

}

class Player{
    List<Entity> team;
    
}

class Move
{
    string name;
    int power;
    string type;

    public Move(string n, int p){
        name = n;
        power = p;

    }

    public int Attack(int def, int attack) {
        int crit;
        int damedge;
        crit = Random.Shared.Next(15);
        if (crit == 1){
            crit = 2;
        }
        else{
            crit = 1;
        }
        damedge= ((42 + crit)*attack*power/def/50 + 2)*3/2;
        return(damedge);



    }
}

class Entity
{
    int hp;
    int maxhp;

    Move attackchosen;
    
    Pokemon basepokemon;

    Move a,b,c,d;
    Entity(int hp, Pokemon poke, Move one, Move two, Move three, Move four){
        this.hp = hp;
        basepokemon = poke;
        a = one;
        b = two;
        c = three;
        d = four;
        maxhp = basepokemon.hp;
        hp = basepokemon.hp;
    }

    public void Battle(Entity opponent){
        opponent.hp -= a.Attack(opponent.basepokemon.def,basepokemon.attack);

    }


}


class Pokemon
{
    
    public string name;
    public int hp,def,attack,speed;

    public List<Move> moves;



    public Pokemon(string n, int hp, int def, int attack, int speed, List<Move> m){
        name = n;
        this.hp = hp;
        this.def = def;
        this.attack = attack;
        this.speed = speed;
        moves = m;

    }

    
}