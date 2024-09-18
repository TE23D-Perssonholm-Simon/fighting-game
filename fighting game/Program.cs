using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


string Pokemontypechart = """C:\Users\simon.perssonholm\Documents\prog1\fighting game.pokemonPokemontypechart.txt""";
Globaldata.Loaddata("hi",Pokemontypechart);


List<Team> switcher(List<Team> teams){
    Team leadteam = teams[0];
    teams[0] = teams[1];
    teams[1] = leadteam;
    return teams;
}
void match(Team player1, Team player2){
    Console.Clear();
    System.Console.WriteLine("secondplayer to move (press enter to progress)");
    Console.ReadLine();
    List<Team> teamorder = new List<Team>();
    teamorder.Add(player1);
    teamorder.Add(player2);
    
    bool matchon = true;
    while (matchon){
        teamorder[0].Makemove(teamorder[1]);
        teamorder[1].Makemove(teamorder[0]);
        if (teamorder[0].action.priority == teamorder[1].action.priority){
            if (teamorder[1].pokemons[0].speed > teamorder[0].pokemons[0].speed){
                teamorder = switcher(teamorder);
            }
        }
        else if(teamorder[1].action.priority > teamorder[0].action.priority){
            teamorder = switcher(teamorder);
        }
        teamorder[0].play(teamorder[1]);
        teamorder[1].play(teamorder[0]);
        Console.ReadLine();

        foreach (Team x in Globaldata.faintorder){
            if (matchon){
            if (x.makefaint()){
                for (int i = 0;i<teamorder.Count; i++){
                    if (teamorder[i].playername == x.playername){
                        System.Console.WriteLine($"{teamorder[i - 1].playername} won!!!");
                        System.Console.WriteLine($"{teamorder[i].playername} is a loser L");
                        matchon = false;
                    }
                }
            }
            }
        }
        if (matchon){
            foreach (Team x in Globaldata.faintorder){
                System.Console.WriteLine($"{x.playername} switched to {x.pokemons[0].basepokemon.name}");
            }
        }
        
        

    }
}

public static class Globaldata{
    public static int g;
    public static Dictionary<string,Pokemon> Pokedex = new Dictionary<string, Pokemon>();
    public static Dictionary<string,Pokemontype> Pokemontypes = new Dictionary<string, Pokemontype>();
    public static Dictionary<string,Pokemonentity> pokeid = new Dictionary<string, Pokemonentity>();

    public static Dictionary<string,Team> teamcollection = new Dictionary<string, Team>();
    public static List<Team> faintorder = new List<Team>();
   
    public static List<string> Pokemontypechartlines;
    public static Dictionary<string,Type> classdict = new Dictionary<string, Type>();
    public static Effect loadeffect(int g){

        Type effecttype = classdict[Pokemontypechartlines[g]];
        List<string> strings = new List<string>();
        while(Pokemontypechartlines[g] != "end"){
            strings.Add(Pokemontypechartlines[g]);
        }
        Effect theeffect = (Effect)Activator.CreateInstance(effecttype,strings);
        return theeffect;
    }
    public static void Loaddata(string pokemons, string Pokemontypechart){
        List<string> Pokemontypechartlines = new List<string>(File.ReadAllLines(Pokemontypechart));
        for (int i = 0; i<16; i++){
            string name = Pokemontypechartlines[16 + i];
            Pokemontypes.Add(name, new Pokemontype(name,i));
        }
        Globaldata.g = 32;
        while (g < Pokemontypechartlines.Count){
            //load moves
            List<Effect> effects = new List<Effect>();
            while (Pokemontypechartlines[g] != "£"){
                while (Pokemontypechartlines[g] != "e"){
                    effects.Add(loadeffect(g));
                }
            }
        }


    }
    public static void addfaint(Team one){
        if (!faintorder.Contains(one)){
            faintorder.Add(one);
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


public class Team{
    public List<Pokemonentity> pokemons = new List<Pokemonentity>();
    public string playername;

    

    public Action action;
    public void play(Team opponent){
        if (action != null){
            action.execute(this,opponent);
        }
    }
    public Team(List<Pokemonentity> team){
        pokemons = team;
    }

    public void Faint(int priority){
        Globaldata.addfaint(this);
        action = null;
    }

    public bool makefaint(){
        System.Console.WriteLine($"{playername}s turn press enter to continue");
        Console.ReadLine();
        if (pokemons.Count > 1){
            Switcheroo theswitch = makeswitch();
            pokemons.RemoveAt(0);
            theswitch.switchto -= 1;
            theswitch.execute(this,this);
            return false;
        }
        else{
            return true;
        }
    }

    public Switcheroo makeswitch(){
    while (true){
        List<string> options = new List<string>();
        string svar;
        foreach(Pokemonentity x in pokemons){
            options.Add(x.basepokemon.name);
                
        }
        svar = Globaldata.Ask("Switch to what pokemon?", options);
        for (int i = 1; i<pokemons.Count; i++){
            if (pokemons[i].basepokemon.name == svar){
                
                return new Switcheroo(i);;
            }
        }
        System.Console.WriteLine($"cant switch to {pokemons[0]} because it is already active");
    }
    }
    public void Makemove(Team opponent){
        Console.Clear();
        System.Console.WriteLine($"{playername}s turn press enter to continue");
        Console.ReadLine();
        Pokemonentity Leadpokemon = pokemons[0];
        while (true){
        System.Console.WriteLine("choose action");
        List<string> options = new List<string>();
        options.Add("battle");
        options.Add("switch");
        string svar = Globaldata.Ask("choose action",options);
        if (svar == "battle"){
            options.Clear();
            foreach (Move x in pokemons[0].moves){
                options.Add(x.name);
            }
            options.Add("Go back");
            svar = Globaldata.Ask("Choose a move",options);
            foreach (Move x in pokemons[0].moves){
                if (x.name == svar){
                    action = x;
                    return;
                }

            }
            
        }
        else if (svar == "switch"){
            options.Clear();
            foreach(Pokemonentity x in pokemons){
                options.Add(x.basepokemon.name);
                
            }
            options.Add("Go back");
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 0; i<pokemons.Count; i++){
                if (pokemons[i].basepokemon.name == svar){
                    action = new Switcheroo(i);
                    return;
                }
            }
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
    public int switchto;
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

public class Faint:Action{
    new public int priority;

    public Faint(int priority){
        this.priority = priority;
    }
    public override void execute(Team attack, Team defend){
        if (attack.pokemons.Count > 1){
            attack.pokemons.RemoveAt(0);
            List<string> options = new List<string>();
            string svar;
            foreach(Pokemonentity x in attack.pokemons){
                    options.Add(x.basepokemon.name);
                
                }
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 0; i<attack.pokemons.Count; i++){
                if (attack.pokemons[i].basepokemon.name == svar){
                    Action switchto = new Switcheroo(i);
                    switchto.execute(attack,attack);
                }
            }
        }
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
    
    public Damadge(List<String> parameters){
        power = int.Parse(parameters[0]);
        this.accuracy = int.Parse(parameters[1]);
        this.Pokemontype = Globaldata.Pokemontypes[parameters[2]];
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

