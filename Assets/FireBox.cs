using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBox : MonoBehaviour
{
    public float mMaxFuel;
    public float mCurrentFuel;
    public float mAddfuel;
    private InputReciever mInputReciever;
    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        mCurrentFuel = 50;
    }

    public void AddFuel()
    {
        Debug.Log("added fuel");
        mCurrentFuel += mAddfuel;
        if (mCurrentFuel >= mMaxFuel)
        {
            mCurrentFuel = mMaxFuel;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.fireBox = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.fireBox = null;
        }
    }
}
