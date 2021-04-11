using System;
using UnityEngine.Events;

public static class EventManage
{
    #region Actions
    public static UnityAction<string> eventOnResourceUpdate;
    #endregion

    #region PublicMethods
    public static void CallOnResourceUpdate(string _resID)
    {
        eventOnResourceUpdate?.Invoke(_resID);
    }
    #endregion
}
