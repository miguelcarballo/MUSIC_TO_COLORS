using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;


public class MIDIReader : MonoBehaviour
{

    //string NameOfMIDIFile = "RiverFlowsInYou";
    //string NameOfMIDIFile = "DuelOfFates";

    MidiFileLoader loader; //MPTK fileLoader
    MidiFilePlayer mfp;
    GraphicManager graphicManager;
    public GameObject laserNoteModel;
    public GameObject cameraHolder;
    public GameObject canvasMenu;
    public GameObject canvasPause;

    bool firstTime = true;
    bool moveCamera = false;
    Vector3 initialCameraHolderPos;

    float delayPlayer = 1.0f;
    float timerC = 0.0f;

    float timeResetAfterPlayingEnd = 4.0f;
    bool midiFinished = false;
    float timeAfterPlayingCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        graphicManager = new GraphicManager(laserNoteModel);
        SessionHandler.isPlaying = false;
        initialCameraHolderPos = cameraHolder.transform.position;
        SessionHandler.setMidiReader(this);

        this.canvasPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SessionHandler.isPlaying)
        {
            
            if (timerC >= delayPlayer)
            {
                if (firstTime)
                {
                    canvasMenu.SetActive(false);
                    LoadMidiPlayer();
                    mfp.MPTK_Play();
                    firstTime = false;
                    moveCamera = true;
                    
                    
                }
            }
            if (moveCamera)
            {
                this.cameraHolder.transform.position = this.cameraHolder.transform.position + this.cameraHolder.transform.forward * Time.deltaTime * graphicManager.getDeltaZTime();
            }
            if (midiFinished) //if the player finished the midi file
            {
                if(timeAfterPlayingCounter >= timeResetAfterPlayingEnd) //after time threshold, everything will be restarted.
                {
                    Debug.Log("%%%%%%%%%%IT IS GOING TO RESET");
                    ResetEverything();
                }
                timeAfterPlayingCounter += Time.deltaTime;
            }
            timerC += Time.deltaTime;

        }
        if (Input.GetKey("d"))
        {
            PauseMidi();
        }

    }


    private void LoadMidiPlayer()
    {

        mfp = FindObjectOfType<MidiFilePlayer>();
        // Set event by script
        mfp.OnEventEndPlayMidi.AddListener(EndPlay);
        mfp.OnEventNotesMidi.AddListener(MidiReadEvents);
        mfp.MPTK_MidiName = SessionHandler.nameCurrentPiece;
    }


    // Event fired when a midi is ended
    public void EndPlay(string midiname, EventEndMidiEnum reason)
    {
        Debug.LogFormat("End playing {0} reason:{1}", midiname, reason);
        if (reason.Equals(EventEndMidiEnum.ApiStop))
        {
            midiFinished = false;
        }
        else
        {
            midiFinished = true;
        }
       
    }

    // Event fired by MidiFilePlayer when midi notes are available
    public void MidiReadEvents(List<MPTKEvent> events)
    {
        foreach (MPTKEvent midievent in events)
        {
            // Log if event is a note on
            //loader.MPTK_DeltaTicksPerQuarterNote
            if (midievent.Command == MPTKCommand.NoteOn)
            {
                Debug.Log($"Note On at {midievent.RealTime} millisecond  Channel:{midievent.Channel} Note:{midievent.Value}  Duration:{midievent.Duration} millisecond  Velocity:{midievent.Velocity}");
                this.graphicManager.createGraphicNoteRepresentation(midievent);
            }
            else if (midievent.Command == MPTKCommand.PatchChange)
                Debug.Log($"Patch Change at {midievent.RealTime} millisecond  Channel:{midievent.Channel}  Preset:{midievent.Value}");
            else if (midievent.Command == MPTKCommand.ControlChange)
            {
                if (midievent.Controller == MPTKController.BankSelectMsb)
                    Debug.Log($"Bank Change at {midievent.RealTime} millisecond  Channel:{midievent.Channel}  Bank:{midievent.Value}");
            }
            // Uncomment to display all MIDI events
            //Debug.Log(mptkEvent.ToString());
        }
    }


    public void ResetEverything()
    {
        //initialCameraHolderPos
        firstTime = true;
        moveCamera = false;

        delayPlayer = 1.0f;
        timerC = 0.0f;

        timeResetAfterPlayingEnd = 4.0f;
        midiFinished = false;
        timeAfterPlayingCounter = 0.0f;
        this.graphicManager.cleanAllTheNoteObjects();

        graphicManager = new GraphicManager(laserNoteModel);
        SessionHandler.isPlaying = false;
        this.cameraHolder.transform.position = initialCameraHolderPos;

        canvasMenu.SetActive(true);
    }

    public void PauseMidi()
    {
        this.canvasPause.SetActive(true);
        SessionHandler.isPlaying = false;
        this.mfp.MPTK_Pause();
    }

    public void ContinueMidi()
    {
        this.mfp.MPTK_UnPause();
        this.canvasPause.SetActive(false);
        SessionHandler.isPlaying = true;
       
    }

    public void StopAndResetToTheBeginning()
    {
        this.canvasPause.SetActive(false);
        this.mfp.MPTK_Stop();
        ResetEverything();
       
    }

    public void FinishApp()
    {
        Application.Quit();
    }
}
