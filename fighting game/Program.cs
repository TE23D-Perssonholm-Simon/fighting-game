using System.Reflection;

string typechart = """C:\Users\simon.perssonholm\Documents\prog1\fighting game.pokemontypechart.txt""";
Globaldata.Loaddata("hi",typechart);


while (true){
    
}



public static class Globaldata{
    public static Dictionary<string,Pokemon> Pokedex = new Dictionary<string, Pokemon>();
    public static Dictionary<string,Type> types = new Dictionary<string, Type>();
    public static List<string> typechartlines;
    public static Dictionary<int,int> teamsorter;
    public static void Loaddata(string pokemons, string typechart){
        List<string> typechartlines = new List<string>(File.ReadAllLines(typechart));
        for (int i = 0; i<16; i++){
            string name = typechartlines[16 + i];
            types.Add(name, new Type(name,i));
        }

    }
    public static int Ask(int nrofoptions){
    while (true){
        string svar = Console.ReadLine();
        try{
            if (int.Parse(svar) <= nrofoptions && int.Parse(svar) > 0){
                return(int.Parse(svar));
            }
        }
        catch{
            
        }
        System.Console.WriteLine("Write the number corresponding to the option ex 1,2,3");
    }
}
}


public class Player{
    Team team1;
    public Player(Team t){
        team1 = t;
    }

    public Move Makemove(Player opponent){
        Pokemonentity Leadpokemon = team1.pokemons[0];
        System.Console.WriteLine("choose action");
        System.Console.WriteLine("1: Battle");
        System.Console.WriteLine("2: Switch");
        int svar = Globaldata.Ask(2);
        if (svar == 1){
            Console.Clear();
            System.Console.WriteLine("choose move");
            System.Console.WriteLine();
        }
        
    }
}

public class Team{
    int id;
    public List<Pokemonentity> pokemons = new List<Pokemonentity>();
    
    public Team(List<Pokemonentity> team){
        pokemons = team;
    }
}
public class Pokemonentity{
    public Pokemon basepokemon;
    public int hp,maxhp,def,attack,speed;
    public Type type1, type2;

    public List<Move> moves = new List<Move>();
    public Pokemonentity(Pokemon e, Move a, Move b,Move c, Move d){
        basepokemon = e;
        hp = basepokemon.hp;
        def = basepokemon.def;
        attack = basepokemon.attack;
        speed = basepokemon.speed;
        type1 = basepokemon.type1;
        type2 = basepokemon.type2;
        hp = maxhp;
        moves.Add(a);
        moves.Add(b);
        moves.Add(c);
        moves.Add(d);
    }
}

public abstract class Action{
    public int priority;
    public abstract void execute(Team attack, Team defend);
}

public class Switcheroo:Action{
    int switchto;
    new public int priority = 10;
    public Switcheroo(int s){
        switchto = s;
    }
    public override void execute(Team attack, Team defend)
    {
        Pokemonentity leadpokemon = attack.pokemons[0];
        attack.pokemons[0] = attack.pokemons[switchto];
        attack.pokemons[switchto] = leadpokemon;
        
    }
}

public class Move:Action{
    string name;
    new public int priority;
    List<Effect> effects = new List<Effect>();
    Effect damadgeeffect;
    public Move(string n,Effect damadgeeffect, List<Effect> e, int p){
        name = n;
        this.damadgeeffect = damadgeeffect;
        effects = e;
        priority = p;
    }
    public override void execute(Team one, Team two)
    {
        Pokemonentity attacker = one.pokemons[0];
        Pokemonentity defender = two.pokemons[0];
        System.Console.WriteLine(name);
        if (damadgeeffect.Play(one,two)){
            foreach(Effect x in effects){

                x.Play(one,two);
            }
        }
    }

}
public abstract class Effect{
    public abstract bool Play(Team a, Team d);
}

public class Damadge:Effect{
    int power;
    int accuracy;
    Type type;
    public Damadge(int p, int accuracy, Type type){
        power = p;
        this.accuracy = accuracy;
        this.type = type;
    }

    public override bool Play(Team a, Team d){
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr > accuracy){
            return false;
        }
        float crit = 1;
        float stab;
        if (type == attacker.type1 || type == attacker.type2){
            stab = 1.5f;
        }
        else{
            stab = 1.0f;
        }
        float typeadvantage = (Globaldata.typechartlines[type.chartnr][defender.type1.chartnr] - '0') * (Globaldata.typechartlines[type.chartnr][defender.type2.chartnr] - '0');
        if (typeadvantage == 0){
            return false;
        }
        int damadge = (int)(((40*crit + 2)*power*attacker.attack/defender.def/50 + 2)*stab*typeadvantage);
        defender.hp =- damadge;
        if (defender.hp <= 0){
            //make a faint function
        }
        else{
            System.Console.WriteLine($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }

        return true;
    }

}






public class Type{
    public string name;
    public int chartnr;

    public Type(string name, int chartnr){
        this.name = name;
        this.chartnr = chartnr;
    }

}

public class Pokemon{
    public int hp,attack,def,speed;
    public string name;

    public Type type1;
    public Type type2;

    public Pokemon(int hp, int attack, int def, int speed, Type type1, Type type2){
        this.hp = hp;
        this.attack = attack;
        this.def = def;
        this.speed = speed;
        this.type1 = type1;
        this.type2 = type2;
    }

}

