using System.Collections;
using UnityEngine;

public class SplashScene : Scene<TransitionData>
{
    [SerializeField] private GameObject _title;
    [SerializeField] private float _pulseInterval;

    private Coroutine _pulse;

    private IEnumerator PulseTitle()
    {
        while (gameObject.activeInHierarchy)
        {
            var s = (Mathf.Sin(Time.time * 1/(_pulseInterval/2)) + 1) * 0.5f + 0.5f;
            _title.transform.localScale = new Vector3(s, s, 1);
            yield return null;
        }
    }

    protected override void OnEnter(TransitionData data)
    {
        _pulse = StartCoroutine(PulseTitle());

    }

    protected override void OnExit()
    {
        StopCoroutine(_pulse);

    }

}