using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour , IPointerClickHandler
{
    public int Id;
    public int Index;
    public bool blockFlip = false;
    [SerializeField] private Image cardImage;
    [SerializeField]private GameController controller;
    [SerializeField] private AudioController audioController;
    public TextMeshProUGUI Label;





    private void Start()
    {
        cardImage.color = Color.gray;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(controller.CanFlip() && !blockFlip)
        {
            StartCoroutine(FlipFront());
        }
    }

    public IEnumerator FlipFront()
    {
        if (controller.First == this)
        {
            yield break;
        }

        blockFlip = true;
        audioController.PlayEffect(audioController.FlipSound);
        for (int i = 0;i<10;i++)
        {
            transform.eulerAngles = new Vector3(0, i*18, 0); 
            yield return new WaitForSeconds(0.02f);
        }
        transform.eulerAngles = new Vector3(0, 0, 0);

        cardImage.color = Color.blue;
        Label.text = this.Id.ToString();
        yield return new WaitForSeconds(0.3f);
        if (controller.First == null)
        {
            controller.First = this;
            blockFlip = false;
            yield break;
        }

        controller.Turn++;
        if (controller.First.Id == this.Id)
        {
            StartCoroutine(controller.First.Hide());
            StartCoroutine(Hide());
            controller.First = null;
            controller.Match++;
            audioController.PlayEffect(audioController.MatchSound);
        }
        else
        {
            StartCoroutine(controller.First.FlipBack());
            StartCoroutine(FlipBack());
            controller.First = null;
            audioController.PlayEffect(audioController.WrongSound);
        }
        controller.Score = controller.Match * 50 - controller.Turn;
        if(controller.Score < 0)
        {
            controller.Score = 0;
        }
    }


    public IEnumerator Hide()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.localScale = transform.localScale * 0.7f ;
            yield return new WaitForSeconds(0.02f);
        }
        controller.CardIDData[Index] = -1;
        blockFlip = false;
        controller.AllCard.Remove(this);
        if (controller.AllCard.Count == 0)
        {
            audioController.PlayEffect(audioController.EndSound);
        }
        Destroy(gameObject);
    }

    public IEnumerator FlipBack()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.eulerAngles = new Vector3(0, -i * 18, 0);
            yield return new WaitForSeconds(0.02f);
        }
        transform.eulerAngles = new Vector3(0, 0, 0);
        Label.text = "";
        cardImage.color = Color.gray;
        blockFlip = false;
    }


}
