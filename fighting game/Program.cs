using System.Reflection;
using System.Security.Cryptography;


string typechart = """C:\Users\simon.perssonholm\Documents\prog1\fighting game.pokemontypechart.txt""";
Globaldata.Loaddata("hi",typechart);

void match(Player player1, Player player2){
    List<Action> actions = new List<Action>();
    Action move1;
    Action move2;
    while (true){
        actions.Clear();
        move1 = player1.Makemove(player2);
        move2 = player2.Makemove(player1);
        if (move1.priority == move2.priority){
            
        }

    }
}

public static class Globaldata{
    public static Dictionary<string,Pokemon> Pokedex = new Dictionary<string, Pokemon>();
    public static Dictionary<string,Type> types = new Dictionary<string, Type>();

    public static Dictionary<string,Team> teamcollection = new Dictionary<string, Team>();
    public static List<string> typechartlines;
    public static void Loaddata(string pokemons, string typechart){
        List<string> typechartlines = new List<string>(File.ReadAllLines(typechart));
        for (int i = 0; i<16; i++){
            string name = typechartlines[16 + i];
            types.Add(name, new Type(name,i));
        }

    }
    public static string Ask(string title, List<string> keys){
        int cursor = 0;
        while (true){
            Console.Clear();
            System.Console.WriteLine(title);
            for (int i = 0; i < keys.Count; i++){
                if (i == cursor){
                    Console.Write("\x1b[47m\x1b[30m");
                    Console.Write(keys[i]);
                    Console.WriteLine("\x1b[0m");
                
                }
                else {
                    Console.WriteLine(keys[i]);
                    }
            }
            string input = Console.ReadKey().Key.ToString().ToLower();
            if (input == "w"){
                if (cursor != 0){
                    cursor--;
                }
            }
            if (input == "s" && cursor + 1 < keys.Count){
                cursor++;              
            }
            if (input == "enter"){
                return(keys[cursor]);        
            }
    }
}
}


public class Player{
    Team team1;
    public Player(Team t){
        team1 = t;
    }

    public Action Makemove(Player opponent){
        Pokemonentity Leadpokemon = team1.pokemons[0];
        while (true){
        System.Console.WriteLine("choose action");
        List<string> options = new List<string>();
        options.Add("battle");
        options.Add("switch");
        string svar = Globaldata.Ask("choose action",options);
        if (svar == "battle"){
            options.Clear();
            foreach (Move x in team1.pokemons[0].moves){
                options.Add(x.name);
            }
            options.Add("Go back");
            svar = Globaldata.Ask("Choose a move",options);
            foreach (Move x in team1.pokemons[0].moves){
                if (x.name == svar){
                    return x;
                }

            }
            
        }
        else if (svar == "switch"){
            options.Clear();
            foreach(Pokemonentity x in team1.pokemons){
                options.Add(x.basepokemon.name);
                
            }
            options.Add("Go back");
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 0; i<team1.pokemons.Count; i++){
                if (team1.pokemons[i].basepokemon.name == svar){
                    return new Switcheroo(i);
                }
            }
        }
        }
        
    }
}

public class Team{
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
        System.Console.WriteLine($"{leadpokemon} switched out");
        System.Console.WriteLine($"{attack.pokemons[0]} switched in");
        
    }
}

public class Move:Action{
    public string name;
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
        if (defender.hp > 0){
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

