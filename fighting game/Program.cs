using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

//fixa namn väljaren

string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
string Pokemontypechart = Path.Combine(projectDirectory, "pokemontypechart.txt");
Globaldata.Loaddata("hi", Pokemontypechart);
Initialize.loadcode();
Console.Clear();

match(Globaldata.teamcollection[0].Clone(), Globaldata.teamcollection[0].Clone());

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
    Globaldata.player1 = player1;
    Globaldata.player2 = player2;

    bool matchon = true;
    while (matchon)
    {
        teamorder[0].Makemove(teamorder[1]);
        teamorder[1].Makemove(teamorder[0]);
        Console.Clear();
        if (teamorder[0].action.priority == teamorder[1].action.priority)
        {
            if (teamorder[1].pokemons[0].speed*teamorder[1].pokemons[0].Paralysis > teamorder[0].pokemons[0].speed*teamorder[0].pokemons[0].Paralysis)
            {
                teamorder = switcher(teamorder);
            }
        }
        else if (teamorder[1].action.priority > teamorder[0].action.priority)
        {
            teamorder = switcher(teamorder);
        }
        
        Globaldata.display(0,7,teamorder[0].play(teamorder[1]));
        Globaldata.display(0,0,player1.Display());
        Globaldata.display(Console.WindowWidth-10,0,player2.Display());
        Console.ReadLine();
        Console.Clear();
        Globaldata.display(0,7,teamorder[1].play(teamorder[0]));
        Globaldata.display(0,0,player1.Display());
        Globaldata.display(Console.WindowWidth-10,0,player2.Display());
        Console.ReadLine();
        foreach (Endofturn x in teamorder[0].pokemons[0].endofturn.Values){
            
            Console.Clear();
            Globaldata.display(0,7,x.Execute(teamorder[0]));
            Globaldata.display(0,0,player1.Display());
            Globaldata.display(Console.WindowWidth-10,0,player2.Display());
            Console.ReadLine();
        }
        foreach (Endofturn x in teamorder[1].pokemons[0].endofturn.Values){
            Console.Clear();
            Globaldata.display(0,7,x.Execute(teamorder[1]));
            Globaldata.display(0,0,player1.Display());
            Globaldata.display(Console.WindowWidth-10,0,player2.Display());
            Console.ReadLine();
        }
        
        

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

                            int g = 0;
                            if (i == 0)
                            {
                                g = 1;
                            }
                            System.Console.WriteLine($"{teamorder[g].playername} won!!!");
                            System.Console.WriteLine($"{teamorder[i].playername} is a loser L");
                            Console.ReadLine();
                            matchon = false;
                        }
                    }
                }

            }

        }
        Globaldata.faintorder.Clear();
        if (matchon)
        {
            foreach (Team x in Globaldata.faintorder)
            {
                System.Console.WriteLine($"{x.playername} switched to {x.pokemons[0].basepokemon.name}");
            }
        }



    }
}










