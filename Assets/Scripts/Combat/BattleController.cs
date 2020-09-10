using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    [SerializeField] [Range(2, 10)] private int _numberOfTeams = 2;
    [SerializeField] [Range(1, 5)] private int _charactersPerTeam = 3;
    [SerializeField] [Range(-10f, 10f)] private float _characterSpacing = 2f;
    [SerializeField] private GameObject _characterPrefab = null;
    [SerializeField] private List<Transform> _teamPositions = new List<Transform>();
    [SerializeField] private List<TextMeshPro> _teamNamesText = new List<TextMeshPro>();
    [SerializeField] private List<string> _teamNames = new List<string>();

    private List<Team> _teams;

    private void Awake()
    {
        _teams = new List<Team>();
        if (_teamPositions.Count < 2)
        {
            Debug.LogError("Need at least two team positions for combat.");
            enabled = false;
        }
        if (_characterPrefab == null)
        {
            Debug.LogError("Character Prefab must be set via inspector.");
            enabled = false;
        }
    }

    private void Start()
    {
        SetupTeams();
    }

    public void NewBattle(int teamSize)
    {
        foreach (var team in _teams)
        {
            team.DestroyTeam();
        }
        _charactersPerTeam = teamSize;
        SetupTeams();
    }

    public Character GetRandomTarget(int currentTeam)
    {
        var validTeamTargets = new List<Team>();
        for (int i = 0; i < _teams.Count; i++)
        {
            if (i != currentTeam && _teams[i].IsAlive)
            {
                validTeamTargets.Add(_teams[i]);
            }
        }
        if (validTeamTargets.Count > 0)
        {
            var teamToTarget = validTeamTargets[Random.Range(0, validTeamTargets.Count)];
            return teamToTarget.GetRandomTarget();
        }
        return null;
    }

    private void SetupTeams()
    {
        DataManager.Shuffle();
        int weaponDataIndex = 0;
        int characterDataIndex = 0;
        _teams.Clear();
        _numberOfTeams = Mathf.Min(_numberOfTeams, _teamPositions.Count);
        _teamNames.Shuffle();
        int teamNameIndex = 0;
        for (int teamIndex = 0; teamIndex < _numberOfTeams; teamIndex++)
        {
            var teamName = _teamNames.Count > 0 ? _teamNames[teamNameIndex] : "" + Random.Range(100,1000);
            var team = new Team(teamName);
            teamNameIndex = (teamNameIndex + 1) % _teamNames.Count;
            if (_teamNamesText.Count > teamIndex)
            {
                _teamNamesText[teamIndex].text = teamName;
            }
            _teams.Add(team);
            var teamSpawnTransform = _teamPositions[teamIndex];
            var basePosition = teamSpawnTransform.position;
            for (int characterIndex = 0; characterIndex < _charactersPerTeam; characterIndex++)
            {
                var position = basePosition;
                position.x = basePosition.x + (characterIndex * _characterSpacing);
                var characterGameObject = Instantiate(_characterPrefab, position, Quaternion.identity, teamSpawnTransform);
                var characterController = characterGameObject.GetComponent<Character>();
                var characterData = DataManager.Characters[characterDataIndex];
                var weaponData = DataManager.Weapons[weaponDataIndex];
                characterController.Battle = this;
                characterController.Populate(characterData, weaponData, teamIndex);
                team.Characters.Add(characterController);
                weaponDataIndex = (weaponDataIndex + 1) % DataManager.Weapons.Count;
                characterDataIndex = (characterDataIndex + 1) % DataManager.Characters.Count;
            }
        }
    }
}
