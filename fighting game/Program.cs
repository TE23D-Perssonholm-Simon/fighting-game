Dictionary<string,Pokemon> Pokedex = new Dictionary<string, Pokemon>();
Dictionary<string,Type> types = new Dictionary<string, Type>();

public class Pokemonentity{
    Pokemon basepokemon;
    int hp,maxhp,def,attack,speed;
    Type type1, type2;
    public Pokemonentity(Pokemon e){
        basepokemon = e;
        hp = basepokemon.hp;
        def = basepokemon.def;
        attack = basepokemon.attack;
        speed = basepokemon.speed;
        type1 = basepokemon.type1;
        type2 = basepokemon.type2;
        hp = maxhp;
    }
}

public class Move{
    int power;


    
}
public abstract class Effect{
    public abstract void play();
}
public class Type{
    string name;
    int chartnr;

}

public class Pokemon{
    public int hp,attack,def,speed;
    public string name;

    public Type type1;
    public Type type2;

    public Pokemon(int hp, int attack, int def, int speed, Type type1, Type type2){
        this.hp = hp;
        this.attack = attack;
        this.def = def;
        this.speed = speed;
        this.type1 = type1;
        this.type2 = type2;
    }

}

