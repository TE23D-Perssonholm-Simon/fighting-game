using System.Security.Cryptography.X509Certificates;

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
    public Team(string teamname, List<Pokemonentity> pokemonentities)
    {
        name = teamname;
        pokemons = pokemonentities;

    }

    public Team Clone()
    {
        List<Pokemonentity> pokemonentities = new List<Pokemonentity>();
        foreach (Pokemonentity x in pokemons)
        {
            pokemonentities.Add(x.Clone());
        }
        return new Team(name, pokemonentities);
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
            Console.ReadLine();
        }
    }
    public Switcheroo set_start(Team opp)
    {
        while (true)
        {
            List<string> options = new List<string>();
            string svar;
            foreach (Pokemonentity x in pokemons)
            {
                options.Add(x.basepokemon.name);

            }
            svar = Globaldata.Switch_Ask("Set start pokemon", options,opp);
            for (int i = 0; i < pokemons.Count; i++)
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
        if (pokemons[0].staticeffekt != null)
        {
            left.Add(pokemons[0].staticeffekt.id);
        }
        return left;
    }
    public string previewdisplay()
    {
        List<string> display = new List<string>();
        display.Add($"{name}:");
        foreach (Pokemonentity x in pokemons)
        {
            display.Add(x.basepokemon.name);
        }
        return string.Join(" ", display);


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
    public void edit()
    {
        Dictionary<string, Pokemonentity> options = new Dictionary<string, Pokemonentity>();
        while (true){
        options.Clear();
        options.Add("Change Name",null);
        foreach (Pokemonentity x in pokemons)
        {
            options.Add(x.basepokemon.name, x);
        }
        options.Add("New", null);
        options.Add("Exit", null);
        string svar = Globaldata.Ask(name, options.Keys.ToList());
        if (svar == "Exit")
        {
            return;
        }
        else if (svar == "New")
        {
            if (pokemons.Count >= 6)
            {
                System.Console.WriteLine("Cant add more pokemon limit reached");
                Console.ReadLine();
            }
            else
            {
                make_pokemonentity();
            }
        }
        else if (svar == "Change Name"){
            System.Console.WriteLine("Change to what");
            name = Console.ReadLine();
        }
        else
        {
            List<string> keys = new List<string>();
            keys.Add("Remove");
            keys.Add("Edit");
            keys.Add("Exit");
            string svar2 = Globaldata.Ask($"What do you want to do with {svar}", keys);
            if (svar2 == "Edit")
            {
                editpokemon(options[svar]);
            }
            if (svar2 == "Remove")
            {
                pokemons.Remove(options[svar]);
            }
        }
        }
    }
    public void make_pokemonentity()
    {
        string basepokemon = Globaldata.Ask("What pokemon", Globaldata.Pokedex.Keys.ToList());
        List<string> options = new List<string>();
        List<string> moves = new List<string>();
        foreach (Move x in Globaldata.Pokedex[basepokemon].learnablemoves)
        {
            options.Add(x.name);
        }
        moves.Add(Globaldata.Ask("Choose a move", options));
        moves.Add(Globaldata.Ask("Choose a move", options));
        moves.Add(Globaldata.Ask("Choose a move", options));
        moves.Add(Globaldata.Ask("Choose a move", options));
        pokemons.Add(Initialize.loadpokemonentity(basepokemon, moves[0], moves[1], moves[2], moves[3]));

    }
    public void editpokemon(Pokemonentity thepokemon)
    {
        Dictionary<string, Move> options = new Dictionary<string, Move>();
        while (true)
        {
            options.Clear();
            foreach (Move move in thepokemon.moves)
            {
                options[move.name] = move;
            }
            options.Add("Exit", thepokemon.moves[0]);
            string svar = Globaldata.Ask("What move do you want to edit", options.Keys.ToList());
            if (svar == "Exit")
            {
                return;
            }
            Move replacemove = options[svar];
            options.Clear();
            foreach (Move x in thepokemon.basepokemon.learnablemoves)
            {
                options.Add(x.name, x);
            }
            options.Add("Exit", null);
            string move2 = Globaldata.Ask("Replace it with what move", options.Keys.ToList());
            if (move2 != "Exit"){
            thepokemon.moves.Remove(replacemove);
            thepokemon.moves.Add(options[move2]);
            }

        }
    }
    public team_initializer serielize(){
        List<Pokemonentity_initialize_data> team = new List<Pokemonentity_initialize_data>();
        foreach(Pokemonentity x in pokemons){
            team.Add(x.serielize());
        }
        team_initializer team_ = new team_initializer();
        team_.name = name;
        team_.team = team;
        return team_;
    }


}
