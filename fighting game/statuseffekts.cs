using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public class Statuseffekt
{
    public string id;
    public string inflictmessage;
    public List<Statuscomponent> components = new List<Statuscomponent>();
    public Statuseffekt(string id, List<Statuscomponent> comp, string inflictmessage)
    {
        this.id = id;
        components = comp;
        this.inflictmessage = inflictmessage;
        Globaldata.statuseffectddict[id] = this;

    }

    public Statuseffekt Clone()
    {
        Statuseffekt effekt = new Statuseffekt(id, components,inflictmessage);
        return effekt;

    }
    public void Infect(Pokemonentity Prey)
    {
        foreach (Statuscomponent x in components)
        {
            switch (x)
            {
                case Movehinderer movehinderer:
                    Prey.movehinderer[id] = movehinderer;
                    break;
                case Endofturn endofturn:
                    Prey.endofturn[id] = endofturn;
                    break;
                case Counter counter:
                    Prey.timer[id] = counter.Clone();
                    break;



                default:
                    break;
            }
        }
    }
    
}
public abstract class Statuscomponent
{
    public void remove(Pokemonentity attacker)
    {
        string id = "";
        
        if (attacker.staticeffekt != null && attacker.staticeffekt.components.Contains(this))
        {
            id = attacker.staticeffekt.id;
            if (id == "Paralysis"){
                attacker.Paralysis = 1;
            }
            attacker.staticeffekt = null;
            if (id == "Burn"){
                attacker.burn = 1;
            }
            return;
        }
        else
        {
            Statuseffekt toremove = null;
            foreach (Statuseffekt x in attacker.statuseffekts)
            {
                if (x.components.Contains(this))
                {
                    id = x.id;
                    toremove = x;
                }
            }
            attacker.statuseffekts.Remove(toremove);
        }
        if (id != "")
        {
            attacker.endofturn[id] = null;
            attacker.noswitch[id] = null;
            attacker.forced[id] = null;
            attacker.movehinderer[id] = null;
            attacker.timer[id] = null;
        }
    }
}
public abstract class Endofturn : Statuscomponent{
    public abstract List<string> Execute(Team a);
}
public class Basicendofturn: Endofturn{
    string name;
    int dividedamadge;
    public Basicendofturn(string name, int dividedamadge){
        this.name = name;
        this.dividedamadge = dividedamadge;
    }
    public override List<string> Execute(Team a){
        List<string> displaystrings = new List<string>();
        Pokemonentity attacker = a.pokemons[0];
        int damadge = (int)(attacker.maxhp * (1f/dividedamadge));
        attacker.hp -= damadge;
        displaystrings.Add($"{attacker.basepokemon.name} took {damadge.ToString()} damadge of {name}");
        return displaystrings;
    }
}
public class BadlyPoisoned:Endofturn{
    int turn;
    public BadlyPoisoned(){
        turn = 0;
    }
    public override List<string> Execute(Team a){
        List<string> displaystrings = new List<string>();
        turn++;
        int damadge = (int)(a.pokemons[0].maxhp * (1f/16f)*turn);
        displaystrings.Add($"{a.pokemons[0]} took {damadge.ToString()} damadge due to poison");
        a.pokemons[0].hp -= damadge;
        return displaystrings;
        
    }
}
public abstract class Movehinderer : Statuscomponent
{
    public abstract List<string> Run(Pokemonentity attacker);
}

public class Basic_Movehinderer : Movehinderer
{
    int oddsofstopping;
    int oddsofremoval;
    string curemessage;
    string failmessage;
    string intromessage;

    public Basic_Movehinderer(int stop, int remove, string fail, string intro, string cure)
    {
        oddsofstopping = stop;
        oddsofremoval = remove;
        failmessage = fail;
        intromessage = intro;
        curemessage = cure;
    }

    public override List<string> Run(Pokemonentity attacker)
    {
        List<string> displaymessage = new List<string>();
        displaymessage.Add($"{attacker.basepokemon.name} {intromessage}");
        int randomnr = Random.Shared.Next(100);
        if (randomnr < oddsofremoval)
        {
            
            displaymessage.Add(curemessage);
            displaymessage.Add("hi");
            remove(attacker);
            return displaymessage;
        }
        randomnr = Random.Shared.Next(100);
        if (oddsofstopping > randomnr)
        {
            displaymessage.Add("false");
            displaymessage.Add($"{attacker.basepokemon.name} {failmessage}");
        }
        else
        {
            displaymessage.Add("hi");
        }
        return displaymessage;
    }
}
public class Timedremoval : Movehinderer{
    string curemessage;
    string failmessage;
    public Timedremoval(string curemessage,string failmessage){
        this.curemessage = curemessage;
        this.failmessage = failmessage;
    }
    
    public override List<string> Run(Pokemonentity attacker)
    {
        List<string> displaystrings = new List<string>();
        string id = "";
        foreach(string strin in attacker.movehinderer.Keys){
            if (attacker.movehinderer[strin] == this){
                id = strin;
            }
        }
        if(attacker.timer[id].number-- <= -1){
            remove(attacker);
            displaystrings.Add($"{attacker.basepokemon.name} {curemessage}");
            displaystrings.Add("True");
            return displaystrings;
        }
        displaystrings.Add($"{attacker.basepokemon.name} {failmessage}");
        displaystrings.Add("false");
        return displaystrings;


    }
}
public class Counter:Statuscomponent{
    public Counter(int startvalue){
        this.number = number;
    }
    
    public Counter Clone(){
        return new Counter(number);
    }
    public int number{get;set;}
}

