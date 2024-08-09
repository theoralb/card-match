using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GameController : MonoBehaviour
{
    public int[] CardIDData;
    private string gameStage = "ready";
    private int col = 4;
    private int row = 3;
    [SerializeField] private Slider rowSlide;
    [SerializeField] private Slider colSlide;
    [SerializeField]private TextMeshProUGUI buttonText;
    [SerializeField]private TextMeshProUGUI turnText;
    [SerializeField]private TextMeshProUGUI matchText ;
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private GameObject cardMaster;
    private List<Card> allCard = new List<Card>();
    public Card First;
    public int Turn = 0;
    public int Match = 0;
    public int Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Init Game-State Coroutine Here
        StartCoroutine(GameStateController());
    }

    IEnumerator GameStateController()
    {
       ResetCard();
       yield return null;
    }


    public static void SecureShuffle(int[] array)
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                byte[] box = new byte[1];
                do rng.GetBytes(box); while (!(box[0] < i * (byte.MaxValue / i)));
                int j = (box[0] % (i + 1));

                // Swap elements array[i] and array[j]
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }


    public void ResetCard(int[] inData=null,int inCol=-1,int inRow = -1 )
    {
        if (inData == null)
        {
            CardIDData = new int[col * row];
            //card Data
            for (int i = 0; i < col * row; i++)
            {
                CardIDData[i] = (i / 2) + 1;
            }
            SecureShuffle(CardIDData);
        }else
        {
            CardIDData = inData;
            col = inCol;
            row = inRow;
        }




        //clear cards
        foreach (var card in allCard)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }            
        }

        allCard.Clear();

        //cards Display
        for (int i = 0; i < col * row; i++)
        {
            if (CardIDData[i] < 1)
            {
                continue;
            }
            var card = Instantiate(cardMaster.gameObject,cardMaster.transform.parent).GetComponent<Card>();
            card.gameObject.SetActive(true);
            card.Id = CardIDData[i];
            card.Index = i;
            card.Label.text = card.Id.ToString();
            card.Label.text = "";
            card.transform.localPosition = Vector3.right*(i%col)*150f + Vector3.down*(i/col)*230f;
            allCard.Add(card);
        }

        buttonText.text = "Start";
        StartGame();
    }

    public bool CanFlip()
    {
        return gameStage == "play";
    }


    public void ColSet(Slider slider)
    {
        col =(int)slider.value * 2;
        StopGame();
    }

    public void RowSet(Slider slider)
    {
        row = (int)slider.value;
        StopGame();
    }

    public void GameToggleClick()
    {
        if (gameStage == "ready")
        {
            StartGame();
        }
        else
        {
            StopGame();
        }
    }

    public void StopGame()
    {
        gameStage = "ready";
        buttonText.text = "Start";
        ResetCard();
    }

    public void StartGame()
    {
        gameStage = "play";
        buttonText.text = "Reset";
    }

    public void Save()
    {
        var write = string.Join(",", CardIDData) + $":{col},{row},{Turn},{Match},{Score}" ;
        PlayerPrefs.SetString("save", write);
        Debug.Log(write);
    }

    public void Load()
    {
        var read = PlayerPrefs.GetString("save");
      
        if (!string.IsNullOrEmpty(read))
        {
            var reads = read.Split(':');
            int[] data = reads[0].Split(',').Select(int.Parse).ToArray();
            int[] param = reads[1].Split(',').Select(int.Parse).ToArray();
            Turn = param[2];
            Match = param[3];
            Score = param[4];
            rowSlide.value = param[0];
            colSlide.value = param[1];
            ResetCard(data, param[0], param[1]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        turnText.text = "Turn: " + Turn;
        matchText.text = "Match: " + Match;
        scoreText.text = "Score: " + Score;
    }
}
