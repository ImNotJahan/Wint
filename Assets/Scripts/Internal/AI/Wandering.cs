using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Wandering : MonoBehaviour
{
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxDistance = 30;

    CharacterController controller;
    float moveTwoards;
    Vector3 targetRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        moveTwoards = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, moveTwoards, 0);

        StartCoroutine(NewMoveTwoards());
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * speed);
    }

    private IEnumerator NewMoveTwoards()
    {
        while (true)
        {
            NewMoveTwoardsRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void NewMoveTwoardsRoutine()
    {
        var floor = transform.eulerAngles.y - maxDistance;
        var ceil = transform.eulerAngles.y + maxDistance;
        moveTwoards = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, moveTwoards, 0);
    }
}
