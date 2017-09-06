using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControllerTest : NetworkBehaviour
{
    public GameObject BulletPrefab;
    public GameObject BulletSpawn;

    public GameObject UIPlayerSpawn;
    private ItemUIPlayer _uiPlayer;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        //transform.parent.Rotate(0, x, 0);
        //transform.parent.Translate(0, 0, z);
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.CmdFire();
        }
    }

    [CommandAttribute]
    private void CmdFire()
    {
        var bullet = (GameObject)Instantiate(
                this.BulletPrefab,
                BulletSpawn.transform
                //BulletSpawn.transform.position,
                //BulletSpawn.transform.rotation
                );

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
        this.CmdAddUIPlayer();
    }

    [CommandAttribute]
    private void CmdAddUIPlayer()
    {
        var go = PanelUIPlayers.Inst.AddUIPlayer(UIPlayerSpawn);
        _uiPlayer = go.GetComponent<ItemUIPlayer>();
        var health = transform.GetComponent<Health>();
        health.SetHealthBar(_uiPlayer.HealthBar);
        NetworkServer.Spawn(go);
    }
}
