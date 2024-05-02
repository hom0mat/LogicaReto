using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBoundaries : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectiveWidth;
    private float objectiveHeight;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectiveWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectiveHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectiveWidth, screenBounds.x * -1 - objectiveWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectiveHeight, screenBounds.y * -1 - objectiveHeight);
        transform.position = viewPos;
    }
}
