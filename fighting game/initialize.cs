public static class Initialize{
    public static void loadcode(){
        List<Statuscomponent> components = new List<Statuscomponent>();
        components.Add(new Movehinderer(25,0,"was paralyzed","is paralyzed",""));
        new Statuseffekt("Paralysis", components,"was paralysed");
        components.Clear();
        List<Effect> effects = new List<Effect>();
        effects.Add(new Staticeffectgiver(100,"Paralysis"));
        new Move("Thunder wave",0,new Statusmove(90),effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(10,"Paralysis"));
        new Move("Thunder Bolt",0,new Special(90,100,Globaldata.Pokemontypes["electric"]),effects);
        effects.Clear();
        effects.Add(new Staticeffectgiver(30,"Paralysis"));
        new Move("Body Slam",0,new Physical(85,100,"normal"),effects);
        List<string> moves = new List<string>();
        moves.Add("Body Slam");
        moves.Add("Flamethrower");
        new Pokemon("Charizard",266,155,144,200,157,184,"fire","flying",moves);
    }
}