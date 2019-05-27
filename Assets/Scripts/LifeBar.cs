using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Image lifeFull;
    public Image lifeEmpty;

    private List<Image> lives = new List<Image>();

    private int fullLives;
    private int emptyLives;

    public void AddLife(int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            if (emptyLives > 0)
            {
                lives[lives.Count - emptyLives].sprite = lifeFull.sprite;
                Image newLife = Instantiate(lifeEmpty);
                newLife.transform.SetParent(this.transform, false);
                lives.Add(newLife);
                fullLives += 1;
            }
            else
            {
                Image newLife = Instantiate(lifeFull);
                newLife.transform.SetParent(this.transform, false);
                lives.Add(newLife);
                fullLives += 1;
            }
        }
    }

    public void SetMaxLives(int ammount)
    {
        lives.ForEach(image => { Destroy(image.gameObject); });
        lives.Clear();
        for (int i = 0; i < ammount; i++)
        {
            Image newLife = Instantiate(lifeFull);
            newLife.transform.SetParent(this.transform, false);
            lives.Add(newLife);
        }
        fullLives = ammount;
        emptyLives = 0;
    }

    public void RemoveLife(int ammount)
    {
        Destroy(lives[lives.Count - 1].gameObject);
        lives.RemoveAt(lives.Count - 1);
    }

    public void GainLife(int ammount)
    {
        if (ammount > 0)
            for (int i = 0; i < ammount; i++)
            {
                if (emptyLives > 0)
                {
                    lives[lives.Count - emptyLives].sprite = lifeFull.sprite;
                    emptyLives -= 1;
                    fullLives += 1;
                }
            }
    }

    public void LooseLife(int ammount)
    {
        if (ammount < 0)
            for (int i = 0; i < -ammount; i++)
            {
                if (fullLives > 0)
                {
                    emptyLives += 1;
                    fullLives -= 1;
                    lives[fullLives].sprite = lifeEmpty.sprite;
                }
            }

    }

}
