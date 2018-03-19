using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Task {
    public enum TaskStatus : byte {
        Detached, // Task has not been attached to a TaskManager
        Pending, // Task has not been initialized
        Working, // Task has been initialized
        Success, // Task completed successfully
        Fail, // Task completed unsuccessfully
        Aborted // Task was aborted
    }

    public TaskStatus Status { get; private set; }

    public bool IsDetached { get { return Status == TaskStatus.Detached; } }
    public bool IsAttached { get { return Status != TaskStatus.Detached; } }
    public bool IsPending { get { return Status == TaskStatus.Pending; } }
    public bool IsWorking { get { return Status == TaskStatus.Working; } }
    public bool IsSuccessful { get { return Status == TaskStatus.Success; } }
    public bool IsFailed { get { return Status == TaskStatus.Fail; } }
    public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
    public bool IsFinished {
        get {
            return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted);
        }
    }

    public Task NextTask { get; private set; }

    internal void SetStatus(TaskStatus newStatus) {
        if (Status == newStatus) return;
        Status = newStatus;
        switch (newStatus) {
            case TaskStatus.Working:
                Init();
                break;
            case TaskStatus.Success:
                OnSuccess();
                CleanUp();
                break;
            case TaskStatus.Aborted:
                OnAbort();
                CleanUp();
                break;
            case TaskStatus.Fail:
                OnFail();
                CleanUp();
                break;
            // These are "internal" states that are relevant for
            // the task manager
            case TaskStatus.Detached:
            case TaskStatus.Pending:
                break;
            default:
                Debug.Log("ArgumentOutOfRangeException");
                break;
                //throw new ArgumentOutOfRangeException(nameof(newStatus),
                //newStatus, null);
        }
    }

    public void Abort() {
        SetStatus(TaskStatus.Aborted);
    }

    public Task Then(Task task) {
        Debug.Assert(!task.IsAttached);
        NextTask = task;
        return task;
    }

    protected virtual void OnAbort() { }
    protected virtual void OnSuccess() { }
    protected virtual void OnFail() { }

    protected virtual void Init() { }
    internal virtual void Update() { }
    protected virtual void CleanUp() { }

}

public class ActionTask : Task {
    private readonly Action _action;
    public ActionTask(Action action) {
        _action = action;
    }
    protected override void Init() {
        SetStatus(TaskStatus.Success);
        _action();
    }
}

public class WaitTask : Task {
    // Get the timestamp in floating point milliseconds from the Unixepoch
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1,
    1);
    private static double GetTimestamp() {
        return (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
    }
    private readonly double _duration;
    private double _startTime;
    public WaitTask(double duration) {
        this._duration = duration;
    }
    protected override void Init() {
        _startTime = GetTimestamp();
    }
    internal override void Update() {
        var now = GetTimestamp();
        var durationElapsed = (now - _startTime) > _duration;
        if (durationElapsed) {
            SetStatus(TaskStatus.Success);
        }
    }
}

public class ProgressionTask : Task{
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
    private static double GetTimestamp() {
        return (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
    }
    private readonly double _duration;
    private double _startTime;
    private readonly Action _progress;

    public ProgressionTask(Action progress, float duration) {
        _progress = progress;
        _duration = duration;
    }

    protected override void Init() {
        _startTime = GetTimestamp();
        _progress();
    }

    internal override void Update() {
        _progress();
        if (_duration != 0) {
            var now = GetTimestamp();
            var durationElapsed = (now - _startTime) > _duration;
            if (durationElapsed) {
                SetStatus(TaskStatus.Success);
            }
        }
    }
}
