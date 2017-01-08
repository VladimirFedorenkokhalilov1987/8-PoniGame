using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
    #region SERIALIZE FIELDS

    [SerializeField]
    private Text _time;

    #endregion

    #region PRIVATE FIELDS

    private TimerStates _timerState;
    private float _currentTime;

    public float CurrentTime
    {
        get
        {
            return _currentTime;
        }

        private set
        {
            _currentTime = value;
            UpdateCurrentTime();
        }
    }

    public string CurrentFormatedTime
    {
        get
        {
            return GetFormattedTime(_currentTime);
        }
    }

    private StringBuilder _timeString;

    #endregion

    #region UNITY EVENTS

    private void FixedUpdate()
    {
        if (_timerState == TimerStates.None || _timerState == TimerStates.Off || _timerState == TimerStates.Paused) return;

        CurrentTime += Time.fixedDeltaTime;
    }

    #endregion

    #region PUBLIC METHODS

    public void ON()
    {
        if(_timerState == TimerStates.On) return;

        _timerState = TimerStates.On;
        CurrentTime = 0;
    }

    public void Pause()
    {
        _timerState = TimerStates.Paused;
    }

    public void Resume()
    {
        if(_timerState == TimerStates.Off) return;
        _timerState = TimerStates.On;
    }

    public void OFF()
    {
        _timerState = TimerStates.Off;
    }

    #endregion

    #region PRIVATE METHODS

    private void UpdateCurrentTime()
    {
        if (_time == null) return;
        _time.text = GetFormattedTime(_currentTime);
    }

    private string GetFormattedTime(float time)
    {
        if(_timeString == null) _timeString = new StringBuilder(5);

        int currentTime = (int)time;
        int minutes = currentTime / 60;
        int seconds = currentTime % 60;

        _timeString.Remove(0,_timeString.Length);
        _timeString.Insert(0, minutes.ToString("00"));
        _timeString.Insert(2,':');
        _timeString.Insert(3, seconds.ToString("00"));

        return _timeString.ToString();
    }

    #endregion

    #region PRIVATE ENUM

    private enum TimerStates
    {
        None,
        On,
        Off,
        Paused
    }

    #endregion
}
