using System;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private int[] cardIDData;
    [SerializeField]private string gameStage = "ready";
    [SerializeField]private int col = 2;
    [SerializeField]private int row = 1;
    [SerializeField]private TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        //Init Game-State Coroutine Here
        StartCoroutine(GameStateController());
    }

    IEnumerator GameStateController()
    {
        // Draw UI finish by editor setup
        // Prepare Card Table
        ResetCard();
       // Count-Down  3 - 2 - 1 GO!

       // Wait Playing State

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


    public void ResetCard()
    {
        cardIDData = new int[col * row];
        for (int i = 0; i < col * row; i++)
        {
            cardIDData[i] = (i / 2) + 1;
        }
        SecureShuffle(cardIDData);


        buttonText.text = "Start";
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
