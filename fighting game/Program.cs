using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
string Pokemontypechart = Path.Combine(projectDirectory, "pokemontypechart.txt");
Globaldata.Loaddata("hi", Pokemontypechart);
Console.Clear();

match(Globaldata.teamcollection[0],Globaldata.teamcollection[1]);

List<Team> switcher(List<Team> teams)
{
    Team leadteam = teams[0];
    teams[0] = teams[1];
    teams[1] = leadteam;
    return teams;
}
void match(Team player1, Team player2)
{
    System.Console.WriteLine("player 1 choose your name");
    player1.playername = Console.ReadLine();
    System.Console.WriteLine("player 2 choose your name");
    player2.playername = Console.ReadLine();
    List<Team> teamorder = new List<Team>();
    teamorder.Add(player1);
    teamorder.Add(player2);

    bool matchon = true;
    while (matchon)
    {
        teamorder[0].Makemove(teamorder[1]);
        teamorder[1].Makemove(teamorder[0]);
        Console.Clear();
        if (teamorder[0].action.priority == teamorder[1].action.priority)
        {
            if (teamorder[1].pokemons[0].speed > teamorder[0].pokemons[0].speed)
            {
                teamorder = switcher(teamorder);
            }
        }
        else if (teamorder[1].action.priority > teamorder[0].action.priority)
        {
            teamorder = switcher(teamorder);
        }
        teamorder[0].play(teamorder[1]);
        teamorder[1].play(teamorder[0]);
        Console.ReadLine();

        foreach (Team x in Globaldata.faintorder)
        {
            if (matchon)
            {
                if (x.makefaint())
                {
                    for (int i = 0; i < teamorder.Count; i++)
                    {
                        if (teamorder[i].playername == x.playername)
                        {
                            System.Console.WriteLine($"{teamorder[i - 1].playername} won!!!");
                            System.Console.WriteLine($"{teamorder[i].playername} is a loser L");
                            matchon = false;
                        }
                    }
                }
            }
        }
        if (matchon)
        {
            foreach (Team x in Globaldata.faintorder)
            {
                System.Console.WriteLine($"{x.playername} switched to {x.pokemons[0].basepokemon.name}");
            }
        }



    }
}






public class Statuseffekt
{
    public List<Statuscomponent> components = new List<Statuscomponent>();
    public Statuseffekt(List<Statuscomponent> comp)
    {
        components = comp;
    }
}
public abstract class Statuscomponent
{

}
public abstract class Movecomponent : Statuscomponent
{
    public abstract Effect run(Effect themove);
}



public abstract class Effect
{
    public abstract bool Play(Team a, Team d);
    public string failmessage;
}

public class Damadge : Effect
{
    int power;
    int accuracy;
    Pokemontype Pokemontype;

    public Damadge(List<String> parameters)
    {
        power = int.Parse(parameters[0]);
        this.accuracy = int.Parse(parameters[1]);
        this.Pokemontype = Globaldata.Pokemontypes[parameters[2]];
    }
    public override bool Play(Team a, Team d)
    {
        int randomnr = Random.Shared.Next(100);
        Pokemonentity attacker;
        attacker = a.pokemons[0];
        Pokemonentity defender = d.pokemons[0];
        if (randomnr > accuracy)
        {
            System.Console.WriteLine("Missed");
            return false;
        }
        float crit = 1;
        float stab;
        if (Pokemontype == attacker.Pokemontype1 || Pokemontype == attacker.Pokemontype2)
        {
            stab = 1.5f;
        }
        else
        {
            stab = 1.0f;
        }
        float Pokemontypeadvantage = (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype1.chartnr] - '0') * (Globaldata.Pokemontypechartlines[Pokemontype.chartnr][defender.Pokemontype2.chartnr] - '0') / 4;
        if (Pokemontypeadvantage == 0)
        {
            System.Console.WriteLine("is immune");
            return false;
        }
        int damadge = (int)(((40 * crit + 2) * power * attacker.attack / defender.def / 50 + 2) * stab * Pokemontypeadvantage);
        defender.hp -= damadge;
        if (defender.hp > 0)
        {
            System.Console.WriteLine($"{attacker.basepokemon.name} dealt {damadge.ToString()} to {defender.basepokemon.name} remaining hp {defender.hp.ToString()}");
        }
        else
        {
            System.Console.WriteLine($"{defender.basepokemon.name} fainted");
        }

        return true;
    }

}