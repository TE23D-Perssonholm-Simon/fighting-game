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
    public override void execute(Team one, Team two)
    {
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
        System.Console.WriteLine(name);
        if (damadgeeffect.Play(one, two))
        {
            foreach (Effect x in effects)
            {
                x.Play(one, two);
            }
        }

    }

}
public abstract class Action
{
    public int priority;
    public abstract void execute(Team attack, Team defend);
}


public class Switcheroo : Action
{
    public int switchto;
    new public int priority = 10;
    public Switcheroo(int s)
    {
        switchto = s;
    }
    public override void execute(Team attack, Team defend)
    {
        Pokemonentity leadpokemon = attack.pokemons[0];
        attack.pokemons[0] = attack.pokemons[switchto];
        attack.pokemons[switchto] = leadpokemon;
        System.Console.WriteLine($"{leadpokemon} switched out");
        System.Console.WriteLine($"{attack.pokemons[0]} switched in");

    }
}

public class Faint : Action
{
    new public int priority;

    public Faint(int priority)
    {
        this.priority = priority;
    }
    public override void execute(Team attack, Team defend)
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
    }
}