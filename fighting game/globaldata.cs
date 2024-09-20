public static class Globaldata
{
    public static Dictionary<string, Pokemon> Pokedex = new Dictionary<string, Pokemon>();
    public static Dictionary<string, Pokemontype> Pokemontypes = new Dictionary<string, Pokemontype>();
    public static Dictionary<string, Pokemonentity> pokeid = new Dictionary<string, Pokemonentity>();
    public static Dictionary<string, Move> movedict = new Dictionary<string, Move>();
    public static List<Team> teamcollection = new List<Team>();
    public static List<Team> faintorder = new List<Team>();

    public static Dictionary<string, Effect> effectdict = new Dictionary<string, Effect>();

    public static List<string> Pokemontypechartlines;
    public static Dictionary<string, Type> effectclassdict = new Dictionary<string, Type>();
    public static int loadeffect(int g)
    {

        Type effecttype = effectclassdict[Pokemontypechartlines[g]];
        g++;
        List<string> strings = new List<string>();
        while (Pokemontypechartlines[g] != "end")
        {
            strings.Add(Pokemontypechartlines[g]);
            g++;
        }
        g++;
        Effect theeffect = (Effect)Activator.CreateInstance(effecttype, strings);
        Globaldata.effectdict.Add(strings[-1], theeffect);
        return g;
    }
    public static int loadmove(int g)
    {


        List<string> strings = new List<string>();
        while (Pokemontypechartlines[g] != "end")
        {
            strings.Add(Pokemontypechartlines[g]);
            g++;
        }
        g++;
        Move damove = new Move(strings);
        movedict[strings[0]] = damove;
        return g;

    }
    public static int loadpokemon(int g)
    {
        List<string> strings = new List<string>();
        while (Pokemontypechartlines[g] != "end")
        {
            strings.Add(Pokemontypechartlines[g]);
            g++;
        }
        g++;
        Pokemon dapokemon = new Pokemon(strings);
        Pokedex[strings[0]] = dapokemon;
        return g;

    }
    public static int loadpokemonentity(int g)
    {
        List<string> strings = new List<string>();
        while (Pokemontypechartlines[g] != "end")
        {
            strings.Add(Pokemontypechartlines[g]);
            g++;
        }
        g++;
        Pokemonentity dapokemon = new Pokemonentity(strings);
        pokeid[strings[0]] = dapokemon;
        return g;

    }
    public static int loadteam(int g)
    {
        List<string> strings = new List<string>();
        while (Pokemontypechartlines[g] != "end")
        {
            strings.Add(Pokemontypechartlines[g]);
            g++;
        }
        g++;
        Team dateam = new Team(strings);
        teamcollection.Add(dateam);
        return g;

    }

    public static void Loaddata(string pokemons, string Pokemontypechart)
    {
        int g = 0;
        try{
        effectclassdict["dmg"] = typeof(Damadge);
        List<string> Pokemontypechartlines = new List<string>(File.ReadAllLines(Pokemontypechart));
        for (int i = 0; i < 16; i++)
        {
            string name = Pokemontypechartlines[16 + i];
            Pokemontypes.Add(name, new Pokemontype(name, i));
        }
        }
        catch(Exception e){
            System.Console.WriteLine(e);
        }
        try{
        while (g < Pokemontypechartlines.Count)
        {
            List<Effect> effects = new List<Effect>();
            while (Pokemontypechartlines[g] != "e")
            {
                g = loadeffect(g);
            }
            g++;
            
            while (Pokemontypechartlines[g] != "e")
            {
                g = loadmove(g);
            }
            g++;
            while (Pokemontypechartlines[g] != "e")
            {
                g = loadpokemon(g);
            }
            g++;
            while (Pokemontypechartlines[g] != "e"){
                g = loadpokemonentity(g);
            }
            g++;
            while (Pokemontypechartlines[g] != "e")
            {
                g = loadteam(g);
            }
            g++;


        }
        }
        catch(Exception e){
            System.Console.WriteLine(e);
            Console.ReadLine();
        }


    }
    public static void addfaint(Team one)
    {
        if (!faintorder.Contains(one))
        {
            faintorder.Add(one);
        }
    }
    public static string Ask(string title, List<string> keys)
    {
        int cursor = 0;
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine(title);
            for (int i = 0; i < keys.Count; i++)
            {
                if (i == cursor)
                {
                    Console.Write("\x1b[47m\x1b[30m");
                    Console.Write(keys[i]);
                    Console.WriteLine("\x1b[0m");

                }
                else
                {
                    Console.WriteLine(keys[i]);
                }
            }
            string input = Console.ReadKey().Key.ToString().ToLower();
            if (input == "w")
            {
                if (cursor != 0)
                {
                    cursor--;
                }
            }
            if (input == "s" && cursor + 1 < keys.Count)
            {
                cursor++;
            }
            if (input == "enter")
            {
                return (keys[cursor]);
            }
        }
    }
}