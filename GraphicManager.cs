using System;
using System.Linq;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;
using VolumetricLines;

public class GraphicManager {
    List<MPTKEvent> listNotesMidi;
    List<NodeNoteGameObj> listNodesNoteGameObj;
    GameObject laserNoteModel;
    static float deltaYnote = 0.15f; //distance between each note (pitch)
    static float deltaZtime = 1.0f; //1 meter each second
    static float minYValue = 0.0f - deltaYnote * 60.0f; //60 is C5, so it should be the middle point. MinVal is adjusted to that
    static float extremeLeftX = -4.0f;
    static float deltaXtime = 1.0f; //1 meters each second
    static float maxTimeWindow = 8.0f;//time to be in the same direction

    

   


    public GraphicManager(GameObject laserNoteModel)
    {
        this.listNotesMidi = new List<MPTKEvent>();
        this.laserNoteModel = laserNoteModel;
        
        this.listNodesNoteGameObj = new List<NodeNoteGameObj>();
    }

    public float getDeltaZTime()
    {
        return deltaZtime;
    }
    public NodeNoteGameObj createGraphicNoteRepresentation(MPTKEvent noteEvent)
    {
        float posX = extremeLeftX + deltaXtime * pointerInX((noteEvent.RealTime) / 1000.0f);

        float posY = minYValue + noteEvent.Value * deltaYnote;
        float posZ = 0.0f + deltaZtime * (noteEvent.RealTime) / 1000.0f; //real time is in ms

        VolumetricLineBehavior lineBehavior = laserNoteModel.GetComponent<VolumetricLineBehavior>();
        lineBehavior.LineColor = getColorOfNoteMIDI(noteEvent.Value);

        
        // scalateObjectDuration(laserNoteModel, noteEvent.Duration);
         GameObject newGObj = GameObject.Instantiate(laserNoteModel, new Vector3(posX, posY, posZ), laserNoteModel.transform.rotation);
         scaleObjectDuration(newGObj, noteEvent.Duration);
         NodeNoteGameObj node = new NodeNoteGameObj(noteEvent, newGObj);
         this.listNodesNoteGameObj.Add(node);

        return node ;
    }


    private float pointerInX(float time) //time in seconds
    {
        float valInX = time % maxTimeWindow;
        if (maxWindowMultMinorTo(time) % 2 != 0) //odd window
        {
            valInX = maxTimeWindow - valInX;
        }

        return valInX;
    }


    private int maxWindowMultMinorTo(float time)
    {
        int wMult = 0;
        while (time > maxTimeWindow * wMult)
        {
            wMult++;
        }
       
        return wMult - 1; 
    }

    private GameObject scaleObjectDuration(GameObject noteG, float durationMillis)
    {
        Vector3 noteGScale = noteG.transform.localScale;
        float scaleX = noteGScale.x;
        float scaleY = noteGScale.y * (durationMillis / 1000.0f);
        float scaleZ = noteGScale.z;
        Vector3 scale = new Vector3(scaleX, scaleY, scaleZ);
        noteG.transform.localScale = scale;


        return noteG;
    }

    private Color getColorOfNoteMIDI(int midiNumber)
    {
        Color color = Color.blue;
        string nameNote = getNoteName(midiNumber);
        if (nameNote.Equals("C"))
        {
            color = new Color(231.0f/255f, 50.0f / 255f, 42.0f / 255f); //redish
        }else if (nameNote.Equals("C#"))
        {
            color = new Color(144.0f / 255f, 115.0f / 255f, 54.0f / 255f);
        }
        else if (nameNote.Equals("D"))
        {
            color = new Color(213.0f / 255f, 173.0f / 255f, 54.0f / 255f);
        }
        else if (nameNote.Equals("D#"))
        {
            color = new Color(30.0f / 255f, 134.0f / 255f, 75.0f / 255f);
        }
        else if (nameNote.Equals("E"))
        {
            color = new Color(81.0f / 255f, 175.0f / 255f, 71.0f / 255f);
        }
        else if (nameNote.Equals("F"))
        {
            color = new Color(183.0f / 255f, 128.0f / 255f, 74.0f / 255f);
        }
        else if (nameNote.Equals("F#"))
        {
            color = new Color(209.0f / 255f, 98.0f / 255f, 53.0f / 255f);
        }
        else if (nameNote.Equals("G"))
        {
            color = new Color(49.0f / 255f, 183.0f / 255f, 208.0f / 255f);
        }
        else if (nameNote.Equals("G#"))
        {
            color = new Color(48.0f / 255f, 138.0f / 255f, 176.0f / 255f);
        }
        else if (nameNote.Equals("A"))
        {
            color = new Color(184.0f / 255f, 92.0f / 255f, 137.0f / 255f);
        }
        else if (nameNote.Equals("A#"))
        {
            color = new Color(156.0f / 255f, 117.0f / 255f, 162.0f / 255f);
        }
        else if (nameNote.Equals("B"))
        {
            color = new Color(214.0f / 255f, 157.0f / 255f, 172.0f / 255f);
        }
        return color;
    }
    //---------

    public static string getNoteName(int noteNum)
    {

        var notes = "C C#D D#E F F#G G#A A#B ";
        string nt;

        nt = notes.Substring((noteNum % 12) * 2, (noteNum % 12) * 2 + 2 - (noteNum % 12) * 2);
       
        string nameNote = nt;

        string nameNoteNS = String.Concat(nameNote.Where(c => !Char.IsWhiteSpace(c)));
        return nameNoteNS;
    }

    public void cleanAllTheNoteObjects()
    {
        foreach (NodeNoteGameObj node in listNodesNoteGameObj){
            node.destroyGameObject();
        }
        this.listNodesNoteGameObj = null;
    }
}

public class NodeNoteGameObj
{
    MPTKEvent note;
    GameObject gameObject;
    public NodeNoteGameObj(MPTKEvent note, GameObject gameObj)
    {
        this.note = note;
        this.gameObject = gameObj;
    }

    public GameObject getGameObject()
    {
        return this.gameObject;
    }

    public float getRealTime()
    {
        return this.note.RealTime;
    }

    public void setVisible()
    {
        this.gameObject.SetActive(true);
    }

    public void destroyGameObject()
    {
        UnityEngine.Object.Destroy(this.gameObject);
    }

}


