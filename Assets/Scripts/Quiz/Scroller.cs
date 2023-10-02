using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scroller: MonoBehaviour,IPointerMoveHandler,IPointerUpHandler,IPointerExitHandler
{
    public Vector2 touchStartPos;
    public Vector2 touchNowPos;
    public Vector2 previousTouchPos;

    public Vector2 start0Pos;
    public List<GameObject> Objects = new List<GameObject>();
    [SerializeField] private QuizManager _quizManager;
    [SerializeField] float _spacing = 1;
    [SerializeField] float _speed = 100;
    [SerializeField] float _higherBound;
    [SerializeField] float _lowerBound;
    [SerializeField] List<Sprite> _sprites;
   
    [Header("Alpha Change properties")]
    [SerializeField] private float _AbsTopAndLowYPosValue = 648;
   
    private float _vertInput;
    public GameObject nearestToCenter;
    private bool _isLerpting;
    private int _lerpCounts = 60;
    [SerializeField] private float _lerpSpeed = 1;
    private float difference;
    public bool _isTouching;
    private Coroutine _coroutine;
  
    private void Start()
    {
        nearestToCenter = new GameObject();
        InitializeImages();
        SetStartPosition();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            _isTouching = false;




        CaruselBoundCondition();
        FindNearestToCenter();
        LerpToCenter();
        AlphaChangeObjects();
       
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(_coroutine);
            _isLerpting = false;
            float distance = Mathf.RoundToInt((transform.GetComponent<RectTransform>().position.y
                       - nearestToCenter.GetComponent<RectTransform>().position.y));
            foreach (var _obj in Objects)
                _obj.GetComponent<RectTransform>().Translate(new Vector2(0, distance));
            
            Debug.Log("LErp Nearest to 0");
        }

        
    }
    private void SetStartPosition()
    {
        for (int i =0; i<Objects.Count; i++)
        {
            if (i == 0)
                continue;

            var rect1 = transform.GetChild(i-1).GetComponent<RectTransform>();
            var rect = Objects[i].GetComponent<RectTransform>();
            float rectXpos = rect.position.x;
            rect.SetPositionAndRotation(rect1.position + new Vector3(0, rect1.sizeDelta.y + _spacing, 0), Quaternion.identity);
            rect.SetPositionAndRotation(new Vector3(rectXpos, rect.position.y, rect.position.z), Quaternion.identity);
            Objects[i].GetComponent<RectTransform>().position = rect.position;
        }
    }

    private void InitializeImages()
    {   
        Objects.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var tmp = transform.GetChild(i).gameObject;
            Objects.Insert(i, tmp);                      
        }     
    }

    private void CaruselBoundCondition()
    {
        if (Objects.Count > 0)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].transform.position.y < _lowerBound)
                {                  
                    var rect1 = transform.GetChild(0).GetComponent<RectTransform>();
                    Objects[i].transform.SetSiblingIndex(0);
                    var rect = Objects[i].GetComponent<RectTransform>();
                    float rectXpos = rect.position.x;
                    rect.SetPositionAndRotation(rect1.position + new Vector3(0, rect1.sizeDelta.y + _spacing, 0), Quaternion.identity);
                    rect.SetPositionAndRotation(new Vector3(rectXpos, rect.position.y, rect.position.z),Quaternion.identity);
                    Objects[i].GetComponent<RectTransform>().position = rect.position;
                }
            }

        }
        if (Objects.Count > 0)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].transform.position.y > _higherBound)
                {
                    var rect1 = transform.GetChild(transform.childCount-1).GetComponent<RectTransform>();
                    Objects[i].transform.SetSiblingIndex(transform.childCount-1);
                    var rect = Objects[i].GetComponent<RectTransform>();
                    float rectXpos = rect.position.x;
                    rect.SetPositionAndRotation(rect1.position - new Vector3(0, rect1.sizeDelta.y + _spacing, 0), Quaternion.identity);
                    rect.SetPositionAndRotation(new Vector3(rectXpos, rect.position.y, rect.position.z), Quaternion.identity);
                    Objects[i].GetComponent<RectTransform>().position = rect.position;
                }
            }

        }
    }
    private void FindNearestToCenter()
    {
        
        
            if (nearestToCenter == null)
                nearestToCenter = Objects[3];

            foreach (var obj in Objects)
                if (Vector2.Distance(obj.transform.position, transform.position)
                    < Vector2.Distance(nearestToCenter.transform.position, transform.position))
                    nearestToCenter = obj;



       
    }
    private void LerpToCenter()
    {

        if (Input.touchCount == 0)
        {

            float distance = transform.GetComponent<RectTransform>().position.y
                - nearestToCenter.GetComponent<RectTransform>().position.y;

            if(!_isLerpting)
            {
                if(_coroutine!=null)
                StopCoroutine(_coroutine);
                
                _coroutine = StartCoroutine(LerpObjects(_lerpSpeed, distance, _lerpCounts));
            }
               
        }


    }
    private IEnumerator LerpObjects(float speed, float distanceToCenter,int lerpCounts)
    {
        if(Input.touchCount==0)
        {
        _isLerpting = true;
            for (int i = 0; i < lerpCounts; i++)
            {  if (Input.touchCount > 0)
                    continue;

                foreach (var _obj in Objects)
                    _obj.GetComponent<RectTransform>().Translate(new Vector2(0, distanceToCenter / lerpCounts));
                yield return new WaitForSeconds(speed / lerpCounts);
                if(i==lerpCounts-1 && Input.touchCount==0)
                {
                    float distance = transform.GetComponent<RectTransform>().position.y
                       - nearestToCenter.GetComponent<RectTransform>().position.y;
                    foreach (var _obj in Objects)
                        _obj.GetComponent<RectTransform>().Translate(new Vector2(0, distance));

                    _quizManager.Answer();
                    Debug.Log("Asnwer");
                }
            }
           
        _isLerpting = false;
        }
        
    }   
    private void AlphaChangeObjects()
    {
        foreach (var o in Objects)
            if (!o.GetComponent<CanvasGroup>())
                o.AddComponent<CanvasGroup>();

        foreach(var o in Objects)
        {
            var canvasGroup = o.GetComponent<CanvasGroup>();
            float difference = Mathf.Abs(o.GetComponent<RectTransform>().localPosition.y) / _AbsTopAndLowYPosValue;
            canvasGroup.alpha = Mathf.Lerp(1,0,difference); 
        }
    }
    public void OnPointerMove(PointerEventData eventData)
    {
      
        if (!_isTouching)
        {
            previousTouchPos = eventData.position;
            _isTouching = true;
        }
        
        _isTouching = true;
            touchNowPos = eventData.position;

            difference = Mathf.Abs(previousTouchPos.y - touchNowPos.y);
        if (Input.touchCount == 0)
            difference = 0;
        Debug.Log("move pointer");

            foreach (var o in Objects)
            {
                if(difference!= 0)
                {
                    var rect = o.GetComponent<RectTransform>();
                    if (touchNowPos.y > previousTouchPos.y)
                        rect.Translate(new Vector2(0, difference * Time.deltaTime * _speed));
                    if (touchNowPos.y < previousTouchPos.y)
                        rect.Translate(new Vector2(0, -difference * Time.deltaTime * _speed));
                }
            }
            previousTouchPos = eventData.position;    
       
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _isTouching = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isTouching = false;
    }
}







