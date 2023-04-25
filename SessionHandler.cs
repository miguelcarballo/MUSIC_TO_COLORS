using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionHandler {
    public static string nameCurrentPiece = "";
    public static bool isPlaying = false;

    private static MIDIReader midiReader= null;

    public static void setMidiReader(MIDIReader midiReaderIn)
    {
        midiReader = midiReaderIn;
    }

    public static void unpause()
    {
        midiReader.ContinueMidi();
    }

    public static void backToMenu()
    {
        midiReader.StopAndResetToTheBeginning();
    }


    public static void quitApp()
    {
        midiReader.FinishApp();
    }

}
