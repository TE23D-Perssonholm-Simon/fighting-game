using System.Buffers;
using System.Reflection.Metadata;

public class Pokemontype
{
    public string name;
    public int chartnr;

    public Pokemontype(string name, int chartnr)
    {
        this.name = name;
        this.chartnr = chartnr;
    }

}

public class Pokemon
{
    public int hp, attack, def, speed ,spattack,spdef;
    public string name;

    public Pokemontype Pokemontype1;
    public Pokemontype Pokemontype2;
    public List<Move> learnablemoves = new List<Move>();

    public Pokemon(string name, int hp, int attack, int def, int special_attack, int special_defence,int speed,string pokemontypeid,string pokemontypeid2,List<string> learnable_moves)
    {
        
        this.name = name;
        this.hp = hp;
        this.attack = attack;
        this.def = def;
        spattack = special_attack;
        spdef = special_defence;
        this.speed = speed;
        this.Pokemontype1 = Globaldata.Pokemontypes[pokemontypeid];
        this.Pokemontype2 = Globaldata.Pokemontypes[pokemontypeid2];
        foreach(string x in learnable_moves){
            learnablemoves.Add(Globaldata.movedict[x]);
        }
        
    }

}
public class Pokemonentity
{
    public Pokemon basepokemon;
    public int hp, maxhp, def, attack, speed,spattack,spdef;

    int _defbuff = 0;

    public float Paralysis = 1;
    public float burn = 1;
    public float defbuff
    {
        get{
            if(_defbuff <= 0){
                return (float)Math.Pow(2,_defbuff);
            }
            else{
                return _defbuff;
            }
        }
        set{
            _defbuff = int.Min((int)value, 6);
            _defbuff = int.Max(_defbuff,-6);
            def = (int)(def*defbuff);
        }
    }
    int _attackbuff = 0;
    public float attackbuff
    {
        get{
            if(_attackbuff <= 0){
                return (float)Math.Pow(2,_attackbuff);
            }
            else{
                return _attackbuff;
            }
        }
        set{
            _attackbuff = int.Min((int)value, 6);
            _attackbuff = int.Max(_attackbuff,-6);
            attack = (int)(attack*attackbuff);
        }
    }
    int _speedbuff = 0;
    public float speedbuff
    {
        get{
            if(_speedbuff <= 0){
                return (float)Math.Pow(2,_speedbuff);
            }
            else{
                return _speedbuff;
            }
        }
        set{
            _speedbuff = int.Min((int)value, 6);
            _speedbuff = int.Max(_speedbuff,-6);
            speed = (int)(speed*speedbuff);
        }
    }
    int _spdefbuff = 0;
    public float spdefbuff
    {
        get{
            if(_spdefbuff <= 0){
                return (float)Math.Pow(2,_spdefbuff);
            }
            else{
                return _spdefbuff;
            }
        }
        set{
            _spdefbuff = int.Min((int)value, 6);
            _spdefbuff = int.Max(_spdefbuff,-6);
            spdef = (int)(spdef*spdefbuff);
        }
    }
    int _spattackbuff = 0;
    public float spattackbuff
    {
        get{
            if(_spattackbuff <= 0){
                return (float)Math.Pow(2,_spattackbuff);
            }
            else{
                return _spattackbuff;
            }
        }
        set{
            _spattackbuff = int.Min((int)value, 6);
            _spattackbuff = int.Max(_spattackbuff,-6);
            spattack = (int)(spattack*spattackbuff);
        }
    }
    public Pokemontype Pokemontype1, Pokemontype2;
    public Statuseffekt staticeffekt = null;
    public List<Statuseffekt> statuseffekts = new List<Statuseffekt>();
    public Dictionary<string,Movehinderer> movehinderer = new Dictionary<string, Movehinderer>();
    public Dictionary<string,Statuscomponent> forced = new Dictionary<string, Statuscomponent>();
    public Dictionary<string,Statuscomponent> noswitch = new Dictionary<string, Statuscomponent>();
    public Dictionary<string,Statuscomponent> endofturn = new Dictionary<string, Statuscomponent>();
    public Dictionary<string,Statuscomponent> timer = new Dictionary<string, Statuscomponent>();
    public List<Move> moves = new List<Move>();
    public Pokemonentity(List<string> strings)
    {
        
        basepokemon = Globaldata.Pokedex[strings[1]];
        hp = basepokemon.hp;
        def = basepokemon.def;
        spdef = basepokemon.spdef;
        attack = basepokemon.attack;
        spattack = basepokemon.spattack;
        speed = basepokemon.speed;
        Pokemontype1 = basepokemon.Pokemontype1;
        Pokemontype2 = basepokemon.Pokemontype2;
        maxhp = hp;

        moves.Add(Globaldata.movedict[strings[2]]);
        moves.Add(Globaldata.movedict[strings[3]]);
        moves.Add(Globaldata.movedict[strings[4]]);
        moves.Add(Globaldata.movedict[strings[5]]);
    }
}