﻿using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class G2C_RoomsHandler : AMHandler<G2C_Rooms>
    {
        protected override void Run(ETModel.Session session, G2C_Rooms message)
        {
            RunAsync(session,message).NoAwait();
        }

        protected async ETVoid RunAsync(ETModel.Session session, G2C_Rooms message)
        {
            //Game.EventSystem.Run(EventIdType.LoginHasFinish);

            FUI fui = Game.Scene.GetComponent<FUIComponent>().Get(FUIType.Hall);

            if (fui == null)
            {
                Log.Error($"再刷新HallViewComponent数据时还未创建HallViewComponent");
            }
            else
            {
                fui.GetComponent<HallViewComponent>().RefreshData(message);
            }

            await ETTask.CompletedTask;
        }
    }
}