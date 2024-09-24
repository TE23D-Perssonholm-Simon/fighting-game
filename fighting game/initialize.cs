using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

public static class Initialize
{
    public static void Loadstatuseffekt(string name, List<Statuscomponent> components, string inflictmessage)
    {
        List<Statuscomponent> statuscomponents = new List<Statuscomponent>();
        foreach (Statuscomponent x in components)
        {
            statuscomponents.Add(x);
        }
        new Statuseffekt(name, statuscomponents, inflictmessage);

    }
    public static void Loadmove(string name, int priority, Effect effect, List<Effect> effects)
    {
        List<Effect> effects1 = new List<Effect>();
        foreach (Effect x in effects)
        {
            effects1.Add(x);
        }
        new Move(name, priority, effect, effects1);
    }

    public static void loadpokemon(string name, int hp, int attack, int def, int special_attack, int special_defence, int speed, string type1, string type2, List<string> moves)
    {
        List<string> learnablemoves = new List<string>();
        foreach (string x in moves)
        {
            learnablemoves.Add(x);
        }
        new Pokemon(name, hp, attack, def, special_attack, special_defence, speed, type1, type2, learnablemoves);
    }
    public static Pokemonentity loadpokemonentity(string basepokemonid,string move1,string move2,string move3,string move4){
        Pokemon basepokemon = Globaldata.Pokedex[basepokemonid];
        List<string> moves = new List<string>();
        moves.Add(move1);
        moves.Add(move2);
        moves.Add(move3);
        moves.Add(move4);
        return new Pokemonentity(basepokemon,moves);
    }
    public static void loadcode()
    {
        List<Statuscomponent> components = new List<Statuscomponent>();
        //25
        components.Add(new Movehinderer(25, 0, "was paralyzed", "is paralyzed", ""));
        Loadstatuseffekt("Paralysis", components, "was paralysed");
        components.Clear();
        components.Add(new Basicendofturn("burn", 16));
        Loadstatuseffekt("Burn", components, "got burned");
        components.Clear();

        System.Console.WriteLine("Status effects loaded");
        Console.ReadLine();

        List<Effect> effects = new List<Effect>();
        effects.Add(new Staticeffectgiver(100, "Paralysis"));
        Loadmove("Thunder Wave", 0, new Statusmove(90), effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(10, "Paralysis"));
        Loadmove("Thunder Bolt", 0, new Special(90, 100, "electric"), effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(30, "Paralysis"));
        Loadmove("Body Slam", 0, new Physical(85, 100, "normal"), effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(100, "Burn"));
        Loadmove("Will-O-Wisp",0,new Statusmove(85),effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(10,"Burn"));
        Loadmove("Flamethrower",0,new Special(90,100,"fire"),effects);

        System.Console.WriteLine("Moves loaded");
        Console.ReadLine();

        List<string> moves = new List<string>();
        moves.Add("Body Slam");
        moves.Add("Will-O-Wisp");
        moves.Add("Flamethrower");
        loadpokemon("Charizard", 266, 155, 144, 200, 157, 184, "fire", "flying", moves);
        moves.Clear();
        moves.Add("Thunder Bolt");
        moves.Add("Thunder Wave");
        moves.Add("Body Slam");
        moves.Add("Thunder Bolt");
        loadpokemon("Pikachu", 230, 166, 103, 166, 148, 184, "electric", "empty type", moves);
        moves.Clear();


        System.Console.WriteLine("Pokemon loaded");
        Console.ReadLine();
        List<Pokemonentity> pokemonentities = new List<Pokemonentity>();
        pokemonentities.Add(loadpokemonentity("Charizard","Flamethrower","Will-O-Wisp","Body Slam","Flamethrower"));
        pokemonentities.Add(loadpokemonentity("Pikachu", "Thunder Bolt","Thunder Wave","Body Slam","Thunder Bolt"));
        Globaldata.teamcollection.Add(new Team(pokemonentities));

        System.Console.WriteLine("Teams loaded");
        Console.ReadLine();





    }
}