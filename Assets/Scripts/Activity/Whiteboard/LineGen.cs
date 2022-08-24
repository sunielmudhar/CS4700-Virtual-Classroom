using Photon.Pun;
using UnityEngine;
using System.IO;

public class LineGen : MonoBehaviour
{
    public GameObject LinePrefab, participant, line, newLine;
    public float fl_DelayTime = 0.1f;
    [SerializeField] Line activeline;
    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        fl_DelayTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(Ray, out hit))   //Check to see if the raycast is hitting the plane
            {
                line = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Activities", "Line"), hit.point + Vector3.up * 0.1f, Quaternion.identity);
                activeline = line.GetComponent<Line>();
            }
        }

        if (activeline != null && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePosVec = ray.origin + ray.direction * 10;

            float[] fl_MousePos = new float[] { mousePosVec.x, mousePosVec.y, mousePosVec.z };

            PV.RPC("drawFunction", RpcTarget.All, "notNullActiveLine", fl_MousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            PV.RPC("drawFunction", RpcTarget.All, "mouseUp", null);
        }
    }

    [PunRPC]
    public void drawFunction(string action, float[] fl_mousePos)
    {

        if (action.Equals("mouseUp"))
        {
            activeline = null;
        }
        else if (action.Equals("notNullActiveLine"))
        {
            if (activeline == null)
            {
                newLine = GameObject.FindWithTag("Line");
                activeline = line.GetComponent<Line>();
            }

            Vector3 mousePos = new Vector3(fl_mousePos[0], fl_mousePos[1], fl_mousePos[2]);
            activeline.UpdateLine(mousePos);
        }

    }
}
