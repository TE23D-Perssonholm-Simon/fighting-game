public abstract class Effect
{
    public abstract List<string> Play(Team a, Team d,int damadge);
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
    public override List<string> Play(Team a, Team d, int dam)
    {
        List<string> displaystrings = new List<string>();
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr > accuracy)
        {
            displaystrings.Add("false");
            displaystrings.Add("Missed");
            return displaystrings;
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
            displaystrings.Add("false");
            displaystrings.Add($"{defender.basepokemon.name} is immune");
            return displaystrings;
        }
        else if(Pokemontypeadvantage < 1){
            displaystrings.Add("Not very effective");
        }
        else if(Pokemontypeadvantage > 1){
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

        return displaystrings;
    }

}

public class Physical : Effect
{
    int power;
    int accuracy;
    Pokemontype Pokemontype;

    public Physical(List<String> parameters)
    {
        power = int.Parse(parameters[1]);
        this.accuracy = int.Parse(parameters[2]);
        this.Pokemontype = Globaldata.Pokemontypes[parameters[3]];
    }
    public override List<string> Play(Team a, Team d,int dam)
    {
        List<string> displaystrings = new List<string>();
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr > accuracy)
        {
            displaystrings.Add("false");
            displaystrings.Add("Missed");
            return displaystrings;
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
            displaystrings.Add("false");
            displaystrings.Add($"{defender.basepokemon.name} is immune");
            return displaystrings;
        }
        else if(Pokemontypeadvantage < 1){
            displaystrings.Add("Not very effective");
        }
        else if(Pokemontypeadvantage > 1){
            displaystrings.Add("SUPER EFFECTIVE");
        }
        float burn = 1;
        if (attacker.staticeffekt.id == "burn"){
            burn = 0.5f;
        }
        int damadge = (int)(((40 * crit + 2) * power * attacker.attack / defender.def / 50 + 2) * stab * Pokemontypeadvantage*burn);
        displaystrings.Add(damadge.ToString());
        defender.hp -= damadge;
        if (defender.hp > 0)
        {
            displaystrings.Add($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else
        {
            displaystrings.Add($"{defender.basepokemon.name} fainted");
        }

        return displaystrings;
    }

}