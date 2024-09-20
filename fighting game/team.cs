public class Team
{
    public List<Pokemonentity> pokemons = new List<Pokemonentity>();
    public string playername;



    public Action action;
    public void play(Team opponent)
    {
        if (action != null)
        {
            action.execute(this, opponent);
        }
    }
    public Team(List<String> strings)
    {
        foreach (string x in strings)
        {
            pokemons.Add(Globaldata.pokeid[x]);
        }

    }

    public void Faint(int priority)
    {
        Globaldata.addfaint(this);
        action = null;
    }

    public bool makefaint()
    {
        System.Console.WriteLine($"{playername}s turn press enter to continue");
        Console.ReadLine();
        if (pokemons.Count > 1)
        {
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
    public void Makemove(Team opponent)
    {
        Console.Clear();
        System.Console.WriteLine($"{playername}s turn press enter to continue");
        Console.ReadLine();
        Pokemonentity Leadpokemon = pokemons[0];
        while (true)
        {
            System.Console.WriteLine("choose action");
            List<string> options = new List<string>();
            options.Add("battle");
            options.Add("switch");
            string svar = Globaldata.Ask("choose action", options);
            if (svar == "battle")
            {
                options.Clear();
                foreach (Move x in pokemons[0].moves)
                {
                    options.Add(x.name);
                }
                options.Add("Go back");
                svar = Globaldata.Ask("Choose a move", options);
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
                for (int i = 0; i < pokemons.Count; i++)
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
