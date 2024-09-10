using System.Security.Cryptography.X509Certificates;

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

public abstract class Action{
    public abstract void Play(Pokemonentity a, Pokemonentity d);
}

public class Switcheroo:Action{
    int switchto;
    Switcheroo(int s){
        switchto = s;
    }

    public override void Play(Pokemonentity a, Pokemonentity d){
        
    }
    
}
public class Move:Action{
    int accuracy;
    int power;
    Type type;
    List<Effect> effects = new List<Effect>();

    public Move(Type t,int a, int p, List<Effect> e){
        effects = e;
        type = t;
        accuracy = a;
        power = p;
    }

    public override void Play(Pokemonentity attacker, Pokemonentity defender){
        int random = Random.Shared.Next(100);
        if (random <= accuracy){
            foreach (Effect x in effects){
                x.play(attacker,defender);
            }
        }
    }
    


    
}
public abstract class Effect{
    public abstract void play(Pokemonentity a, Pokemonentity d);
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

