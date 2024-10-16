using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
    /// <summary>
    /// Method will set Canvas Game Object in the GameStateCanvas field inside GameManager
    /// </summary>
    void Start()
    {
        GameManager.Instance.GameStateCanvas = this.gameObject.transform.Find("Canvas").gameObject;
    }
}
