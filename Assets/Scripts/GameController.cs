using Assets.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Floor Floor;
    public Player Player;
    public Player PlayerPrefab;
    public OverlayScreen OverlayScreen;
    public Agent[] Agents => _agents.Where(a => a != null).ToArray();
    public Agent[] AgentsPrefab;
    public int Score => _score;
    public int Level => _level;
    public int InitialAmmunition = 1;
    public int CloverToCapture = 3;
    public int AmmunitionToCapture = 3;
    public int LifesToCapture = 5;
    public int LifesLimit => OverlayScreen.Lifes.Length;

    private int _score;
    private int _scoreLimit;
    private int _chasing;
    private int _ammunition;
    private int _lifes = 3;
    private int _level = 1;
    private Agent[] _agents = Array.Empty<Agent>();
    private Vector3 _initialPlayerPosition;
    private Quaternion _initialPlayerRotation;

    public void Start()
    {
        _initialPlayerPosition = Player.transform.position;
        _initialPlayerRotation = Player.transform.rotation;

        WayPointBase.LoadWayPoints();
        InitPhase();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1) Pause();
            else Continue();
        }
    }

    public void Begin()
    {
        SceneManager.LoadScene(Scenes.Game);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        SceneManager.LoadScene(Scenes.Pause, LoadSceneMode.Additive);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(Scenes.Pause);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(Scenes.GameOver, LoadSceneMode.Additive);
    }

    public void NextPhase()
    {
        OverlayScreen.UpdateLevel(++_level);
        SceneManager.LoadScene(Scenes.NextPhase, LoadSceneMode.Additive);
        Time.timeScale = 0;
    }

    public void BeginNextPhase()
    {
        InitPhase();
        SceneManager.UnloadSceneAsync(Scenes.NextPhase);
        Time.timeScale = 1;
    }

    public void InitPhase()
    {
        DestroyAgents();
        Destroy(Player.gameObject);

        _score = 0;
        _chasing = 0;
        _ammunition = _level == 1 ? InitialAmmunition : 0;

        OverlayScreen.UpdateScore(_score);
        OverlayScreen.UpdateAmmunition(_ammunition);
        OverlayScreen.ToggleChase(false);

        while (_lifes < LifesLimit)
            OverlayScreen.ToggleHeart(_lifes++, true);

        Floor.CreateCandies();
        Floor.CreateRandomLifes(LifesToCapture);
        Floor.CreateRandomClovers(CloverToCapture);
        Floor.CreateRandomAmmunitions(AmmunitionToCapture);

        CreatePlayer();
        CreateAgents();

        Player.IncrementVelocity(_level * 0.15f);

        if (LifesToCapture > 1) LifesToCapture--;
        if (CloverToCapture > 1) CloverToCapture--;
        if (AmmunitionToCapture > 1) AmmunitionToCapture--;

        foreach (var agent in Agents)
            agent.IncrementSpeed(_level * 0.18f);

        _scoreLimit = WayPointBase.CountChildsWithTag(Tags.Candy);
    }

    private void CreatePlayer()
    {
        Player = Instantiate(
           PlayerPrefab,
           _initialPlayerPosition,
           _initialPlayerRotation
        );
        Player.tag = Tags.Player;
        Player.GameController = this;
    }

    private void DestroyAgents()
    {
        foreach (var agent in Agents)
            Destroy(agent.gameObject);
        _agents = new Agent[0];
    }

    private void CreateAgents()
    {
        var agents = new List<Agent>();
        var wayPoints = GameObject.FindGameObjectsWithTag(Tags.WayPointCenter);
        foreach (var agentPrefab in AgentsPrefab)
        {
            var index = AgentsPrefab.ToList().IndexOf(agentPrefab);
            if (wayPoints.Count() <= index) break;
            var wayPoint = wayPoints.ElementAt(index);
            var agent = Instantiate(
                agentPrefab, 
                wayPoint.transform.position,
                wayPoint.transform.rotation
            );
            agent.tag = Tags.Agent;
            agent.Player = Player;
            agent.GameController = this;
            agents.Add(agent);
        }
        _agents = agents.ToArray();
    }

    public void RestartPhase()
    {
        InitPhase();

        SceneManager.UnloadSceneAsync(Scenes.GameOver);
        Time.timeScale = 1;
    }

    public void IncrementScore()
    {
        _score++;
        OverlayScreen.UpdateScore(_score);

        if (_score >= _scoreLimit) NextPhase();
    }

    public void IncrementAmmunition()
    {
        _ammunition++;
        OverlayScreen.UpdateAmmunition(_ammunition);
    }

    public void ConsumeAmmunition()
    {
        if (!CanConsumeAmmunition()) return;

        _ammunition--;
        OverlayScreen.UpdateAmmunition(_ammunition);
    }

    public bool CanConsumeAmmunition() => _ammunition > 0;

    public void Hurt()
    {
        OverlayScreen.ToggleHeart(--_lifes, false);
        if (_lifes <= 0) GameOver();
    }

    public void Heal()
    {
        if (!CanHeal()) return;
        OverlayScreen.ToggleHeart(_lifes++, true);
    }

    public bool CanHeal() => LifesLimit > _lifes;
    
    public void Luck()
    {
        if (!CanLuck()) return;

        foreach (var agent in Agents.Where(s => s.Chasing))
            StopChasePlayer(agent);
    }

    public bool CanLuck() => Agents.Any(a => a.Chasing);

    public void BeginChasePlayer(Agent agent)
    {
        if (_chasing++ == 0) OverlayScreen.ToggleChase(true);

        OverlayScreen.AddChaseAvatar(agent.AvatarPrefab);
    }

    public void StopChasePlayer(Agent agent)
    {
        if (--_chasing == 0) OverlayScreen.ToggleChase(false);

        agent.StopChasing();
    }

    public void DestroyScorePoint()
    {
        if (_scoreLimit <= 0) return;

        _scoreLimit--;
    }

    public void Exit() => Application.Quit();
}
