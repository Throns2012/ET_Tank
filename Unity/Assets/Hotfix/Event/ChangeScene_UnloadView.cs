﻿using System;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.ChangeScene)]
    class ChangeScene_UnloadView : AEvent
    {
        public override void Run()
        {
            try
            {
                Game.Scene.GetComponent<FUIComponent>().RemoveChildren();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}