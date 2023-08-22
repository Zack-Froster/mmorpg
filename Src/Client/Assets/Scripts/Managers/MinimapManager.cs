﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap minimap;

        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get
            {
                return minimapBoundingBox;
            }
        }

        public Transform PlayerTransform
        {
            get
            {
                if(User.Instance.CurrentPlayerInputController == null)
                {
                    return null;
                }
                return User.Instance.CurrentPlayerInputController.transform;
            }
        }
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMinimap(Collider minimapBoundingBox)
        {
            this.minimapBoundingBox = minimapBoundingBox;
            if(this.minimap != null)
            {
                this.minimap.UpdateMap();
            }
        }
    }
}
