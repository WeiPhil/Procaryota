using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoundary : MonoBehaviour
{
    private static GameBoundary _instance;
    public static GameBoundary Instance { get { return _instance; } }

    BoxCollider2D collider2d;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        collider2d = GetComponent<BoxCollider2D>();
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(Vector2.one);
        float height = edgeVector.y * 2;
        float width = edgeVector.x * 2;
        collider2d.size = new Vector2(width, height);
        DontDestroyOnLoad(gameObject);
    }

    public Bounds GetBounds()
    {
        return collider2d.bounds;
    }


    void OnTriggerExit2D(Collider2D other)
    {
        EnnemyController ennemyController = other.GetComponent<EnnemyController>();
        if (ennemyController != null)
        {
            Vector2 position = new Vector2(GetBounds().max.x, Mathf.Clamp(other.bounds.center.y, GetBounds().min.y + 1.0f, GetBounds().max.y - 1.0f));
            ennemyController.RestartAttack(position);
        }
        else if (other.tag != "Player")
        {
            Destroy(other.gameObject);
        }

    }
}
