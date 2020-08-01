using UnityEngine;

public class FireBox : MonoBehaviour
{
    public float mMaxFuel;
    public float mCurrentFuel;
    public float mAddfuel;

    //private InputReciever mInputReciever;
    private FuelController mFuelController;
    void Start()
    {
    //    mInputReciever = GetComponent<InputReciever>();
        mFuelController = FindObjectOfType<FuelController>();
        //mCurrentFuel = mMaxFuel;
        mCurrentFuel = 50;//for test
    }

    public void AddFuel()
    {
        mFuelController.Reload(mAddfuel);
        Debug.Log("added fuel");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            // safety first.
            if (player)
            {
                player.fireBox = this;
            }
            else 
            {
                // log error.
            }
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
