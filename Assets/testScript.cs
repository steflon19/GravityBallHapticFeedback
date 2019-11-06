using UnityEngine;
using UnityEditor;
using System.IO;

public class HandleTextFile
{
    [MenuItem("Tools/Write file")]
    static void WriteString()
    {
        string path = "Assets/Resources/ParticipantsData/" + MainMenu.participantID.ToString() + ".csv";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("ParticipantID," + MainMenu.participantID.ToString());

        writer.WriteLine("some data," + (MainMenu.participantID / 2).ToString());
        writer.WriteLine("some more data," + (MainMenu.participantID * 2).ToString());
        writer.Close();
        /*
        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>(MainMenuPassID.participantID.ToString());

        //Print the text from the file
        Debug.Log(asset.text);*/
    }

}