using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public bool Active
    {
        get { return this.gameObject.activeInHierarchy; }
        set 
        { 
            this.gameObject.SetActive(value);

            if (value)
                Open();
            else
                Close();
        }
    }

    public virtual void Open()
    {

    }

    public virtual void Close()
    {

    }

}
