public abstract class Effect
{
    public abstract Damadgeeffectdata Play(Team a, Team d, int damadge);
}

public class Damadgeeffectdata
{
    public bool hit;
    public int damadge;
    public List<string> displaystrings;
    public Damadgeeffectdata(bool h, int d, List<string> strings)
    {
        hit = h;
        damadge = d;
        displaystrings = strings;
    }
}

public class Staticeffectgiver : Effect
{
    Statuseffekt statuseffekt;
    int oddofcausing;
    string inflictmessage;
    public Staticeffectgiver(int odds, string effectid)
    {
        oddofcausing = odds;
        statuseffekt = Globaldata.statuseffectddict[effectid];
        this.inflictmessage = statuseffekt.inflictmessage;
    }

    public override Damadgeeffectdata Play(Team a, Team d, int dmg)
    {
        List<string> displaystrings = new List<string>();
        int randomnr = Random.Shared.Next(101);
        if (oddofcausing >= randomnr)
        {
            if (d.pokemons[0].staticeffekt != null)
            {
                displaystrings.Add($"{d.pokemons[0].basepokemon.name} already has a status condition");
            }
            else
            {
                displaystrings.Add($"{d.pokemons[0].basepokemon.name} {inflictmessage}");
                d.pokemons[0].staticeffekt = statuseffekt.Clone();
                if (d.pokemons[0].staticeffekt.id == "Paralysis"){
                    d.pokemons[0].Paralysis = 0.5f;
                }
                d.pokemons[0].staticeffekt.Infect(d.pokemons[0]);
            }

        }
        return new Damadgeeffectdata(true,0,displaystrings);
    }

}
public class Statusmove : Effect
{
    int accuracy;
    public Statusmove(int accuracy)
    {
        this.accuracy = accuracy;
    }
    public override Damadgeeffectdata Play(Team a, Team d, int dam)
    {
        List<string> displaystrings = new List<string>();
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr >= accuracy)
        {
            displaystrings.Add("Missed");
            return new Damadgeeffectdata(false,0,displaystrings);
        }
        return new Damadgeeffectdata(true,0,displaystrings);
    }

}

public class Special : Effect
{
    int power;
    int accuracy;
    Pokemontype Pokemontype;

    // public Special(List<String> parameters)
    // {
    //     power = int.Parse(parameters[1]);
    //     this.accuracy = int.Parse(parameters[2]);
    //     this.Pokemontype = Globaldata.Pokemontypes[parameters[3]];
    // }
    public Special(int power, int accuracy, String pokemontype)
    {
        this.power = power;
        this.accuracy = accuracy;
        this.Pokemontype = Globaldata.Pokemontypes[pokemontype];
    }
    public override Damadgeeffectdata Play(Team a, Team d, int dam)
    {
        List<string> displaystrings = new List<string>();
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr >= accuracy)
        {
            displaystrings.Add("Missed");
            return new Damadgeeffectdata(false,0,displaystrings);
        }
        float crit = 1;
        float stab;
        if (Pokemontype == attacker.Pokemontype1 || Pokemontype == attacker.Pokemontype2)
        {
            stab = 1.5f;
        }
        else
        {
            stab = 1.0f;
        }
        float Pokemontypeadvantage = (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype1.chartnr] - '0') * (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype2.chartnr] - '0');
        Pokemontypeadvantage = Pokemontypeadvantage / 4;

        if (Pokemontypeadvantage == 0)
        {
            displaystrings.Add($"{defender.basepokemon.name} is immune");
            return new Damadgeeffectdata(false,0,displaystrings);
        }
        else if (Pokemontypeadvantage < 1)
        {
            displaystrings.Add("Not very effective");
        }
        else if (Pokemontypeadvantage > 1)
        {
            displaystrings.Add("SUPER EFFECTIVE");
        }
        int damadge = (int)(((40 * crit + 2) * power * attacker.spattack / defender.spdef / 50 + 2) * stab * Pokemontypeadvantage);
        defender.hp -= damadge;
        if (defender.hp > 0)
        {
            displaystrings.Add($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else
        {
            displaystrings.Add($"{defender.basepokemon.name} fainted");
        }

        return new Damadgeeffectdata(true,damadge,displaystrings);
    }

}

public class Physical : Effect
{
    int power;
    int accuracy;
    Pokemontype Pokemontype;

