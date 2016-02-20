using UnityEngine;
using System.Collections;

public class Brusher : Singleton<Brusher>, Actor
{

    private Animator _animator;
    public Player player;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        player.registerActor(this);
    }

    public void Trigger(int index)
    {
        switch (index)
        {
            case 0:
                _animator.Play("Left Brush");
                break;
            case 1:
                _animator.Play("Right Brush");
                break;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void deactivate()
    {
        gameObject.SetActive(false);
    }

    public void activate()
    {
        gameObject.SetActive(true);
    }
}
