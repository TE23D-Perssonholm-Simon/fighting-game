using System.Security.Cryptography.X509Certificates;

public class Statuseffekt
{
    public string id;
    public string inflictmessage;
    public List<Statuscomponent> components = new List<Statuscomponent>();
    public Statuseffekt(string id, List<Statuscomponent> comp, string inflictmessage)
    {
        this.id = id;
        components = comp;
        this.inflictmessage = inflictmessage;
        Globaldata.statuseffectddict[id] = this;

    }

    public Statuseffekt Clone()
    {
        Statuseffekt effekt = new Statuseffekt(id, components,inflictmessage);
        return effekt;

    }
    public void Infect(Pokemonentity Prey)
    {
        foreach (Statuscomponent x in components)
        {
            switch (x)
            {
                case Movehinderer movehinderer:
                    Prey.movehinderer[id] = movehinderer;
                    break;



                default:
                    break;
            }
        }
    }
}
public abstract class Statuscomponent
{
    public void remove(Pokemonentity attacker)
    {
        string id = "";
        if (attacker.staticeffekt.components.Contains(this))
        {
            id = attacker.staticeffekt.id;
            attacker.staticeffekt = null;
        }
        else
        {
            foreach (Statuseffekt x in attacker.statuseffekts)
            {
                if (x.components.Contains(this))
                {
                    id = x.id;
                    attacker.statuseffekts.Remove(x);
                }
            }
        }
        if (id != "")
        {
            attacker.endofturn[id] = null;
            attacker.noswitch[id] = null;
            attacker.forced[id] = null;
            attacker.movehinderer[id] = null;
            attacker.timer[id] = null;
        }

        if (id == "Paralysis")
        {
            attacker.Paralysis = 1;
        }
    }
}
public class Movehinderer : Statuscomponent
{
    int oddsofstopping;
    int oddsofremoval;
    string curemessage;
    string failmessage;
    string intromessage;

    public Movehinderer(int stop, int remove, string fail, string intro, string cure)
    {
        oddsofstopping = stop;
        oddsofremoval = remove;
        failmessage = fail;
        intromessage = intro;
        curemessage = cure;
    }

    public List<string> Run(Pokemonentity attacker)
    {
        List<string> displaymessage = new List<string>();
        displaymessage.Add(intromessage);
        int randomnr = Random.Shared.Next(100);
        if (randomnr > oddsofremoval)
        {
            displaymessage.Add("hi");
            displaymessage.Add(curemessage);
            remove(attacker);
            return displaymessage;
        }
        randomnr = Random.Shared.Next(100);
        if (oddsofstopping < randomnr)
        {
            displaymessage.Add("fail");
            displaymessage.Add(failmessage);
        }
        else
        {
            displaymessage.Add("hi");
        }
        return displaymessage;
    }
}


