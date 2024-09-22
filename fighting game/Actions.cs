public class Move : Action
{
    public string name;
    new public int priority;
    List<Effect> effects = new List<Effect>();
    Effect damadgeeffect;
    public Move(List<string> strings)
    {
        name = strings[0];
        this.damadgeeffect = Globaldata.effectdict[strings[1]];
        for (int i = 2; i < strings.Count - 1; i++)
        {
            effects.Add(Globaldata.effectdict[strings[i]]);
        }
        priority = int.Parse(strings[strings.Count - 1]);
    }
    public override List<string> execute(Team one, Team two)
    {
        List<string> displaystrings = new List<string>();
        Effect localdamadgeeffect = damadgeeffect;
        Pokemonentity attacker = one.pokemons[0];
        Pokemonentity defender = two.pokemons[0];
        foreach (Statuseffekt x in attacker.statuseffekts)
        {
            foreach (Statuscomponent e in x.components)
            {
                if (e is Movecomponent movecomponent)
                {
                    localdamadgeeffect = movecomponent.run(localdamadgeeffect);
                }

            }
        }
        if (attacker.staticeffekt != null)
        {
            foreach (Statuscomponent e in attacker.staticeffekt.components)
            {
                if (e is Movecomponent movecomponent)
                {
                    localdamadgeeffect = movecomponent.run(localdamadgeeffect);
                }

            }
        }
        displaystrings.Add(name);
        displaystrings.AddRange(damadgeeffect.Play(one, two));
        if (displaystrings[1] != "false")
        {
            foreach (Effect x in effects)
            {
                displaystrings.AddRange(x.Play(one, two));
            }
        }
        else {
            displaystrings.RemoveAt(1);
        }
        if (defender.hp < 0){
            two.Faint(10);
        }
        return displaystrings;

    }

}
public abstract class Action
{
    public virtual int priority{get; set;} = 0;
    public abstract List<string> execute(Team attack, Team defend);
}


public class Switcheroo : Action
{
    public int switchto;
    public override int priority{get; set;} = 10;
    public Switcheroo(int s)
    {
        switchto = s;
    }
    public override List<string> execute(Team attack, Team defend)
    {
        List<string> displaystrings = new List<string>();
        Pokemonentity leadpokemon = attack.pokemons[0];
        attack.pokemons[0] = attack.pokemons[switchto];
        attack.pokemons[switchto] = leadpokemon;
        displaystrings.Add($"{leadpokemon.basepokemon.name} switched out");
        displaystrings.Add($"{attack.pokemons[0].basepokemon.name} switched in");
        return displaystrings;

    }
}

public class Faint : Action
{
    new public int priority;

    public Faint(int priority)
    {
        this.priority = priority;
    }
    public override List<string> execute(Team attack, Team defend)
    {
        if (attack.pokemons.Count > 1)
        {
            attack.pokemons.RemoveAt(0);
            List<string> options = new List<string>();
            string svar;
            foreach (Pokemonentity x in attack.pokemons)
            {
                options.Add(x.basepokemon.name);

            }
            svar = Globaldata.Ask("Switch to what pokemon?", options);
            for (int i = 0; i < attack.pokemons.Count; i++)
            {
                if (attack.pokemons[i].basepokemon.name == svar)
                {
                    Action switchto = new Switcheroo(i);
                    switchto.execute(attack, attack);
                    
                }
            }
        }
        return (new List<string>());
    }

}