using UnityEngine;

public class FireBox : MonoBehaviour
{
    public float mMaxFuel;
    public float mCurrentFuel;
    public float mAddfuel;

    private InputReciever mInputReciever;
    private FuelController mFuelController;
    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        mFuelController = FindObjectOfType<FuelController>();
        //mCurrentFuel = mMaxFuel;
        mCurrentFuel = 50;//for test
    }

    public void AddFuel()
    {
        Debug.Log("added fuel");
        mFuelController.Reload(mAddfuel);
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
