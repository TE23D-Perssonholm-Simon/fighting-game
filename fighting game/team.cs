public class Team
{
    public List<Pokemonentity> pokemons = new List<Pokemonentity>();
    public string name;



    public Action action;
    public List<string> play(Team opponent)
    {
        List<string> displaystrings = new List<string>();
        if (action != null)
        {
            displaystrings = action.execute(this, opponent);
        }
        return (displaystrings);
    }
    public Team(string teamname,List<Pokemonentity> pokemonentities)
    {
        pokemons = pokemonentities;

    }

    public Team Clone(){
        List<Pokemonentity> pokemonentities = new List<Pokemonentity>();
        foreach (Pokemonentity x in pokemons){
            pokemonentities.Add(x.Clone());
        }
        return new Team(name,pokemonentities);
    }

    public void Faint(int priority)
    {
        Globaldata.addfaint(this);
        action = null;
    }

    public bool makefaint()
    {
        
        if (pokemons.Count > 1)
        {
            Console.Clear();
            System.Console.WriteLine($"{name}s turn press enter to continue");
            Console.ReadLine();
            Switcheroo theswitch = makeswitch();
            pokemons.RemoveAt(0);
            theswitch.switchto -= 1;
            theswitch.execute(this, this);
            return false;
        }
        else
        {
            return true;
        }
    }

    public Switcheroo makeswitch()
    {
        while (true)
        {
            List<string> options = new List<string>();
            string svar;
            foreach (Pokemonentity x in pokemons)
            {
                options.Add(x.basepokemon.name);

            }
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 1; i < pokemons.Count; i++)
            {
                if (pokemons[i].basepokemon.name == svar)
                {

                    return new Switcheroo(i); ;
                }
            }
            System.Console.WriteLine($"cant switch to {pokemons[0]} because it is already active");
        }
    }
    public List<string> Display()
    {
        List<string> left = new List<string>();
        left.Add(name);
        left.Add(pokemons[0].basepokemon.name);
        left.Add(pokemons[0].hp.ToString());
        left.Add(pokemons[0].Pokemontype1.name);
        if (pokemons[0].Pokemontype2.name != "empty type")
        {
            left.Add(pokemons[0].Pokemontype2.name);
        }
        if (pokemons[0].staticeffekt != null){
            left.Add(pokemons[0].staticeffekt.id);
        }
        return left;
    }
    public void Makemove(Team opponent)
    {
        Console.Clear();
        System.Console.WriteLine($"{name}s turn press enter to continue");
        Console.ReadLine();
        Pokemonentity Leadpokemon = pokemons[0];
        while (true)
        {
            System.Console.WriteLine("choose action");
            List<string> options = new List<string>();
            options.Add("battle");
            options.Add("switch");
            string svar = Globaldata.battleask("choose action", options);
            if (svar == "battle")
            {
                options.Clear();
                foreach (Move x in pokemons[0].moves)
                {
                    options.Add(x.name);
                }
                options.Add("Go back");
                svar = Globaldata.battleask("Choose a move", options);
                foreach (Move x in pokemons[0].moves)
                {
                    if (x.name == svar)
                    {
                        action = x;
                        return;
                    }

                }

            }
            else if (svar == "switch")
            {
                options.Clear();
                foreach (Pokemonentity x in pokemons)
                {
                    options.Add(x.basepokemon.name);

                }
                options.Add("Go back");
                svar = Globaldata.Ask("Switch to what pokemon?", options);
                if (svar == pokemons[0].basepokemon.name)
                {
                    System.Console.WriteLine($"Cant switch to {pokemons[0].basepokemon.name} because it is already active");
                    Console.ReadLine();
                }
                for (int i = 1; i < pokemons.Count; i++)
                {
                    if (pokemons[i].basepokemon.name == svar)
                    {
                        action = new Switcheroo(i);
                        return;
                    }
                }

            }
        }

    }
}
