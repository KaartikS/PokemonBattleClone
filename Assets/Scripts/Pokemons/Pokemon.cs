using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        // Generate Moves
        Moves = new List<Move>();
        foreach(var  move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4)
            {
                break;
            }
        }

    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }

    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }

    }

    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }

    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }

    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }

    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;

        if (UnityEngine.Random.value * 100f <= 6.25)
        {
            critical = 2f;
        }

        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };

        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;


        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * typeEffectiveness * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;

        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
            return damageDetails;
        }
        else
        {
            return damageDetails;
        }
    }

    public Move GetRandomMove()
    {
        int randomIndex = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[randomIndex];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }

}
