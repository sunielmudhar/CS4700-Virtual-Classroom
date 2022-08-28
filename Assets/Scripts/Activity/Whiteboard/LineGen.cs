using Photon.Pun;
using UnityEngine;
using System.IO;

public class LineGen : MonoBehaviour
{
    [SerializeField] public GameObject linePrefab, participant, line;
    [SerializeField] Line currentLine;
    [SerializeField] static bool bl_CanDraw = false;
    PhotonView PV;

    Vector3 mousePosVec;
    RaycastHit hit;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        participant = GameObject.Find("CurrentParticipant");

        if (Input.GetMouseButtonDown(0) && participant.tag.Equals("Student") && bl_CanDraw)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Plane"))   //Check to see if the raycast is hitting the plane
            {
                PV.RPC("drawFunction", RpcTarget.All, hit.point);
            }
        }

        if (currentLine != null && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                mousePosVec = hit.point;
                PV.RPC("helperFunction", RpcTarget.All, "notNullActiveLine", mousePosVec);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            PV.RPC("helperFunction", RpcTarget.All, "mouseUp", null);
        }
    }

    [PunRPC]
    public void drawFunction(Vector3 hitPoint)
    {
        line = Instantiate(linePrefab, hitPoint, Quaternion.identity);
        currentLine = line.GetComponent<Line>();
    }

    [PunRPC]
    public void helperFunction(string action, Vector3 mousePosVec)
    {
        if (action.Equals("mouseUp"))
        {
            currentLine = null;
        }
        else if (action.Equals("notNullActiveLine"))
        {
            currentLine.UpdateLine(mousePosVec);
        }
    }

    public static void CanDraw(bool state)
    {
        bl_CanDraw = state;
    }
}
