using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

//fixa namn väljaren
try{
string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
string Pokemontypechart = Path.Combine(projectDirectory, "pokemontypechart.txt");
string teamdata = Path.Combine(projectDirectory, "TeamData.json");
Globaldata.Loaddata(Pokemontypechart);
Initialize.loadcode();
FileManager.load(teamdata);
Console.Clear();
List<string> options = new List<string>();
options.Add("Team Editor");
options.Add("Start Game");
options.Add("Save");
string svar;
while(true){
    svar = Globaldata.Ask("Simons fantastiska Pokemon Spel", options);
    if (svar == "Team Editor"){
        TeamBuilder.team_editor();
    }
    if (svar == "Start Game"){
        match();
    }
    if (svar == "Save"){
        FileManager.write();
    }
}

}
catch(Exception e){
    System.Console.WriteLine(e);
    Console.ReadLine();
}

List<Team> switcher(List<Team> teams)
{
    Team leadteam = teams[0];
    teams[0] = teams[1];
    teams[1] = leadteam;
    return teams;
}
void match()
{
    Dictionary<string,Team> options = new Dictionary<string, Team>();
    foreach (Team x in Globaldata.teamcollection){
        options[x.previewdisplay()] = x;
    }
    Team player1 = options[Globaldata.Ask($"player1 choose your team",options.Keys.ToList())].Clone();
    System.Console.WriteLine("player 1 choose your name");
    player1.name = Console.ReadLine();
    while(player1.name.Length < 10 && player1.name.Length > 0){
        System.Console.WriteLine("Type a name between 1-10 characters");
        player1.name = Console.ReadLine();
    }
    Team player2 = options[Globaldata.Ask("Player 2 choose your team",options.Keys.ToList())].Clone();
    System.Console.WriteLine("player 2 choose your name");
    player2.name = Console.ReadLine();
    while(player2.name.Length < 10 && player2.name.Length > 0){
        System.Console.WriteLine("Type a name between 1-10 characters");
        player2.name = Console.ReadLine();
    }
    List<Team> teamorder = new List<Team>();
    teamorder.Add(player1);
    teamorder.Add(player2);
    Globaldata.player1 = player1;
    Globaldata.player2 = player2;
    Switcheroo one = player1.set_start(player2);
    Switcheroo two = player2.set_start(player1);
    one.execute(player1,player2);
    two.execute(player2,player1);
    bool matchon = true;
    while (matchon)
    {
        player1.Makemove(teamorder[1]);
        player2.Makemove(teamorder[0]);
        Console.Clear();
        System.Console.WriteLine(teamorder[0].action.priority);
        System.Console.WriteLine(teamorder[1].action.priority);
        Console.ReadLine();
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
        Globaldata.display(Console.WindowWidth-12,0,player2.Display());
        Console.ReadLine();
        Console.Clear();
        Globaldata.display(0,7,teamorder[1].play(teamorder[0]));
        Globaldata.display(0,0,player1.Display());
        Globaldata.display(Console.WindowWidth-10,0,player2.Display());
        Console.ReadLine();
        foreach(Counter x in teamorder[0].pokemons[0].timer.Values){
            x.count();
        }
        foreach(Counter x in teamorder[1].pokemons[0].timer.Values){
            x.count();
        }
        foreach (Endofturn x in teamorder[0].pokemons[0].endofturn.Values){
            
            Console.Clear();
            Globaldata.display(0,7,x.Execute(teamorder[0]));
            Globaldata.display(0,0,player1.Display());
            Globaldata.display(Console.WindowWidth-10,0,player2.Display());
            Console.ReadLine();
        }
        if (teamorder[0].pokemons[0].hp <= 0){
            Globaldata.faintorderadd(teamorder[0]);
        }
        foreach (Endofturn x in teamorder[1].pokemons[0].endofturn.Values){
            Console.Clear();
            Globaldata.display(0,7,x.Execute(teamorder[1]));
            Globaldata.display(0,0,player1.Display());
            Globaldata.display(Console.WindowWidth-10,0,player2.Display());
            Console.ReadLine();
        }
        if (teamorder[1].pokemons[0].hp <= 0){
            Globaldata.faintorderadd(teamorder[1]);
        }
        
        

        foreach (Team x in Globaldata.faintorder)
        {
            if (matchon)
            {

                if (x.makefaint())
                {
                    for (int i = 0; i < teamorder.Count; i++)
                    {

                        if (teamorder[i].name == x.name)
                        {

                            int g = 0;
                            if (i == 0)
                            {
                                g = 1;
                            }
                            System.Console.WriteLine($"{teamorder[g].name} won!!!");
                            System.Console.WriteLine($"{teamorder[i].name} is a loser L");
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
                System.Console.WriteLine($"{x.name} switched to {x.pokemons[0].basepokemon.name}");
            }
        }



    }
}










