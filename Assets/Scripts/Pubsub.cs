using System;
using System.Collections.Generic;
using UnityEngine;

public static class Pubsub
{
    public enum Type
    {
        NewBlocOnTower,
        NewInteractiveBloc,
        GameOver,
        BlocTouchTower
    }

    private static Dictionary<Type, Action<object>> _eventMap = new Dictionary<Type, Action<object>>();

    public static void Register(Type type, Action<object> callback)
    {
        if (_eventMap.ContainsKey(type))
        {
            _eventMap[type] += callback;
        }
        else
        {
            _eventMap.Add(type, callback);
        }
    }

    public static void Unregister(Type type, Action<object> callback)
    {
        if (_eventMap.ContainsKey(type))
        {
            _eventMap[type] -= callback;
        }
    }

    public static void Publish(Type type, object args)
    {
        if (_eventMap.ContainsKey(type))
        {
            Debug.Log("Publish event " + type);
            _eventMap[type](args);
        }
        else
        {
            Debug.LogWarning("Publish event " + type + " has no listeners");
        }
    }
}

public class PubsubHandler
{
    private Action<object> callback;
    private Pubsub.Type type;

    public PubsubHandler(Pubsub.Type type, Action<object> callback)
    {
        Pubsub.Register(type, callback);
        this.callback = callback;
        this.type = type;
    }

    public void Dispose()
    {
        Pubsub.Unregister(type, callback);
    }

    public static PubsubHandler HandleNewBlocOnTower(Action<int, GameObject> callback)
    {
        return new PubsubHandler(Pubsub.Type.NewBlocOnTower, (arg) =>
        {
            object[] args = (object[])arg;
            int count = (int)args[0];
            GameObject bloc = (GameObject)args[1];
            callback(count, bloc);
        });
    }

    public static PubsubHandler HandleNewInteractiveBloc(Action<GameObject> callback)
    {
        return new PubsubHandler(Pubsub.Type.NewInteractiveBloc, (arg) =>
        {
            GameObject bloc = (GameObject)arg;
            callback(bloc);
        });
    }
    public static PubsubHandler HandleGameOver(Action<object> callback)
    {
        return new PubsubHandler(Pubsub.Type.GameOver, (ignore) =>
        {
            callback(ignore);
        });
    }

    public static PubsubHandler HandleBlocTouchTower(Action<InteractiveBloc> callback)
    {
        return new PubsubHandler(Pubsub.Type.BlocTouchTower, (arg) =>
        {
            InteractiveBloc bloc = (InteractiveBloc)arg;
            callback(bloc);
        });
    }
}