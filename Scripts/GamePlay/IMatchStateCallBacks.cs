using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchStateCallBacks 
{
    void OnAspectSelectionState();
    void OnCardSelectionState();
    void OnCardComparingState();
    void OnEndOfRoundState();
}
