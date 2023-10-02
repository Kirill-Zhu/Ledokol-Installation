using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private List<Scroller> _srollers;
    

  
    public bool isMatches = false;

    private async void Start()
    {
        await Task.Delay(1000);
        ResetAnswers();
       
    }
    [ContextMenu("Answer")]
    public void Answer()
    {
        for(int i=0; i<_srollers.Count;i++)
        {
            if (i == 0)
                continue;
            if (_srollers[i].nearestToCenter.transform.name == _srollers[i - 1].nearestToCenter.transform.name)
            {
                isMatches = true;
                _srollers[i].nearestToCenter.gameObject.GetComponent<AnswerBubble>().ShowCorretAnswer();
                _srollers[i - 1].nearestToCenter.gameObject.GetComponent<AnswerBubble>().ShowCorretAnswer();
            }  
        }
    }

    public void ResetAnswers()
    {
      for(int i=0 ; i<_srollers.Count; i++)
        {
            foreach(var objects in _srollers[i].Objects)
            {
                objects.GetComponent<AnswerBubble>().ResetAnswer(); 
            }
          
        }
    }
 
   
}
