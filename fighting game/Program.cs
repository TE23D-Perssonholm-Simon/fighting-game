Info move1;
Info move2;
Player one;
Player two;
Player first;
Player second;

string file = "C:\Users\simon.perssonholm\Documents\pokemontypechart.txt";
int damedge;
if (File.Exists(file)){
        Globaldata.typechart = new List<string>(File.ReadAllLines(file));
}

while(true){
    if(one.team[0].basepokemon.speed > two.team[0].basepokemon.speed){
        first = one;
        second = two;
    }
    else{
        first = two;
        second = one;
    }
    move1 = first.Move();
    move2 = second.Move();
    if(move1.Switchtonr != 0){
        
    }

    if(move2.Switchtonr != 0){
    }

    if(move1.Switchtonr == 0){
        damedge = move1.themove.Attack(second.team[0].basepokemon, first.team[0].basepokemon);
        second.team[0].hp -= damedge;
        System.Console.WriteLine($"{first.team[0].basepokemon.name} dealt {damedge.ToString()} damedge to {second.team[0].basepokemon.name} remaining hp {second.team[0].hp}");
    }

    if(move2.Switchtonr == 0){
        damedge = move2.themove.Attack(first.team[0].basepokemon, second.team[0].basepokemon);
        first.team[0].hp -= damedge;
        System.Console.WriteLine($"{second.team[0].basepokemon.name} dealt {damedge.ToString()} damedge to {first.team[0].basepokemon.name} remaining hp {first.team[0].hp}");
    }
    


    

}

public class Globaldata
{
    public static List<string> typechart;
}

class Type{
    public string name;
    public int typechartnr;
    public Type(string n, int t){
        name = n;
        typechartnr = t;
    }
}


class Player{
    public List<Entity> team;
    string name;

    

    public Player(string n,List<Entity> t){
        name = n;
        team = t;
        
    }

    public void Play(Info action){
    }

    public Info Move(){
        string answer;
        int switcher;
        System.Console.WriteLine($"{team[0].basepokemon.name}");
        System.Console.WriteLine("fight");
        System.Console.WriteLine("switch");
        while (true) {
        answer = Console.ReadLine().ToLower();
        if (answer == "fight"){
            return(new Info(team[0].Battle(), 0));
        }

        if (answer == "switch"){
            Console.Clear();
            System.Console.WriteLine("Who do you want to switch to?");
            int i = 0;
            foreach(Entity x in team){
                if (i > 0){
                System.Console.WriteLine($"{i}: {x.basepokemon.name}");
                }
                i++;
            }
            answer = Console.ReadLine().ToLower();
            try {
                switcher = int.Parse(answer);
                if (switcher > 0 && switcher < team.Count()){
                    return(new Info(team[0].a,switcher));
                }
                System.Console.WriteLine($"Write the number corrisponding to the pokemon ex 1 for {team[1].basepokemon.name}");

            }

            catch{
                System.Console.WriteLine("Write the number corrisponding to the pokemon ex 1");
            }

        }
        }



        
    }



}
class Info{
    public Move themove;
    public int Switchtonr;

    public Info(Move m,int s){
        themove = m;
        Switchtonr = s;
    }
}
class Move
{
    public string name;
    public int power;
    public Type type;

    public Move(string n, int p, Type t){
        name = n;
        power = p;
        type = t;
    }

    public int Attack(Pokemon attacker, Pokemon defender) {
        int crit;
        int damedge;
        float stab;
        List<string> tc = Globaldata.typechart;
        float typeadvantage = (Globaldata.typechart[type.typechartnr][defender.type.typechartnr] -'0') * (Globaldata.typechart[type.typechartnr][defender.type2.typechartnr] - '0')/4;

        
        
        crit = Random.Shared.Next(15);
        if (crit == 1){
            crit = 2;
        }
        else{
            crit = 1;
        }
        if (attacker.type == type){
            stab = 1.5f; 
        }
        else {
            stab = 1;
        }

        damedge= (int)(((42 + crit)*attacker.attack*power/defender.def/50 + 2)*stab*typeadvantage);
        return(damedge);



    }
}


class Entity
{
    public int hp;
    int maxhp;

    Move attackchosen;
    
    public Pokemon basepokemon;

    public Move a,b,c,d;
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

    public Move Battle(){
        System.Console.WriteLine("choose a move");
        System.Console.WriteLine($"A: {a.name}: power {a.power.ToString()}");
        System.Console.WriteLine($"B: {b.name}: power {b.power.ToString()}");
        System.Console.WriteLine($"C: {b.name}: power {b.power.ToString()}");
        System.Console.WriteLine($"D: {c.name}: power {c.power.ToString()}");
        while (true){
            string answer = Console.ReadLine().ToLower();
            if (answer == "a"){
                return(a);
            }
            else if(answer == "b"){
                return(b);
            }
            else if(answer == "c"){
                return(c);
            }
            else if(answer == "d"){
                return(d);
            }
            System.Console.WriteLine("type a, b, c or d");
        }
    }


}


class Pokemon
{
    
    public string name;
    public int hp,def,attack,speed;

    public List<Move> moves;

    public Type type;

    public Type type2;



    public Pokemon(string n, int hp, int def, int attack, int speed, List<Move> m,Type t1, Type t2){
        name = n;
        this.hp = hp;
        this.def = def;
        this.attack = attack;
        this.speed = speed;
        moves = m;
        type = t1;
        type2 = t2;

    }

    
}