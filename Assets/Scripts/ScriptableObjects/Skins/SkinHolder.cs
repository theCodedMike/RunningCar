using System.Collections.Generic;
using System.Utility;
using UnityEngine;

namespace ScriptableObjects.Skins
{
    [CreateAssetMenu(fileName = "SkinHolder", menuName = "RunningCar/SkinHolder", order = 25)]
    public class SkinHolder : ScriptableObject
    {
        public List<Skin> skinsBody = new();
        public List<Skin> skinsWheels = new();

        #region Body
        public Skin GetSkinBody(int id)
        {
            Skin skin = skinsBody[id];
            if(id == 0)
                skin.Unlock();
            return skin;
        }

        public Skin GetSelectedSkinBody()
        {
            int id = CustomPlayerPrefs.GetInt("skinSelectedBody", 0);
            if(id < skinsBody.Count)
                return skinsBody[id];
            else
            {
                SetSelectedSkinBody(0);
                return skinsBody[0];
            }
        }
        
        public void SetSelectedSkinBody(int id) => CustomPlayerPrefs.SetInt("skinSelectedBody", id);
        #endregion



        #region Wheels

        public Skin GetSkinWheels(int id)
        {
            Skin skin = skinsWheels[id];
            if(id == 0)
                skin.Unlock();
            return skin;
        }

        public Skin GetSelectedSkinWheels()
        {
            int id = CustomPlayerPrefs.GetInt("skinSelectedWheels", 0);
            if(id < skinsWheels.Count)
                return skinsWheels[id];
            else
            {
                SetSelectedSkinWheels(0);
                return skinsWheels[0];
            }
        }
        public void SetSelectedSkinWheels(int id) => CustomPlayerPrefs.SetInt("skinSelectedWheels", id);
        #endregion


        public void UnlockAll() => skinsBody.ForEach(skin => skin.Unlock());

        public void LockAll()
        {
            for (int i = 0; i < skinsBody.Count; i++)
            {
                if(i == 0)
                    skinsBody[i].Unlock();
                else
                    skinsBody[i].Lock();
            }
            
            CustomPlayerPrefs.SetInt("skinSelected", 0);
        }
    }
}
