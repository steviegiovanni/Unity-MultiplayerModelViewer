using ModelViewer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class MultiuserMPOPartInterface : Photon.PunBehaviour
    {
        private TaskList _taskList;
        public TaskList TaskList {
            get
            {
                if(_taskList == null)
                {
                    Transform parent = this.transform.parent;
                    while(parent.gameObject.GetComponent<TaskList>() == null)
                        parent = parent.parent;
                    _taskList = parent.GetComponent<TaskList>();
                }
                return _taskList;
            }
        }

        /// <summary>
        /// transfer ownership when requested
        /// </summary>
        public override void OnOwnershipRequest(object[] viewAndPlayer)
        {
            if (photonView.isMine)
            {
                foreach(var task in TaskList.Tasks)
                {
                    if (task.GameObject == this.gameObject)
                        task.Enabled = false;
                }

                PhotonView view = viewAndPlayer[0] as PhotonView;
                PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
                photonView.TransferOwnership(requestingPlayer);
            }
        }
    }
}
