public static class Initialize{
    public static void loadcode(){
        List<Statuscomponent> components = new List<Statuscomponent>();
        components.Add(new Movehinderer(25,0,"was paralyzed","is paralyzed",""));
        new Statuseffekt("Paralysis", components,"was paralysed");
        components.Clear();
        System.Console.WriteLine("Status effects loaded");
        Console.ReadLine();
        List<Effect> effects = new List<Effect>();
        effects.Add(new Staticeffectgiver(100,"Paralysis"));
        new Move("Thunder Wave",0,new Statusmove(90),effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(10,"Paralysis"));
        new Move("Thunder Bolt",0,new Special(90,100,Globaldata.Pokemontypes["electric"]),effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(30,"Paralysis"));
        new Move("Body Slam",0,new Physical(85,100,"normal"),effects);

        System.Console.WriteLine("Moves loaded");
        Console.ReadLine();

        List<string> moves = new List<string>();
        moves.Add("Body Slam");
        //moves.Add("Flamethrower");
        new Pokemon("Charizard",266,155,144,200,157,184,"fire","flying",moves);
        moves.Clear();
        moves.Add("Thunder Bolt");
        moves.Add("Thunder Wave");
        moves.Add("Body Slam");
        new Pokemon("Pikachu",230,166,103,166,148,184,"electric","empty type",moves);
        moves.Add("Thunder Bolt");

        System.Console.WriteLine("Pokemon loaded");
        Console.ReadLine();
        List<Pokemonentity> pokemonentities = new List<Pokemonentity>();
        pokemonentities.Add(new Pokemonentity(Globaldata.Pokedex["Pikachu"],moves));
        Globaldata.teamcollection.Add(new Team(pokemonentities));

        System.Console.WriteLine("Teams loaded");
        Console.ReadLine();
        
        


        
    }
}