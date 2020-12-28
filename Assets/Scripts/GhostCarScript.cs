using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GhostCarScript : MonoBehaviour
{
    public bool autoDrive;
    public bool saveGhostCar;

    private float m_ghostFrequencySamples = 1;
    private float m_frequencyBetweenSamples = 1;

    private List<Vector3> ghostCarPositions = new List<Vector3>();
    private List<Quaternion> ghostCarRotations = new List<Quaternion>();
    
    private Vector3 m_lastSamplePosition;
    private Vector3 m_nextPosition;
    private Quaternion m_lastSampleRotation;
    private Quaternion m_nextRotation;

    private float m_currentTimeBetweenSamples;
    private float m_currenttimeBetweenPlaySamples;
    private int m_currentGhostSample = 1;
    private bool savedFile;

    void Start()
    {
        m_lastSamplePosition = transform.position;
        m_lastSampleRotation = transform.rotation;
        ReadGhostCarTXT();
    }

    void Update()
    {
        if(saveGhostCar) SaveGhostCarInfo();
        if (autoDrive)
        {
            ShowGhostCar();
        }
    }

    void FixedUpdate()
    {
        if (autoDrive)
        {
            float percentageBetweenFrames = m_currenttimeBetweenPlaySamples / m_ghostFrequencySamples;
            transform.position = Vector3.Slerp(m_lastSamplePosition, m_nextPosition, percentageBetweenFrames);
            transform.rotation = Quaternion.Slerp(m_lastSampleRotation, m_nextRotation, percentageBetweenFrames);
        }
    }

    public void GetDataAt(int sample, out Vector3 position, out Quaternion rotation)
    {
        position = ghostCarPositions[sample];
        rotation = ghostCarRotations[sample];
    }

    public void AddNewData(Transform transform)
    {
        ghostCarPositions.Add(transform.position);
        ghostCarRotations.Add(transform.rotation);
    }

    private void ShowGhostCar()
    {
        m_currenttimeBetweenPlaySamples += Time.deltaTime;

        if (m_currenttimeBetweenPlaySamples >= m_ghostFrequencySamples)
        {
            m_lastSamplePosition = m_nextPosition;
            m_lastSampleRotation = m_nextRotation;

            if (m_currentGhostSample == ghostCarPositions.Count)
            {
                m_currentGhostSample = 0;
            }

            GetDataAt(m_currentGhostSample, out m_nextPosition, out m_nextRotation);
            m_currenttimeBetweenPlaySamples -= m_ghostFrequencySamples;
            m_currentGhostSample++;
        }
    }

    private void SaveGhostCarInfo()
    {
        m_currentTimeBetweenSamples += Time.deltaTime;
        if (m_currentTimeBetweenSamples >= m_frequencyBetweenSamples)
        {
            AddNewData(transform);
            m_currentTimeBetweenSamples -= m_frequencyBetweenSamples;
        }
    }

    public void WriteGhostCarTXT()
    {
        string path = "Assets/Resources/"+ transform.name +".txt";
        StreamWriter writer = new StreamWriter(path, false);

        writer.WriteLine(ghostCarPositions.Count.ToString());
        for (int i = 0; i < ghostCarPositions.Count; i++)
        {
            writer.WriteLine(ghostCarPositions[i] + "/" + ghostCarRotations[i]);
        }
        writer.Close();
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
    }

    public void ReadGhostCarTXT()
    {
        try
        {
            string path = "Assets/Resources/"+ transform.name + ".txt";
            StreamReader reader = new StreamReader(path);

            var numOfSamples = reader.ReadLine();

            ghostCarPositions.Clear();
            ghostCarRotations.Clear();
            for (int i = 0; i < int.Parse(numOfSamples); i++)
            {
                var line = reader.ReadLine();
                var ghostPos = line.Split('/')[0];
                var ghostRot = line.Split('/')[1];
                ghostPos = ghostPos.Replace("(", "");
                ghostPos = ghostPos.Replace(")", "");
                ghostPos = ghostPos.Replace(" ", "");
                ghostRot = ghostRot.Replace("(", "");
                ghostRot = ghostRot.Replace(")", "");
                ghostRot = ghostRot.Replace(" ", "");

                var xP = ghostPos.Split(',')[0].Replace(".", ",");
                float xPos;
                float.TryParse(xP, out xPos);
                var yP = ghostPos.Split(',')[1].Replace(".", ",");
                float yPos;
                float.TryParse(yP, out yPos);
                var zP = ghostPos.Split(',')[2].Replace(".", ",");
                float zPos;
                float.TryParse(zP, out zPos);
                ghostCarPositions.Add(new Vector3(xPos, yPos, zPos));

                var wR = ghostRot.Split(',')[0].Replace(".", ",");
                float wRot;
                float.TryParse(wR, out wRot);
                var xR = ghostRot.Split(',')[1].Replace(".", ",");
                float xRot;
                float.TryParse(xR, out xRot);
                var yR = ghostRot.Split(',')[2].Replace(".", ",");
                float yRot;
                float.TryParse(yR, out yRot);
                var zR = ghostRot.Split(',')[3].Replace(".", ",");
                float zRot;
                float.TryParse(zR, out zRot);
                ghostCarRotations.Add(new Quaternion(wRot, xRot, yRot, zRot));
            }
            reader.Close();
            GetDataAt(0, out m_nextPosition, out m_nextRotation);
        }
        catch
        {
            Debug.Log("No ghost car info detected.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish" && !savedFile)
        {
            savedFile = true;
            WriteGhostCarTXT();
        }
    }
}
