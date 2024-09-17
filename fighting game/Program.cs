using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


string Pokemontypechart = """C:\Users\simon.perssonholm\Documents\prog1\fighting game.pokemonPokemontypechart.txt""";
Globaldata.Loaddata("hi",Pokemontypechart);



void match(Player player1, Player player2){
    Action move1;
    Console.Clear();
    System.Console.WriteLine("secondplayer to move (press enter to progress)");
    Console.ReadLine();
    Action move2;
    bool fainted1 = false;
    bool fainted2 = false;
    while (true){
        move1 = player1.Makemove(player2);
        move2 = player2.Makemove(player1);
        if (move1.priority == move2.priority){
            if (player1.team1.pokemons[0].speed >= player2.team1.pokemons[0].speed){
                player1.Play(move1,player2);
                if (player2.team1.pokemons[0].hp > 0){
                    player2.Play(move2,player1);
                }
                else{
                    fainted2 = true;
                }
            }
            else{
                player2.Play(move2,player1);
                if (player1.team1.pokemons[0].hp > 0){
                    player1.Play(move1,player2);
                }
                else{
                    fainted1 = true;
                }
            }
            
        }
        else if(move1.priority > move2.priority){
            player1.Play(move1,player2);
            if (player2.team1.pokemons[0].hp > 0){
                player2.Play(move2,player1);
            }
            else{
                fainted2 = true;
            }
        }
        else{
            player2.Play(move2,player1);
            if (player1.team1.pokemons[0].hp > 0){
                player1.Play(move1,player2);
            }
            else{
                fainted1 = true;
            }
            
        }

        if (fainted1){
            if (player1.Faint(player2.team1)){
                System.Console.WriteLine($"{player1.name} won!!!");
                Console.ReadLine();
                break;
            }
            
        }
        if (fainted2){
            if (player2.Faint(player1.team1)){
                System.Console.WriteLine($"{player2.name} won!!!");
                Console.ReadLine();
                break;
            }
        }
        if (fainted1){
            System.Console.WriteLine($"{player1.team1.pokemons[0]} switched in");
        }
        if (fainted2){
            System.Console.WriteLine($"{player2.team1.pokemons[0]} switched in");
        }


    }
}

public static class Globaldata{
    public static Dictionary<string,Pokemon> Pokedex = new Dictionary<string, Pokemon>();
    public static Dictionary<string,Pokemontype> Pokemontypes = new Dictionary<string, Pokemontype>();

