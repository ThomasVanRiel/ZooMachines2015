using UnityEngine;
using System.Collections;

public class TeamController : MonoBehaviour
{
    public static int AmountOfTeams = 3;

    public int CurrentTeam = 0;
    public Color TeamColor = Color.black;

    private float _oldScrollValue = 0;
    public float ScrollDifference = 1;

    public Renderer TeamColorRenderer;

    // Component
    private IInputReceiver _input;

    // Use this for initialization
    void Start ()
    {
        _input = GetComponent<IInputReceiver>();

        UpdateColor();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    float currentScroll = _input.GetMouseScroll();
	    if (Mathf.Abs(_oldScrollValue - currentScroll) >= ScrollDifference)
	    {
	        if (_oldScrollValue > currentScroll)
	            --CurrentTeam;
	        else
	            ++CurrentTeam;

            // Infinite scrolling
	        if (CurrentTeam < 0)
	            CurrentTeam = AmountOfTeams - 1;
            else if (CurrentTeam >= AmountOfTeams)
                CurrentTeam = 0;

	        _oldScrollValue = currentScroll;
            UpdateColor();
	    }
	}

    void UpdateColor()
    {
        TeamColor = HSBColor.ToColor(new HSBColor((float) CurrentTeam/AmountOfTeams, 1, 1));
        TeamColorRenderer.material.color = TeamColor;
    }

    public void DisableTeamIndication()
    {
        TeamColorRenderer.gameObject.SetActive(false);
    }
}
