using ModelViewer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class MultiuserMPOInterface : Photon.PunBehaviour, IPunObservable
    {
        public bool testData;
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

        // Use this for initialization
        void Start()
        {

        }

        public override void OnOwnershipRequest(object[] viewAndPlayer)
        {
            MPO.ReleaseCage();
            
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
                if (!photonView.isMine)
                   photonView.RequestOwnership();
                MPO.GrabCage();
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                MPO.ReleaseCage();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                MPO.Select();
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                MPO.Deselect();
            }

            //}
        }

        [PunRPC]
        void ChatMessage(string a, string b, PhotonMessageInfo info)
        {
            Debug.Log(string.Format("Info: {0} {1} {2}", info.sender, info.photonView, info.timestamp));
            Debug.Log(a + " sends " + b);
        }
    }
}
