using UnityEngine;
using System.Collections;

public class TeamController : MonoBehaviour
{
    public static int AmountOfTeams = 3;

    public int CurrentTeam = 0;
    private Color _teamColor = Color.black;

    public Color TeamColor
    {
        get { return _teamColor; }
        set
        {
            _teamColor = value;
        }
    }

    private float _oldScrollValue = 0;
    public float ScrollDifference = 1;

//    public Projector PlayerIndicator;
    private Material _mat;

    private static bool _canSwitchTeam = true;
    public static bool CanSwitchTeam
    {
        get { return _canSwitchTeam; }
        set { _canSwitchTeam = value; }
    }

    // Component
    private IInputReceiver _input;
    public Renderer Renderer;

    // Use this for initialization
    void Start ()
    {
        _input = GetComponent<IInputReceiver>();
        _mat = Renderer.material;

        UpdateColor();
    }
	
	// Update is called once per frame
	void Update ()
	{
        // Only react to scroll wheel if players can switch team.
	    if (!CanSwitchTeam)
	        return;

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
        _mat.color = TeamColor;
    }
    
    IEnumerator WaitAndFadeTeamColor(float wait, float t)
    {
        yield return new WaitForSeconds(wait);

        // Convert to HSB
        HSBColor newColor = new HSBColor(TeamColor);
        float timeRemaining = t;
        while (timeRemaining > 0)
        {
            // Desaturate
            newColor.s = timeRemaining / t;
            TeamColor = HSBColor.ToColor(newColor);
            // Decrease time
            timeRemaining -= Time.fixedDeltaTime;
            UpdateColor();
            yield return new WaitForEndOfFrame();
        }
        TeamColor = Color.white;
        _mat.SetInt("_EnableSeethrough", 0);
    }

    public void IsKilled()
    {
        StartCoroutine(WaitAndFadeTeamColor(1.5f, 2));
    }
}
