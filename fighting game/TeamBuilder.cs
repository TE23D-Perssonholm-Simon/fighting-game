public static class TeamBuilder{
    public static void team_editor(){
        while(true){
        Dictionary<string,Team> keys = new Dictionary<string, Team>();
        List<Team> teams = Globaldata.teamcollection;
        for (int i = 0; i < teams.Count && i<50; i++){
            keys.Add(teams[i].previewdisplay(),teams[i]);
        }
        keys.Add("Exit",teams[0]);
        string answer = Globaldata.Ask("Team Editor",keys.Keys.ToList());
        if (answer == "Exit"){
            return;
        }
        keys[answer].edit();
        }

    }
}