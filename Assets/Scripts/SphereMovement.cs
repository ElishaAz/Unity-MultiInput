using MultiInput;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;

    private IMouse mouse;
    private bool initialized = false;

    public void Setup(IMouse mouse)
    {
        this.mouse = mouse;
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        var movement = mouse.GetMouseMovement();

        transform.Translate(movement.Movement * speed);
    }
}