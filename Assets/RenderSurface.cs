using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class RenderSurface : MonoBehaviour {
  public void Redraw() {
    GetComponent<Renderer>().material.shader = SettingsManager.Instance.ActiveShader;
  }
}
