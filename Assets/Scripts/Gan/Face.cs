using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Face : MonoBehaviour
{
    public Image[] faces;
    public TMP_Text output;

    public void SetFace(double[] faces, double output)
    {
        for (int i = 0; i < this.faces.Length; i++)
            this.faces[i].color = new Color((float) (1 - faces[i]), (float) (1 - faces[i]), (float) (1 - faces[i]), 1);
        this.output.text = Math.Round(output * 100, 1) + "%";
        if(output < 0.5)
            this.output.color = Color.red;
        else 
            this.output.color = Color.green;
    }
}