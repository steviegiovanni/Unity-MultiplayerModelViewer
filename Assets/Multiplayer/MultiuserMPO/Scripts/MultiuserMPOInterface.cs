using ModelViewer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// multiplayer interface that encapsulates the multi parts object class so that it will work with multiplayer
    /// </summary>
    public class MultiuserMPOInterface : Photon.PunBehaviour, IPunObservable
    {
        public bool testData;

        /// <summary>
        /// the multiparts object associated with this interface
        /// </summary>
        private MultiPartsObject _mpo;
        public MultiPartsObject MPO {
            get {
                if (_mpo == null)
                    _mpo = GetComponent<MultiPartsObject>();
                return _mpo;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // we own this player: send the others our data
                stream.SendNext(testData);
            }
            else
            {
                // network player, receive data
                this.testData = (bool)stream.ReceiveNext();
            }
        }

        /// <summary>
        /// transfer ownership when requested
        /// </summary>
        public override void OnOwnershipRequest(object[] viewAndPlayer)
        {
            //MPO.ReleaseCage();
            
            if (photonView.isMine)
            {
                PhotonView view = viewAndPlayer[0] as PhotonView;
                PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
                photonView.TransferOwnership(requestingPlayer);
            }
            //base.OnOwnershipRequest(viewAndPlayer);
        }

        // Update is called once per frame
        void Update()
        {
            //if (photonView.isMine)
            //{
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                testData = !testData;
                photonView.RPC("ChatMessage", PhotonTargets.AllViaServer, photonView.viewID.ToString(), "lala");
            }

            // test input without AR/VR setup
            if (Input.GetKeyUp(KeyCode.Q))
            {
                GrabCage();
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                ReleaseCage();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                Select();
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                Deselect();
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                Grab();
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                Release();
            }

            //}
        }

        [PunRPC]
        void ChatMessage(string a, string b, PhotonMessageInfo info)
        {
            Debug.Log(string.Format("Info: {0} {1} {2}", info.sender, info.photonView, info.timestamp));
            Debug.Log(a + " sends " + b);
        }

        public void GrabCage()
        {
            // take ownership of the cage
            if (!photonView.isMine)
                photonView.RequestOwnership();

            MPO.GrabCage();
        }

        public void ReleaseCage()
        {
            MPO.ReleaseCage();
        }

        public void Select()
        {
            MPO.Select();
        }

        public void Deselect()
        {
            MPO.Deselect();
        }

        public void Grab()
        {
            // take ownership of each of the selected node
            foreach(var node in MPO.SelectedNodes)
            {
                if (!node.GameObject.GetComponent<PhotonView>().isMine)
                    node.GameObject.GetComponent<PhotonView>().RequestOwnership();
            }
            MPO.GrabIfPointingAt();
        }

        public void Release() {
            MPO.Release();
        }
    }
}
