public abstract class Effect
{
    public abstract bool Play(Team a, Team d);
    public string failmessage;
}


public class Special : Effect
{
    int power;
    int accuracy;
    Pokemontype Pokemontype;

    public Special(List<String> parameters)
    {
        power = int.Parse(parameters[1]);
        this.accuracy = int.Parse(parameters[2]);
        this.Pokemontype = Globaldata.Pokemontypes[parameters[3]];
    }
    public override bool Play(Team a, Team d)
    {
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr > accuracy)
        {
            System.Console.WriteLine("Missed");
            return false;
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
        Pokemontypeadvantage = Pokemontypeadvantage/4;
        
        if (Pokemontypeadvantage == 0)
        {
            System.Console.WriteLine("is immune");
            return false;
        }
        else if(Pokemontypeadvantage < 1){
            System.Console.WriteLine("Not very effective");
        }
        else if(Pokemontypeadvantage > 1){
            System.Console.WriteLine("SUPER EFFECTIVE");
        }
        int damadge = (int)(((40 * crit + 2) * power * attacker.spattack / defender.spdef / 50 + 2) * stab * Pokemontypeadvantage);
        defender.hp -= damadge;
        if (defender.hp > 0)
        {
            System.Console.WriteLine($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else
        {
            System.Console.WriteLine($"{defender.basepokemon.name} fainted");
        }

        return true;
    }

}
public class Physical : Effect
{
    int power;
    int accuracy;
    Pokemontype Pokemontype;

    public Physical(List<String> parameters)
    {
        //name = 0
        power = int.Parse(parameters[1]);
        this.accuracy = int.Parse(parameters[2]);
        this.Pokemontype = Globaldata.Pokemontypes[parameters[3]];
    }
    public override bool Play(Team a, Team d)
    {
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr > accuracy)
        {
            System.Console.WriteLine("Missed");
            return false;
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
        float Pokemontypeadvantage = (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype1.chartnr] - '0') * (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype2.chartnr] - '0') / 4;
        if (Pokemontypeadvantage == 0)
        {
            System.Console.WriteLine("is immune");
            return false;
        }
        int damadge = (int)(((40 * crit + 2) * power * attacker.attack / defender.def / 50 + 2) * stab * Pokemontypeadvantage);
        defender.hp -= damadge;
        if (defender.hp > 0)
        {
            System.Console.WriteLine($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else
        {
            System.Console.WriteLine($"{defender.basepokemon.name} fainted");
        }

        return true;
    }

}