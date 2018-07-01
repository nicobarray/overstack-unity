using UnityEngine;

public class MyEventSystem : MonoBehaviour
{
    private InteractiveBloc bloc;
    private PubsubHandler OnNewInteractiveBloc;

    void OnEnable()
    {
        OnNewInteractiveBloc = PubsubHandler.HandleNewInteractiveBloc((GameObject bloc) =>
        {
            print("MyEventSystem::HandleNewInteractiveBloc -- Save new bloc " + bloc);
            this.bloc = bloc.GetComponent<InteractiveBloc>();
        });
    }

    void OnDisable()
    {
        OnNewInteractiveBloc.Dispose();
    }

    public void OnPointerClick()
    {
        print("OnPointerClick -- " + this.bloc);

        if (this.bloc != null)
        {
            this.bloc.TransitionToFall();
        }
    }
}