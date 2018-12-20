using UnityEngine;

public class DetectClicks : MonoBehaviour
{
    public bool debug = true;

	private Camera _camera;
    private Hit _hit;


    void Start()
	{
	    _camera = gameObject.GetComponent<Camera>();
	}
	
	void Update ()
	{
        if (Input.GetMouseButtonDown(0))
		{
		    RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
		    {
                _hit = new Hit(hit, _camera);
                Click();
		    }			
		}
	    if (Input.GetMouseButtonUp(0))
	    {
	        _hit = null;
	    }
    }

    public void Click()
    {
        _hit.Object.SendMessage("Click", _hit.Position, SendMessageOptions.DontRequireReceiver);
        if(debug)Debug.Log(_hit.Object.name);
    }

    public void Drag()
    {
        var newPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _hit.ScreenPoint.z));
        _hit?.Object?.SendMessage("Drag", newPos + _hit.PositionObjectOffset, SendMessageOptions.DontRequireReceiver);
    }

    private class Hit
    {
        public readonly GameObject Object;
        public readonly Vector3 Position;
        public readonly Vector3 PositionObjectOffset;
        public readonly Vector3 ScreenPoint;

        public Hit(RaycastHit hit, Camera camera)
        {
            Object = hit.transform.gameObject;
            Position = hit.point;
            PositionObjectOffset = Object.transform.position - Position;
            ScreenPoint = camera.WorldToScreenPoint(Position);
        }
    }
}
