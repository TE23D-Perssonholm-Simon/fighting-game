public static class TeamBuilder
{
    public static void team_editor()
    {
        while (true)
        {
            Dictionary<string, Team> keys = new Dictionary<string, Team>();
            List<Team> teams = Globaldata.teamcollection;
            string teamkey;
            for (int i = 0; i < teams.Count && i < 50; i++)
            {
                teamkey = teams[i].previewdisplay();
                
                if(!keys.ContainsKey(teamkey)){
                    keys.Add(teamkey, teams[i]);
                }
                else{
                    teams[i].name = $"{teams[i].name} 2";
                }
            }
            keys.Add("Create New",null);
            keys.Add("Exit", teams[0]);
            
            string answer = Globaldata.Ask("Team Editor", keys.Keys.ToList());
            if (answer == "Exit")
            {
                FileManager.Write();
                return;
            }
            else if (answer == "Create New"){
                teams.Add(new Team("",new List<Pokemonentity>()));
            }
            else{
                keys[answer].edit();
            }
        }

    }
}