    public static Dictionary<string,Team> teamcollection = new Dictionary<string, Team>();
    
   
    public static List<string> Pokemontypechartlines;
    public static void Loaddata(string pokemons, string Pokemontypechart){
        List<string> Pokemontypechartlines = new List<string>(File.ReadAllLines(Pokemontypechart));
        for (int i = 0; i<16; i++){
            string name = Pokemontypechartlines[16 + i];
            Pokemontypes.Add(name, new Pokemontype(name,i));
        }
        int g = 32;
        while (g < Pokemontypechartlines.Count){
            //load effects
            while (Pokemontypechartlines[g] != "£"){
                Type effecttype = Type.GetType(Pokemontypechartlines[g]);
                if (effecttype != null){
                    Effect effect = (Effect)Activator.CreateInstance(effecttype);
                }

            }
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
    public Team team1;
    public string name;
    public Player(Team t){
        team1 = t;
    }

    public bool Play(Action damove,Player defender){
        Team def = defender.team1;
        damove.execute(team1,def);
        if (defender.team1.pokemons[0].hp == 0){
            return false;
        }
        return true;
    }

    public bool Faint(Team opponent){
        List<string> options = new List<string>();
        string svar;
        if (team1.pokemons.Count > 1){
            for(int i = 1; i < options.Count; i++){
                options.Add(team1.pokemons[i].basepokemon.name);
                
            }
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 0; i<team1.pokemons.Count; i++){
                if (team1.pokemons[i].basepokemon.name == svar){
                    new Switcheroo(i).execute(team1,opponent);
                    return true;
                }
            }
        }
        else{
            return true;
        }
        return true;
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
    public bool faintedthisturn;
    
    public Team(List<Pokemonentity> team){
        pokemons = team;
    }

    public void faint(){
        List<string> options = new List<string>();
        faintedthisturn = true;
        string svar;
            foreach(Pokemonentity x in pokemons){
                options.Add(x.basepokemon.name);
                
            }
            options.Add("Go back");
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 0; i<pokemons.Count; i++){
                if (pokemons[i].basepokemon.name == svar){
                    Action switchto = new Switcheroo(i);
                    switchto.execute(this,this);
                }
            }

    }
}
public class Pokemonentity{
    public Pokemon basepokemon;
    public int hp,maxhp,def,attack,speed;
    public Pokemontype Pokemontype1, Pokemontype2;
    public Statuseffekt staticeffekt = null;
    public List<Statuseffekt> statuseffekts = new List<Statuseffekt>();
    public List<Move> moves = new List<Move>();
    public Pokemonentity(Pokemon e, Move a, Move b,Move c, Move d){
        basepokemon = e;
        hp = basepokemon.hp;
        def = basepokemon.def;
        attack = basepokemon.attack;
        speed = basepokemon.speed;
        Pokemontype1 = basepokemon.Pokemontype1;
        Pokemontype2 = basepokemon.Pokemontype2;
        hp = maxhp;
        moves.Add(a);
        moves.Add(b);
        moves.Add(c);
        moves.Add(d);
    }
}

public class Statuseffekt{
    public List<Statuscomponent> components = new List<Statuscomponent>();
    public Statuseffekt(List<Statuscomponent> comp){
        components = comp;
    }
}
public abstract class Statuscomponent{

}
public abstract class Movecomponent:Statuscomponent{
    public abstract Effect run(Effect themove);
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
        Effect localdamadgeeffect = damadgeeffect;
        
        Pokemonentity attacker = one.pokemons[0];
        Pokemonentity defender = two.pokemons[0];
        foreach (Statuseffekt x in attacker.statuseffekts){
            foreach(Statuscomponent e in x.components){
                if (e is Movecomponent movecomponent){
                    localdamadgeeffect = movecomponent.run(localdamadgeeffect);
                }
                
            }
        }
        if (attacker.staticeffekt != null){
            foreach(Statuscomponent e in attacker.staticeffekt.components){
                if (e is Movecomponent movecomponent){
                    localdamadgeeffect = movecomponent.run(localdamadgeeffect);
                }
                
        }
        }
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
    public string failmessage;
}

public class Damadge:Effect{
    int power;
    int accuracy;
    Pokemontype Pokemontype;
    
    public Damadge(int p, int accuracy, Pokemontype Pokemontype){
        power = p;
        this.accuracy = accuracy;
        this.Pokemontype = Pokemontype;
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
        if (Pokemontype == attacker.Pokemontype1 || Pokemontype == attacker.Pokemontype2){
            stab = 1.5f;
        }
        else{
            stab = 1.0f;
        }
        float Pokemontypeadvantage = (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype1.chartnr] - '0') * (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype2.chartnr] - '0')/4;
        if (Pokemontypeadvantage == 0){
            return false;
        }
        int damadge = (int)(((40*crit + 2)*power*attacker.attack/defender.def/50 + 2)*stab*Pokemontypeadvantage);
        defender.hp =- damadge;
        if (defender.hp > 0){
            System.Console.WriteLine($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else {
            System.Console.WriteLine($"{defender.basepokemon.name} fainted");
        }

        return true;
    }

}






public class Pokemontype{
    public string name;
    public int chartnr;

    public Pokemontype(string name, int chartnr){
        this.name = name;
        this.chartnr = chartnr;
    }

}

public class Pokemon{
    public int hp,attack,def,speed;
    public string name;

    public Pokemontype Pokemontype1;
    public Pokemontype Pokemontype2;

    public Pokemon(int hp, int attack, int def, int speed, Pokemontype Pokemontype1, Pokemontype Pokemontype2){
        this.hp = hp;
        this.attack = attack;
        this.def = def;
        this.speed = speed;
        this.Pokemontype1 = Pokemontype1;
        this.Pokemontype2 = Pokemontype2;
    }

}

