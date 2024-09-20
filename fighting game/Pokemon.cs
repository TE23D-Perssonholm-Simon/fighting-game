public class Pokemontype
{
    public string name;
    public int chartnr;

    public Pokemontype(string name, int chartnr)
    {
        this.name = name;
        this.chartnr = chartnr;
    }

}

public class Pokemon
{
    public int hp, attack, def, speed ,spattack,spdef;
    public string name;

    public Pokemontype Pokemontype1;
    public Pokemontype Pokemontype2;
    public List<Move> learnablemoves = new List<Move>();

    public Pokemon(List<string> strings)
    {
        
        this.name = strings[0];
        this.hp = int.Parse(strings[1]);
        this.attack = int.Parse(strings[2]);
        this.def = int.Parse(strings[3]);
        spattack = int.Parse(strings[4]);
        spdef = int.Parse(strings[5]);
        this.speed = int.Parse(strings[6]);
        this.Pokemontype1 = Globaldata.Pokemontypes[strings[7]];
        this.Pokemontype2 = Globaldata.Pokemontypes[strings[8]];
        for (int i = 9; i<strings.Count(); i++){
            learnablemoves.Add(Globaldata.movedict[strings[i]]);
        }
        
    }

}
public class Pokemonentity
{
    public Pokemon basepokemon;
    public int hp, maxhp, def, attack, speed,spattack,spdef;
    public Pokemontype Pokemontype1, Pokemontype2;
    public Statuseffekt staticeffekt = null;
    public List<Statuseffekt> statuseffekts = new List<Statuseffekt>();
    public List<Move> moves = new List<Move>();
    public Pokemonentity(List<string> strings)
    {
        
        basepokemon = Globaldata.Pokedex[strings[1]];
        hp = basepokemon.hp;
        def = basepokemon.def;
        spdef = basepokemon.spdef;
        attack = basepokemon.attack;
        spattack = basepokemon.spattack;
        speed = basepokemon.speed;
        Pokemontype1 = basepokemon.Pokemontype1;
        Pokemontype2 = basepokemon.Pokemontype2;
        maxhp = hp;

        moves.Add(Globaldata.movedict[strings[2]]);
        moves.Add(Globaldata.movedict[strings[3]]);
        moves.Add(Globaldata.movedict[strings[4]]);
        moves.Add(Globaldata.movedict[strings[5]]);
    }
}