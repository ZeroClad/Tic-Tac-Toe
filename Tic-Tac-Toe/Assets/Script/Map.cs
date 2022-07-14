using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IPointerClickHandler
{
    // 0 - Empty; 1 - Blue; 2 - Red
    public int[][] MapData;
    private RectTransform parentRect;
    public RectTransform PieceMaster;
    public GameObject BluePiece, RedPiece;
    
    // Start is called before the first frame update
    void Start()
    {
        GameController.Map = this;
        MapData = new int[4][];
        for (int i = 0; i <= 3; i++)
        {
            MapData[i] = new int[4];
            for (int j = 0; j <= 3; j++)
                MapData[i][j] = 0;
        }
        parentRect = this.transform.parent.GetComponent<RectTransform>();
    }

    public void Restart()
    {
        for (int i = 1; i <= 3; i++)
            for (int j = 1; j <= 3; j++)
                MapData[i][j] = 0;
        foreach (var o in PieceMaster.GetComponentsInChildren<RectTransform>())
        {
            if (o.gameObject == PieceMaster.gameObject)
                continue;
            Destroy(o.gameObject);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 v = currentMousePosition - parentRect.transform.position;
        Vector3 pos = new Vector3(v.x / parentRect.transform.localScale.x, v.y / parentRect.transform.localScale.y, parentRect.position.z);

        pos.x = pos.x / Screen.width * 1920;
        pos.y = pos.y / Screen.height * 1080;

        return pos;
    }

    private Vector3Int GetCoordFromPosition(Vector3 v)
    {
        return new((int)v.x + 1, (int)v.y + 1, 0);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GameController.GameOver)
            return;
        if (GameController.Mode == 0 && GameController.CurrentPlayer == GameController.COM.SelfNumber)
            return;
        var v = GetCoordFromPosition(GetMousePosition());
        if (v.x < 1 || v.x > 3 || v.y < 1 || v.y > 3)
            return;
        if (MapData[v.x][v.y] != 0)
            return;
        Move(v);
    }

    public void Move(Vector3Int v)
    {
        Debug.Log($"{v.x}, {v.y}, {GameController.CurrentPlayer}");
        MapData[v.x][v.y] = GameController.CurrentPlayer;

        GameObject o;
        if (GameController.CurrentPlayer == 1)
            o = GameObject.Instantiate(BluePiece);
        else
            o = GameObject.Instantiate(RedPiece);
        o.transform.SetParent(PieceMaster);
        o.transform.localPosition = v;
        o.transform.localScale = Vector3.one;

        GameController.TurnEnd();

    }

    public void Move(Vector2Int v)
    {
        Move(new Vector3Int(v.x, v.y, 0));
    }
}
