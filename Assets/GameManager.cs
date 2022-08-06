using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.Networking;
using TMPro;
public class GameManager : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Camera mainCam;
    public Camera secodnaryCam;
    public GameObject tree;
    public Transform spawnObject;
    public Transform selectedObject;
    public TMP_InputField path;
    public GameObject rock;
    public LevelScript levelScript;
    public float speed;
    public GameObject submitBtn;
    public GameObject drawBtn;
    public GameObject note;
    public GameObject hintCanvas;
    public void DisableNote(){
        note.SetActive(false);
    }
    private void Awake() {
        disableDrawMode();
        Invoke(nameof(DisableNote),1.5f);
    }
    public void postImage(){
        Texture2D texture = renderer.sprite.texture;
        print(Convert.ToBase64String(texture.EncodeToPNG()));
        StartCoroutine(Upload(Convert.ToBase64String(texture.EncodeToPNG())));
    }
    public void saveImage(){
        Texture2D texture = renderer.sprite.texture;
        print(Convert.ToBase64String(texture.EncodeToPNG()));
        StartCoroutine(UploadAtPath(Convert.ToBase64String(texture.EncodeToPNG()),path.text));
    }
    IEnumerator Upload(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("img", data);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/predict", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                levelScript.Interact(www.downloadHandler.text);
                //Spawn(www.downloadHandler.text);
                disableDrawMode();
            }
        }
    }
    IEnumerator UploadAtPath(string data,string path){
         WWWForm form = new WWWForm();
        form.AddField("img", data);
        form.AddField("path",path);
        print("sending request");
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/save", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                
                //Spawn(www.downloadHandler.text);
            }
        }
    }
    public void enableDrawMode(){
        mainCam.enabled= false;
        secodnaryCam.enabled = true;
        submitBtn.SetActive(true);
        drawBtn.SetActive(false);
        hintCanvas.SetActive(true);
    }
    public void disableDrawMode(){
        mainCam.enabled= true;
        submitBtn.SetActive(false);
        drawBtn.SetActive(true);
        secodnaryCam.enabled = false;
        hintCanvas.SetActive(false);
    }
    public void Spawn(string val){
        val = val.ToLower();
        if(val.Contains("tree")){
            GameObject _tree = Instantiate(tree,spawnObject.position,Quaternion.identity);
            selectedObject = _tree.transform;
        }else{
            GameObject _tree = Instantiate(rock,spawnObject.position,Quaternion.identity);
            selectedObject = _tree.transform;
        }
    }
    private void Update() {
        if(selectedObject){
            Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            Vector3 position = selectedObject.position;
            position.x += inputs.x * speed * Time.deltaTime;
            position.z += inputs.y * speed * Time.deltaTime;
            selectedObject.position = position;
            if(Input.GetKeyDown(KeyCode.LeftControl)){
                selectedObject = null;
            }
        }
    }
}
