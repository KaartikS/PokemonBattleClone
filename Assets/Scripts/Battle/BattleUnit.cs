using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    
    public Pokemon Pokemon { get; set; }
    Image image;
    Vector3 originalImagePos;
    Color originalColour;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalImagePos = image.transform.localPosition;
        originalColour = image.color;
    }

    public void Setup()
    {
        Pokemon = new Pokemon(_base, level);
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }

        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, originalImagePos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalImagePos.y);

        image.transform.DOLocalMoveX(originalImagePos.x, 1f);

    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();

        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalImagePos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalImagePos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalImagePos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColour, 0.1f));

    }

    public void PlayerFaintAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.transform.DOLocalMoveY(originalImagePos.y -150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
