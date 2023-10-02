using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class HorizontalScroller : MonoBehaviour
{
    [SerializeField] private List<Transform> _positions;
    [SerializeField] private List<GameObject> _imageObjects;
    [SerializeField] private int _lerpCycles=20;
    [SerializeField] private float _secMove=1;
    [SerializeField] float _lerpValue;
    private void Start()
    {
        SetObjectsPosAndRot();
    }
    
    [ContextMenu("Slide Right")]
    public async void SlideRight()
    {
        _lerpValue += _secMove / _lerpCycles;
        

        for(int i=0; i<_imageObjects.Count; i++)
        {

            try
            {
                _imageObjects[i].transform.position = Vector2.Lerp(_imageObjects[i].transform.position, _positions[i + 1].position, _lerpValue);
                _imageObjects[i].transform.rotation = Quaternion.Lerp(_imageObjects[i].transform.rotation, _positions[i + 1].rotation, _lerpValue);
            }
            catch
            {
                _imageObjects[i].transform.position = Vector2.Lerp(_imageObjects[i].transform.position, _positions[0].position, _lerpValue);
                _imageObjects[i].transform.rotation = Quaternion.Lerp(_imageObjects[i].transform.rotation, _positions[0].rotation, _lerpValue);
            }
        }
        await Task.Delay(1000/_lerpCycles);
        if (_lerpValue < 1)
            SlideRight();
    }
    public void SlideLeft()
    {

    }

    
    private void SetObjectsPosAndRot()
    {
        for(int i =0; i<_imageObjects.Count; i++)
        {
            _imageObjects[i].transform.position = _positions[i].position;
            _imageObjects[i].transform.rotation = _positions[i].rotation;
        }
    }

}
