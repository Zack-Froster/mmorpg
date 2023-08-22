using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandler();

        public event OnEquipChangeHandler OnEquipChanged;

        public Item[] EquipsOnBody = new Item[(int)EquipSlot.SlotMax];

        byte[] Data;

        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(data);
        }

        public bool Contains(int equipId)
        {
            for (int i = 0; i < this.EquipsOnBody.Length; i++)
            {
                if (EquipsOnBody[i] != null && EquipsOnBody[i].Id == equipId)
                {
                    return true;
                }
            }
            return false;
        }

        public Item GetEquip(EquipSlot slot)
        {
            return EquipsOnBody[(int)slot];
        }

        unsafe void ParseEquipData(byte[] data)
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < this.EquipsOnBody.Length; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (itemId > 0)
                    {
                        EquipsOnBody[i] = ItemManager.Instance.Items[itemId];
                    }
                    else
                    {
                        EquipsOnBody[i] = null;
                    }
                }
            }
        }

        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = Data)
            {
                for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemid = (int*)(pt + i * sizeof(int));
                    if (EquipsOnBody[i] == null)
                    {
                        *itemid = 0;
                    }
                    else
                    {
                        *itemid = EquipsOnBody[i].Id;
                    }
                }
            }
            return this.Data;
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }

        public void OnEquipItem(Item equip)
        {
            if (this.EquipsOnBody[(int)equip.EquipInfo.Slot] != null && this.EquipsOnBody[(int)equip.EquipInfo.Slot].Id == equip.Id)
            {
                return;
            }
            this.EquipsOnBody[(int)equip.EquipInfo.Slot] = ItemManager.Instance.Items[equip.Id];

            if (OnEquipChanged != null)
            {
                OnEquipChanged();
            }
        }

        public void OnUnEquipItem(EquipSlot slot)
        {
            if (this.EquipsOnBody[(int)slot] != null)
            {
                this.EquipsOnBody[(int)slot] = null;
                if (OnEquipChanged != null)
                {
                    OnEquipChanged();
                }
            }
        }
    }
}