    // public Physical(List<String> parameters)
    // {
    //     power = int.Parse(parameters[1]);
    //     this.accuracy = int.Parse(parameters[2]);
    //     this.Pokemontype = Globaldata.Pokemontypes[parameters[3]];
    // }
    public Physical(int power, int accuracy, string pokemontypeid)
    {
        this.power = power;
        this.accuracy = accuracy;
        Pokemontype = Globaldata.Pokemontypes[pokemontypeid];
    }
    public override Damadgeeffectdata Play(Team a, Team d, int dam)
    {
        List<string> displaystrings = new List<string>();
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr >= accuracy)
        {
            displaystrings.Add("Missed");
            return new Damadgeeffectdata(false,0,displaystrings);
        }
        float crit = 1;
        float stab;
        if (Pokemontype == attacker.Pokemontype1 || Pokemontype == attacker.Pokemontype2)
        {
            stab = 1.5f;
        }
        else
        {
            stab = 1.0f;
        }
        float Pokemontypeadvantage = (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype1.chartnr] - '0') * (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype2.chartnr] - '0');
        Pokemontypeadvantage = Pokemontypeadvantage / 4;

        if (Pokemontypeadvantage == 0)
        {
            displaystrings.Add($"{defender.basepokemon.name} is immune");
            return new Damadgeeffectdata(false,0,displaystrings);
        }
        else if (Pokemontypeadvantage < 1)
        {
            displaystrings.Add("Not very effective");
        }
        else if (Pokemontypeadvantage > 1)
        {
            displaystrings.Add("SUPER EFFECTIVE");
        }
        
        int damadge = (int)(((40 * crit + 2) * power *attacker.burn* attacker.attack / defender.def / 50 + 2) * stab * Pokemontypeadvantage);
        defender.hp -= damadge;
        if (defender.hp > 0)
        {
            displaystrings.Add($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else
        {
            displaystrings.Add($"{defender.basepokemon.name} fainted");
        }

        return new Damadgeeffectdata(true,damadge,displaystrings);
    }

}
public class Opponent_statchanger: Effect{
    int attack;
    int special_attack;
    int defence;
    int special_defence;
    int speed;
    public Opponent_statchanger(int attack_changer,int special_attack_changer, int defence_changer,int special_defence_changer,int speed_changer){
        attack = attack_changer;
        special_attack = special_attack_changer;
        defence = defence_changer;
        special_defence = special_defence_changer;
        speed = speed_changer;
    }
    public override Damadgeeffectdata Play(Team a,Team d, int damadge){
        List<string> displaystring = new List<string>();
        Pokemonentity ptc = d.pokemons[0];
        ptc.attackbuff = attack;
        ptc.spattackbuff = special_attack;
        ptc.spdefbuff = special_defence;
        ptc.defbuff = defence;
        ptc.speedbuff = speed;
        displaystring.Add($"{ptc.basepokemon.name} got their stats changed");
        
        return new Damadgeeffectdata(true,0,displaystring);

    }

}
public class Player_statchanger: Effect{
    int attack;
    int special_attack;
    int defence;
    int special_defence;
    int speed;
    public Player_statchanger(int attack_changer,int special_attack_changer, int defence_changer,int special_defence_changer,int speed_changer){
        attack = attack_changer;
        special_attack = special_attack_changer;
        defence = defence_changer;
        special_defence = special_defence_changer;
        speed = speed_changer;
    }
    public override Damadgeeffectdata Play(Team a,Team d, int damadge){
        List<string> displaystring = new List<string>();
        Pokemonentity ptc = a.pokemons[0];
        ptc.attackbuff = attack;
        ptc.spattackbuff = special_attack;
        ptc.spdefbuff = special_defence;
        ptc.defbuff = defence;
        ptc.speedbuff = speed;
        displaystring.Add($"{ptc.basepokemon.name} got their stats changed");
        
        return new Damadgeeffectdata(true,0,displaystring);

    }

}
public class Switch_effect:Effect{
    String sw;
    public Switch_effect(string a_or_d){
        sw = a_or_d;
    }
    public override Damadgeeffectdata Play(Team a, Team d, int damadge)
    {
        List<string> displaystrings = new List<string>();
        Team toswitch;
        if (sw == "a"){
            toswitch = a;
        }
        else{
            toswitch = d;
        }
        displaystrings.AddRange(toswitch.makeswitch().execute(a,d));
        return new Damadgeeffectdata(true,0,displaystrings);
    }
}

public class Heal_effect:Effect{
    public int healprocent;
    public Heal_effect(int heal_procent){
        healprocent = heal_procent;
    }
    public override Damadgeeffectdata Play(Team a, Team d, int damadge)
    {
        List<string> displaystrings = new List<string>();
        a.pokemons[0].hp = Math.Min(a.pokemons[0].hp + a.pokemons[0].maxhp * healprocent /100,a.pokemons[0].maxhp);
        displaystrings.Add($"{a.pokemons[0].basepokemon.name} healed to {a.pokemons[0].hp.ToString()}");
        return new Damadgeeffectdata(true,0,displaystrings);
    }
}