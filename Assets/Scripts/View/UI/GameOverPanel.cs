using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameView _gameView;
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        _group.alpha = 0;
        _pool.SpaceshipDespawned += Show;
        _restartButton.onClick.AddListener(Restart);
    }

    private void Show(View ship)
    {
        _group.alpha = 1;
    }

    private void Restart()
    {
        if (_group.alpha > 0)
        {
            _group.alpha = 0;
            _gameView.Restart();
        }
    }
}
