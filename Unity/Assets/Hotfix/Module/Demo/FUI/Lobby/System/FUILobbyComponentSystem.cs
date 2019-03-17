using System;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILobbyComponentSystem : AwakeSystem<FUILobbyComponent>
    {
        public override void Awake(FUILobbyComponent self)
        {
            FUI lobby = self.GetParent<FUI>();
            lobby.Get("EnterMapBtn").GObject.asButton.onClick.Add(() => EnterMapBtnOnClick(self));
            lobby.Get("ShopBtn").GObject.asButton.onClick.Add(() => ShopBtnOnClick(self));
        }

        public static void EnterMapBtnOnClick(FUILobbyComponent self)
        {
            EnterMapAsync().NoAwait();
        }
        
        public static void ShopBtnOnClick(FUILobbyComponent self)
        {
            Game.EventSystem.Run(EventIdType.ShopBtnOnClick);
        }

        private static async ETVoid EnterMapAsync()
        {
            try
            {
                // ����Unit��Դ
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"unit.unity3d");

                // ���س�����Դ
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("map.unity3d");
                // �л���map����
                using (SceneChangeComponent sceneChangeComponent = ETModel.Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(SceneType.Map);
                }

                G2C_EnterMap g2CEnterMap = await ETModel.SessionComponent.Instance.Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
                PlayerComponent.Instance.MyPlayer.UnitId = g2CEnterMap.UnitId;

                Game.Scene.AddComponent<OperaComponent>();

                // �߼��㲻Ӧ��ȥ����UI���߼���ֻ�����߼������׳��¼�����UI���Լ�ȥ�����¼�
                Game.EventSystem.Run(EventIdType.EnterMapFinish);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}