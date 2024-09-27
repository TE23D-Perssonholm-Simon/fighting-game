using System.Text.Json;

public static class FileManager{
    static string filepath;
    public static void load(string filepath){
        FileManager.filepath = filepath;
        string jsonfile = File.ReadAllText(filepath);
        Jsonclass teams = JsonSerializer.Deserialize<Jsonclass>(jsonfile);
        System.Console.WriteLine(teams == null);
        Console.ReadLine();
        System.Console.WriteLine(teams.list == null);
        Console.ReadLine();
        foreach (team_initializer x in teams.list){
            Globaldata.teamcollection.Add(x.Initialize());
        }
    }
    public static void write(){
        List<team_initializer> team_Initializers = new List<team_initializer>();
        foreach(Team x in Globaldata.teamcollection){
            team_Initializers.Add(x.serielize());
        }
        Jsonclass data = new Jsonclass(team_Initializers);
        string Jsonstring = JsonSerializer.Serialize(data,new JsonSerializerOptions{WriteIndented = true});
        File.WriteAllText(FileManager.filepath,Jsonstring);
    }
}
public class Pokemonentity_initialize_data{
    public string basepokemonid{ get; set;}
    public List<string> moves{get;set;}
    public Pokemonentity Initialize(){
        return new Pokemonentity(Globaldata.Pokedex[basepokemonid],moves);

    }
    public Pokemonentity_initialize_data(string basepokemonid,List<string> moves){
        this.basepokemonid = basepokemonid;
        this.moves = moves;
    }
    
}
public class team_initializer{
    public string name{get;set;}
    public List<Pokemonentity_initialize_data> team {get; set;}
    public Team Initialize(){
        List<Pokemonentity> pokemons = new List<Pokemonentity>();
        foreach(Pokemonentity_initialize_data x in team){
            pokemons.Add(x.Initialize());
        }
        return new Team(name,pokemons);
    }
    public team_initializer(string name,List<Pokemonentity_initialize_data> pokemons){
        this.name = name;
        team = pokemons;
    }
}

public class Jsonclass{
    public List<team_initializer> list {get; set;}
    public Jsonclass(List<team_initializer> teams){
        list = teams;
    }
}