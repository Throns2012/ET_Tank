﻿using System;
using System.Collections.Generic;
using ETModel;
using FairyGUI;

namespace ETHotfix
{
    [ObjectSystem]
    public class CreateRoomAwakeComponent : AwakeSystem<CreateRoomViewComponent>
    {
        public override void Awake(CreateRoomViewComponent self)
        {
            self.Awake();
        }
    }

    public class CreateRoomViewComponent : FUIBase
    {

        private GButton m_closeBtn;
        private GButton m_enterBtn;

        private GLabel m_roomName;

        private GComboBox m_peopleNum;

        private GComboBox m_map;

        private GComboBox m_bigModel;

        private GComboBox m_smallModel;

        private string[] m_nums = new string[] { "1v1", "2v2", "3v3", "4v4", "5v5" };

        private List<string> m_mapNames = new List<string>();

        private List<string> m_mapId = new List<string>();

        private string[] m_models = new string[] { "回合制","时间制" };

        private string[] m_rounds = new[] {"1 回合" ,"5 回合", "10 回合", "12 回合" };

        private string[] m_times = new[] { "1 min","5 min", "10 min", "12 min" };


        public void Awake()
        {
            this.FUIComponent = this.GetParent<FUI>();
            this.StartFUI();
        }

        private void StartFUI()
        {
            this.m_closeBtn = this.FUIComponent.Get("Close").GObject.asButton;

            this.m_closeBtn.onClick.Set(this.OnClose);

            this.m_enterBtn = this.FUIComponent.Get("Enter").GObject.asButton;

            this.m_enterBtn.onClick.Set(this.EnterBtn_OnClick);

            IConfig[] configs = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(Map)) as IConfig[];

            this.m_roomName = this.FUIComponent.Get("n7").GObject.asLabel;

            this.m_peopleNum = this.FUIComponent.Get("n2").GObject.asComboBox;

            this.m_peopleNum.items = m_nums;

            this.m_peopleNum.values = m_nums;

            this.m_peopleNum.selectedIndex = 0;

            this.m_map = this.FUIComponent.Get("n6").GObject.asComboBox;

            foreach (IConfig conf in configs)
            {
                Map map = conf as Map;

                this.m_mapNames.Add(map.ChineseMapName);

                this.m_mapId.Add(map.Id.ToString());
            }

            this.m_map.items = this.m_mapNames.ToArray();

            this.m_map.values = this.m_mapId.ToArray();
            //this.m_map.items = this.m_map.values = this.m_mapNames;

            this.m_map.selectedIndex = 0;

            this.m_bigModel = this.FUIComponent.Get("n10").GObject.asComboBox;

            this.m_bigModel.items = this.m_bigModel.values = this.m_models;

            this.m_smallModel = this.FUIComponent.Get("n11").GObject.asComboBox;

            this.m_bigModel.onChanged.Set(BigModel_OnChange);

            this.m_bigModel.value =this.m_models[0];

            this.m_smallModel.items = this.m_smallModel.values = this.m_rounds;

            this.m_smallModel.selectedIndex = 0;
            //this.m_smallModel.items = this.m_smallModel.values = this.
        }

        private void BigModel_OnChange(EventContext context)
        {
            if (this.m_bigModel.selectedIndex == 0)
            {
                this.m_smallModel.items = this.m_smallModel.values = this.m_rounds;
            }
            else
            {
                this.m_smallModel.items = this.m_smallModel.values = this.m_times;
            }
            this.m_smallModel.selectedIndex = 0;
        }



        private int GetPeopleNum()
        {
            return Convert.ToInt32(this.m_peopleNum.value.Split('v')[0]);
        }

        private int GetMapName()
        {
            return Convert.ToInt32(this.m_map.value);
        }
        /// <summary>
        /// 1回合制   2时间制
        /// </summary>
        /// <returns></returns>
        private int GetBigModel()
        {
            return this.m_bigModel.selectedIndex + 1;
        }

        private int GetSmallModel()
        {
            return Convert.ToInt32(this.m_smallModel.value.Split(' ')[0]);
        }

        private string GetRoomName()
        {
            return this.m_roomName.text;
        }
        private void EnterBtn_OnClick()
        {
            this.Send_C2G_CreateRoom().NoAwait();

            this.OnClose();
        }
        private async ETVoid Send_C2G_CreateRoom()
        {
            C2G_CreateRoom msg= new C2G_CreateRoom();

            msg.PeopleNum = this.GetPeopleNum();

            msg.MapId = this.GetMapName();

            msg.BigModel = this.GetBigModel();

            msg.SmallModel = this.GetSmallModel();

            msg.RoomNam = this.GetRoomName();

            ETModel.SessionComponent.Instance.Session.Send(msg);

            await ETTask.CompletedTask;

            //G2C_RoomDetailInfo response = (G2C_RoomDetailInfo) await ETModel.SessionComponent.Instance.Session.Call(msg);

            // response.LeftCamp
            //
            // List<RoomViewComponent.RoomOnePeople> leftList =new List<RoomViewComponent.RoomOnePeople>();
            //
            // RoomViewComponent.RoomOnePeople item = new RoomViewComponent.RoomOnePeople();
            //
            // item.m_level = 1;
            //
            // item.m_isOver = true;
            //
            // item.m_name = PlayerComponent.Instance.MyPlayer.Name;
            //
            // leftList.Add(item);


            //await FUIFactory.Create<RoomViewComponent, G2C_RoomDetailInfo>(FUIType.Room,response);
        }
    }
}